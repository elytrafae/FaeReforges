using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API.ItemConditions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    internal class RedRidingTinkererHammer : SimpleTinkererHammerItem {

        public const int ARMOR_PEN = 5;
        public const int CRIT_CHANCE = 1;

        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;
        public override int? CustomPrice => 30;
        public override int CustomCurrency => CustomCurrencyID.DefenderMedals;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(ARMOR_PEN, CRIT_CHANCE);
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetCritChance(DamageClass.Generic) += CRIT_CHANCE;
            player.GetArmorPenetration(DamageClass.Generic) += ARMOR_PEN;
        }
    }
}
