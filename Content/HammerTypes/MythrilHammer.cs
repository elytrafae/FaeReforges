using FaeReforges.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.HammerTypes {
    public class MythrilHammer : AbstractHammerType {
        public override void SetDefaults() {
            this.negativeReforgeChance = 45;
            this.reforgeCost = 115;
            this.color = Color.Turquoise;
        }

        public override void OnUpdateAccessory(Item item, Player player) {
            player.statDefense += 1;
        }

        public override void AddRecipesForHammer(Item item) {
            Recipe.Create(item.type, 1)
                .AddIngredient(ItemID.MythrilBar, 25)
                .AddIngredient(ItemID.Pearlwood, 10)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<TinkererAnvil>()
                .Register();
        }

    }
}
