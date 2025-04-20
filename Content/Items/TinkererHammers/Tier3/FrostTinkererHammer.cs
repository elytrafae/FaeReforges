using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class FrostTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Pink;
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) {
            if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                hitModifiers.SourceDamage += 0.1f;
            }
        }

        public override void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) {
            if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                hurtModifiers.SourceDamage += 0.1f;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.FrostCore, 1)
                .AddIngredient(ItemID.IceBlock, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
