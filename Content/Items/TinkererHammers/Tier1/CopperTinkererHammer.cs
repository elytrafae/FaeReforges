using FaeLibrary.API.ItemConditions;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class CopperTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 1, copper: 50);
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.statDefense++;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
