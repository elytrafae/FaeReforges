using FaeLibrary.API.ItemConditions;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using FaeReforges.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class SpectreTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(gold: 2, silver: 30);
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) {
            // Enemy is dead!
            if (victim.life <= 0) {
                SpawnProjectile(victim.Center, item, attacker);
            }
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) {
            // Enemy is dead!
            if (victim.statLife <= 0) {
                SpawnProjectile(victim.Center, item, attacker);
            }
        }

        private void SpawnProjectile(Vector2 position, int itemID, Player attacker) {
            if (attacker.whoAmI != Main.myPlayer) {
                return;
            }
            int projType = ModContent.ProjectileType<FriendlyDungeonSpirit>();
            if (attacker.ownedProjectileCounts[projType] >= 3) {
                return;
            }
            int baseDamage = 50;
            float baseKB = 2.5f;
            DamageClass damageType = ContentSamples.ItemsByType[itemID].DamageType;
            Projectile proj = Projectile.NewProjectileDirect(attacker.GetSource_FromThis(), position, Vector2.Zero, projType, (int)attacker.GetDamage(damageType).ApplyTo(baseDamage), attacker.GetKnockback(damageType).ApplyTo(baseKB), attacker.whoAmI);
            if (proj != null && proj.active) {
                proj.originalDamage = baseDamage;
                proj.DamageType = damageType;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 15)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
