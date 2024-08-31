using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerLocalization : ModSystem {

        public static string LocalizationCategory => "TinkererHammerType";

        public static LocalizedText CostTooltip { get; private set; }
        public static LocalizedText NegativeReforgeChanceTooltip { get; private set; }
        public static LocalizedText WeaponEffectPrefix { get; private set; }
        public static LocalizedText AccessoryEffectPrefix { get; private set; }
        public static LocalizedText TutorialTooltip { get; private set; }
        public static LocalizedText ReforgedWithTooltip { get; private set; }
        public static LocalizedText UIInsertHammer { get; private set; }
        public static LocalizedText UIThatIsNotAHammer { get; private set; }

        public static LocalizedText NeverCondition { get; private set; }

        public override void SetStaticDefaults() {
            CostTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CostTooltip)}");
            NegativeReforgeChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(NegativeReforgeChanceTooltip)}");
            WeaponEffectPrefix = Mod.GetLocalization($"{LocalizationCategory}.{nameof(WeaponEffectPrefix)}");
            AccessoryEffectPrefix = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AccessoryEffectPrefix)}");
            TutorialTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(TutorialTooltip)}");
            ReforgedWithTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ReforgedWithTooltip)}");
            UIInsertHammer = Mod.GetLocalization($"{LocalizationCategory}.{nameof(UIInsertHammer)}");
            UIThatIsNotAHammer = Mod.GetLocalization($"{LocalizationCategory}.{nameof(UIThatIsNotAHammer)}");

            NeverCondition = Mod.GetLocalization($"Conditions.Never");
        }
        


    }
}
