﻿using CalamityHunt.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityHunt.Content.Items.Dyes
{
    public class GoopDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Iridescent Goop Dye");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            if (!Main.dedServ)
            {
                Effect goopShader = ModContent.Request<Effect>($"{nameof(CalamityHunt)}/Assets/Effects/GoopDye", AssetRequestMode.ImmediateLoad).Value;

                GameShaders.Armor.BindShader(ModContent.ItemType<GoopDye>(), new ArmorShaderData(new Ref<Effect>(goopShader), "LiquidPass"))
                    .UseColor(new Color(39, 31, 34))
                    .Shader.Parameters["uMap"].SetValue(ModContent.Request<Texture2D>($"{nameof(CalamityHunt)}/Assets/Textures/RainbowMap", AssetRequestMode.ImmediateLoad).Value);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(0, 1, 50);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EntropyMatter>()
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}
