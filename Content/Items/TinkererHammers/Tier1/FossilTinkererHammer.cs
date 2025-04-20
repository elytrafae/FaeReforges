using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class FossilTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;

        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.pickSpeed += 0.04f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.FossilOre, 10)
                .AddIngredient(ItemID.Cactus, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
