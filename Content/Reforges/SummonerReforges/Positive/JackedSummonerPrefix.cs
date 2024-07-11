using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Reforges.SummonerReforges.Positive
{
    public class JackedSummonerPrefix : AbstractSummonerPrefix
    {
        public override bool IsPositive => true;

        public override float DamageBoost => 0.15f;
        public override float KnockbackBoost => 0.15f;
        public override int MinionCritChance => 15;
        public override float MinionSpeedBuff => -0.12f;
    }
}
