using FaeReforges.Content.Buffs;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Systems.PrivatePickups;
using FaeReforges.Systems.ReforgeHammers;
using FaeReforges.Systems;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace FaeReforges.Content.PrivatePickups {
    internal class ManaStarPickup : PrivatePickup {

        int timer = 0;

        public override void SetDefaults() {
            width = 64;
            height = 64;
            texture = MiscSpritesSystem.ManaStarPickup;
            reactToPlayer = true;
            timer = 0;
        }

        public override void Update() {
            timer++;
            if (timer % 3 == 0) {
                Dust.NewDust(position, width, height, DustID.YellowStarDust, 0, 0, 0, Color.AliceBlue);
            }
            if (timer > 400) {
                Kill();
            }
            if (ReforgeHammerUtility.GetHammerItemType(Main.LocalPlayer.HeldItem) != ModContent.ItemType<ApprenticeTinkererHammer>()) {
                Kill();
            }
        }

        public override bool OnCollidePlayer(Player player) {
            int manaToGive = Math.Min(player.statManaMax2 / 5, player.statManaMax2 - player.statMana);
            player.statMana += manaToGive;
            player.ManaEffect(manaToGive);
            SoundEngine.PlaySound(SoundID.Shatter);
            return true;
        }

    }
}
