using FaeReforges.Content.Buffs;
using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {
    public class MyReforgeHammerPlayer : ModPlayer {

        public int dodgeChanceThousandth = 0; // 10 = 1% Dodge Chance
        public int flightTimeThousandth = 1000; // 10 = 1% Flight Time
        public bool accessoryReforgedWithSolar = false;

        private const int VORTEX_CRIT_COOLDOWN = 600; // 10 seconds
        private Dictionary<int, int> vortexCrits = new Dictionary<int, int>();

        private const int NEBULA_BOOSTER_COOLDOWN = 30;// 0.5 seconds
        private int timeSinceLastNebulaBooster = NEBULA_BOOSTER_COOLDOWN;

        public int stardustHammerAccessoryCount = 0;
        public int stardustTimeLeft = 0;
        public int lastStardustTime = 0;

        public override void ResetEffects() {
            dodgeChanceThousandth = 0;
            flightTimeThousandth = 1000;
            accessoryReforgedWithSolar = false;
            stardustHammerAccessoryCount = 0;
        }

        public override bool FreeDodge(Player.HurtInfo info) {
            bool dodge = Main.rand.Next(1000) < dodgeChanceThousandth;
            if (dodge) {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 120 : 80);
            }
            return dodge;
        }

        // TODO: Make the hook for this.
        // IL edit Hurt(PlayerDeathReason damageSource, int Damage, int hitDirection, out Player.HurtInfo info, bool pvp = false, bool quiet = false, int cooldownCounter = -1, bool dodgeable = true, float armorPenetration = 0f, float scalingArmorPenetration = 0f, float knockback = 4.5f)
        public void OnDodge() {
            if (ReforgeHammerUtility.GetHammerItemType(Player.HeldItem) == ModContent.ItemType<HallowedTinkererHammer>()) {
                Player.AddBuff(ModContent.BuffType<HallowedAggression>(), 600, false);
            }
        }

        public override void UpdateLifeRegen() {
            if (Player.HasBuff<ChlorophyteRejuvenation>()) {
                Player.lifeRegen += 2; // 1 HP per second
            }
        }

        public override void UpdateBadLifeRegen() {
            int venomHammerType = ModContent.ItemType<VenomiteTinkererHammer>();
            if (ReforgeHammerUtility.GetHammerItemType(Player.HeldItem) == venomHammerType || ReforgeHammerUtility.HasAnySummonHammer(Player, venomHammerType)) {
                Player.lifeRegen -= 4;
            }
        }

        public override void PostUpdateEquips() {
            Player.wingTimeMax = Player.wingTimeMax * flightTimeThousandth / 1000;
            if (stardustTimeLeft > 0) {
                Player.aggro = -999999999;
            }
        }

        public override void PostUpdate() {
            foreach (int key in vortexCrits.Keys) {
                vortexCrits[key] -= 1;
                if (vortexCrits[key] <= 0) { 
                    vortexCrits.Remove(key);
                }
            }
            timeSinceLastNebulaBooster++;
            if (stardustTimeLeft > 0) {
                stardustTimeLeft--;
                PlayStardustDyingWarning();
                if (!IsAnyNonStardustDyingPlayerAlive()) {
                    stardustTimeLeft = 0;
                }
                if (stardustTimeLeft <= 0) {
                    KillMeStardust();
                }
            }
        }

        public bool TriggerVortexCrit(int itemType) {
            if (vortexCrits.ContainsKey(itemType)) {
                return false;
            }
            vortexCrits[itemType] = VORTEX_CRIT_COOLDOWN;
            return true;
        }

        public bool TriggerNebulaBooster() {
            if (timeSinceLastNebulaBooster >= NEBULA_BOOSTER_COOLDOWN && Main.rand.NextBool(3)) {
                timeSinceLastNebulaBooster = 0;
                return true;
            }
            return false;
        }

        public override void OnHurt(Player.HurtInfo info) {
            if (info.Damage > 5) {
                Player.AddBuff(ModContent.BuffType<SolarEclipse>(), 300, false);
            }
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
            return Player.whoAmI == Main.myPlayer && !(damageSource is StardustInstakillDeathReason) && stardustTimeLeft > 0;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
            if (damageSource is StardustInstakillDeathReason) {
                return true;
            }
            if (stardustHammerAccessoryCount > 0 || stardustTimeLeft > 0) {
                if (stardustTimeLeft <= 0) {
                    stardustTimeLeft = stardustHammerAccessoryCount * 120; // 2 seconds per accessory
                    lastStardustTime = stardustTimeLeft;
                    if (!IsAnyNonStardustDyingPlayerAlive()) {
                        stardustTimeLeft = 0;
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
            Player.respawnTimer -= lastStardustTime;
        }

        private bool IsAnyNonStardustDyingPlayerAlive() {
            return IsAnyOtherPlayerAlive((player) => { return player.GetModPlayer<MyReforgeHammerPlayer>().stardustTimeLeft <= 0; });
        }

        private bool IsAnyOtherPlayerAlive(Func<Player, bool> predicate = null) {
            foreach (Player player in Main.ActivePlayers) {
                if (player.respawnTimer <= 0 && (predicate == null || predicate(player))) {
                    return true;
                }
            }
            return false;
        }

        private void KillMeStardust() {
            Player.KillMe(new StardustInstakillDeathReason(Player.name, Mod), 9999999, 0);
        }

        private void PlayStardustDyingWarning() {
            if (lastStardustTime < 300 || // If the total time is less than 5 seconds, abort
                stardustTimeLeft > 300 || // If we are not on the last 5 seconds, abort
                stardustTimeLeft <= 0  || // Don't make the noise on 0!
                Player.whoAmI != Main.myPlayer) { // If this is not the main player, abort!
                return;
            }
            if (stardustTimeLeft % 60 == 0) {
                SoundEngine.PlaySound(MySoundStyles.TouhouWarningDeep);
            }
        }

        public class StardustInstakillDeathReason : PlayerDeathReason {
            public StardustInstakillDeathReason(string playerName, Mod mod) {
                SourceCustomReason = mod.GetLocalization("DeathReasons.StardustMinionTimeExpired").Format(playerName);
            }
        }

    }
}
