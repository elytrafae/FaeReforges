using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    public class EnableWhipCritsDetour : ModSystem {

        private static Hook standardCritCalcsHook;
        private static Hook showStatTooltipLineHook;


        public override void Load() {
            Type summonMeleeSpeedType = typeof(SummonMeleeSpeedDamageClass);

            PropertyInfo propInfo = summonMeleeSpeedType.GetProperty("UseStandardCritCalcs", BindingFlags.Public | BindingFlags.Instance);
            standardCritCalcsHook = new Hook(propInfo.GetGetMethod(), UseStandardCritCalcs_Detour, applyByDefault: false);
            standardCritCalcsHook.Apply();

            MethodInfo methodInfo = summonMeleeSpeedType.GetMethod("ShowStatTooltipLine", BindingFlags.Public | BindingFlags.Instance);
            showStatTooltipLineHook = new Hook(methodInfo, ShowStatTooltipLine_Detour, applyByDefault: false);
            showStatTooltipLineHook.Apply();
        }

        public override void Unload() {
            standardCritCalcsHook?.Dispose();
            standardCritCalcsHook = null;
            showStatTooltipLineHook?.Dispose();
            showStatTooltipLineHook = null;
        }

        // This is refrenced in string form
        private bool UseStandardCritCalcs_Detour(Func<SummonMeleeSpeedDamageClass, bool> orig, SummonMeleeSpeedDamageClass self) {
            return true;
        }

        private bool ShowStatTooltipLine_Detour(Func<SummonMeleeSpeedDamageClass, Player, string, bool> orig, SummonMeleeSpeedDamageClass self, Player player, string lineName) {
            if (lineName == "CritChance") {
                return true;
            }
            return orig(self, player, lineName);
        }


    }

    internal class ShowStatTooltipLineClass {
        public bool ShowStatTooltipLine(Player player, string lineName) {
            return true;
            // Original code . . . ? How TF does this work?!
        }
    }

}
