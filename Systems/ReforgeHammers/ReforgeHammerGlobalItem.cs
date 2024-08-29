using FaeReforges.Content;
using FaeReforges.Content.HammerTypes;
using FaeReforges.Content.Items;
using FaeReforges.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers
{
    public class ReforgeHammerGlobalItem : GlobalItem {

        public override bool InstancePerEntity => true;

        // null means that the item was not reforged with any hammer (could be naturally generated with a prefix)
        AbstractHammerType hammerType = null;

        const string HAMMER_SAVE_NAME = "elytrafae_ReforgeHammer";

        public override void SaveData(Item item, TagCompound tag) {
            if (hammerType != null) {
                tag.Add(HAMMER_SAVE_NAME, hammerType.FullName);
            }
        }

        public override void LoadData(Item item, TagCompound tag) {
            if (tag.TryGet(HAMMER_SAVE_NAME, out string hammerName)) {
                hammerType = ReforgeHammerSaveSystem.GetHammerTypeFromName(hammerName);
            }
        }

        public AbstractHammerType GetHammer() {
            return hammerType;
        }

        public void SetHammer(AbstractHammerType hammer) {
            hammerType = hammer;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (hammerType != null) {
                TooltipLine line = new TooltipLine(Mod, "ReforgeHammerType", AbstractHammerType.ReforgedWithTooltip.Format(hammerType.DisplayName));
                line.OverrideColor = hammerType.color;
                tooltips.Add(line);

                if (item.accessory) {
                    if (hammerType.AccessoryEffect.Value.Length > 0) {
                        tooltips.Add(new TooltipLine(Mod, "TinkererHammerAccessoryEffect", AbstractHammerType.AccessoryEffectPrefix.Format(hammerType.AccessoryEffect)));
                    }
                } else {
                    if (hammerType.WeaponEffect.Value.Length > 0) {
                        tooltips.Add(new TooltipLine(Mod, "TinkererHammerWeaponEffect", AbstractHammerType.WeaponEffectPrefix.Format(hammerType.WeaponEffect)));
                    }
                }
            }
            
        }

        public override bool CanReforge(Item item) {
            NPC tinkerer = GetGoblinTinkerer();
            if (tinkerer == null) {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("No Goblin Tinkerer anywhere . . . ? What happened?!"), Color.Red);
                return false;
            }
            return false;
            if (!HasTinkererAnvilNearby(tinkerer.Center)) {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Sorry, but I can't do reforges without a Tinkerer Anvil nearby!"), Color.Red);
                return false;
            }
            return true;
        }


        private NPC? GetGoblinTinkerer() {
            foreach (NPC npc in Main.ActiveNPCs) {
                if (npc.type == NPCID.GoblinTinkerer) {
                    return npc;
                }
            }
            return null;
        }

        private const int RADIUS = 200;

        private bool HasTinkererAnvilNearby(Vector2 pos) {
            int type = ModContent.TileType<Content.Tiles.TinkererAnvil>();
            Rectangle rect = new Rectangle((int)pos.X - RADIUS / 2, (int)pos.Y - RADIUS / 2, RADIUS, RADIUS);
            for (int i = rect.Left; i <= rect.Right; i++) {
                for (int j = rect.Top; j <= rect.Bottom; j++) {
                    if (WorldGen.InWorld(i, j) && WorldGen.TileType(i, j) == type) {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
