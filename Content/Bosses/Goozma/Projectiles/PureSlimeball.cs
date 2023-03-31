﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityHunt.Content.Bosses.Goozma.Projectiles
{
    public class PureSlimeball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 220;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.localAI[0] = Main.rand.NextFloat(0.9f, 1.15f);
            Projectile.localAI[1] = Main.rand.NextFloat(30f);
            Projectile.rotation = Main.rand.NextFloat(-1f, 1f);

            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(SoundID.Item33, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {
                    Color glowColor = new GradientColor(SlimeUtils.GoozColorArray, 0.4f, 0.5f).ValueAt(Projectile.localAI[1]);
                    glowColor.A /= 2;
                    Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Projectile.velocity + Main.rand.NextVector2Circular(5, 5), 0, glowColor, 1.5f).noGravity = true;
                }
            }
        }

        public ref float Time => ref Projectile.ai[0];

        public override void AI()
        {
            Projectile.scale = (float)Math.Sqrt(Utils.GetLerpValue(-2, 17, Time, true) * Utils.GetLerpValue(220, 190, Time, true)) * Projectile.localAI[0];
            if (Time > 8)
                Projectile.velocity *= 0.955f;

            if (Main.rand.NextBool(8))
            {
                Color glowColor = new GradientColor(SlimeUtils.GoozColorArray, 0.4f, 0.5f).ValueAt(Projectile.localAI[1]);
                glowColor.A /= 2;
                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(20, 20), DustID.FireworksRGB, Main.rand.NextVector2Circular(5, 5), 0, glowColor, 1.2f).noGravity = true;
            }

            Time++;
            Projectile.localAI[1]++;
            Projectile.rotation = (float)Math.Sin(Projectile.localAI[1] * 0.03f) * Projectile.direction * 0.15f + Projectile.velocity.X * 0.15f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Color glowColor = new GradientColor(SlimeUtils.GoozColorArray, 0.2f, 0.2f).ValueAt(Projectile.localAI[0]);
                glowColor.A /= 2;
                Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, Main.rand.NextVector2Circular(6, 6), 0, glowColor, 1f).noGravity = true;

                if (Main.rand.NextBool(2))
                    Dust.NewDustPerfect(Projectile.Center, 4, Main.rand.NextVector2Circular(3, 3), 0, Color.Black, 1.5f).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Asset<Texture2D> glow = ModContent.Request<Texture2D>($"{nameof(CalamityHunt)}/Assets/Textures/Goozma/GlowSoft");
            Rectangle baseFrame = texture.Frame(1, 3, 0, 0);
            Rectangle glowFrame = texture.Frame(1, 3, 0, 1);
            Rectangle outlineFrame = texture.Frame(1, 3, 0, 2);

            Color bloomColor = new GradientColor(SlimeUtils.GoozColorArray, 0.4f, 0.5f).ValueAt(Projectile.localAI[1]);
            bloomColor.A = 0;

            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, outlineFrame, bloomColor * 0.7f, Projectile.rotation, baseFrame.Size() * 0.5f, Projectile.scale * 1.1f, 0, 0);
            Main.EntitySpriteDraw(glow.Value, Projectile.Center - Main.screenPosition, null, bloomColor * 0.15f, Projectile.rotation, glow.Size() * 0.5f, Projectile.scale * 3f, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, baseFrame, lightColor, Projectile.rotation, baseFrame.Size() * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, glowFrame, bloomColor, Projectile.rotation, glowFrame.Size() * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, glowFrame, bloomColor * 0.8f, Projectile.rotation, glowFrame.Size() * 0.5f, Projectile.scale * 1.05f, 0, 0);

            return false;
        }
    }
}
