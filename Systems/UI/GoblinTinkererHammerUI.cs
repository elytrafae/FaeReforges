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
using static Terraria.UI.ItemSlot;
using FaeReforges.Systems.UI.UIElements;
using Terraria.GameContent.UI;

namespace FaeReforges.Systems.UI {

    public class GoblinTinkererHammerUI : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        private GoblinTinkererHammerUIItemSlot slot;
        private UIText textElement;

        const int WIDTH = 200;
        const int HEIGHT = 200;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(50, 0f); 
            area.Top.Set(320, 0f); 
            area.Width.Set(WIDTH, 0f); 
            area.Height.Set(HEIGHT, 0f);

            /* Slot code moved! */

            textElement = new UIText("");
            textElement.TextColor = Color.White;
            textElement.Top.Set(10, 0f);
            textElement.Left.Set(55, 0f);
            area.Append(textElement);

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
            if (slot == null) { // The slot init code is here because UI initializes before the player for some reason . . .
                slot = new GoblinTinkererHammerUIItemSlot();
                slot.Top.Set(0, 0f);
                slot.Left.Set(0, 0f);
                area.Append(slot);
            }
            if (!Main.InReforgeMenu)
                return;

            string text;
            Color color = Color.White;
            Item item = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (item == null || item.type == ItemID.None) {
                text = ReforgeHammerLocalization.UIInsertHammer.Value;
            } else {
                ReforgeHammerType hammerType = ReforgeHammerRegistry.GetHammerTypeForItemType(item.type);
                if (hammerType == null) {
                    color = Color.Red;
                    text = ReforgeHammerLocalization.UIThatIsNotAHammer.Value;
                } else {
                    text = item.AffixName();
                    color = ItemRarity.GetColor(item.rare);
                }
            }

            textElement.SetText(text);
            textElement.TextColor = color;

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
            // Vanilla: Inventory +1
            //layers.FindIndex(layer => { Console.WriteLine("Layer Name? " + layer.Name); return false; });
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex+1, new LegacyGameInterfaceLayer(
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
