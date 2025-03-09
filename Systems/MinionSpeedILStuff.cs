using FaeReforges.Systems.ReforgeHammerContent;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems.MinionOccupancy {
    public class MinionSpeedILStuff : ModSystem {

        public override void Load() {
            IL_Projectile.Update += IL_Projectile_Update; ;
        }

        private void IL_Projectile_Update(ILContext il) {
            try {
                // Start the Cursor at the start
                var c = new ILCursor(il);
                // Try to find where the minion slot array is read
                c.GotoNext(i => i.MatchLdfld<Projectile>("extraUpdates"));

                // Move the cursor after it
                c.Index++;
                // Push the Projectile instance onto the stack
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                // Call a delegate using the int and Item from the stack.
                c.EmitDelegate<Func<int, Projectile, int>>((numUpdates, projectile) => {
                    // First, we do this:
                    //projectile.GetGlobalProjectile<MyReforgeHammerProjectile>().UpdatePreAI(projectile);

                    float rangedSpeed = 1f; // Putting ranged speed stuff due to technical stuff.
                    if (projectile.DamageType.CountsAsClass(DamageClass.Ranged) && projectile.TryGetOwner(out Player player)) {
                        rangedSpeed = player.GetModPlayer<MyReforgeHammerPlayer>().rangerVelocity.ApplyTo(1f);
                    }

                    // Actual number of updates is numUpdates + 1!
                    SummonerReforgesGlobalProjectile globProj = projectile.GetGlobalProjectile<SummonerReforgesGlobalProjectile>();
                    float minionSpeed = globProj.bonusSpeed * rangedSpeed;
                    float bonusUpdates = (numUpdates + 1) * minionSpeed;
                    int actualBonusUpdates = (int)bonusUpdates;
                    globProj.excessUpdates += (bonusUpdates - actualBonusUpdates);
                    int usedExcessUpdates = (int)globProj.excessUpdates;
                    globProj.excessUpdates -= usedExcessUpdates;
                    return numUpdates + actualBonusUpdates + usedExcessUpdates;
                });
                // After the delegate, the stack will once again have a float and the ret instruction will return from this method
            } catch (Exception e) {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<FaeReforges>(), il);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }
        }

    }
}
