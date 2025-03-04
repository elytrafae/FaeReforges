using FaeReforges.Content.Reforges;
using FaeReforges.Systems.Config;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace FaeReforges.Systems.VanillaReforges {
    internal class VanillaReforgeValueIL : ModSystem {

        public override void Load() {
            IL_Item.Prefix += IL_Item_Prefix;
        }

        private void IL_Item_Prefix(MonoMod.Cil.ILContext il) {
            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(i => i.MatchLdsfld(typeof(PrefixID).GetField("Count", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)));
            cursor.Index++;
            cursor.GotoNext(i => i.MatchLdsfld(typeof(PrefixID).GetField("Count", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)));

            cursor.EmitLdloc2();
            cursor.EmitLdloc(11);
            cursor.EmitDelegate(SetValue);
            cursor.EmitStloc(11);
        }

        public float SetValue(int prefix, float valueMult) {
            int tier = ReforgeTierSystem.GetPrefixTier(prefix);
            if (tier != 0) {
                return ReforgeTierSystem.GetValueMult(tier);
            }
            return valueMult;
        }
    }
}
