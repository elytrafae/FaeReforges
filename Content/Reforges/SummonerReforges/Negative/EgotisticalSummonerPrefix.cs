using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges {
    public class EgotisticalSummonerPrefix : AbstractSummonerPrefix {
        public override bool IsPositive => false;

        public override float MinionOccupancyReduction => -0.5f;
    }
}
