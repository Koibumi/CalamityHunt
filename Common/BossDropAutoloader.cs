﻿using CalamityHunt.Content.Items.Autoloaded;
using CalamityHunt.Content.Tiles.Autoloaded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityHunt.Common
{
    public static class BossDropAutoloader
    {
        private static Mod Mod => ModContent.GetInstance<CalamityHunt>();

        public static int AddBossRelic(string bossName, string texturePath)
        {
            //Load the relic item but cache it first
            AutoloadedBossRelicItem relicItem = new AutoloadedBossRelicItem(bossName, texturePath);
            Mod.AddContent(relicItem);

            //Load the tile using the item's tile type
            AutoloadedBossRelic relicTile = new AutoloadedBossRelic(bossName, relicItem.Type, texturePath);
            Mod.AddContent(relicTile);

            //Set the relic item's type to be the one of the relic tile (so it can properly place it)
            relicItem.TileType = relicTile.Type;

            //Return the relic item's type so that npcs can drop it
            return relicItem.Type;
        }
    }
}
