using FaeLibrary.API.Bars;
using FaeLibrary.Implementation.Cooldowns;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Content.Items.TinkererHammers.Tier2;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Bars {
    internal class HoneycombHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<HoneycombHammerCooldown>();

        public override bool Visible => ReforgeHammerUtility.ShouldCooldownBarDisplay<HoneycombTinkererHammer>();
        public override Color BackgroundColor => new(0.4f, 0.2f, 0f);
        public override Color BarColor => new(1f, 0.9f, 0f);

    }
}
