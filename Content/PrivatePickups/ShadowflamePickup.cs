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
    internal class ShadowflamePickup : PrivatePickup {

        int timer = 0;

        public override void SetDefaults() {
            width = 64;
            height = 64;
            texture = MiscSpritesSystem.ShadowflamePickup;
            reactToPlayer = true;
            frame = new Rectangle(0, 0, 64, 64);
            timer = 0;
        }

        public override void Update() {
            timer++;
            // Animate the flame!

            if (timer % 10 == 0) {
                Rectangle f = frame.Value;
                f.X += 64;
                if (f.X >= 256) {
                    f.X = 0;
                }
                frame = f;
            }
            if (timer % 3 == 0) {
                Dust.NewDust(position, width, height, DustID.Shadowflame);
            }
            if (timer > 400) {
                Kill();
            }
            if (ReforgeHammerUtility.GetHammerItemType(Main.LocalPlayer.HeldItem) != ModContent.ItemType<SquireTinkererHammer>()) {
                Kill();
            }
        }

        public override bool OnCollidePlayer(Player player) {
            player.AddBuff(ModContent.BuffType<TempShadowflameFlask>(), 10 * 60, false);
            SoundEngine.PlaySound(SoundID.Shatter);
            return true;
        }

    }
}
