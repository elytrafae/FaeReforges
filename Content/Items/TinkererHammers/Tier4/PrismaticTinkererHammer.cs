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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class PrismaticTinkererHammer : SimpleTinkererHammerItem {

        public const float POWER = 0.03f;

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override int HammerTier => 4;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(POWER * 100);
        }

        public override void SetHammerDefaults() {
            Item.color = Color.Red;
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (player.wingTime < player.wingTimeMax || player.rocketTime < player.rocketTimeMax) {
                player.moveSpeed += POWER; // This variable also affects acceleration. Don't ask.
            }
        }

        // TODO: Add effect

    }
}
