using FaeReforges.Content.Reforges;
using FaeReforges.Content.Reforges.Accessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.VanillaReforges {
    internal class VanillaReforgeGlobalItem : GlobalItem {

        // TODO: Make item value modifiers the same across all reforges
        public override bool AllowPrefix(Item item, int pre) {
            return item.accessory || pre >= PrefixID.Count || DynamicReforgeLoader.vanillaOverrides[pre] != null;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (item.prefix == PrefixID.Arcane) {
                int oldLine = tooltips.FindIndex(l => l.Name == "PrefixAccMaxMana");
                if (oldLine != -1) {
                    TooltipLine line = new TooltipLine(Mod, "PrefixMaxManaRegen", ManaRegenAccessoryReforge.RegenTooltip.Format(3)) {
                        IsModifier = true,
                        IsModifierBad = false
                    };
                    tooltips[oldLine] = line;
                }
            }
        }

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration) {
            //speed *= player.GetModPlayer<player>
        }

    }
}
