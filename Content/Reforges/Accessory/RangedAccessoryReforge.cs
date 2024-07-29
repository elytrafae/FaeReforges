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
    public class RangedAccessoryReforge : ModPrefix {

        public class ReforgeLoader : ILoadable {
            public void Load(Mod mod) {
                mod.AddContent(new RangedAccessoryReforge("Economic", 4));
                mod.AddContent(new RangedAccessoryReforge("CostEffective", 3));
                mod.AddContent(new RangedAccessoryReforge("Solvent", 2));
                mod.AddContent(new RangedAccessoryReforge("Fruitful", 1));
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
            modPlayer.shootVelocityPoints += power;
            modPlayer.ammoSavePoints += power;
        }
        public override void ModifyValue(ref float valueMult) {
            valueMult *= VanillaReforgePlayer.ACCESSORY_VALUES_PER_POWER[power];
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
        }

    }
}
