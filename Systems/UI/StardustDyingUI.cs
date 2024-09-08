using Microsoft.Xna.Framework.Graphics;
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
using SteelSeries.GameSense;
using FaeReforges.Systems.Config;
using FaeReforges.Systems.ReforgeHammerContent;

namespace FaeReforges.Systems.UI {
    public class StardustDyingResourceBar : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private Color emptyColor;
        private Color fullColor;

        const int WIDTH = 600;
        const int HEIGHT = 50;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            //ClientConfig config = ModContent.GetInstance<ClientConfig>();
            area = new UIElement();
            area.Left.Set(-WIDTH/2, 0.5f);
            area.Top.Set(0, 0.8f);
            area.Width.Set(WIDTH, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(HEIGHT, 0f);

            barFrame = new UIImage(MiscSpritesSystem.StardustDyingBar); // Frame of our resource bar
            barFrame.Left.Set(0, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(WIDTH, 0f);
            barFrame.Height.Set(HEIGHT, 0f);

            text = new UIText(":3", 0.8f, true); // text to show stat
            text.TextColor = new Color(0, 180, 220);
            text.ShadowColor = new Color(0, 0, 0);
            text.HAlign = 0.5f;
            text.Width.Set(WIDTH, 0f);
            text.Height.Set(HEIGHT, 0f);
            text.Top.Set(12, 0f);
            text.Left.Set(0, 0f);

            emptyColor = new Color(25, 25, 25); // A dark purple
            fullColor = new Color(0, 240, 255); // A light purple

            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            var modPlayer = Main.LocalPlayer.GetModPlayer<MyReforgeHammerPlayer>();
            if (modPlayer.stardustTimeLeft <= 0)
                return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.GetModPlayer<MyReforgeHammerPlayer>();
            // Calculate quotient
            float quotient = ((float)modPlayer.stardustTimeLeft) / modPlayer.lastStardustTime; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 4;
            hitbox.Width -= 4;
            hitbox.Y += 4;
            hitbox.Height -= 4;

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
            int fullWidth = (int)(hitbox.Width * quotient);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(hitbox.Left, hitbox.Y, fullWidth, hitbox.Height), fullColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(hitbox.Left + fullWidth, hitbox.Y, hitbox.Width - fullWidth, hitbox.Height), emptyColor);
        }

        public override void Update(GameTime gameTime) {
            //ClientConfig config = ModContent.GetInstance<ClientConfig>();
            var modPlayer = Main.LocalPlayer.GetModPlayer<MyReforgeHammerPlayer>();
            if (modPlayer.stardustTimeLeft <= 0)
                return;

            // Setting the text per tick to update and show our resource values.
            int nr = modPlayer.stardustTimeLeft / 6;
            //int nr = (int)Math.Round(modPlayer.stardustTimeLeft / 6f);
            string formattedNr = (nr / 10) + "." + (nr % 10);
            text.SetText(StardustDyingUISystem.StardustDyingResourceText.Format(formattedNr));
            base.Update(gameTime);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class StardustDyingUISystem : ModSystem {
        private UserInterface StardustDyingResourceBarUserInterface;

        internal StardustDyingResourceBar StardustDyingResourceBar;

        public static LocalizedText StardustDyingResourceText { get; private set; }

        public override void Load() {
            StardustDyingResourceBar = new();
            StardustDyingResourceBarUserInterface = new();
            StardustDyingResourceBarUserInterface.SetState(StardustDyingResourceBar);

            string category = "UI";
            StardustDyingResourceText ??= Mod.GetLocalization($"{category}.StardustDyingResourceText");
        }

        public override void UpdateUI(GameTime gameTime) {
            StardustDyingResourceBarUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Resource Bar",
                    delegate {
                        StardustDyingResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
