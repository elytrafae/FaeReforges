using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using FaeReforges.Systems;
using FaeLibrary.API;
using Terraria.GameContent.Creative;

namespace FaeReforges.Content.Reforges.Accessory {
    public class ExtraCritChanceAccessoryReforge : ModPrefix {

        public class ReforgeLoader : ILoadable {
            public void Load(Mod mod) {
                Add(mod, "Fortunate", 3);
                Add(mod, "Accurate", 1);
            }

            private void Add(Mod mod, string name, int tier) {
                ExtraCritChanceAccessoryReforge reforge = new(name, tier);
                mod.AddContent(reforge);
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
            valueMult *= ReforgeTierSystem.GetValueMult(power);
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            yield return new TooltipLine(Mod, "PrefixMaxManaRegen", CritChanceTooltip.Format(power)) {
                IsModifier = true,
                IsModifierBad = power < 0
            };
        }

        public static LocalizedText CritChanceTooltip { get; private set; }

        public override void SetStaticDefaults() {
            CritChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CritChanceTooltip)}");
            CustomIDSets.PrefixTiers[Type] = power;
        }

    }
}
