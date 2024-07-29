using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges.Accessory {
    public class ExtraCritChanceAccessoryReforge : ModPrefix {

        public class ReforgeLoader : ILoadable {
            public void Load(Mod mod) {
                mod.AddContent(new ExtraCritChanceAccessoryReforge("Fortunate", 3));
                mod.AddContent(new ExtraCritChanceAccessoryReforge("Accurate", 1));
            }

            public void Unload() {
            }
        }

        readonly int power;
        readonly string name;

        public override string Name => name;
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public ExtraCritChanceAccessoryReforge(string name, int power) {
            this.name = name;
            this.power = power;
        }

        public override void ApplyAccessoryEffects(Player player) {
            player.GetCritChance(DamageClass.Generic) += power;
        }

        public override void ModifyValue(ref float valueMult) {
            valueMult *= VanillaReforgePlayer.ACCESSORY_VALUES_PER_POWER[power];
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            yield return new TooltipLine(Mod, "PrefixMaxManaRegen", Tooltip.Format(power)) {
                IsModifier = true,
                IsModifierBad = power < 0
            };
        }

        public static LocalizedText Tooltip { get; private set; }

        public override void SetStaticDefaults() {
            Tooltip = Language.GetText("CommonItemTooltip.PercentIncreasedCritChance");
        }

    }
}
