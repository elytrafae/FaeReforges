using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class CobaltTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int Value => Terraria.Item.buyPrice(silver: 40);

        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count % 5 == 0) {
                player.ApplyEquipFunctional(ContentSamples.ItemsByType[ItemID.CobaltShield], false);
                player.statDefense += ContentSamples.ItemsByType[ItemID.CobaltShield].defense;
                player.statDefense -= 2;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
