using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FaeReforges.Systems
{
    public class SummonerReforgesGlobalProjectile : GlobalProjectile
    {

        public override bool InstancePerEntity => true;
        public float initOccupancy = 1f;
        public float bonusSpeed = 1f;
        public float excessUpdates = 0f;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            initOccupancy = projectile.minionSlots;
            if (source is EntitySource_ItemUse itemSource)
            {
                projectile.minionSlots = initOccupancy * itemSource.Item.GetGlobalItem<SummonerReforgesGlobalItem>().minionOccupancyMult;
                this.bonusSpeed = itemSource.Item.GetGlobalItem<SummonerReforgesGlobalItem>().minionSpeedMult;
            }
        }

    }
}
