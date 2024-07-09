using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems
{
    public class MinionOccupancyILStuff : ModSystem
    {

        public override void Load()
        {
            IL_Player.FreeUpPetsAndMinions += IL_Player_FreeUpPetsAndMinions;
        }

        private void IL_Player_FreeUpPetsAndMinions(ILContext il)
        {
            // The code is wrapped in a try catch in case of errors with the IL editing
            try
            {
                // Start the Cursor at the start
                var c = new ILCursor(il);
                // Try to find where the minion slot array is read
                c.GotoNext(i => i.MatchLdsfld<ItemID.Sets>("StaffMinionSlotsRequired"));
                c.GotoNext(i => i.MatchLdelemR4());

                // Move the cursor after it
                c.Index++;
                // Push the Item instance onto the stack
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);
                // Call a delegate using the float and Item from the stack.
                c.EmitDelegate<Func<float, Item, float>>((minionSlots, item) =>
                {
                    return minionSlots * item.GetGlobalItem<SummonerReforgesGlobalItem>().minionOccupancyMult;
                });
                // After the delegate, the stack will once again have a float and the ret instruction will return from this method
            }
            catch (Exception e)
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<FaeReforges>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }

    }
}
