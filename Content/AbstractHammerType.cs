using FaeReforges.Content.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content
{
    public abstract class AbstractHammerType : ModTexturedType, ILocalizedModType, ICloneable {

        public string LocalizationCategory => "TinkererHammerType";
        public LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), () => Regex.Replace(this.GetType().Name, "([A-Z])", " $1").Trim());
        public LocalizedText WeaponEffect => this.GetLocalization(nameof(WeaponEffect), () => "");
        public LocalizedText AccessoryEffect => this.GetLocalization(nameof(AccessoryEffect), () => "");


        // TODO: Add functionality to these
        public Color color;
        public Action<Item> onApplyWeapon;
        public Action<Item> onApplyAccessory;
        public Action<Item> onUpdateWeapon;
        public Action<Item> onUpdateAccessory;
        public int negativeReforgeChance = 0;
        public int reforgeCost = 0;

        protected override sealed void Register() {
            ModTypeLookup<AbstractHammerType>.Register(this);
        }

        public override void Load() {
            Mod.AddContent(new TinkererHammer(Mod, this));
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public static LocalizedText CostTooltip { get; private set; }
        public static LocalizedText NegativeReforgeChanceTooltip { get; private set; }
        public static LocalizedText WeaponEffectPrefix { get; private set; }
        public static LocalizedText AccessoryEffectPrefix { get; private set; }
        public static LocalizedText TutorialTooltip { get; private set; }
        public static LocalizedText ReforgedWithTooltip { get; private set; }
        public static LocalizedText ReforgeUnlockMessage { get; private set; }


        public override sealed void SetupContent() {
            //Console.WriteLine("SetupContent " + this.Mod + " || " + this.LocalizationCategory + " || " + this.Name);
            this.SetDefaults();
            this.SetStaticDefaults();
            _ = DisplayName;
            _ = WeaponEffect;
            _ = AccessoryEffect;

            CostTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CostTooltip)}");
            NegativeReforgeChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(NegativeReforgeChanceTooltip)}");
            WeaponEffectPrefix = Mod.GetLocalization($"{LocalizationCategory}.{nameof(WeaponEffectPrefix)}");
            AccessoryEffectPrefix = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AccessoryEffectPrefix)}");
            TutorialTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(TutorialTooltip)}");
            ReforgedWithTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ReforgedWithTooltip)}");
            ReforgeUnlockMessage = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ReforgeUnlockMessage)}");
        }



        public abstract void SetDefaults();

    }
}
