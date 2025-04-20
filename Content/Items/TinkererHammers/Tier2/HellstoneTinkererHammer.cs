using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Cooldowns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class HellstoneTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override LocalizedText WeaponEffectText => base.WeaponEffectText.WithFormatArgs(ModContent.GetInstance<HellstoneHammerCooldown>().DisplayCooldownTicks / 60f);

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) {
            Explosion(item, attacker, victim);
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) {
            Explosion(item, attacker, victim);
        }

        private void Explosion(int item, Player attacker, Entity victim) {
            if (attacker.whoAmI == Main.myPlayer) {
                if (ModContent.GetInstance<HellstoneHammerCooldown>().ConsumeCharge()) {
                    DamageClass dmgClass = ContentSamples.ItemsByType[item].DamageType;
                    Projectile proj = Projectile.NewProjectileDirect(attacker.GetSource_FromThis(), victim.Center, Vector2.Zero, ProjectileID.Volcano, (int)attacker.GetDamage(dmgClass).ApplyTo(50), attacker.GetKnockback(dmgClass).ApplyTo(0), attacker.whoAmI);
                    if (proj != null) {
                        proj.DamageType = dmgClass;
                    }
                }
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddIngredient(ItemID.AshWood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
