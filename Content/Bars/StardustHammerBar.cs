using FaeLibrary.API.Bars;
using FaeLibrary.Implementation.Cooldowns;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Content.Items.TinkererHammers.Tier2;
using FaeReforges.Content.Items.TinkererHammers.Tier4;
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
    internal class StardustHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<StardustHammerCooldown>();

        public override bool Visible => ReforgeHammerUtility.HasAnySummonHammer<StardustTinkererHammer>(Main.LocalPlayer);
        public override Color BackgroundColor => new(2, 77, 189);
        public override Color BarColor => new(8, 244, 252);

    }
}
