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
using Terraria.ID;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Systems.UI {

    public class GoblinTinkererHammerUI : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        private UIItemSlot slot;

        const int WIDTH = 200;
        const int HEIGHT = 200;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(0,0.05f); 
            area.Top.Set(30, 0.3f); 
            area.Width.Set(WIDTH, 0f); 
            area.Height.Set(HEIGHT, 0f);

            slot = new UIItemSlot(ReforgeHammerSaveSystem.reforgeHammerStorage, 0, 1);
            slot.Top.Set(0, 0f);
            slot.Left.Set(0, 0f);
            //slot.Width.Set(slot.Width.Pixels + slot.Width.Pixels/2, 0f);
            //slot.Height.Set(slot.Height.Pixels + slot.Height.Pixels / 2, 0f);
            area.Append(slot);

            Append(area);
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
        }

        public override void Update(GameTime gameTime) {
            ClientConfig config = ModContent.GetInstance<ClientConfig>();
            if (!Main.InReforgeMenu)
                return;
            
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
