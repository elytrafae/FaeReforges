using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class DemoniteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 2;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.maxRunSpeed *= 1.02f;
            player.runAcceleration *= 1.02f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.Ebonwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
