using FaeLibrary.API;
using FaeLibrary.API.Enums;
using FaeLibrary.API.ClassExtensions;
using FaeReforges.Content.Buffs;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Content.Items.TinkererHammers.Tier4;
using FaeReforges.Content.Projectiles;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {

    // I had no ideas for a better name
    public class MyReforgeHammerPlayer2 : ModPlayer, IFaeModPlayer {

        public int hammerOfSightCount = 0;
        public int hammerOfMightCount = 0;
        public int hammerOfFrightCount = 0;
        public int chlorophyteHammerCount = 0;
        public int terraHammerCount = 0;
        public bool shroomiteReforgeActive = false;

        // NOT STATS
        public int nebulaDamageStored = 0;
        public int nebulaTicksWithoutIndicator = 0;
        public int stardustDuration = 0;

        public override void ResetEffects() {
            hammerOfSightCount = 0;
            hammerOfMightCount = 0;
            hammerOfFrightCount = 0;
            chlorophyteHammerCount = 0;
            terraHammerCount = 0;
            shroomiteReforgeActive = false;
        }

        public override void PostUpdateEquips() {
            if (!shroomiteReforgeActive && Player.HasBuff<MushroomMending>()) {
                Player.ClearBuff(ModContent.BuffType<MushroomMending>());
            }
            if (ModContent.GetInstance<FrightHammerCooldown>().IsCoolingDown()) {
                Player.GetAttackSpeed(DamageClass.Generic) += hammerOfFrightCount * 0.03f;
                Player.GetSummonSpeed() += 0.03f;
                Player.moveSpeed += hammerOfFrightCount * 0.03f;
            }
        }

        public void OnDodge(Player.HurtInfo info, DodgeType dodgeType) {
            if (ReforgeHammerUtility.IsReforgedWith<HallowedTinkererHammer>(Player.HeldItem, Enums.HammerEffectContext.WEAPON)) {
                Player.AddBuff(ModContent.BuffType<HallowedAggression>(), 10 * 60);
            }
            OnHurtOrDodge(info);
        }

        public override void OnHurt(Player.HurtInfo info) {
            ModContent.GetInstance<FrightHammerCooldown>().CompletelyResetCooldown();
            OnHurtOrDodge(info);
        }

        public void OnHurtOrDodge(Player.HurtInfo info) {
            if (ModContent.GetInstance<StardustHammerCooldown>().ConsumeCharge()) {
                stardustDuration += StardustTinkererHammer.BRIEF_MOMENT_DURATION;
            }
        }

        public override bool FreeDodge(Player.HurtInfo info) {
            // NOTE: Everything here is only ran on the client! No need to check for that.
            if (Main.rand.Next(0, 100) < chlorophyteHammerCount * 2) {
                // Chlorophyte Reforge Dodge
                float initialAngle = Main.rand.NextFloat((float)(Math.PI * 2));
                Vector2 pos = Main.LocalPlayer.Center;
                int damage = (int)Main.LocalPlayer.GetDamage(DamageClass.Default).ApplyTo(50);
                float knockback = Main.LocalPlayer.GetKnockback(DamageClass.Default).ApplyTo(1);
                double angleStep = (Math.PI * 2 / chlorophyteHammerCount);
                for (int i = 0; i < chlorophyteHammerCount; i++) {
                    double finalAngle = initialAngle + i * angleStep;
                    Vector2 velocity = new Vector2((float)Math.Cos(finalAngle), (float)Math.Sin(finalAngle)) * 5;
                    Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), pos, velocity, ModContent.ProjectileType<FriendlyStinger>(), damage, knockback, Main.myPlayer);
                }
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 180 : 120);
                return true;
            }
            return false;
        }

        public override void PostUpdate() {
            // DEBUG
            //ChatHelper.DisplayMessage(NetworkText.FromLiteral("Wing: " + Player.wingTime + " / " + Player.wingTimeMax), Color.Aqua, byte.MaxValue);
            //ChatHelper.DisplayMessage(NetworkText.FromLiteral("Rocket: " + Player.rocketTime + " / " + Player.rocketTimeMax), Color.BlueViolet, byte.MaxValue);

            NebulaHammerCooldown nebulaCooldown = ModContent.GetInstance<NebulaHammerCooldown>();
            if (nebulaDamageStored >= NebulaTinkererHammer.DAMAGE_PER_MANA && nebulaCooldown.ConsumeCharge()) {
                if (Player.statMana < Player.statManaMax2) {
                    Player.statMana++;
                    if (nebulaDamageStored >= NebulaTinkererHammer.DAMAGE_PER_MANA * 2 && nebulaTicksWithoutIndicator < 4) {
                        nebulaTicksWithoutIndicator++;
                    } else {
                        Player.ManaEffect(nebulaTicksWithoutIndicator + 1);
                        nebulaTicksWithoutIndicator = 0;
                    }
                } else if (nebulaTicksWithoutIndicator > 0) {
                    Player.ManaEffect(nebulaTicksWithoutIndicator);
                    nebulaTicksWithoutIndicator = 0;
                }
                // The built up damage goes to waste if you are at max mana!
                nebulaDamageStored -= NebulaTinkererHammer.DAMAGE_PER_MANA;
            }

            if (stardustDuration > 0) {
                stardustDuration--;
            }
        }

        public void StoreNebulaDamage(int dmg) {
            nebulaDamageStored = Math.Min(nebulaDamageStored + dmg, 4000);
        }

        public static MyReforgeHammerPlayer2 Get(Player player) {
            return player.GetModPlayer<MyReforgeHammerPlayer2>();
        }

    }
}
