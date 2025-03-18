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
using FaeReforges.Systems;

namespace FaeReforges.Content.Reforges {
    public class DynamicReforgeLoader : ILoadable {
        public static VanillaReforgeOverrideData[] vanillaOverrides = new VanillaReforgeOverrideData[PrefixID.Count];

        public void Load(Mod mod) {
            vanillaOverrides = new VanillaReforgeOverrideData[PrefixID.Count];
            Stream stream = mod.GetFileStream("Assets/BalancingData/Reforges_v2.txt");

            using (StreamReader reader = new StreamReader(stream)) {
                string name;
                int tier;
                float[] values;

                // First section is Universal. Damage, Knockback and Crit
                while (ReadDataLine(reader, out name, out tier, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, tier, values[0], values[1], 0, 0, 0, 0, (int)(values[2] * 100));
                }

                // Second section is Common. Damage, Knockback, Crit and Speed
                while (ReadDataLine(reader, out name, out tier, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.AnyWeapon, tier, values[0], values[1], values[3], 0, 0, 0, (int)(values[2] * 100));
                }

                // Third section is Melee. Damage, Knockback, Crit, Speed and Size
                while (ReadDataLine(reader, out name, out tier, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.Melee, tier, values[0], values[1], values[3], values[4], 0, 0, (int)(values[2] * 100));
                }

                // Fourth section is Ranged. Damage, Knockback, Crit, Speed and Velocity
                while (ReadDataLine(reader, out name, out tier, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.Ranged, tier, values[0], values[1], values[3], 0, values[4], 0, (int)(values[2] * 100));
                }

                // Fifth section is Mage. Damage, Knockback, Crit, Speed and Mana Cost Reduction
                while (ReadDataLine(reader, out name, out tier, out values)) {
                    RegisterSimple(mod, name, PrefixCategory.Magic, tier, values[0], values[1], values[3], 0, 0, values[4], (int)(values[2] * 100));
                }

                // Final, optional section is Summoner. Damage, Knockback, Frenzy, Speed, Cost
                if (ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                    while (ReadDataLine(reader, out name, out tier, out values)) {
                        SummonerPrefixTemplate reforge = new SummonerPrefixTemplate(name, tier, values[0], values[1], values[2], values[3], values[4]);
                        mod.AddContent(reforge);
                        ReforgeTierSystem.SetPrefixTier(reforge.Type, tier);
                    }
                }
            }

        }

        private void RegisterSimple(Mod mod, string name, PrefixCategory category, int tier, float damage, float knockback, float speed, float size, float velocity, float mana, int crit) {
            /* Handled by the converter
            if (damage == 0 && knockback == 0 && speed == 0 && size == 0 && velocity == 0 && mana == 0 && crit == 0) {
                Console.WriteLine("Skipped loading override for " + name + ". Empty reforge data!");
                return;
            }
            */
            FieldInfo field = typeof(PrefixID).GetField(name);
            if (field != null) {
                int pre = (int)field.GetValue(null);
                vanillaOverrides[pre] = new VanillaReforgeOverrideData(damage, knockback, speed, size, velocity, mana, crit);
                ReforgeTierSystem.SetPrefixTier(pre, tier);
            } else {
                SimpleCustomReforgeTemplate modPrefix = new SimpleCustomReforgeTemplate(name, category, damage, knockback, speed, size, velocity, mana, crit);
                mod.AddContent(modPrefix);
                ReforgeTierSystem.SetPrefixTier(modPrefix.Type, tier);
            }
        }

        private bool ReadDataLine(StreamReader reader, out string name, out int tier, out float[] values) {
            string line = reader.ReadLine();
            if (line[0] == '#') {
                name = "";
                tier = 0;
                values = null;
                return false; // End of section
            }
            string[] bits = line.Split(' ');
            values = new float[bits.Length-2];
            name = bits[0];
            tier = int.Parse(bits[1]);
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
