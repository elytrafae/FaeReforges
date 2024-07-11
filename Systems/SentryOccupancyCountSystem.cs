using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    internal class SentryOccupancyCountSystem : ModSystem {

        public override void Load() {
            On_Player.UpdateMaxTurrets += On_Player_UpdateMaxTurrets;
        }

        private void On_Player_UpdateMaxTurrets(On_Player.orig_UpdateMaxTurrets orig, Player self) {
            // We intentionally won't call orig!
            if (Main.myPlayer != self.whoAmI)
                return;

            double sentrySlotsOccupied = 0f;
            List<Projectile> list = new List<Projectile>();
            for (int i = 0; i < 1000; i++) {
                if (Main.projectile[i].WipableTurret) {
                    Projectile proj = Main.projectile[i];
                    list.Add(proj);
                    sentrySlotsOccupied += proj.GetGlobalProjectile<SummonerReforgesGlobalProjectile>().sentryOccupancy;
                }
            }

            int num = 0;
            while (sentrySlotsOccupied > self.maxTurrets && ++num < 1000) {
                Projectile projectile = list[0];
                for (int j = 1; j < list.Count; j++) {
                    if (list[j].timeLeft < projectile.timeLeft)
                        projectile = list[j];
                }

                sentrySlotsOccupied -= projectile.GetGlobalProjectile<SummonerReforgesGlobalProjectile>().sentryOccupancy;
                projectile.Kill();
                list.Remove(projectile);
            }
        }
    }
}
