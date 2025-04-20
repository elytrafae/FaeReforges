using FaeLibrary.API.ItemConditions;
using System;
using Terraria.ID;
using Terraria;
using FaeReforges.Systems.PrivatePickups;
using FaeReforges.Content.PrivatePickups;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    internal class SquireTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 3;
        public override int? CustomPrice => 10;
        public override int CustomCurrency => CustomCurrencyID.DefenderMedals;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsMeleeWeapon;

        int timer = 0; // No need to sync this LMAO :3:3:3
        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            if (player.whoAmI != Main.myPlayer) {
                return;
            }
            timer++;
            if (timer >= 120) {
                timer = 0;
                ReforgeHammerUtility.BasicPrivatePickupSpawnPositionCode(player, out int x, out int y);
                PrivatePickupManager.Spawn<ShadowflamePickup>(x, y);
                SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot.WithVolumeScale(0.7f));
            }
        }

    }
}
