using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.PrivatePickups;
using FaeReforges.Systems.PrivatePickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    internal class ApprenticeTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 3;
        public override int? CustomPrice => 10;
        public override int CustomCurrency => CustomCurrencyID.DefenderMedals;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsMagicWeapon;

        int timer = 0; // No need to sync this LMAO :3:3:3
        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            if (player.whoAmI != Main.myPlayer) {
                return;
            }
            timer++;
            if (timer >= 120) {
                timer = 0;
                ReforgeHammerUtility.BasicPrivatePickupSpawnPositionCode(player, out int x, out int y);
                PrivatePickupManager.Spawn<ManaStarPickup>(x, y);
                SoundEngine.PlaySound(SoundID.Item29.WithVolumeScale(0.6f));
            }
        }

    }
}
