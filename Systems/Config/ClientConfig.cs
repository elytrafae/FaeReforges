using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace FaeReforges.Systems.Config {
    internal class ClientConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("WhipFrenzy")]
        [Range(0, 100)]
        [DefaultValue(65)]
        [Increment(1)]
        [DrawTicks]
        public int UIOffsetHorizontal = 0;

        [Range(0, 100)]
        [DefaultValue(5)]
        [Increment(1)]
        [DrawTicks]
        public int UIOffsetVertical = 0;

        [DefaultValue(false)]
        public bool displayWhipFrenzyAlways;
    }
}
