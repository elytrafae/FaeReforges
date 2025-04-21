using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class BeetleTinkererHammer : SimpleTinkererHammerItem {
        public const int SPAWN_CHANCE_PERCENT = 30;
        public const int BEETLE_DAMAGE_PERCENT = 60;

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;
        public override int HammerTier => 4;

        public override LocalizedText WeaponEffectText => base.WeaponEffectText.WithFormatArgs(SPAWN_CHANCE_PERCENT, BEETLE_DAMAGE_PERCENT);

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone, DamageClass dmgClass) {
            SpawnBeetle(damageDone, hitInfo.Knockback, dmgClass, attacker, victim, hitInfo.HitDirection);
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo, DamageClass dmgClass) {
            SpawnBeetle(hurtInfo.Damage, hurtInfo.Knockback, dmgClass, attacker, victim, hurtInfo.HitDirection);
        }

        public static void SpawnBeetle(int origDamage, float origKB, DamageClass dmgClass, Player owner, Entity victim, int direction) {
            if (owner.whoAmI != Main.myPlayer) {
                return;
            }
            int damage = (origDamage * BEETLE_DAMAGE_PERCENT - 1) / 100 + 1; // INT division rounded up
            float kb = origKB * BEETLE_DAMAGE_PERCENT / 100;
            if (damage <= 0) {
                return;
            }
            Vector2 velocity = new Vector2(direction, 0) * 3;//new Vector2((float)Math.Cos(MathHelper.ToRadians(direction)), (float)Math.Sin(MathHelper.ToRadians(direction))) * 3;
            Projectile proj = Projectile.NewProjectileDirect(owner.GetSource_OnHit(victim), victim.Center, velocity, ModContent.ProjectileType<ShotBeetle>(), damage, kb, owner.whoAmI);
            if (proj != null && proj.active) {
                proj.DamageType = dmgClass;
            }
        }
    }
}
