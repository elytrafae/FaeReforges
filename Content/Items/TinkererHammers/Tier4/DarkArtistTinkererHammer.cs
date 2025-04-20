using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using FaeLibrary.Implementation;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class DarkArtistTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;
        public override int? CustomPrice => 30;
        public override int CustomCurrency => CustomCurrencyID.DefenderMedals;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetAttackSpeed(DamageClass.Generic) -= 0.015f;
            player.GetSummonSpeed() -= 0.015f;
        }
    }
}
