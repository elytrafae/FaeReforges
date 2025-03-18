using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class MeteoriteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 30);
        public override int HammerTier => 2;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            damage += 0.07f;
        }

        public override void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) {
            hitModifiers.DamageVariationScale *= 3;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
