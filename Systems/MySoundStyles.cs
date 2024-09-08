using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    public class MySoundStyles {
        private const string MODNAME = "FaeReforges";

        public static readonly SoundStyle TouhouWarning = Register("th_se_timeout");
        public static readonly SoundStyle TouhouWarningDeep = TouhouWarning.WithPitchOffset(-0.5f);


        private static SoundStyle Register(string name) {
            return new SoundStyle(MODNAME + "/Assets/Sounds/" + name);
        }

    }
}
