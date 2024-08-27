using FaeReforges.Systems.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items {
    public class TinkererHammer : ModItem {

        private Mod mod;
        private AbstractHammerType hammerType;

        public TinkererHammer(Mod mod, AbstractHammerType hammerType) {
            this.mod = mod;
            this.hammerType = hammerType;
        }

        public override string Name => hammerType.FullName;
        public override LocalizedText DisplayName => hammerType.DisplayName;
        public override string Texture => hammerType.Texture;

        public override void SetDefaults() {
            Item.maxStack = 1;
            Item.consumable = true;
        }

        // I don't want you to load on your own, Tinkerer Hammer! I will load you when I want to!
        public class DynamicLoader : ILoadable {
            public void Load(Mod mod) {
            }

            public void Unload() {
            }
        }

    }
}
