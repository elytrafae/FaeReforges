using FaeReforges.Systems.VanillaReforges;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using FaeReforges.Systems;
using FaeLibrary.API.ClassExtensions;

namespace FaeReforges.Content.Reforges.Accessory {
    public class RangedAccessoryReforge : ModPrefix {

        public class ReforgeLoader : ILoadable {
            public void Load(Mod mod) {
                Add(mod, "Economic", 4);
                Add(mod, "CostEffective", 3);
                Add(mod, "Solvent", 2);
                Add(mod, "Fruitful", 1);
            }

            private void Add(Mod mod, string name, int tier) {
                RangedAccessoryReforge reforge = new(name, tier);
                mod.AddContent(reforge);
            }

            public void Unload() {
            }
        }

        readonly int power;
        readonly string name;

        public override string Name => name;
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public RangedAccessoryReforge(string name, int power) {
            this.name = name;
            this.power = power;
        }

        public override void ApplyAccessoryEffects(Player player) {
            VanillaReforgePlayer modPlayer = player.GetModPlayer<VanillaReforgePlayer>();
            modPlayer.ammoSavePoints += power;
            player.GetRangedVelocity() += power / 100f;
        }
        public override void ModifyValue(ref float valueMult) {
            valueMult *= ReforgeTierSystem.GetValueMult(power);
        }


        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            yield return new TooltipLine(Mod, "PrefixShootVelocity", ShootVelocityTooltip.Format(power)) {
                IsModifier = true,
                IsModifierBad = power < 0
            };
            yield return new TooltipLine(Mod, "PrefixAmmoSave", AmmoSaveTooltip.Format(power)) {
                IsModifier = true,
                IsModifierBad = power < 0
            };
        }

        public static LocalizedText ShootVelocityTooltip { get; private set; }
        public static LocalizedText AmmoSaveTooltip { get; private set; }

        public override void SetStaticDefaults() {
            ShootVelocityTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ShootVelocityTooltip)}");
            AmmoSaveTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AmmoSaveTooltip)}");
            CustomIDSets.PrefixTiers[Type] = power;
        }

    }
}
