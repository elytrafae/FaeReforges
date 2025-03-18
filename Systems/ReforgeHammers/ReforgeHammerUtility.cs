using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace FaeReforges.Systems.ReforgeHammers {
    public static class ReforgeHammerUtility {

        public static int GetHammerItemType(Item item) {
            if (item == null || item.type == ItemID.None) {
                return ItemID.None;
            }
            return item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammerItemTypeOrNone();
        }

        public static bool HasAnySummonHammer(Player player, int hammerType) {
            foreach (Projectile proj in Main.ActiveProjectiles) {
                if (proj.owner == player.whoAmI && (proj.sentry || proj.minion) && proj.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().GetHammerItemTypeOrNone() == hammerType) {
                    return true;
                }
            }
            return false;
        }

        public static void ProcessAbilityLines(string text, List<TooltipLine> tooltips, string baseKey, Mod mod, LocalizedText prefix) {
            if (text.Length > 0) {
                string[] textLines = text.Split("\n");
                if (textLines.Length == 1) {
                    tooltips.Add(new TooltipLine(mod, baseKey, prefix.Format(text)));
                } else {
                    tooltips.Add(new TooltipLine(mod, baseKey + "0", prefix.Format("")));
                    for (int i = 0; i < textLines.Length; i++) {
                        tooltips.Add(new TooltipLine(mod, baseKey + (i + 1), "  " + textLines[i]));
                    }
                }
            }
        }

        public static void DrawAbilityTooltipLineThing(ReadOnlyCollection<DrawableTooltipLine> lines, Color lineColor, string lineId) {
            int i = 0;
            while (i < lines.Count && !lines[i].FullName.StartsWith("FaeReforges/" + lineId)) { i++; }

            if (i >= lines.Count) {
                return;
            }

            // First Weapon Ability line found!
            int firstIndex = i + 1; // Skip the first line
            int lastIndex = firstIndex;
            while (i < lines.Count && lines[i].FullName.StartsWith("FaeReforges/" + lineId)) {
                lastIndex = i;
                i++;
            }

            if (firstIndex >= lastIndex) {
                return;
            }

            // First and last lines found!
            DrawableTooltipLine firstLine = lines[firstIndex];
            DrawableTooltipLine lastLine = lines[lastIndex];
            Main.spriteBatch.Draw(MiscSpritesSystem.TooltipLineTop.Value, new Vector2(firstLine.X + 0, firstLine.Y - 2), null, lineColor, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(MiscSpritesSystem.TooltipLineMiddle.Value, new Vector2(firstLine.X + 0, firstLine.Y + 2), null, lineColor, 0, Vector2.Zero, new Vector2(1, lastLine.Y - firstLine.Y + 14), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(MiscSpritesSystem.TooltipLineBottom.Value, new Vector2(lastLine.X + 0, lastLine.Y + 16), null, lineColor, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
        }

    }
}
