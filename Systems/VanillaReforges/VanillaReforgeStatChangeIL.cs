using FaeReforges.Content.Reforges;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.VanillaReforges {
    public class VanillaReforgeStatChangeIL : ModSystem {

        public override void Load() {
            On_Item.TryGetPrefixStatMultipliersForItem += On_Item_TryGetPrefixStatMultipliersForItem;
        }

        // Friendship ended with IL editing
        // Now Hooks are my best friend :3
        private bool On_Item_TryGetPrefixStatMultipliersForItem(On_Item.orig_TryGetPrefixStatMultipliersForItem orig, Item self, int rolledPrefix, out float dmg, out float kb, out float spd, out float size, out float shtspd, out float mcst, out int crt) {

            if (rolledPrefix < DynamicReforgeLoader.vanillaOverrides.Length && rolledPrefix >= 0 && DynamicReforgeLoader.vanillaOverrides[rolledPrefix] != null) {
                DynamicReforgeLoader.vanillaOverrides[rolledPrefix].ApplyTo(out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt);
                return (dmg == 1f || Math.Round((double)((float)self.damage * dmg)) != (double)self.damage) &&
                    (spd == 1f || Math.Round((double)((float)self.useAnimation * spd)) != (double)self.useAnimation) &&
                    (mcst == 1f || Math.Round((double)((float)self.mana * mcst)) != (double)self.mana) &&
                    (kb == 1f || self.knockBack != 0f);
            }

            return orig(self, rolledPrefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt);
        }

    }
}
