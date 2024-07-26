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
            IL_Item.TryGetPrefixStatMultipliersForItem += IL_Item_TryGetPrefixStatMultipliersForItem;
        }

        private void IL_Item_TryGetPrefixStatMultipliersForItem(MonoMod.Cil.ILContext il) {
            try {
                // Start the Cursor at the start
                var c = new ILCursor(il);
                // Try to find where the minion slot array is read
                c.GotoNext(i => i.MatchSwitch(out ILLabel[] labels));
                ILCursor[] cursors;
                c.FindNext(out cursors, i => i.MatchRet());

                // Move the cursor before it?
                //c.Index--;
                // Push the Projectile instance onto the stack
                //c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                // Call a delegate using the int and Item from the stack.

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, 1);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 2);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 3);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 4);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 5);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 6);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 7);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarga, 8);
                c.Emit(Mono.Cecil.Cil.OpCodes.Call, typeof(VanillaReforgeStatChangeIL).GetMethod("ApplyCustomStats"));

                var c2 = new ILCursor(il);
                c2.GotoNext(i => i.MatchCallvirt<ModPrefix>("SetStats"));
                c2.Index++;
                ILLabel label = c2.MarkLabel();
                c.Emit(Mono.Cecil.Cil.OpCodes.Brtrue, /* Insert label to jump to here */ label);

                // After the delegate, the stack will once again have a float and the ret instruction will return from this method
            } catch (Exception e) {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<FaeReforges>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }

        public static bool ApplyCustomStats(int rolledPrefix, out float dmg, out float kb, out float spd, out float size, out float shtspd, out float mcst, out int crt) {
            dmg = 10f;
            kb = 1f;
            spd = 1f;
            size = 1f;
            shtspd = 1f;
            mcst = 1f;
            crt = 0;
            return true;
        }
    }
}
