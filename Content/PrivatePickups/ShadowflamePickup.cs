using FaeReforges.Systems;
using FaeReforges.Systems.PrivatePickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using FaeReforges.Systems.ReforgeHammers;
using Terraria.ModLoader;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Content.Buffs;

namespace FaeReforges.Content.PrivatePickups {
    internal class ShadowflamePickup : SimpleTinkererHammerPrivatePickup {

        public override void SimpleSetDefaults() {
            texture = MiscSpritesSystem.ShadowflamePickup;
            reactToPlayer = true;
            frame = new Rectangle(0, 0, 64, 64);
            dustType = DustID.Shadowflame;
            killSound = SoundID.DD2_FlameburstTowerShot.WithPitchOffset(-2f).WithVolumeScale(0.7f);
            hammerType = ModContent.ItemType<SquireTinkererHammer>();
        }

        public override bool SimpleOnCollide(Player player) {
            player.AddBuff(ModContent.BuffType<TempShadowflameFlask>(), 10 * 60, false);
            return true;
        }

        public override void SimpleUpdate() {
            if (timer % 10 == 0) {
                Rectangle f = frame.Value;
                f.X += 64;
                if (f.X >= 256) {
                    f.X = 0;
                }
                frame = f;
            }
        }

    }
}
