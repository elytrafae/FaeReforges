using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges {
    public class AnemicSummonerPrefix : AbstractSummonerPrefix {
        public override bool IsPositive => false;

        public override float KnockbackBoost => -0.4f;
    }
}
