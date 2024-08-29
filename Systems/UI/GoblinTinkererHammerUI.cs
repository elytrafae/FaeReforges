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
using FaeReforges.Systems.WhipFrenzy;
using SteelSeries.GameSense;
using FaeReforges.Systems.Config;
using FaeReforges.Content;
using System.Collections;

namespace FaeReforges.Systems.UI {
    internal struct HammerUIElement {
        public AbstractHammerType hammer;
        public UIImage image;
    }

    public class GoblinTinkererHammerUI : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        // TODO: Figure out how to make search bars, and slowly remove old code I used as the template

        private List<HammerUIElement> hammersUI = new List<HammerUIElement>();

        const int WIDTH = 300;
        const int HEIGHT = 300;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(0, 30f); 
            area.Top.Set(0, 30f); 
            area.Width.Set(WIDTH, 0f); 
            area.Height.Set(HEIGHT, 0f);

            Append(area);
        }

        private void PopulateAreaWithHammers() {
            IEnumerable allHammers = ModContent.GetContent<AbstractHammerType>();
            int i = 0;
            foreach (AbstractHammerType hammer in allHammers) {
                Console.WriteLine("Hammer :3");
                HammerUIElement hammerUI = new HammerUIElement();
                hammerUI.hammer = hammer;
                hammerUI.image = new UIImage(ModContent.Request<Texture2D>(hammer.Texture));
                hammerUI.image.Left.Set(36 * i, 0f);
                hammerUI.image.Top.Set(0, 0f);
                hammerUI.image.Width.Set(32, 0f);
                hammerUI.image.Height.Set(32, 0f);
                area.Append(hammerUI.image);
                hammersUI.Add(hammerUI);
                i++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (!Main.InReforgeMenu)
                return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);

            /*
            var modPlayer = Main.LocalPlayer.WhipFrenzyPlayer();
            // Calculate quotient
            float quotient = modPlayer.GetBarFill(); // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 9;
            hitbox.Y += 12;
            hitbox.Height -= 19;

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(hitbox.Left, hitbox.Y, (int)(hitbox.Width*quotient), hitbox.Height), modPlayer.IsFrenzyReady() ? fullColor : emptyColor);
            */
        }

        public override void Update(GameTime gameTime) {
            ClientConfig config = ModContent.GetInstance<ClientConfig>();
            if (!Main.InReforgeMenu)
                return;

            if (hammersUI.Count <= 0) {
                PopulateAreaWithHammers();
            }
            
            base.Update(gameTime);
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class GoblinTinkererHammerUISystem : ModSystem {
        private UserInterface GoblinTinkererHammerUserInterface;

        internal GoblinTinkererHammerUI GoblinTinkererHammerUIInstance;

        public static LocalizedText GoblinTinkererHammerText { get; private set; }

        public override void Load() {
            GoblinTinkererHammerUIInstance = new();
            GoblinTinkererHammerUserInterface = new();
            GoblinTinkererHammerUserInterface.SetState(GoblinTinkererHammerUIInstance);

            string category = "UI";
            GoblinTinkererHammerText ??= Mod.GetLocalization($"{category}.GoblinTinkererHammerText");
        }

        public override void UpdateUI(GameTime gameTime) {
            GoblinTinkererHammerUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "FaeReforges: Goblin Tinkerer Hammer UI",
                    delegate {
                        GoblinTinkererHammerUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
