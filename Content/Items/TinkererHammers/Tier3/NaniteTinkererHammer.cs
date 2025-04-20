using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class NaniteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.statLifeMax2 += 15;
            player.GetModPlayer<MyReforgeHammerPlayer>().naniteRegenCount += 1; // 0.5HP/s while above 75% HP
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 30)
                .AddIngredient(ItemID.Nanites, 15)
                .AddIngredient(ItemID.Wire, 5)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }

    }
}
