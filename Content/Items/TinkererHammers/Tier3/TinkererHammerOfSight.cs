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
    public class TinkererHammerOfSight : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 30);
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            MyReforgeHammerPlayer2.Get(player).hammerOfSightCount++;
        }


        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
