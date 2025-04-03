using FaeReforges.Systems.UI;
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
        public static Asset<Texture2D> TooltipLineTop;
        public static Asset<Texture2D> TooltipLineMiddle;
        public static Asset<Texture2D> TooltipLineBottom;

        public static Asset<Texture2D> ShadowflamePickup;
        public static Asset<Texture2D> ManaStarPickup;

        public override void Load() {
            PX = GetModSprite("px");
            WhipFrenzyBar = GetModSprite("WhipFrenzyBar");
            TinkererHammerHandle = GetModSprite("TinkererHammerHandle");
            TinkererHammerHead = GetModSprite("TinkererHammerHead");
            StardustDyingBar = GetModSprite("StardustDyingBar");
            TooltipLineTop = GetModSprite("TooltipLineTop");
            TooltipLineMiddle = GetModSprite("TooltipLineMiddle");
            TooltipLineBottom = GetModSprite("TooltipLineBottom");

            ShadowflamePickup = GetModSprite("Pickups/Shadowflame");
            ManaStarPickup = GetModSprite("Pickups/ManaStar");
        }

        public override void Unload() {
            PX = null;
            WhipFrenzyBar = null;
            TinkererHammerHandle = null;
            TinkererHammerHead = null;
            StardustDyingBar = null;
            TooltipLineTop = null;
            TooltipLineMiddle = null;
            TooltipLineBottom = null;

            ShadowflamePickup = null;
            ManaStarPickup = null;
        }

        public Asset<Texture2D> GetModSprite(string name) {
            return ModContent.Request<Texture2D>(Mod.Name + "/Assets/Sprites/" + name);
        }

    }
}
