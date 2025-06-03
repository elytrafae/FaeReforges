using System;
using FaeLibrary.API.ItemConditions;
using FaeLibrary.Implementation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class VortexTinkererHammer : SimpleTinkererHammerItem {

        public const int PROJECTILE_COOLDOWN = 6;
        public const int PROJECTILE_SALVO_COUNT = 2;
        public const float ACCESSORY_VELOCITY_BOOST = 0.05f;
        public const string BONUS_PROJECTILE_CONTEXT = "FaeReforges_VortexBonusShot";
        public override int Rarity => ItemRarityID.Red;
        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsRangedWeapon, ItemCondition.IsAccessory);
        public override int HammerTier => 4;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(ACCESSORY_VELOCITY_BOOST*100);
        }

        public override string GetWeaponEffectText(Item item) {
            return WeaponEffectText.Format(PROJECTILE_COOLDOWN, PROJECTILE_SALVO_COUNT);
        }

        int projectilesCreated = 0; // ONLY FOR LOCAL PLAYER!

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetRangedVelocity() += ACCESSORY_VELOCITY_BOOST;
        }

        public override void HammerOnCreateProjectile(int item, Projectile projectile, IEntitySource source) {
            if (projectile.owner == Main.myPlayer) {
                Player player = Main.LocalPlayer;
                projectilesCreated++;
                if (projectilesCreated >= PROJECTILE_COOLDOWN) {
                    if ((source.Context == null || !source.Context.Equals(BONUS_PROJECTILE_CONTEXT)) && source is IEntitySource_WithStatsFromItem itemUseSource) {
                        Item weapon = itemUseSource.Item;
                        if (TryGetAmmoNotOfType(player, weapon.useAmmo, out Item ammo)) {
                            projectilesCreated -= PROJECTILE_COOLDOWN;

                            for (int i = 0; i < PROJECTILE_SALVO_COUNT; i++) {
                                double angle = (Math.PI / (PROJECTILE_SALVO_COUNT - 1))*i + Math.Atan2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
                                Vector2 position = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 16 + player.Center;

                                Vector2 velocity = Main.MouseWorld - position;
                                velocity.Normalize();
                                velocity *= (weapon.shootSpeed + ammo.shootSpeed);

                                int damage = (int)(player.GetDamage(weapon.DamageType).ApplyTo(weapon.damage) + player.GetDamage(ammo.DamageType).ApplyTo(ammo.damage));
                                float kb = player.GetKnockback(weapon.DamageType).ApplyTo(weapon.knockBack) + player.GetKnockback(ammo.DamageType).ApplyTo(ammo.knockBack);

                                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(weapon, ammo.type, BONUS_PROJECTILE_CONTEXT), position, velocity, ammo.shoot, damage, kb);
                            }
                        }
                    }
                    
                    
                }
            }
        }

        private bool TryGetAmmoNotOfType(Player player, int ammoType, out Item ammo) { 
            for (int i = 54; i <= 57; i++) {
                Item item = player.inventory[i];
                if (item.IsAir) {
                    continue;
                }
                if (item.ammo == AmmoID.None) {
                    continue;
                }
                if (item.ammo != ammoType && item.shoot > ProjectileID.None) {
                    ammo = item;
                    return true;
                }
            }
            ammo = new Item(0);
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentVortex)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }

    }
}
