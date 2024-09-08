using FaeReforges.Content;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class FaeReforges : Mod
	{

        public override object Call(params object[] args) {
            if (args[0] is not string) { // We are not using assert here because of the unique error message!
                throw new ArgumentException("The first parameter of this mod's Mod.Call MUST be a string representing a valid command name!");
            }
            string command = (string)args[0];
            switch (command) {
                case "RegisterTinkererHammer":
                    Item item1 = AssertType<Item>(args, 1);
                    ReforgeHammerType type = new ReforgeHammerType();
                    ReforgeHammerRegistry.RegisterHammerType(item1, type);
                    return type;

                case "GetTinkererHammerItemIdForItem":
                    Item item2 = AssertType<Item>(args, 1);
                    return ReforgeHammerUtility.GetHammerItemType(item2);

                case "HasAnySummonsReforgedWith":
                    Player player1 = AssertType<Player>(args, 1);
                    int hammerID1 = AssertType<int>(args, 2);
                    return ReforgeHammerUtility.HasAnySummonHammer(player1, hammerID1);

                case "GetTinkererHammerItemIdForProjectile":
                    Projectile projectile1 = AssertType<Projectile>(args, 1);
                    return projectile1.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().GetHammerItemTypeOrNone();

                default:
                    throw new ArgumentException("The first parameter of this mod's Mod.Call MUST be a string representing a valid command name!");
            }
            // Never reaches here because of the Argument Exception in default above!
        }

        private static T AssertType<T>(object[] args, int argnr) {
            if (args[argnr] is not T) {
                throw new ArgumentException("Argument #" + argnr + " must be of type " + typeof(T).Name + "!");
            }
            return (T)args[argnr];
        }

    }
}
