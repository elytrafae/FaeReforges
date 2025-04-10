using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class ChlorophyteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(gold: 2, silver: 30);
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            MyReforgeHammerPlayer2.Get(player).chlorophyteHammerCount++;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
