using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.InfoDisplays {
    public class MinionInfoDisplay : InfoDisplay {

        public override string HoverTexture => Texture + "_Hover";

        public LocalizedText DisplayText => this.GetLocalization(nameof(DisplayText));

        public override void SetStaticDefaults() {
            _ = DisplayText;
        }

        public override bool Active() {
            return true;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor) {
            double minionCount = 0;
            foreach (var proj in Main.ActiveProjectiles) {
                if (proj.minion && proj.owner == Main.myPlayer) {
                    minionCount += proj.minionSlots;
                }
            }
            return DisplayText.Format(minionCount, Main.player[Main.myPlayer].maxMinions);
            //return String.Format("{0:F2}", minionCount) + "/" + Main.player[Main.myPlayer].maxMinions + " Minions";
        }

    }
}
