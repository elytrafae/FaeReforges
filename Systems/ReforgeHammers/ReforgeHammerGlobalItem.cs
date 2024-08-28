using FaeReforges.Content;
using FaeReforges.Content.HammerTypes;
using FaeReforges.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // null means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        AbstractHammerType hammerType = null;

        const string HAMMER_SAVE_NAME = "elytrafae_ReforgeHammer";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerType != null) {
                tag.Add(HAMMER_SAVE_NAME, hammerType.FullName);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out string hammerName)) {
                hammerType = ReforgeHammerSaveSystem.GetHammerTypeFromName(hammerName);
            }
        }

        public AbstractHammerType GetHammer() {
            return hammerType;
        }

        public void SetHammer(AbstractHammerType hammer) {
            hammerType = hammer;
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed) {
            item.GetGlobalItem<ReforgeHammerGlobalItem>().SetHammer(ModContent.GetContent<StoneHammer>().First());
        }

        public override void OnConsumeAmmo(Item weapon, Item ammo, Player player) {
            weapon.GetGlobalItem<ReforgeHammerGlobalItem>().SetHammer(ModContent.GetContent<StoneHammer>().First());
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (hammerType != null) {
                tooltips.Add(new TooltipLine(Mod, "ReforgeHammerType", AbstractHammerType.ReforgedWithTooltip.Format(hammerType.DisplayName)));
            }
        }

    }
}
