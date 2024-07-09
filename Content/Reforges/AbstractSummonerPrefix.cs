using FaeReforges.Systems;
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

        public virtual float MinionSpeedBuff => 0f;
        public virtual float MinionOccupancyReduction => 0f;

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override bool CanRoll(Item item) {
            return ItemID.Sets.StaffMinionSlotsRequired[item.type] > 0;
        }

        public override void Apply(Item item) {
            SummonerReforgesGlobalItem globItem = item.GetGlobalItem<SummonerReforgesGlobalItem>();
            globItem.minionOccupancyMult = 1f - this.MinionOccupancyReduction;
            globItem.minionSpeedMult = 1f + this.MinionSpeedBuff;
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            // Due to inheritance, this code runs for ExamplePrefix and ExampleDerivedPrefix. We add 2 tooltip lines, the first is the typical prefix tooltip line showing the stats boost, while the other is just some additional flavor text.

            // The localization key for Mods.ExampleMod.Prefixes.PowerTooltip uses a special format that will automatically prefix + or - to the value.
            // This shared localization is formatted with the Power value, resulting in different text for ExamplePrefix and ExampleDerivedPrefix.
            // This results in "+1 Power" for ExamplePrefix and "+2 Power" for ExampleDerivedPrefix.
            // Power isn't an actual stat, the effects of Power are already shown in the "+X% damage" tooltip, so this example is purely educational.
            yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyReductionTooltip.Format(MinionOccupancyReduction)) {
                IsModifier = true, // Sets the color to the positive modifier color.
            };
            // This localization is not shared with the inherited classes. ExamplePrefix and ExampleDerivedPrefix have their own translations for this line.
            yield return new TooltipLine(Mod, "PrefixSummonerSpeed", SummonSpeedBuffTooltip.Format(MinionSpeedBuff)) {
                IsModifier = true,
            };
            // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
            /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
        }

        public static LocalizedText SummonOccupancyReductionTooltip { get; private set; }
        public static LocalizedText SummonSpeedBuffTooltip { get; private set; }

        public override void SetStaticDefaults() {
            // This seemingly useless code is required to properly register the key for the translation lines
            SummonOccupancyReductionTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonOccupancyReductionTooltip)}");
            SummonSpeedBuffTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonSpeedBuffTooltip)}");
        }
    }
}
