using FaeReforges.Content.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content
{
    public abstract class AbstractHammerType : ModTexturedType, ILocalizedModType, ICloneable {

        public string LocalizationCategory => "TinkererHammerType";
        public LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName));
        public LocalizedText Description => this.GetLocalization(nameof(Description));

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

        public override sealed void SetupContent() {
            //Console.WriteLine("SetupContent " + this.Mod + " || " + this.LocalizationCategory + " || " + this.Name);
            this.SetStaticDefaults();
            _ = DisplayName;
            _ = Description;
        }

        public abstract void SetDefaults();

    }
}
