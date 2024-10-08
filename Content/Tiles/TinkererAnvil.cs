﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.Tiles {
    public class TinkererAnvil : ModTile {

        public override void SetStaticDefaults() {
            // Properties
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

            DustType = DustID.Adamantite;
            AdjTiles = new int[] {  };

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            // Etc
            AddMapEntry(new Color(200, 200, 200), ModContent.GetInstance<Items.Placeable.TinkererAnvil>().DisplayName);
        }

        public override void NumDust(int x, int y, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        /*
        public override bool RightClick(int i, int j) {
            string stuff = "";
            foreach (ReforgeHammerType hammer in ReforgeHammerSaveSystem.GetAllUnlockedHammers()) {
                stuff += hammer.Name + " , ";
            }
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(stuff), Color.DeepPink);
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Selected Hammer: " + ReforgeHammerSaveSystem.GetSelectedHammer().Name), Color.DeepPink);
            return true;
        }
        */

    }
}
