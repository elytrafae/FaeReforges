using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges.Positive
{
    public class BouncySummonerPrefix : AbstractSummonerPrefix
    {
        public override bool IsPositive => true;

        public override float KnockbackBoost => 0.65f;
    }
}
