using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class PlatinumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 12);
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsSummonWeapon, ItemCondition.IsAccessory);

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count % 5 == 0) {
                player.maxMinions++;
            }
        }

        public override void HammerOnApplyWeapon(Item item) {
            item.GetGlobalItem<SummonerReforgesGlobalItem>().summonSpeedMult *= 1.06f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
