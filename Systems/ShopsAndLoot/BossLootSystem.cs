using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ShopsAndLoot {
    internal class BossLootSystem : ModSystem {


        // TODO: Try to make this much more performant
        // Current idea: Make these lists sorted by ID, binary search the ID, and then sequentially go forward and backward
        // until you bump into the start/end of the list or a different ID
        public static List<Tuple<int, IItemDropRule>> NPCLootList = new();
        public static List<Tuple<int, IItemDropRule>> ItemLootList = new();

        public void AddLootToBossAndBag(int npcID, int itemID, IItemDropRule rule) {
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(rule);

            NPCLootList.Add(new(npcID, notExpertRule));
            ItemLootList.Add(new(itemID, rule));
        }

    }

    internal class BossLootNPC : GlobalNPC {

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (Tuple<int, IItemDropRule> tuple in BossLootSystem.NPCLootList) {
                if (tuple.Item1 == npc.type) {
                    npcLoot.Add(tuple.Item2);
                }
            }
        }

    }

    internal class BossLootItem : GlobalItem {

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            foreach (Tuple<int, IItemDropRule> tuple in BossLootSystem.ItemLootList) {
                if (tuple.Item1 == item.type) {
                    itemLoot.Add(tuple.Item2);
                }
            }
        }

    }

}
