using FaeReforges.Content;
using FaeReforges.Content.Items;
using FaeReforges.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using static System.Net.Mime.MediaTypeNames;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerEnhancedGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // ItemID.None means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        int hammerItemId = ItemID.None;
        private static bool prefixFromReforge = false;
        readonly PrefixDefinition[] previousPrefixes = [default, default];
        private static int previousPrefixWorkaround = 0;

        const string HAMMER_SAVE_NAME = "ReforgeHammerId";
        const string PREVIOUS_PREFIXES_NAME = "PreviousPrefix";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItemId > ItemID.None) {
                tag.Add(HAMMER_SAVE_NAME, hammerItemId);
            }
            for (int i = 0; i < previousPrefixes.Length; i++) {
                tag.Add(PREVIOUS_PREFIXES_NAME + i, previousPrefixes[i]);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out int hammer)) {
                SetHammer(hammer);
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
            ReforgeHammerType hammerType = GetHammer();
            if (hammerType != null) {
                if (item.accessory) {
                    hammerType.onApplyAccessory(item);
                } else {
                    hammerType.onApplyWeapon(item);
                }
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context) {
            
        }

        public ReforgeHammerType GetHammer() {
            if (hammerItemId == ItemID.None) {
                return null;
            }
            return ReforgeHammerRegistry.GetHammerTypeForItemType(hammerItemId);
        }

        public int GetHammerItemTypeOrNone() {
            return hammerItemId;
        }

        public void SetHammer(int hammerId) {
            hammerItemId = hammerId;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            ReforgeHammerType hammerType = GetHammer();
            if (hammerType != null) {
                // hammerItemId is 100% NOT None!
                Item dummyItem = ContentSamples.ItemsByType[GetHammerItemTypeOrNone()];
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", ReforgeHammerLocalization.ReforgedWithTooltip.Format(dummyItem.Name));
                line.OverrideColor = ItemRarity.GetColor(dummyItem.rare);
                tooltips.Add(line);

                if (item.accessory) {
                    if (hammerType.AccessoryEffect.Value.Length > 0) {
                        tooltips.Add(new TooltipLine(Mod, "TinkererHammerAccessoryEffect", ReforgeHammerLocalization.AccessoryEffectPrefix.Format(hammerType.AccessoryEffect)));
                    }
                } else {
                    if (hammerType.WeaponEffect.Value.Length > 0) {
                        tooltips.Add(new TooltipLine(Mod, "TinkererHammerWeaponEffect", ReforgeHammerLocalization.WeaponEffectPrefix.Format(hammerType.WeaponEffect)));
                    }
                }
            }

            for (int i = 0; i < previousPrefixes.Length; i++) {
                string valStr = "NULL";
                if (previousPrefixes[i] != null) { 
                    valStr = previousPrefixes[i].ToString();
                }
                tooltips.Add(new TooltipLine(Mod, "PreviousPrefixDebug" + i, "PrevPref #" + i + ": " + valStr));
            }
            
        }

        public override bool CanReforge(Item item) {
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer != null && hammer.type != ItemID.None) {
                ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
                if (hammerType != null) {
                    if (hammerType.reforgableCondition.IsMet(item)) {
                        return true;
                    }
                    SoundEngine.PlaySound(SoundID.AbigailCry);
                }
            }
            SoundEngine.PlaySound(SoundID.NPCHit40);
            return false;
        }

        /*
        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount) {
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer == null || hammer.type == ItemID.None) {
                return true;
            }
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
            if (hammerType == null) {
                return true;
            }
            reforgePrice = (reforgePrice * hammerType.reforgeCost) / 100;
            return true;
        }

        public override void PreReforge(Item item) {
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer == null || hammer.type == ItemID.None) {
                return;
            }
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
            if (hammerType == null) {
                return;
            }
            prefixFromReforge = true;
            isReforgePrefixPositive = Main.rand.Next(100) >= hammerType.negativeReforgeChance;
        }
        */

        public override void PreReforge(Item item) {
            prefixFromReforge = true;
            previousPrefixWorkaround = item.prefix;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand) {
            int tier = rand.NextBool(2) ? 1 : 0;
            if (prefixFromReforge) {
                Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
                if (hammer != null && hammer.type != ItemID.None) {
                    ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
                    if (hammerType != null) {
                        tier = hammerType.hammerTier;
                    }
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
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer == null || hammer.type == ItemID.None) {
                return;
            }
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
            if (hammerType == null) {
                return;
            }
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().SetHammer(hammer.type);
            ApplyOnApplyEffects(item);
        }

        /*
        public override bool AllowPrefix(Item item, int pre) {
            if (!prefixFromReforge) {
                return true;
            }
            bool isPositive = ReforgeTierSystem.IsPrefixForAccessories(pre) ? ReforgeTierSystem.GetAccessoryPrefixTier(pre) >= 3 : ReforgeTierSystem.IsPrefixPositive(pre);
            return isPositive == isReforgePrefixPositive;
        }
        */

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyWeaponDamage(item, player, ref damage);
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyWeaponCrit(item, player, ref crit);
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyWeaponKnockback(item, player, ref knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyManaCost(item, player, ref reduce, ref mult);
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.modifyItemScale(item, player, ref scale);
        }

        public override float UseSpeedMultiplier(Item item, Player player) {
            ReforgeHammerType hammerType = item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer();
            if (hammerType != null) {
                return hammerType.useSpeedMultiplier(item, player);
            }
            return 1f;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onUpdateAccessory(item, player, hideVisual);
        }

        // These are all for melee. The projectiles are handled elsewhere

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamageNpc(item.type, player, target, ref modifiers);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamagePvp(item.type, player, target, ref modifiers);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamageNpc(item.type, player, target, hit, damageDone);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamagePvp(item.type, player, target, hurtInfo);
        }

        public override bool CanUseItem(Item item, Player player) {
            ReforgeHammerType hammer = item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer();
            if (hammer == null) {
                return true;
            }
            return hammer.canUseItem(item, player);
        }

    }
}
