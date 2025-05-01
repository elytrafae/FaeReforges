using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Content.Buffs;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class StardustTinkererHammer : SimpleTinkererHammerItem {
        public const int BONUS_TAG_DAMAGE = 12;
        public const int BONUS_TAG_CRIT = 6;
        public const float SUMMON_RUSH_SPEED = 5;
        public const int BRIEF_MOMENT_DURATION = 30;
        public override int Rarity => ItemRarityID.Red;
        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsSummonerWeapon, ItemCondition.IsAccessory);
        public override int HammerTier => 4;

        protected virtual LocalizedText WhipWeaponEffect => this.GetLocalization(nameof(WhipWeaponEffect));
        protected virtual LocalizedText SummonWeaponEffect => this.GetLocalization(nameof(SummonWeaponEffect));

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone, DamageClass dmgClass) {
            if (ItemCondition.IsWhipWeapon.IsMet(ContentSamples.ItemsByType[item])) {
                victim.AddBuff(ModContent.BuffType<StardustHammerTagEffect>(), 4 * 60);
            }
        }

        public override string GetWeaponEffectText(Item item) {
            if (item == null) {
                return WeaponEffectText.Format(BONUS_TAG_DAMAGE, BONUS_TAG_CRIT, SUMMON_RUSH_SPEED, ModContent.GetInstance<StardustHammerCooldown>().DisplayCooldownTicks / 60f, ModContent.GetInstance<StardustHammerCooldown>().Charges);
            }
            if (ItemCondition.IsWhipWeapon.IsMet(item)) { 
                return WhipWeaponEffect.Format(BONUS_TAG_DAMAGE, BONUS_TAG_CRIT);
            }
            return SummonWeaponEffect.Format(SUMMON_RUSH_SPEED, ModContent.GetInstance<StardustHammerCooldown>().DisplayCooldownTicks / 60f, ModContent.GetInstance<StardustHammerCooldown>().Charges);
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count % 5 == 0) {
                player.maxMinions++;
                player.maxTurrets++;
            }
        }

        // Hammer effect implemented elsewhere
    }
}
