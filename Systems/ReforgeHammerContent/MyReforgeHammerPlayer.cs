using FaeLibrary.Implementation;
using FaeReforges.Content.Buffs;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {
    public class MyReforgeHammerPlayer : ModPlayer {

        public int dodgeChanceThousandth = 0; // 10 = 1% Dodge Chance
        public bool accessoryReforgedWithSolar = false;
        public int crimtaneAccessoryCount = 0;
        public int naniteRegenCount = 0;
        public int commonPositiveRegen = 0;

        private const short VORTEX_CRIT_COOLDOWN = 600; // 10 seconds
        private Dictionary<int, short> vortexCrits = new Dictionary<int, short>();

        private const int NEBULA_BOOSTER_COOLDOWN = 30;// 0.5 seconds
        private int timeSinceLastNebulaBooster = NEBULA_BOOSTER_COOLDOWN;

        // Currently unused content-wise. Will keep implementation around for later, just in case //
        public const ushort stardustHammerTimePerAccessory = 120; // 2 seconds
        public ushort stardustHammerAccessoryCount = 0;
        public ushort stardustTimeLeft = 0;
        public ushort lastStardustTime = 0;
        ////////////////////////////////////////////////////////////////

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)FaeReforges.MessageType.MyReforgeHammerPlayerSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write(stardustTimeLeft);
            packet.Write(lastStardustTime);
            packet.Send(toWho, fromWho);
        }

        // Called in FaeReforges.Networking.cs
        public void ReceivePlayerSync(BinaryReader reader) {
            stardustTimeLeft = reader.ReadUInt16();
            lastStardustTime = reader.ReadUInt16();
        }

        public override void CopyClientState(ModPlayer targetCopy) {
            MyReforgeHammerPlayer clone = (MyReforgeHammerPlayer)targetCopy;
            clone.stardustTimeLeft = stardustTimeLeft;
            clone.lastStardustTime = lastStardustTime;
        }

        public override void SendClientChanges(ModPlayer clientPlayer) {
            MyReforgeHammerPlayer clone = (MyReforgeHammerPlayer)clientPlayer;
            if (stardustTimeLeft != clone.stardustTimeLeft || lastStardustTime != clone.lastStardustTime)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }

        public override void ResetEffects() {
            dodgeChanceThousandth = 0;
            accessoryReforgedWithSolar = false;
            crimtaneAccessoryCount = 0;
            stardustHammerAccessoryCount = 0;
            naniteRegenCount = 0;
            commonPositiveRegen = 0;
        }

        public override bool FreeDodge(Player.HurtInfo info) {
            bool dodge = Main.rand.Next(1000) < dodgeChanceThousandth;
            if (dodge) {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 120 : 80);
            }
            return dodge;
        }

        public override void UpdateLifeRegen() {
            if (!ModContent.GetInstance<CrimsonHammerCooldown>().IsCoolingDown()) { // If it has been 15 seconds of not taking damage . . .
                Player.lifeRegen += crimtaneAccessoryCount;
            }
            if (Player.statLife * 4 >= Player.statLifeMax2 * 3) { // If the player is at or above 75% HP
                Player.lifeRegen += naniteRegenCount;
            }
            Player.lifeRegen += commonPositiveRegen;
        }

        public override void PostUpdateEquips() {
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

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
            if (npc.HasBuff<PartyFever>()) {
                modifiers.SourceDamage *= PartyFever.OUTGOING_DAMAGE_MULTIPLIER;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
            if (proj.TryGetSourceNPC(out NPC npc)) {
                if (npc.HasBuff<PartyFever>()) {
                    modifiers.SourceDamage *= PartyFever.OUTGOING_DAMAGE_MULTIPLIER;
                }
            }
        }

        public override void OnHurt(Player.HurtInfo info) {
            ModContent.GetInstance<CrimsonHammerCooldown>().CompletelyResetCooldown();
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
                    stardustTimeLeft = (ushort)(stardustHammerAccessoryCount * stardustHammerTimePerAccessory); // 2 seconds per accessory
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
                CustomReason = NetworkText.FromKey("DeathReasons.StardustMinionTimeExpired", playerName);
            }
        }

    }
}
