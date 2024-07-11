using FaeReforges.Content.Reforges;
using FaeReforges.Systems.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.Utilities;

namespace FaeReforges.Systems {
    internal class SummonerReforgesGlobalItem : GlobalItem {

        // TODO: Add sentry summon count

        public float minionOccupancyMult = 1f;
        public float minionSpeedMult = 1f;
        public int minionCritBonus = 0;
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            // IDK How TF to fix this
            if (entity.type <= ItemID.None || entity.type >= ItemID.Count) {
                return false;
            }
            ItemDefinition definition = new ItemDefinition(entity.type);
            if (ModContent.GetInstance<ServerConfig>().WhitelistedWands.Contains(definition)) {
                return true;
            }
            if (ModContent.GetInstance<ServerConfig>().BlacklistedWands.Contains(definition)) {
                return false;
            }
            return (entity.CountsAsClass(DamageClass.Summon) && !entity.CountsAsClass(DamageClass.SummonMeleeSpeed)) || entity.sentry; // Hopefully this is sufficient
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand) {
            if (!ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                return -1;
            }
            return rand.NextFromCollection<AbstractSummonerPrefix>(ModContent.GetContent<AbstractSummonerPrefix>().ToList<AbstractSummonerPrefix>()).Type;
        }

        public override bool CanUseItem(Item item, Player player) {
            if (item.sentry) {
                return minionOccupancyMult <= player.maxTurrets;
            }
            return minionOccupancyMult * ItemID.Sets.StaffMinionSlotsRequired[item.type] <= player.maxMinions;
        }

        public override bool AllowPrefix(Item item, int pre) {
            if (!ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                return true;
            }
            return pre >= PrefixID.Count;
        }

        public override bool? CanAutoReuseItem(Item item, Player player) {
            return ModContent.GetInstance<ServerConfig>().SummonWandsAreReusable ? true : null;
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
            mult *= ModContent.GetInstance<ServerConfig>().SummonWandsManaCost / 100f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (ModContent.GetInstance<ServerConfig>().SummonWandsManaCost == 0) { 
                tooltips.RemoveAll(line => line.Name == "UseMana");
            }
        }

    }
}
