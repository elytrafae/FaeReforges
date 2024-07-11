using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace FaeReforges.Systems.Config {
    internal class ServerConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Reforges")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool EnableCustomSummonerReforges = true;
        [ReloadRequired]
        public List<ItemDefinition> BlacklistedWands = new List<ItemDefinition>();
        [ReloadRequired]
        public List<ItemDefinition> WhitelistedWands = new List<ItemDefinition>();

        [Header("Balance")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool MakeAllMinionsHaveLocalIFrames = true;
        [ReloadRequired]
        public List<ProjectileDefinition> MinionsThatShouldHaveStaticIFrames = new List<ProjectileDefinition>();


        [Header("QOL")]
        [DefaultValue(true)]
        public bool SummonWandsAreReusable = true;
        [Range(0, 100)]
        [DefaultValue(0)]
        [Increment(1)]
        [DrawTicks]
        public int SummonWandsManaCost = 0;
    }
}
