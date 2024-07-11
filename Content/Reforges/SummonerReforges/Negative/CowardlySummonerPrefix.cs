using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges {
    public class CowardlySummonerPrefix : AbstractSummonerPrefix {
        public override bool IsPositive => false;

        public override float MinionSpeedBuff => -0.3f;
    }
}
