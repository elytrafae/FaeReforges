using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    internal class MiscSpritesSystem : ModSystem {

        public static Asset<Texture2D> PX;
        public static Asset<Texture2D> WhipFrenzyBar;

        public override void Load() {
            PX = GetModSprite("px");
            WhipFrenzyBar = GetModSprite("WhipFrenzyBar");
        }

        public override void Unload() {
            PX = null;
            WhipFrenzyBar = null;
        }

        public Asset<Texture2D> GetModSprite(String name) {
            return ModContent.Request<Texture2D>(Mod.Name + "/Assets/Sprites/" + name);
        }

    }
}
