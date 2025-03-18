using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class PalladiumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 21);
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) {
            attacker.AddBuff(ModContent.BuffType<PalladiumRejuvenation>(), 120);
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) {
            attacker.AddBuff(ModContent.BuffType<PalladiumRejuvenation>(), 120);
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.PalladiumBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
