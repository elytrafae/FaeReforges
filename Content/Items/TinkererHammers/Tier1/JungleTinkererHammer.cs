using FaeLibrary.API.ItemConditions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class JungleTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 54);
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;
        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) {
            victim.AddBuff(BuffID.Poisoned, 300);
        }
        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) {
            victim.AddBuff(BuffID.Poisoned, 300, false);
        }
        public override void HammerEnchantmentVisuals(Player player, int itemID, Vector2 position, int height, int width) {
            if (Main.rand.NextBool(5)) {
                Dust.NewDust(position, width, height, DustID.Poisoned);
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Stinger, 12)
                .AddIngredient(ItemID.JungleSpores, 8)
                .AddIngredient(ItemID.Vine, 3)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
