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
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerEnhancedGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // null means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        Item hammerItem = null;
        private static bool prefixFromReforge = false;

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
            Item hammer = ReforgeHammerSaveSystem.GetSelectedHammer();
            if (hammer != null && hammer.type != ItemID.None && ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type) != null) {
                return true;
            }
            SoundEngine.PlaySound(SoundID.NPCHit40);
            return false;
        }

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount) {
            Item hammer = ReforgeHammerSaveSystem.GetSelectedHammer();
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
            prefixFromReforge = true;
        }

        public override void PostReforge(Item item) {
            prefixFromReforge = false;
            Item hammer = ReforgeHammerSaveSystem.GetSelectedHammer();
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
            Item hammer = ReforgeHammerSaveSystem.GetSelectedHammer();
            if (hammer == null || hammer.type == ItemID.None) {
                return true;
            }
            ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type);
            if (hammerType == null) {
                return true;
            }
            bool isPositive = ReforgeTierSystem.IsPrefixForAccessories(pre) ? ReforgeTierSystem.GetAccessoryPrefixTier(pre) >= 3 : ReforgeTierSystem.IsPrefixPositive(pre);
            int negChance = hammerType.negativeReforgeChance;

            //if (negChance == 50) {
            //    return true;
            //}

            // We are gonna generalize and say that getting a positive or negative reforge is a 50/50
            // This means that we have to halve the negative chance we will use here. We can do this by just rolling out of 200 (?)
            // If we get a positive reforge, and the original negative chance is above 50%, we roll with the half chance to deny it
            // If we get a negative reforge, and the original negative chance is below 50%, we roll with the half chance to deny it
            //if ((negChance <= 50) == isPositive) {
            //    return true;
            //}

            // If we wanna roll to toss out a negative, we have to invert the chance 
            // Ex: If we have 45% negative chance, then we wanna deny a negative reforge 65% of the time
            // If we have 0% negative chance, we wanna deny a negative reforge 100% of the time
            int rerollChance = isPositive ? negChance : 100 - negChance;
            return Main.rand.Next(100) > rerollChance;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onUpdateAccessory(item, player, hideVisual);
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamageNpc(item, player, target, modifiers);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamagePvp(item, player, target, modifiers);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamageNpc(item, player, target, hit, damageDone);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamagePvp(item, player, target, hurtInfo);
        }


    }
}
