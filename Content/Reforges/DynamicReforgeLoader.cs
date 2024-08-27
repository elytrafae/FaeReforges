using FaeReforges.Systems.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using System.Collections;
using Terraria.ID;
using System.Reflection;
using System.IO;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;

namespace FaeReforges.Content.Reforges {
    public class DynamicReforgeLoader : ILoadable {
        public static VanillaReforgeOverrideData[] vanillaOverrides = new VanillaReforgeOverrideData[PrefixID.Count];

        public void Load(Mod mod) {
            vanillaOverrides = new VanillaReforgeOverrideData[PrefixID.Count];
            Stream stream = mod.GetFileStream("Assets/BalancingData/Reforges.txt");

            using (StreamReader reader = new StreamReader(stream)) {
                string name;
                bool positive;
                float[] values;

                // First section is Universal. Damage, Knockback and Crit
                while (ReadDataLine(reader, out name, out positive, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, positive, values[0], values[1], 0, 0, 0, 0, (int)(values[2] * 100));
                }

                // Second section is Common. Damage, Knockback, Crit and Speed
                while (ReadDataLine(reader, out name, out positive, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, positive, values[0], values[1], values[3], 0, 0, 0, (int)(values[2] * 100));
                }

                // Third section is Melee. Damage, Knockback, Crit, Speed and Size
                while (ReadDataLine(reader, out name, out positive, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, positive, values[0], values[1], values[3], values[4], 0, 0, (int)(values[2] * 100));
                }

                // Fourth section is Ranged. Damage, Knockback, Crit, Speed and Velocity
                while (ReadDataLine(reader, out name, out positive, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, positive, values[0], values[1], values[3], 0, values[4], 0, (int)(values[2] * 100));
                }

                // Fifth section is Mage. Damage, Knockback, Crit, Speed and Mana Cost Reduction
                while (ReadDataLine(reader, out name, out positive, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, positive, values[0], values[1], values[3], 0, 0, values[4], (int)(values[2] * 100));
                }

                // Final, optional section is Summoner. Damage, Knockback, Frenzy, Speed, Cost
                if (ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                    while (ReadDataLine(reader, out name, out positive, out values)) {
                        mod.AddContent(new SummonerPrefixTemplate(name, positive, values[0], values[1], 0, values[3], values[4], values[2]));
                    }
                }
            }

        }

        private void RegisterSimple(Mod mod, string name, PrefixCategory category, bool positive, float damage, float knockback, float speed, float size, float velocity, float mana, int crit) {
            /* Handled by the converter
            if (damage == 0 && knockback == 0 && speed == 0 && size == 0 && velocity == 0 && mana == 0 && crit == 0) {
                Console.WriteLine("Skipped loading override for " + name + ". Empty reforge data!");
                return;
            }
            */
            FieldInfo field = typeof(PrefixID).GetField(name);
            if (field != null) {
                vanillaOverrides[(int)field.GetValue(null)] = new VanillaReforgeOverrideData(damage, knockback, speed, size, velocity, mana, crit);
            } else {
                mod.AddContent(new SimpleCustomReforgeTemplate(name, category, positive, damage, knockback, speed, size, velocity, mana, crit));
            }
        }

        private bool ReadDataLine(StreamReader reader, out string name, out bool positive, out float[] values) {
            string line = reader.ReadLine();
            if (line[0] == '#') {
                name = "";
                positive = false;
                values = null;
                return false; // End of section
            }
            string[] bits = line.Split(' ');
            values = new float[bits.Length-2];
            name = bits[0];
            positive = bits[1][0] == 'p';
            for (int i = 2; i < bits.Length; i++) {
                values[i-2] = float.Parse(bits[i]);
            }
            return true;
        }

        public void Unload() {
            vanillaOverrides = null;
        }

        
    }
}
