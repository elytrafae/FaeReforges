using FaeReforges.Systems.ReforgeHammerContent;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    internal class OnDodgeIL : ModSystem {

        public override void Load() {
            IL_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float += IL_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float;
        }

        private void IL_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float(MonoMod.Cil.ILContext il) {
            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(i => i.MatchCall(typeof(PlayerLoader).GetMethod("FreeDodge")));
            while (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdcR8(0), i => i.MatchRet())) {
                cursor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                //cursor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, 4);
                cursor.Emit(Mono.Cecil.Cil.OpCodes.Call, GetType().GetMethod("OnDodge", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static));
                cursor.Index++;
            }
            
        }

        public static void OnDodge(Player player) { 
            player.GetModPlayer<MyReforgeHammerPlayer>().OnDodge();
        }
    }
}
