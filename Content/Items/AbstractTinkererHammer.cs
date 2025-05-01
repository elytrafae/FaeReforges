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
        protected virtual LocalizedText WeaponEffectText => this.GetLocalization(nameof(WeaponEffectText), () => "");
        protected virtual LocalizedText AccessoryEffectText => this.GetLocalization(nameof(AccessoryEffectText), () => "");

        /// <summary>
        /// Get the string for this hammer's weapon effect description. Override to modify said string.
        /// </summary>
        /// <param name="item">The reforged item this text will be displayed on. null if there is no item in the context or if the item is the hammer itself.</param>
        /// <returns>The localized string</returns>
        public virtual string GetWeaponEffectText(Item item) {
            return WeaponEffectText.Value;
        }

        /// <summary>
        /// Get the string for this hammer's accessory effect description. Override to modify said string.
        /// </summary>
        /// <param name="item">The reforged item this text will be displayed on. null if there is no item in the context or if the item is the hammer itself.</param>
        /// <returns>The localized string</returns>
        public virtual string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Value;
        }

        public override void SetStaticDefaults() {
            _ = WeaponEffectText;
            _ = AccessoryEffectText;
        }

        // Overridable methods
        public virtual void HammerOnApplyWeapon(Item item) { }
        public virtual void HammerOnApplyAccessory(Item item) { }
        public virtual void HammerOnUpdateWeaponHeld(Item item, Player player) { }
        public virtual void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) { }
        public virtual void HammerChangeWeaponDealDamagePvp(int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers, DamageClass dmgClass) { }
        public virtual void HammerOnWeaponDealDamagePvp(int item, Player attacker, Player victim, Player.HurtInfo hurtInfo, DamageClass dmgClass) { }
        public virtual void HammerChangeWeaponDealDamageNpc(int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers, DamageClass dmgClass) { }
        public virtual void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone, DamageClass dmgClass) { }
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

        public const string WEAPON_ABILITY_TOOLTIP = "TinkererHammerWeaponEffect";
        public const string ACCESSORY_ABILITY_TOOLTIP = "TinkererHammerAccessoryEffect";

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerTier", ReforgeHammerLocalization.HammerTier.Format(HammerTier)));
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerFilter", ReforgeHammerLocalization.HammerFilter.Format(ReforgeableCondition.Text)));

            if (WeaponEffectText.Value.Length > 0) {
                ReforgeHammerUtility.ProcessAbilityLines(GetWeaponEffectText(null), tooltips, WEAPON_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.WeaponEffectPrefix);
            }
            if (AccessoryEffectText.Value.Length > 0) {
                ReforgeHammerUtility.ProcessAbilityLines(GetAccessoryEffectText(null), tooltips, ACCESSORY_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.AccessoryEffectPrefix);
            }
            tooltips.Add(new TooltipLine(Mod, "TinkererHammerTutorial", ReforgeHammerLocalization.TutorialTooltip.Value));
        }

        public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines) {
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(148, 0, 255), WEAPON_ABILITY_TOOLTIP);
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(255, 0, 165), ACCESSORY_ABILITY_TOOLTIP);
        }

    }
}
