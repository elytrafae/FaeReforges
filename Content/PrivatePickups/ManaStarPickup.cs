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
    public class ManaStarPickup : SimpleTinkererHammerPrivatePickup {

        public override void SimpleSetDefaults() {
            texture = MiscSpritesSystem.ManaStarPickup;
            reactToPlayer = true;
            dustType = DustID.YellowStarDust;
            dustColor = Color.Blue;
            killSound = SoundID.Item29.WithPitchOffset(-2f).WithVolumeScale(0.6f);
            hammerType = ModContent.ItemType<ApprenticeTinkererHammer>();
        }

        public override bool SimpleOnCollide(Player player) {
            int manaToGive = Math.Min(player.statManaMax2 / 5, player.statManaMax2 - player.statMana);
            player.statMana += manaToGive;
            player.ManaEffect(manaToGive);
            return true;
        }

    }
}
