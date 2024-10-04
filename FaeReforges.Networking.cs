using FaeReforges.Content;
using FaeReforges.Systems.ReforgeHammerContent;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges {
    public partial class FaeReforges {

        // TODO: Finish this!
        internal enum MessageType : byte {
            MyReforgeHammerPlayerSync,
            SummonCombatText
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType) {
                case MessageType.MyReforgeHammerPlayerSync:
                    byte playerNumber = reader.ReadByte();
                    MyReforgeHammerPlayer examplePlayer = Main.player[playerNumber].GetModPlayer<MyReforgeHammerPlayer>();
                    examplePlayer.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server) {
                        // Forward the changes to the other clients
                        examplePlayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.SummonCombatText:
                    bool isNPC = reader.ReadBoolean();
                    ushort victimId = reader.ReadUInt16();
                    Color color = reader.ReadRGB();
                    string messageId = reader.ReadNullTerminatedString();

                    Entity victim = isNPC ? Main.npc[victimId] : Main.player[victimId];

                    if (Main.netMode == NetmodeID.Server) {
                        SendDisplayCombatText(victim, messageId, color, whoAmI);
                    } else {
                        // Don't distribute further if not server! Just display it and stfu
                        DisplayCombatText(victim, messageId, color);
                    }
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }

        // Works on Players and NPCs only!
        // fromWho = -1 means that this is the person who starts the send chain! Replace with Main.myPlayer
        public void SendDisplayCombatText(Entity victim, string messageId, Color color, int fromWho = -1) {
            DisplayCombatText(victim, messageId, color);
            if (Main.netMode == NetmodeID.SinglePlayer) {
                return; // Do not send if singleplayer
            }

            ushort victimId = (ushort)victim.whoAmI;
            ModPacket packet = this.GetPacket();
            packet.Write((byte)MessageType.SummonCombatText);
            packet.Write(victim is NPC);
            packet.Write(victimId);
            packet.WriteRGB(color);
            packet.WriteNullTerminatedString(messageId);

            if (fromWho == -1) {
                fromWho = Main.myPlayer;
            }
            if (Main.netMode == NetmodeID.Server) {
                // Redistribute packet among players
                packet.Send(-1, fromWho);
            } else {
                // Send initial packet to server
                packet.Send(255, -1);
            }
        }

        private void DisplayCombatText(Entity victim, string messageId, Color color) {
            Rectangle rectangle = new Rectangle((int)victim.Center.X + (int)Main.rand.NextFloat(-20, 20), (int)victim.position.Y + 30, 50, 50);
            CombatText.NewText(rectangle, color, this.GetLocalization(messageId).Value, true);
        }

        

    }
}
