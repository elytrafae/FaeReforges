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
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerEnhancedGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // ItemID.None means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        int hammerItemId = ItemID.None;
        //private static bool prefixFromReforge = false;
        //private static bool isReforgePrefixPositive = false;

        const string HAMMER_SAVE_NAME = "ReforgeHammerId";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItemId > ItemID.None) {
                tag.Add(HAMMER_SAVE_NAME, hammerItemId);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out int hammer)) {
                SetHammer(hammer);
                ApplyOnApplyEffects(item);
            }
        }

        public override void NetSend(Item item, BinaryWriter writer) {
            writer.Write(hammerItemId);
        }

        public override void NetReceive(Item item, BinaryReader reader) {
            SetHammer(reader.ReadInt32());
            ApplyOnApplyEffects(item);
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
            
        }

        /*
        public override bool CanReforge(Item item) {
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer != null && hammer.type != ItemID.None && ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type) != null) {
                return true;
            }
            SoundEngine.PlaySound(SoundID.NPCHit40);
            return false;
        }
        */

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

        public override void PostReforge(Item item) {
            //prefixFromReforge = false;
            //isReforgePrefixPositive = false;
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
