using FaeLibrary.API.ItemConditions;
using FaeLibrary.API.ClassExtensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class SilverTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsRangedWeapon, ItemCondition.IsAccessory);

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetRangedVelocity() += 0.03f;
        }

        public override void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers, DamageClass dmgClass) {
            hitModifiers.ArmorPenetration += 5;
        }

        public override void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers, DamageClass dmgClass) {
            hurtModifiers.ArmorPenetration += 5;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
