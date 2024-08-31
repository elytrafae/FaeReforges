using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.LootTableModification {
    internal class MoonLordBag : GlobalItem {

        public static readonly Condition NEVER_CONDITION = new Condition(ReforgeHammerLocalization.NeverCondition.Key, () => { return false; });

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            return entity.type == ItemID.MoonLordBossBag;
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            itemLoot.Add(new OneFromOptionsDropRule(1, 1, ModContent.ItemType<SolarTinkererHammer>(), ModContent.ItemType<NebulaTinkererHammer>(), ModContent.ItemType<VortexTinkererHammer>(), ModContent.ItemType<StardustTinkererHammer>()));
        }

    }
}
