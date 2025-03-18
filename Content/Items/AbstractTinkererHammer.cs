using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items {
    public abstract class AbstractTinkererHammer : ModItem {

        public abstract int HammerTier { get; }
        public virtual ItemCondition ReforgeableCondition => ItemCondition.Any;
        public virtual LocalizedText WeaponEffectText => this.GetLocalization(nameof(WeaponEffectText), () => "");
        public virtual LocalizedText AccessoryEffectText => this.GetLocalization(nameof(AccessoryEffectText), () => "");

        public override void SetStaticDefaults() {
            _ = WeaponEffectText;
            _ = AccessoryEffectText;
        }

        // Overridable methods
        public virtual void HammerOnApplyWeapon(Item item) { }
        public virtual void HammerOnApplyAccessory(Item item) { }
        public virtual void HammerOnUpdateWeaponHeld(Item item, Player player) { }
        public virtual void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) { }
        public virtual void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) { }
        public virtual void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) { }
        public virtual void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) { }
        public virtual void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) { }
        public virtual bool HammerCanUseItem(Item item, Player attacker) { return true; }
        public virtual void HammerOnCreateProjectile(int item, Projectile projectile, IEntitySource source) { }

        public virtual void HammerModifyWeaponDamage(Item item, Player player, ref StatModifier damage) { }
        public virtual void HammerModifyWeaponCrit(Item item, Player player, ref float crit) { }
        public virtual void HammerModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) { }
        public virtual void HammerModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
        public virtual void HammerModifyManaCost(Item item, Player player, ref float reduce, ref float mult) { }
        public virtual void HammerModifyItemScale(Item item, Player player, ref float scale) { }
        public virtual float HammerUseSpeedMultiplier(Item item, Player player) { return 1f; }
        public virtual void HammerEnchantmentVisuals(Player player, int itemID, Vector2 position, int height, int width) { }
        public virtual bool? HammerUseItem(Item item, Player player) { return null; }
        public virtual void HammerWhileUsingWeapon(Item item, Player player) { }

        /*
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            int consumableIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Consumable");
            int materialIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Material");
            int nameIndex = tooltips.FindIndex(tooltip => tooltip.Name == "ItemName");
            int tooltipIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Tooltip0");
            int insertIndex = Math.Max(consumableIndex, Math.Max(materialIndex, nameIndex)) + 1;
            if (insertIndex == 0) {
                if (tooltipIndex != -1) {
                    insertIndex = tooltipIndex - 1;
                } else {
                    insertIndex = tooltips.Count;
                }
            }

            //TooltipLineHelper("TinkererHammerCost", ReforgeHammerLocalization.CostTooltip.Format(hammerType.reforgeCost), ref tooltips, ref insertIndex);
            //TooltipLineHelper("TinkererHammerNegativeChance", ReforgeHammerLocalization.NegativeReforgeChanceTooltip.Format(hammerType.negativeReforgeChance), ref tooltips, ref insertIndex);
            TooltipLineHelper("TinkererHammerTier", ReforgeHammerLocalization.HammerTier.Format(HammerTier), ref tooltips, ref insertIndex);
            TooltipLineHelper("TinkererHammerFilter", ReforgeHammerLocalization.HammerFilter.Format(ReforgeableCondition.Text), ref tooltips, ref insertIndex);
            if (WeaponEffectText.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerWeaponEffect", ReforgeHammerLocalization.WeaponEffectPrefix.Format(WeaponEffectText), ref tooltips, ref insertIndex);
            }
            if (AccessoryEffectText.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerAccessoryEffect", ReforgeHammerLocalization.AccessoryEffectPrefix.Format(AccessoryEffectText), ref tooltips, ref insertIndex);
            }
            TooltipLineHelper("TinkererHammerTutorial", ReforgeHammerLocalization.TutorialTooltip.Value, ref tooltips, ref insertIndex);
        }

        private void TooltipLineHelper(string name, string text, ref List<TooltipLine> tooltips, ref int index) {
            tooltips.Insert(index, new TooltipLine(Mod, name, text));
            index++;
        }
        */

        public const string WEAPON_ABILITY_TOOLTIP = "TinkererHammerWeaponEffect";
        public const string ACCESSORY_ABILITY_TOOLTIP = "TinkererHammerAccessoryEffect";

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerTier", ReforgeHammerLocalization.HammerTier.Format(HammerTier)));
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerFilter", ReforgeHammerLocalization.HammerFilter.Format(ReforgeableCondition.Text)));

            if (WeaponEffectText.Value.Length > 0) {
                ReforgeHammerUtility.ProcessAbilityLines(WeaponEffectText.Value, tooltips, WEAPON_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.WeaponEffectPrefix);
            }
            if (AccessoryEffectText.Value.Length > 0) {
                ReforgeHammerUtility.ProcessAbilityLines(AccessoryEffectText.Value, tooltips, ACCESSORY_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.AccessoryEffectPrefix);
            }
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerTutorial", ReforgeHammerLocalization.TutorialTooltip.Value));
        }

        public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines) {
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(148, 0, 255), WEAPON_ABILITY_TOOLTIP);
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(255, 0, 165), ACCESSORY_ABILITY_TOOLTIP);
        }

    }
}
