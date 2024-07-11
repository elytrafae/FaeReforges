using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges.Positive
{
    public class ElderlySummonerPrefix : AbstractSummonerPrefix
    {
        public override bool IsPositive => true;

        public override float DamageBoost => 0.6f;
        public override float MinionSpeedBuff => -0.4f;
    }
}
