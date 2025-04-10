using FaeLibrary.Implementation.Cooldowns;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using FaeLibrary.API.Bars;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Bars {
    internal class FrightHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<FrightHammerCooldown>();
        public override Color BackgroundColor => new Color(255, 211, 178);
        public override Color BarColor => new Color(209, 47, 11);
        public override float Fullness => 1f - base.Fullness;
        public override bool Visible => base.Visible && MyReforgeHammerPlayer2.Get(Main.LocalPlayer).hammerOfFrightCount > 0;

    }
}
