using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges
{
    public class SummonerReforges : AbstractSummonerPrefix
    {

        public virtual float MinionSpeedBuff => 7000f;
        public virtual float MinionOccupancyReduction => 1f;

    }
}
