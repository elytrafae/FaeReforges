﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using FaeReforges.Systems.WhipFrenzy;
using SteelSeries.GameSense;
using FaeReforges.Systems.Config;

namespace FaeReforges.Systems.UI {
    public class WhipFrenzyResourceBar : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private Color emptyColor;
        private Color fullColor;

        const int WIDTH = 182;
        const int HEIGHT = 60;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            ClientConfig config = ModContent.GetInstance<ClientConfig>();
            area = new UIElement();
            area.Left.Set(0, 0f); // Place the resource bar to the left of the hearts.
            area.Top.Set(30, 0f); // Placing it just a bit below the top of the screen.
            area.Width.Set(WIDTH, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(HEIGHT, 0f);

            barFrame = new UIImage(MiscSpritesSystem.WhipFrenzyBar); // Frame of our resource bar
            barFrame.Left.Set(22, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(138, 0f);
            barFrame.Height.Set(34, 0f);

            text = new UIText("0/0", 0.8f); // text to show stat
            text.HAlign = 0.5f;
            text.Width.Set(138, 0f);
            text.Height.Set(34, 0f);
            text.Top.Set(40, 0f);
            text.Left.Set(0, 0f);

            emptyColor = new Color(0, 41, 178); // A dark purple
            fullColor = new Color(0, 148, 255); // A light purple

            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (Main.LocalPlayer.numMinions <= 0)
                return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.WhipFrenzyPlayer();
            // Calculate quotient
            float quotient = modPlayer.GetBarFill(); // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 9;
            hitbox.Y += 12;
            hitbox.Height -= 19;

            /*
            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1) {
                // float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
            */
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(hitbox.Left, hitbox.Y, (int)(hitbox.Width*quotient), hitbox.Height), modPlayer.IsFrenzyReady() ? fullColor : emptyColor);
        }

        public override void Update(GameTime gameTime) {
            ClientConfig config = ModContent.GetInstance<ClientConfig>();
            if (Main.LocalPlayer.numMinions <= 0 && !config.displayWhipFrenzyAlways)
                return;

            
            area.Left.Set(-area.Width.Pixels/2f, config.UIOffsetHorizontal / 100f);
            area.Top.Set(-area.Height.Pixels/2f, config.UIOffsetVertical / 100f);
            var modPlayer = Main.LocalPlayer.WhipFrenzyPlayer();
            // Setting the text per tick to update and show our resource values.
            text.SetText(WhipFrenzyUISystem.WhipFrenzyResourceText.Format(modPlayer.GetBarFill()*100));
            base.Update(gameTime);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class WhipFrenzyUISystem : ModSystem {
        private UserInterface WhipFrenzyResourceBarUserInterface;

        internal WhipFrenzyResourceBar WhipFrenzyResourceBar;

        public static LocalizedText WhipFrenzyResourceText { get; private set; }

        public override void Load() {
            WhipFrenzyResourceBar = new();
            WhipFrenzyResourceBarUserInterface = new();
            WhipFrenzyResourceBarUserInterface.SetState(WhipFrenzyResourceBar);

            string category = "UI";
            WhipFrenzyResourceText ??= Mod.GetLocalization($"{category}.WhipFrenzyResourceText");
        }

        public override void UpdateUI(GameTime gameTime) {
            WhipFrenzyResourceBarUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Resource Bar",
                    delegate {
                        WhipFrenzyResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
