﻿using FaeReforges.Systems.UI;
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
        public static Asset<Texture2D> TinkererHammerHandle;
        public static Asset<Texture2D> TinkererHammerHead;
        public static Asset<Texture2D> StardustDyingBar;

        public override void Load() {
            PX = GetModSprite("px");
            WhipFrenzyBar = GetModSprite("WhipFrenzyBar");
            TinkererHammerHandle = GetModSprite("TinkererHammerHandle");
            TinkererHammerHead = GetModSprite("TinkererHammerHead");
            StardustDyingBar = GetModSprite("StardustDyingBar");
        }

        public override void Unload() {
            PX = null;
            WhipFrenzyBar = null;
            TinkererHammerHandle = null;
            TinkererHammerHead = null;
            StardustDyingBar = null;
        }

        public Asset<Texture2D> GetModSprite(String name) {
            return ModContent.Request<Texture2D>(Mod.Name + "/Assets/Sprites/" + name);
        }

    }
}
