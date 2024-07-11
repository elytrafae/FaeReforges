using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges {
    public class DepressedSummonerPrefix : AbstractSummonerPrefix {
        public override bool IsPositive => false;

        public override float DamageBoost => -0.1f;
        public override float KnockbackBoost => -0.1f;
        public override float MinionSpeedBuff => -0.1f;
    }
}
