using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    internal class ShinobiInfiltratorTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;
        public override int? CustomPrice => 30;
        public override int CustomCurrency => CustomCurrencyID.DefenderMedals;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;


        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count == 5) {
                player.AddBuff(ModContent.BuffType<NinjaMinionBuff>(), 2);
            }
        }
    }
}
