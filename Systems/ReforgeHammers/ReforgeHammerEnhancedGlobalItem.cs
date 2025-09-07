using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Items;
using FaeReforges.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerEnhancedGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // ItemID.None means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        int hammerItemId = ItemID.None;
        // This one is updated even if a phantom hammer is used. Essentially, this is the actual last hammer used on this item, while hammerItemId is the effect's source
        int lastHammerUsed = ItemID.None;
        private static bool prefixFromReforge = false;
        readonly PrefixDefinition[] previousPrefixes = [default, default, default];
        private static int previousPrefixWorkaround = 0;

        const string HAMMER_SAVE_NAME = "ReforgeHammerDefinition";
        const string PREVIOUS_PREFIXES_NAME = "PreviousPrefix";
        const string LAST_HAMMER_SAVE_NAME = "LastHammerDefinition";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItemId > ItemID.None) {
                tag.Add(HAMMER_SAVE_NAME, new ItemDefinition(hammerItemId));
            }
            for (int i = 0; i < previousPrefixes.Length; i++) {
                tag.Add(PREVIOUS_PREFIXES_NAME + i, previousPrefixes[i]);
            }
            if (lastHammerUsed > ItemID.None) {
                tag.Add(LAST_HAMMER_SAVE_NAME, new ItemDefinition(lastHammerUsed));
            }
            
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out ItemDefinition hammer)) {
                SetHammer(hammer.Type);
                ApplyOnApplyEffects(item);
            }
            for (int i = 0; i < previousPrefixes.Length; i++) {
                if (tag.TryGet(PREVIOUS_PREFIXES_NAME + i, out PrefixDefinition definition)) {
                    previousPrefixes[i] = definition;
                }
            }
            if (tag.TryGet(LAST_HAMMER_SAVE_NAME, out ItemDefinition lastHammer)) {
                lastHammerUsed = lastHammer.Type;
            } else {
                if (hammerItemId > ItemID.None) {
                    lastHammerUsed = hammerItemId;
                }
            }
        }

        public override void NetSend(Item item, BinaryWriter writer) {
            writer.Write(hammerItemId);
            writer.Write(lastHammerUsed);
            for (int i = 0; i < previousPrefixes.Length; i++) {
                if (previousPrefixes[i] == null) {
                    previousPrefixes[i] = new PrefixDefinition();
                }
                writer.Write(previousPrefixes[i].ToString());
            }
        }

        public override void NetReceive(Item item, BinaryReader reader) {
            SetHammer(reader.ReadInt32());
            ApplyOnApplyEffects(item);
            lastHammerUsed = reader.ReadInt32();
            for (int i = 0; i < previousPrefixes.Length; i++) {
                previousPrefixes[i] = PrefixDefinition.FromString(reader.ReadString());
            }
        }

        private void ApplyOnApplyEffects(Item item) {
            GetHammer(item, HammerEffectContext.ACCESSORY)?.HammerOnApplyAccessory(item);
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerOnApplyWeapon(item);
        }

        public override void OnCreated(Item item, ItemCreationContext context) {
            
        }

        public AbstractTinkererHammer GetHammer(Item item, HammerEffectContext effectContext) {
            if (hammerItemId == ItemID.None) {
                return null;
            }
            Item sampleItem = ContentSamples.ItemsByType[hammerItemId];
            if (sampleItem.ModItem != null && sampleItem.ModItem is AbstractTinkererHammer hammer) {
                if (effectContext == HammerEffectContext.NONE) {
                    return hammer;
                }
                if (effectContext == HammerEffectContext.WEAPON && ItemCondition.IsWeapon.IsMet(item) && CustomIDSets.ShouldReceiveWeaponReforgeHammerBenefits[item.type]) {
                    return hammer;
                }
                if (effectContext == HammerEffectContext.ACCESSORY && item.accessory && CustomIDSets.ShouldReceiveAccessoryReforgeHammerBenefits[item.type]) {
                    return hammer;
                }
                return null;
            }
            return null;
        }

        /*
        public int GetHammerItemTypeOrNone() {
            return hammerItemId;
        }
        */

        public void SetHammer(int hammerId) {
            hammerItemId = hammerId;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            AbstractTinkererHammer hammerType = GetHammer(item, HammerEffectContext.NONE);

            if (lastHammerUsed > ItemID.None) {
                Item dummyItem = ContentSamples.ItemsByType[lastHammerUsed];
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", ReforgeHammerLocalization.ReforgedWithTooltip.Format(dummyItem.Name));
                line.OverrideColor = ItemRarity.GetColor(dummyItem.rare);
                tooltips.Add(line);
            }

            if (hammerType != null) {
                
                // if a phantom hammer was used...
                if (lastHammerUsed != hammerType.Type) {
                    Item dummyItem = ContentSamples.ItemsByType[hammerType.Type];
                    TooltipLine line = new TooltipLine(Mod, "ReforgeHammerEffectType", ReforgeHammerLocalization.ReforgeEffectTooltip.Format(dummyItem.Name));
                    line.OverrideColor = ItemRarity.GetColor(dummyItem.rare);
                    tooltips.Add(line);
                }
                

                if (ItemCondition.IsWeapon.Predicate(item) && CustomIDSets.ShouldReceiveWeaponReforgeHammerBenefits[item.type]) {
                    ReforgeHammerUtility.ProcessAbilityLines(hammerType.GetWeaponEffectText(item), tooltips, AbstractTinkererHammer.WEAPON_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.WeaponEffectPrefix);
                }
                if (item.accessory && CustomIDSets.ShouldReceiveAccessoryReforgeHammerBenefits[item.type]) {
                    ReforgeHammerUtility.ProcessAbilityLines(hammerType.GetAccessoryEffectText(item), tooltips, AbstractTinkererHammer.ACCESSORY_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.AccessoryEffectPrefix);
                }
                
            }
            
        }

        public override bool CanReforge(Item item) {
            Item hammerItem = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammerItem != null && hammerItem.ModItem != null && hammerItem.ModItem is AbstractTinkererHammer hammer) {
                if (hammer.ReforgeableCondition.IsMet(item)) {
                    return true;
                }
                SoundEngine.PlaySound(SoundID.MenuClose);
                return false;
            }
            SoundEngine.PlaySound(SoundID.MenuClose);
            return false;
        }

        public override void PreReforge(Item item) {
            prefixFromReforge = true;
            previousPrefixWorkaround = item.prefix;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand) {
            int tier = rand.NextBool(2) ? 1 : 0;
            if (prefixFromReforge) {
                Item hammerItem = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
                if (hammerItem != null && hammerItem.ModItem != null && hammerItem.ModItem is AbstractTinkererHammer hammer) {
                    tier = hammer.HammerTier;
                }
            }
            if (tier == 0) {
                return 0; // Prefix is now none!
            }
            List<int> prefixes = new();
            foreach (int pre in ReforgeTierSystem.GetAllReforgesOfTier(tier)) {
                if (item.CanApplyPrefix(pre)) {
                    prefixes.Add(pre);
                }
            }
            if (prefixes.Count <= 0) {
                return 0;
            }

            if (prefixFromReforge) {
                // Take previous prefixes out of the equation!
                // Is there is only one prefix at any point, don't do it!
                if (prefixes.Count > 1) {
                    prefixes.Remove(previousPrefixWorkaround);
                    for (int i = 0; i < previousPrefixes.Length; i++) {
                        if (prefixes.Count <= 1) {
                            break;
                        }
                        if (previousPrefixes[i] != null) {
                            prefixes.Remove(previousPrefixes[i].Type);
                        }
                    }
                }

                // Add the "current" prefix to the list of previous prefixes, if it had any
                for (int i = previousPrefixes.Length - 2; i >= 0; i--) {
                    previousPrefixes[i + 1] = previousPrefixes[i];
                }
                if (previousPrefixWorkaround != 0) { // Fun . . .
                    previousPrefixes[0] = new PrefixDefinition(previousPrefixWorkaround);
                } else {
                    previousPrefixes[0] = new PrefixDefinition();
                }
            }

            // Roll a new prefix
            return rand.NextFromList(prefixes.ToArray());
        }

        public override void PostReforge(Item item) {
            prefixFromReforge = false;
            Item hammerItem = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammerItem != null && hammerItem.ModItem != null && hammerItem.ModItem is AbstractTinkererHammer hammer) {
                lastHammerUsed = hammer.Type;
                if (!hammer.PhantomHammer) {
                    SetHammer(hammer.Type);
                }
                ApplyOnApplyEffects(item);
            }
        }

        public override void PostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines) {
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(148, 0, 255), AbstractTinkererHammer.WEAPON_ABILITY_TOOLTIP);
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(255, 0, 165), AbstractTinkererHammer.ACCESSORY_ABILITY_TOOLTIP);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyWeaponDamage(item, player, ref damage);
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyWeaponCrit(item, player, ref crit);
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyWeaponKnockback(item, player, ref knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyManaCost(item, player, ref reduce, ref mult);
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerModifyItemScale(item, player, ref scale);
        }

        public override float UseSpeedMultiplier(Item item, Player player) {
            AbstractTinkererHammer hammerType = GetHammer(item, HammerEffectContext.WEAPON);
            if (hammerType != null) {
                return hammerType.HammerUseSpeedMultiplier(item, player);
            }
            return 1f;
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerEnchantmentVisuals(player, item.type, new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height);
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            AbstractTinkererHammer hammerType = GetHammer(item, HammerEffectContext.ACCESSORY);
            if (hammerType != null) {
                Dictionary<int, int> counts = player.GetModPlayer<ReforgeHammerEnhancedModPlayer>().AccessoryReforgeCounts; // Shorthand
                if (!counts.TryGetValue(hammerType.Type, out int value)) {
                    value = 1;
                    counts.Add(hammerType.Type, value);
                } else {
                    counts[hammerType.Type] = ++value;
                }
                hammerType.HammerOnUpdateAccessory(item, player, value, hideVisual);
            }
        }

        public override bool? UseItem(Item item, Player player) {
            return GetHammer(item, HammerEffectContext.WEAPON)?.HammerUseItem(item, player);
        }

        public override void HoldItem(Item item, Player player) {
            AbstractTinkererHammer hammer = GetHammer(item, HammerEffectContext.WEAPON);
            if (hammer != null) {
                hammer.HammerOnUpdateWeaponHeld(item, player);
                if (player.itemTime > 0) {
                    hammer.HammerWhileUsingWeapon(item, player);
                }
            }
        }

        // These are all for melee. The projectiles are handled elsewhere

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerChangeWeaponDealDamageNpc(item.type, player, target, ref modifiers, item.DamageType);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerChangeWeaponDealDamagePvp(item.type, player, target, ref modifiers, item.DamageType);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerOnWeaponDealDamageNpc(item.type, player, target, hit, damageDone, item.DamageType);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            GetHammer(item, HammerEffectContext.WEAPON)?.HammerOnWeaponDealDamagePvp(item.type, player, target, hurtInfo, item.DamageType);
        }

        public override bool CanUseItem(Item item, Player player) {
            AbstractTinkererHammer hammer = GetHammer(item, HammerEffectContext.WEAPON);
            if (hammer == null) {
                return true;
            }
            return hammer.HammerCanUseItem(item, player);
        }

    }
}
