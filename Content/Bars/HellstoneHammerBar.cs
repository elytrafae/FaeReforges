using FaeLibrary.API.Bars;
using FaeLibrary.API.Cooldown;
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
    internal class HellstoneHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<HellstoneHammerCooldown>();

        public override bool Visible => ReforgeHammerUtility.ShouldCooldownBarDisplay<HellstoneTinkererHammer>();

        public override Color BackgroundColor => new(0.7f, 0.4f, 0f);
        public override Color BarColor => new(1f, 0.7f, 0f);

    }
}
