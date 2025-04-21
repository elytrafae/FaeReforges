using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Items;
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
        private static bool prefixFromReforge = false;
        readonly PrefixDefinition[] previousPrefixes = [default, default];
        private static int previousPrefixWorkaround = 0;

        const string HAMMER_SAVE_NAME = "ReforgeHammerDefinition";
        const string PREVIOUS_PREFIXES_NAME = "PreviousPrefix";


        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItemId > ItemID.None) {
                tag.Add(HAMMER_SAVE_NAME, new ItemDefinition(hammerItemId));
            }
            for (int i = 0; i < previousPrefixes.Length; i++) {
                tag.Add(PREVIOUS_PREFIXES_NAME + i, previousPrefixes[i]);
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
        }

        public override void NetSend(Item item, BinaryWriter writer) {
            writer.Write(hammerItemId);
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
            for (int i = 0; i < previousPrefixes.Length; i++) {
                previousPrefixes[i] = PrefixDefinition.FromString(reader.ReadString());
            }
        }

        private void ApplyOnApplyEffects(Item item) {
            AbstractTinkererHammer hammerType = GetHammer();
            if (hammerType != null) {
                if (item.accessory) {
                    hammerType.HammerOnApplyAccessory(item);
                }
                if (ItemCondition.IsWeapon.IsMet(item)) {
                    hammerType.HammerOnApplyWeapon(item);
                }
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context) {
            
        }

        public AbstractTinkererHammer GetHammer() {
            if (hammerItemId == ItemID.None) {
                return null;
            }
            Item sampleItem = ContentSamples.ItemsByType[hammerItemId];
            if (sampleItem.ModItem != null && sampleItem.ModItem is AbstractTinkererHammer hammer) {
                return hammer;
            }
            return null;
        }

        public int GetHammerItemTypeOrNone() {
            return hammerItemId;
        }

        public void SetHammer(int hammerId) {
            hammerItemId = hammerId;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            AbstractTinkererHammer hammerType = GetHammer();
            if (hammerType != null) {
                // hammerItemId is 100% NOT None!
                Item dummyItem = ContentSamples.ItemsByType[GetHammerItemTypeOrNone()];
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", ReforgeHammerLocalization.ReforgedWithTooltip.Format(dummyItem.Name));
                line.OverrideColor = ItemRarity.GetColor(dummyItem.rare);
                tooltips.Add(line);

                if (ItemCondition.IsWeapon.Predicate(item)) {
                    ReforgeHammerUtility.ProcessAbilityLines(hammerType.WeaponEffectText.Value, tooltips, AbstractTinkererHammer.WEAPON_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.WeaponEffectPrefix);
                }
                if (item.accessory) {
                    ReforgeHammerUtility.ProcessAbilityLines(hammerType.AccessoryEffectText.Value, tooltips, AbstractTinkererHammer.ACCESSORY_ABILITY_TOOLTIP, Mod, ReforgeHammerLocalization.AccessoryEffectPrefix);
                }
                
            }

            /*
            for (int i = 0; i < previousPrefixes.Length; i++) {
                string valStr = "NULL";
                if (previousPrefixes[i] != null) { 
                    valStr = previousPrefixes[i].ToString();
                }
                tooltips.Add(new TooltipLine(Mod, "PreviousPrefixDebug" + i, "PrevPref #" + i + ": " + valStr));
            }
            */
            
        }

        public override bool CanReforge(Item item) {
            Item hammerItem = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammerItem != null && hammerItem.ModItem != null && hammerItem.ModItem is AbstractTinkererHammer hammer) {
                if (hammer.ReforgeableCondition.IsMet(item)) {
                    return true;
                }
                SoundEngine.PlaySound(SoundID.AbigailCry);
                return false;
            }
            SoundEngine.PlaySound(SoundID.NPCHit40);
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
                item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().SetHammer(hammer.Type);
                ApplyOnApplyEffects(item);
            }
        }

        public override void PostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines) {
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(148, 0, 255), AbstractTinkererHammer.WEAPON_ABILITY_TOOLTIP);
            ReforgeHammerUtility.DrawAbilityTooltipLineThing(lines, new Color(255, 0, 165), AbstractTinkererHammer.ACCESSORY_ABILITY_TOOLTIP);
        }

        private static AbstractTinkererHammer? Hammer(Item item) => item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer();

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            Hammer(item)?.HammerModifyWeaponDamage(item, player, ref damage);
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            Hammer(item)?.HammerModifyWeaponCrit(item, player, ref crit);
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) {
            Hammer(item)?.HammerModifyWeaponKnockback(item, player, ref knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Hammer(item)?.HammerModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
            Hammer(item)?.HammerModifyManaCost(item, player, ref reduce, ref mult);
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale) {
            Hammer(item)?.HammerModifyItemScale(item, player, ref scale);
        }

        public override float UseSpeedMultiplier(Item item, Player player) {
            AbstractTinkererHammer hammerType = Hammer(item);
            if (hammerType != null) {
                return hammerType.HammerUseSpeedMultiplier(item, player);
            }
            return 1f;
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
            Hammer(item)?.HammerEnchantmentVisuals(player, item.type, new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height);
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            AbstractTinkererHammer hammerType = Hammer(item);
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
            return Hammer(item)?.HammerUseItem(item, player);
        }

        public override void HoldItem(Item item, Player player) {
            AbstractTinkererHammer hammer = GetHammer();
            if (hammer != null) {
                hammer.HammerOnUpdateWeaponHeld(item, player);
                if (player.itemTime > 0) {
                    hammer.HammerWhileUsingWeapon(item, player);
                }
            }
        }

        // These are all for melee. The projectiles are handled elsewhere

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            Hammer(item)?.HammerChangeWeaponDealDamageNpc(item.type, player, target, ref modifiers, item.DamageType);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            Hammer(item)?.HammerChangeWeaponDealDamagePvp(item.type, player, target, ref modifiers, item.DamageType);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            Hammer(item)?.HammerOnWeaponDealDamageNpc(item.type, player, target, hit, damageDone, item.DamageType);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            Hammer(item)?.HammerOnWeaponDealDamagePvp(item.type, player, target, hurtInfo, item.DamageType);
        }

        public override bool CanUseItem(Item item, Player player) {
            AbstractTinkererHammer hammer = Hammer(item);
            if (hammer == null) {
                return true;
            }
            return hammer.HammerCanUseItem(item, player);
        }

    }
}
