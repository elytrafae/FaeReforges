using FaeLibrary.API.ItemConditions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using FaeReforges.Content.Buffs;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class PartyTinkererHammer : SimpleTinkererHammerItem {

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;
        public override int HammerTier => 4;

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone, DamageClass dmgClass) {
            victim.AddBuff(ModContent.BuffType<PartyFever>(), 90);
            for (int i = 0; i < 8; i++) {
                PartyFever.SpawnConfetti(victim, attacker.GetSource_OnHit(victim));
            }
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo, DamageClass dmgClass) {
            victim.AddBuff(ModContent.BuffType<PartyFever>(), 90);
            for (int i = 0; i < 8; i++) {
                PartyFever.SpawnConfetti(victim, attacker.GetSource_OnHit(victim));
            }
        }

    }
}
