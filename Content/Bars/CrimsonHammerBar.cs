using FaeLibrary.API.Bars;
using FaeLibrary.Implementation.Cooldowns;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Systems.ReforgeHammerContent;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Bars {
    internal class CrimsonHammerBar : FaeSimpleCooldownBar {

        public override IFaeCooldown Cooldown => ModContent.GetInstance<CrimsonHammerCooldown>();
        public override Color BackgroundColor => new Color(0.3f, 0f, 0f);
        public override Color BarColor => new Color(0.8f, 0f, 0f);
        public override bool Visible => Main.LocalPlayer.GetModPlayer<MyReforgeHammerPlayer>().crimtaneAccessoryCount > 0;

    }
}
