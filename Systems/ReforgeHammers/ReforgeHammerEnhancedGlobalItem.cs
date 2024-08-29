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
using Terraria.Chat;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerEnhancedGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // null means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        Item hammerItem = null;

        const string HAMMER_SAVE_NAME = "elytrafae_ReforgeHammerItem";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerItem != null) {
                tag.Add(HAMMER_SAVE_NAME, hammerItem);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out Item hammer)) {
                hammerItem = hammer;
            }
        }

        public ReforgeHammerType GetHammer() {
            return ReforgeHammerRegistry.GetHammerTypeForItemType(hammerItem.type);
        }

        public void SetHammer(Item hammer) {
            hammerItem = hammer;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (hammerItem != null && GetHammer() != null) {
                ReforgeHammerType hammerType = GetHammer();
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", ReforgeHammerLocalization.ReforgedWithTooltip.Format(hammerItem.Name)); // TODO: Add rarity color
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
            return hammer != null && hammer.type != ItemID.None && ReforgeHammerRegistry.GetHammerTypeForItemType(hammer.type) != null;
        }


    }
}
