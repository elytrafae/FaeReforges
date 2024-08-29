﻿using FaeReforges.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.HammerTypes {
    public class StoneHammer : AbstractHammerType {
        public override void SetDefaults() {
            this.negativeReforgeChance = 70;
            this.reforgeCost = 150;
            this.color = Color.DarkGray;
        }

        public override void AddRecipesForHammer(Item item) {
            Recipe.Create(item.type, 1)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient(ItemID.Wood, 10)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<TinkererAnvil>()
                .Register();
        }

    }
}
