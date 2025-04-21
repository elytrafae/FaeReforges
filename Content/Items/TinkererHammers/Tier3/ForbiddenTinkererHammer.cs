using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class ForbiddenTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers, DamageClass dmgClass) {
            hitModifiers.SourceDamage *= ForbiddenHammerDamageMultiplier(attacker);
        }

        public override void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers, DamageClass dmgClass) {
            hurtModifiers.SourceDamage *= ForbiddenHammerDamageMultiplier(attacker);
        }

        private static float ForbiddenHammerDamageMultiplier(Player player) {
            if (player.statLifeMax2 <= 0) {
                return 1f;
            }
            return 1f + (1f - (((float)player.statMana) / player.statManaMax2)) * 0.15f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                .AddIngredient(ItemID.FossilOre, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
