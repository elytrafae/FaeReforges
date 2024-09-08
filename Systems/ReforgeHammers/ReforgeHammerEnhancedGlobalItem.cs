using FaeReforges.Content;
using FaeReforges.Content.Items;
using FaeReforges.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        // null means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        Item hammerItem = null;
        private static bool prefixFromReforge = false;
        private static bool isReforgePrefixPositive = false;

        const string HAMMER_SAVE_NAME = "elytrafae_ReforgeHammerItem";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItem != null) {
                tag.Add(HAMMER_SAVE_NAME, hammerItem);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out Item hammer)) {
                hammerItem = hammer;
                ReforgeHammerType hammerType = GetHammer();
                if (hammerType != null) {
                    if (item.accessory) {
                        hammerType.onApplyAccessory(item);
                    } else {
                        hammerType.onApplyWeapon(item);
                    }
                }
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context) {
            
        }

        public ReforgeHammerType GetHammer() {
            if (hammerItem == null) {
                return null;
            }
            return ReforgeHammerRegistry.GetHammerTypeForItemType(hammerItem.type);
        }

        public int GetHammerItemTypeOrNone() {
            return hammerItem == null ? ItemID.None : hammerItem.type;
        }

        public void SetHammer(Item hammer) {
            hammerItem = hammer;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            ReforgeHammerType hammerType = GetHammer();
            if (hammerType != null) {
                // hammerItem is 100% declared here!
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", ReforgeHammerLocalization.ReforgedWithTooltip.Format(hammerItem.Name));
                line.OverrideColor = ItemRarity.GetColor(hammerItem.rare);
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

        public override bool CanReforge(Item item) {
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer != null && hammer.type != ItemID.None && ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type) != null) {
                return true;
            }
            SoundEngine.PlaySound(SoundID.NPCHit40);
            return false;
        }

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

        public override void PostReforge(Item item) {
            prefixFromReforge = false;
            isReforgePrefixPositive = false;
            Item hammer = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (hammer == null || hammer.type == ItemID.None) {
                return;
            }
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
            if (hammerType == null) {
                return;
            }
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().SetHammer(hammer);
            if (item.accessory) {
                hammerType.onApplyAccessory(item);
            } else {
                hammerType.onApplyWeapon(item);
            }
        }

        public override bool AllowPrefix(Item item, int pre) {
            if (!prefixFromReforge) {
                return true;
            }
            bool isPositive = ReforgeTierSystem.IsPrefixForAccessories(pre) ? ReforgeTierSystem.GetAccessoryPrefixTier(pre) >= 3 : ReforgeTierSystem.IsPrefixPositive(pre);
            return isPositive == isReforgePrefixPositive;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onUpdateAccessory(item, player, hideVisual);
        }

        // These are all for melee. The projectiles are handled elsewhere

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamageNpc(item, player, target, ref modifiers);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamagePvp(item, player, target, ref modifiers);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamageNpc(item, player, target, hit, damageDone);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamagePvp(item, player, target, hurtInfo);
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
