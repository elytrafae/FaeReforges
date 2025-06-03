using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API.ItemConditions;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class FishyTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count == 2) {
                player.wet = true;
                player.wetCount = 100;
                player.ignoreWater = true; // NOTE: If this line is removed, the player's movement is still not slowed outside of actual water!
                player.noFallDmg = true; // Have to add this as a workaround
            }
        }
    }
}
