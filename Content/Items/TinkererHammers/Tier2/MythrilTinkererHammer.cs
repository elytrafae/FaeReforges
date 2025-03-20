using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class MythrilTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 44);
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;
        public override void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) {
            victim.GetLifeStats(out int HP, out int maxHP);
            float healthRatio = ((float)HP)/maxHP;
            hitModifiers.FinalDamage *= 1f + healthRatio * 0.2f; // 20% more damage based on missing health 
        }

        public override void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) {
            float healthRatio = ((float)victim.statLife)/victim.statLifeMax2;
            hurtModifiers.FinalDamage *= 1f + healthRatio * 0.2f; // 20% more damage based on missing health 
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
