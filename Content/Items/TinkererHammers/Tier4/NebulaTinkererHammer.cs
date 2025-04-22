using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class NebulaTinkererHammer : SimpleTinkererHammerItem {

        public const int DAMAGE_PER_MANA = 200; // 100 Damage per 1 Mana = 1% of damage turned into mana
        public const int MANA_COST_REDUCTION_PERCENT = 2;

        public override int Rarity => ItemRarityID.Red;
        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsMagicWeapon, ItemCondition.IsAccessory);
        public override int HammerTier => 4;

        public override LocalizedText AccessoryEffectText => base.AccessoryEffectText.WithFormatArgs(MANA_COST_REDUCTION_PERCENT);
        public override LocalizedText WeaponEffectText => base.WeaponEffectText.WithFormatArgs(100f/DAMAGE_PER_MANA, 60f / ModContent.GetInstance<NebulaHammerCooldown>().DisplayCooldownTicks);

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.manaCost -= (MANA_COST_REDUCTION_PERCENT / 100f);
        }

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone, DamageClass dmgClass) {
            MyReforgeHammerPlayer2.Get(attacker).StoreNebulaDamage(damageDone);
        }

        public override void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo, DamageClass dmgClass) {
            MyReforgeHammerPlayer2.Get(attacker).StoreNebulaDamage(hurtInfo.Damage);
        }

    }
}
