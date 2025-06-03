using System;
using System.Collections.Generic;
using FaeReforges.Content.Items.TinkererHammers.Tier4;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ShopsAndLoot {
    internal class BossLootSystem : ModSystem {

        private static Dictionary<int, List<IItemDropRule>> NPCLootList = new();
        private static Dictionary<int, List<IItemDropRule>> ItemLootList = new();
        private static bool AreListsInitialized = false;

        public override void Unload() {
            NPCLootList.Clear();
            ItemLootList.Clear();
            AreListsInitialized = false;
        }

        public static void AddLoot() {
            AddLootToBossAndBag(NPCID.DD2Betsy, ItemID.BossBagBetsy, ItemDropRule.Common(ModContent.ItemType<BetsyTinkererHammer>()));
            AddLootToBossAndBag(NPCID.HallowBoss, ItemID.FairyQueenBossBag, ItemDropRule.Common(ModContent.ItemType<PrismaticTinkererHammer>()));
            AddLootToBossAndBag(NPCID.DukeFishron, ItemID.FishronBossBag, ItemDropRule.Common(ModContent.ItemType<FishyTinkererHammer>()));
            AddLootToNPC(NPCID.CultistBoss, ItemDropRule.Common(ModContent.ItemType<CultistTinkererHammer>()));
        }

        public static void AddLootToItem(int itemID, IItemDropRule rule) {
            if (!ItemLootList.TryGetValue(itemID, out List<IItemDropRule> ruleList)) { 
                ruleList = [];
                ItemLootList.Add(itemID, ruleList);
            }
            ruleList.Add(rule);
        }

        public static void AddLootToNPC(int npcID, IItemDropRule rule) {
            if (!NPCLootList.TryGetValue(npcID, out List<IItemDropRule> ruleList)) {
                ruleList = [];
                NPCLootList.Add(npcID, ruleList);
            }
            ruleList.Add(rule);
        }

        public static void AddLootToBossAndBag(int npcID, int itemID, IItemDropRule rule) {
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(rule);

            AddLootToNPC(npcID, notExpertRule);
            AddLootToItem(itemID, rule);
        }

        public static void InitializeLists() {
            if (AreListsInitialized) {
                return;
            }
            AreListsInitialized = true;
            AddLoot();
        }

        private static void IterateID(int id, ILoot loot, Dictionary<int, List<IItemDropRule>> dict) {
            if (dict.TryGetValue(id, out List<IItemDropRule> list)) {
                foreach (IItemDropRule rule in list) { 
                    loot.Add(rule);
                }
            }
        }

        public static void IterateNPCID(int id, NPCLoot loot) {
            IterateID(id, loot, NPCLootList);
        }

        public static void IterateItemID(int id, ItemLoot loot) {
            IterateID(id, loot, ItemLootList);
        }

    }

    internal class BossLootNPC : GlobalNPC {

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            BossLootSystem.InitializeLists();
            BossLootSystem.IterateNPCID(npc.type, npcLoot);
        }

    }

    internal class BossLootItem : GlobalItem {

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            BossLootSystem.InitializeLists();
            BossLootSystem.IterateItemID(item.type, itemLoot);
        }

    }

}
