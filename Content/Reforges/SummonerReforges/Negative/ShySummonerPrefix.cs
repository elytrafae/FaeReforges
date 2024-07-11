using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges {
    public class ShySummonerPrefix : AbstractSummonerPrefix {
        public override bool IsPositive => false;

        public override float KnockbackBoost => -0.15f;
        public override float MinionOccupancyReduction => -0.25f;
    }
}
