using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Systems.WhipFrenzy {
    public class WhipFrenzyPlayer : ModPlayer {

        int frenzy = 0;
        int maxFrenzy = 1000;
        bool isFrenzyOn = false;

        public bool IsFrenzyReady() {
            return frenzy >= maxFrenzy && !isFrenzyOn;
        }

        public bool IsFrenzyActive() {
            return isFrenzyOn;
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


        public override void PostUpdateEquips() {
            if (IsFrenzyActive()) {
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.2f;
            }
        }

        public override void PostUpdate() {
            if (IsFrenzyActive()) {
                frenzy -= 5;
                if (frenzy <= 0) {
                    StopFrenzy();
                }
            } else {
                // Passive Whip Frenzy Gain for DEBUG!
                frenzy += 2;

            }
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Frenzy: " + frenzy + "/" + maxFrenzy), Color.Azure);
        }

    }

    public static class PlayerExtension { 
    
        public static WhipFrenzyPlayer WhipFrenzyPlayer(this Player player) { return player.GetModPlayer<WhipFrenzyPlayer>(); }

    }
}
