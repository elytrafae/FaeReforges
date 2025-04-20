using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class TitaniumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count % 5 == 0) {
                player.ApplyEquipFunctional(ContentSamples.ItemsByType[ItemID.HermesBoots], false);
                player.statDefense += ContentSamples.ItemsByType[ItemID.HermesBoots].defense;
                player.moveSpeed += 0.03f;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
