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
    public class TerraTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int Value => Terraria.Item.buyPrice(gold: 10);
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override int HammerTier => 3;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            MyReforgeHammerPlayer2.Get(player).terraHammerCount++;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddIngredient(ItemID.SpectreBar, 5)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddIngredient(ItemID.MeteoriteBar, 5)
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddIngredient(ItemID.Rope, 10)
                .Register();
        }
    }
}
