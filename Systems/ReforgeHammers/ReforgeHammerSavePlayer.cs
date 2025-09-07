using System;
using System.Data;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerSavePlayer : ModPlayer {

        public Item[] reforgeHammerStorage = new Item[1] { new Item(0) };
        private const string HAMMER_SLOT_TAG = "ACTIVE_TINKERER_HAMMER";

        public override void SaveData(TagCompound tag) {
            tag.Add(HAMMER_SLOT_TAG, reforgeHammerStorage[0]);
        }

        public override void LoadData(TagCompound tag) {
            if (tag.TryGet(HAMMER_SLOT_TAG, out Item hammer)) {
                reforgeHammerStorage[0] = hammer;
            }
        }

        public Item GetSelectedHammer() {
            return reforgeHammerStorage[0];
        }

        public void SetSelectedHammer(Item hammer) {
            if (hammer == null) {
                return;
            }
            reforgeHammerStorage[0] = hammer;
        }

        public static Item GetSelectedHammer(Player player) {
            return player.GetModPlayer<ReforgeHammerSavePlayer>().GetSelectedHammer();
        }

        public static void SetSelectedHammer(Player player, Item item) {
            player.GetModPlayer<ReforgeHammerSavePlayer>().SetSelectedHammer(item);
        }

        public static Item GetSelectedHammerOfMyPlayer() {
            return GetSelectedHammer(Main.player[Main.myPlayer]);
        }

        public static void SetSelectedHammerMyPlayer(Item item) {
            SetSelectedHammer(Main.player[Main.myPlayer], item);
        }

        public static Item[] GetReforgeHammerStorageOfMyPlayer() { 
            return Main.player[Main.myPlayer].GetModPlayer<ReforgeHammerSavePlayer>().reforgeHammerStorage;
        }


        private static int PhantomColorTimer = 0;
        private static Color rainbowColor = new Color(255, 0, 0);
        private static readonly byte RAINBOW_SPEED = 2;
        private static readonly Color PhantomColor1 = new(0, 232, 112);
        private static readonly Color PhantomColor2 = new(0, 232, 204);

        public override void PostUpdate() {
            PhantomColorTimer += 2;
            if (PhantomColorTimer >= 360) {
                PhantomColorTimer -= 360;
            }
            UpdateRaindowColor();
        }

        public static Color GetPhantomColor() {
            float measure = (float)Math.Sin(MathHelper.ToRadians(PhantomColorTimer)) * 0.5f + 0.5f;
            Color tempColor1 = PhantomColor1 * measure;
            Color tempColor2 = PhantomColor2 * (1 - measure);
            return new Color(tempColor1.R + tempColor2.R, tempColor1.G + tempColor2.G, tempColor1.B + tempColor2.B, tempColor1.A + tempColor2.A);
        }

        public static void UpdateRaindowColor() {
            if (rainbowColor.R > 0 && rainbowColor.B == 0) {
                if (rainbowColor.R < RAINBOW_SPEED) {
                    byte diff = (byte)(RAINBOW_SPEED - rainbowColor.R);
                    rainbowColor.R = 0;
                    rainbowColor.G = 255;
                    rainbowColor.B = diff;
                } else {
                    rainbowColor.R -= RAINBOW_SPEED;
                    rainbowColor.G += RAINBOW_SPEED;
                }
            } else if (rainbowColor.G > 0 && rainbowColor.R == 0) {
                if (rainbowColor.G < RAINBOW_SPEED) {
                    byte diff = (byte)(RAINBOW_SPEED - rainbowColor.G);
                    rainbowColor.G = 0;
                    rainbowColor.B = 255;
                    rainbowColor.R = diff;
                } else {
                    rainbowColor.G -= RAINBOW_SPEED;
                    rainbowColor.B += RAINBOW_SPEED;
                }
            } else {
                if (rainbowColor.B < RAINBOW_SPEED) {
                    byte diff = (byte)(RAINBOW_SPEED - rainbowColor.B);
                    rainbowColor.B = 0;
                    rainbowColor.R = 255;
                    rainbowColor.G = diff;
                } else {
                    rainbowColor.B -= RAINBOW_SPEED;
                    rainbowColor.R += RAINBOW_SPEED;
                }
            }
        }

        public static Color GetRainbowColor() => rainbowColor;

        public static Color GetLighterRainbowColor() => new(rainbowColor.R / 255f * 0.7f + 0.3f, rainbowColor.G / 255f * 0.7f + 0.3f, rainbowColor.B / 255f * 0.7f + 0.3f); //rainbowColor.MultiplyRGB(new(0.7f, 0.7f, 0.7f)).ToVector3() + new Vector3(0.3f, 0.3f, 0.3f);

    }
}
