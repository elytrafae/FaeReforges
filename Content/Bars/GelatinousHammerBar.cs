using FaeLibrary.API.Bars;
using FaeLibrary.Implementation.Cooldowns;
using FaeReforges.Content.Cooldowns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Content.Bars {
    internal class GelatinousHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<GelatinousHammerCooldown>();
        public override Color BarColor => Color.DeepPink;

    }
}
