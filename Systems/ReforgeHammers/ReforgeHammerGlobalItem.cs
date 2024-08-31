using FaeReforges.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammers {

    // Yes, this is for the hammers themselves
    internal class ReforgeHammerGlobalItem : GlobalItem {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(item.type);
            if (hammerType == null) {
                return;
            }

            int consumableIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Consumable");
            int materialIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Material");
            int nameIndex = tooltips.FindIndex(tooltip => tooltip.Name == "ItemName");
            int tooltipIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Tooltip0");
            int insertIndex = Math.Max(consumableIndex, Math.Max(materialIndex, nameIndex)) + 1;
            if (insertIndex == 0) {
                if (tooltipIndex != -1) {
                    insertIndex = tooltipIndex - 1;
                } else {
                    insertIndex = tooltips.Count;
                }
            }

            TooltipLineHelper("TinkererHammerCost", ReforgeHammerLocalization.CostTooltip.Format(hammerType.reforgeCost), ref tooltips, ref insertIndex);
            TooltipLineHelper("TinkererHammerNegativeChance", ReforgeHammerLocalization.NegativeReforgeChanceTooltip.Format(hammerType.negativeReforgeChance), ref tooltips, ref insertIndex);
            if (hammerType.WeaponEffect.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerWeaponEffect", ReforgeHammerLocalization.WeaponEffectPrefix.Format(hammerType.WeaponEffect), ref tooltips, ref insertIndex);
            }
            if (hammerType.AccessoryEffect.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerAccessoryEffect", ReforgeHammerLocalization.AccessoryEffectPrefix.Format(hammerType.AccessoryEffect), ref tooltips, ref insertIndex);
            }
            TooltipLineHelper("TinkererHammerTutorial", ReforgeHammerLocalization.TutorialTooltip.Value, ref tooltips, ref insertIndex);
        }

        private void TooltipLineHelper(string name, string text, ref List<TooltipLine> tooltips, ref int index) {
            tooltips.Insert(index, new TooltipLine(Mod, name, text));
            index++;
        }

    }
}
