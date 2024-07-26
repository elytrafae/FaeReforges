using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Systems.WhipFrenzy {
    public class WhipFrenzyPlayer : ModPlayer {

        double frenzy = 0;
        int maxFrenzy = 1000;
        bool isFrenzyOn = false;
        int timeSinceLastMinionHit = 0;

        public bool IsFrenzyReady() {
            return frenzy >= maxFrenzy && !isFrenzyOn;
        }

        public bool IsFrenzyActive() {
            return isFrenzyOn;
        }

        public float GetBarFill() { 
            return Utils.Clamp((float)frenzy / maxFrenzy, 0f, 1f);
        }

        public void ActivateFrenzy() { 
            if (!this.IsFrenzyReady()) { return; }
            frenzy = maxFrenzy;
            isFrenzyOn = true;
            // Add sound and visual effects
        }

        public static bool IsEligibleWhip(Item item) {
            return item.shoot != ProjectileID.None && ProjectileID.Sets.IsAWhip[item.shoot];
            return (item.DamageType == DamageClass.SummonMeleeSpeed || item.DamageType.GetEffectInheritance(DamageClass.SummonMeleeSpeed)) && item.shoot != ProjectileID.None;
        }

        public void StopFrenzy() {
            isFrenzyOn = false;
            frenzy = 0;
            // Add sound and visual effects
        }

        public override bool CanUseItem(Item item) {
            if (IsFrenzyActive()) {
                return IsEligibleWhip(item);
            }
            return true;
        }

        /*
        public override void PostUpdateEquips() {
            if (IsFrenzyActive()) {
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.2f;
            }
        }
        */

        public override void PostUpdate() {
            if (IsFrenzyActive()) {
                frenzy -= 5;
                if (frenzy <= 0) {
                    StopFrenzy();
                }
            } else {
                timeSinceLastMinionHit++;
                if (timeSinceLastMinionHit >= 120) {
                    frenzy = Math.Max(frenzy - (timeSinceLastMinionHit-120)/60f, 0);
                }
            }
            if (KeybindSystem.WhipFrenzyKeybind.JustPressed && Main.myPlayer == Player.whoAmI) {
                ActivateFrenzy();
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
            if (IsFrenzyActive()) { 
                return; 
            }
            var modProj = proj.GetGlobalProjectile<SummonerReforgesGlobalProjectile>();
            double frenzyDelta = modProj.whipFrenzyChargeMult * Math.Max((double)proj.minionSlots, 0.1) * Math.Max(proj.usesIDStaticNPCImmunity ? proj.idStaticNPCHitCooldown : proj.localNPCHitCooldown, 1) / (Math.Max(proj.extraUpdates + 1, 1) * modProj.bonusSpeed);
            frenzy = Math.Min(frenzyDelta, maxFrenzy);
            if (frenzyDelta > 0) 
                timeSinceLastMinionHit = 0;
        }

    }

    public static class PlayerExtension { 
    
        public static WhipFrenzyPlayer WhipFrenzyPlayer(this Player player) { return player.GetModPlayer<WhipFrenzyPlayer>(); }

    }
}
