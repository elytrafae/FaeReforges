using FaeReforges.Systems.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges {
    public class DynamicReforgeLoader : ILoadable {
        public void Load(Mod mod) {
            
            string json = Encoding.UTF8.GetString(mod.GetFileBytes("Assets/BalancingData/Reforges.json"));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            if (ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                LoadSummoner(jsonObj.Summons, mod);
            }
            
        }

        public void Unload() {
        }

        private void LoadSimple(JArray reforges, Mod mod, PrefixCategory category) {
            for (int i = 0; i < reforges.Count; i++) {
                dynamic reforge = reforges[i];
                string name = reforge["Modifier Name"];
                float damage = GetOrDefault<float>(reforge, "Damage", 0f); // TODO: Continue this
                float knockback = GetOrDefault<float>(reforge, "Knockback", 0f);
                float speed = GetOrDefault<float>(reforge, "Speed", 0);
                float size = GetOrDefault<float>(reforge, "Size", 0);
                float velocity = GetOrDefault<float>(reforge, "Shoot Velocity", 0);
                float mana = GetOrDefault<float>(reforge, "Mana Cost", 0);
                int crit = (int)(GetOrDefault<float>(reforge, "Crit", 0) * 100);
                bool positive = reforge.Positive;
                mod.AddContent(new SimpleCustomReforgeTemplate(name, category, positive, damage, knockback, speed, size, velocity, mana, crit));
            }
        }

        private void LoadSummoner(JArray reforges, Mod mod) {
            for (int i = 0; i < reforges.Count; i++) {
                dynamic reforge = reforges[i];
                string name = reforge["Modifier Name"];
                float damage = GetOrDefault<float>(reforge, "Damage", 0f); // TODO: Continue this
                float knockback = GetOrDefault<float>(reforge, "Knockback", 0f);
                float frenzy = GetOrDefault<float>(reforge, "Whip Frenzy Charge", 0f);
                int crit = (int)(GetOrDefault<float>(reforge, "Crit", 0) * 100);
                float speed = GetOrDefault<float>(reforge, "Speed", 0);
                float cost = GetOrDefault<float>(reforge, "Occupancy Reduction", 0);
                bool positive = reforge.Positive;
                mod.AddContent(new SummonerPrefixTemplate(name, positive, damage, knockback, crit, speed, cost, frenzy));
            }
        }

        private static T GetOrDefault<T>(dynamic obj, string prop, T def) {
            try {
                return (T)obj[prop];
            } catch {
                return def;
            }
        }
    }
}
