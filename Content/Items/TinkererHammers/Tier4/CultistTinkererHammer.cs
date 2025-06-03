using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FaeLibrary.Implementation;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class CultistTinkererHammer : SimpleTinkererHammerItem {

        public const int FLIGHT_TIME_TICKS = 30;

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override int HammerTier => 4;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(FLIGHT_TIME_TICKS/60f);
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetWingTimeStat().Flat += FLIGHT_TIME_TICKS;
        }

    }
}
