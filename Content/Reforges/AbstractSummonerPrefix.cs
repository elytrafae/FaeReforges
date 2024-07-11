using FaeReforges.Systems;
using FaeReforges.Systems.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges {
    public abstract class AbstractSummonerPrefix : ModPrefix {

        public override bool IsLoadingEnabled(Mod mod) {
            return ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges;
        }

        public virtual float MinionSpeedBuff => 0f;
        public virtual float MinionOccupancyReduction => 0f;
        public virtual int MinionCritChance => 0;

        public virtual float DamageBoost => 0f;
        public virtual float KnockbackBoost => 0f;

        public abstract bool IsPositive { get; }

        public override PrefixCategory Category => PrefixCategory.Custom;

        public override bool CanRoll(Item item) {
            return ItemID.Sets.StaffMinionSlotsRequired[item.type] > 0;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {
            damageMult = 1f + DamageBoost;
            knockbackMult = 1f + KnockbackBoost;
        }

        public override void Apply(Item item) {
            if (item.TryGetGlobalItem<SummonerReforgesGlobalItem>(out SummonerReforgesGlobalItem globItem)) {
                globItem.minionOccupancyMult = 1f - this.MinionOccupancyReduction;
                globItem.minionSpeedMult = 1f + this.MinionSpeedBuff;
                globItem.minionCritBonus = this.MinionCritChance;
            }
        }

        public override bool AllStatChangesHaveEffectOn(Item item) {
            return item.TryGetGlobalItem(out SummonerReforgesGlobalItem _);
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            /*
            if (MinionOccupancyReduction > 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyReductionTooltip.Format(MinionOccupancyReduction * 100)) {
                    IsModifier = true, // Sets the color to the positive modifier color.
                    IsModifierBad = false
                };
            } else if (MinionOccupancyReduction < 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyIncreaseTooltip.Format(-MinionOccupancyReduction * 100)) {
                    IsModifier = true, // Sets the color to the positive modifier color.
                    IsModifierBad = true
                };
            }
            */

            if (MinionOccupancyReduction != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyTooltip.Format(-MinionOccupancyReduction * 100)) {
                    IsModifier = true,
                    IsModifierBad = MinionOccupancyReduction < 0
                };
            }


            if (MinionSpeedBuff != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerSpeed", SummonSpeedBuffTooltip.Format(MinionSpeedBuff * 100)) {
                    IsModifier = true,
                    IsModifierBad = MinionSpeedBuff < 0
                };
            }

            if (MinionCritChance != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerCrit", SummonCritChanceTooltip.Format(MinionCritChance)) {
                    IsModifier = true,
                    IsModifierBad = MinionCritChance < 0
                };
            }
            // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
            /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
        }

        public override void ModifyValue(ref float valueMult) {
            valueMult = IsPositive ? 1.5f : 0.5f;
        }

        public static LocalizedText SummonOccupancyTooltip { get; private set; }
        //public static LocalizedText SummonOccupancyIncreaseTooltip { get; private set; }
        public static LocalizedText SummonSpeedBuffTooltip { get; private set; }
        public static LocalizedText SummonCritChanceTooltip { get; private set; }

        public override void SetStaticDefaults() {
            SummonOccupancyTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonOccupancyTooltip)}");
            //SummonOccupancyIncreaseTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonOccupancyIncreaseTooltip)}");
            SummonSpeedBuffTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonSpeedBuffTooltip)}");
            SummonCritChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonCritChanceTooltip)}");
        }
    }
}
