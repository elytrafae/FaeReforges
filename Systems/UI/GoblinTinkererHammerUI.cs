using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using FaeReforges.Content;
using Terraria.ID;
using FaeReforges.Systems.ReforgeHammers;
using FaeReforges.Systems.UI.UIElements;
using Terraria.GameContent.UI;
using FaeReforges.Content.Items;

namespace FaeReforges.Systems.UI {

    public class GoblinTinkererHammerUI : UIState {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        private GoblinTinkererHammerUIItemSlot slot;
        private UIText mainTextElement;
        private UIText tierTextElement;
        private UIText filterTextElement;

        const int WIDTH = 200;
        const int HEIGHT = 200;
        const int TEXT_LINE_DISTANCE = 25;
        const int FIRST_LINE_PIXELS = 10;

        public override void OnInitialize() {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(50, 0f); 
            area.Top.Set(320, 0f); 
            area.Width.Set(WIDTH, 0f); 
            area.Height.Set(HEIGHT, 0f);

            /* Slot code moved! */

            mainTextElement = new UIText("");
            mainTextElement.TextColor = Color.White;
            mainTextElement.Top.Set(FIRST_LINE_PIXELS, 0f);
            mainTextElement.Left.Set(55, 0f);
            area.Append(mainTextElement);

            tierTextElement = new UIText("");
            tierTextElement.TextColor = Color.White;
            tierTextElement.Top.Set(FIRST_LINE_PIXELS + TEXT_LINE_DISTANCE, 0f);
            tierTextElement.Left.Set(55, 0f);
            area.Append(tierTextElement);

            filterTextElement = new UIText("");
            filterTextElement.TextColor = Color.White;
            filterTextElement.Top.Set(FIRST_LINE_PIXELS + TEXT_LINE_DISTANCE*2, 0f);
            filterTextElement.Left.Set(55, 0f);
            area.Append(filterTextElement);

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
                slot.Top.Set(10, 0f);
                slot.Left.Set(5, 0f);
                area.Append(slot);
            }
            if (!Main.InReforgeMenu)
                return;

            string text;
            Color color = Color.White;
            tierTextElement.SetText("");
            filterTextElement.SetText("");
            Item item = ReforgeHammerSavePlayer.GetSelectedHammerOfMyPlayer();
            if (item == null || item.type == ItemID.None) {
                text = ReforgeHammerLocalization.UIInsertHammer.Value;
            } else {
                if (item.ModItem != null && item.ModItem is AbstractTinkererHammer hammer) {
                    text = item.AffixName();
                    color = ItemRarity.GetColor(item.rare);
                    tierTextElement.SetText(ReforgeHammerLocalization.HammerTier.Format(hammer.HammerTier));
                    filterTextElement.SetText(ReforgeHammerLocalization.HammerFilter.Format(hammer.ReforgeableCondition.Text));
                    filterTextElement.TextColor = Main.reforgeItem.IsAir ? Color.Gray : hammer.ReforgeableCondition.Predicate(Main.reforgeItem) ? Color.White : Color.Red;
                } else {
                    color = Color.Red;
                    text = ReforgeHammerLocalization.UIThatIsNotAHammer.Value;
                }
            }

            mainTextElement.SetText(text);
            mainTextElement.TextColor = color;

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
