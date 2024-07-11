using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges.Positive
{
    public class GamblerSummonerPrefix : AbstractSummonerPrefix
    {
        public override bool IsPositive => true;

        public override float DamageBoost => -0.2f;
        public override int MinionCritChance => 50;
    }
}
