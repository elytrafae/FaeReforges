using FaeReforges.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammers {
    internal class ReforgeHammerEnhancedGlobalProjectile : GlobalProjectile {

        private int hammerItemId = ItemID.None;
        private int createdByItemId = ItemID.None;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            Item createdBy = null;
            if (source is EntitySource_ItemUse itemUseSource) {
                createdBy = itemUseSource.Item;
            } else if (source is IEntitySource_WithStatsFromItem statsSource) {
                createdBy = statsSource.Item;
            } else if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                ReforgeHammerEnhancedGlobalProjectile modProj = parentProj.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>();
                createdByItemId = modProj.createdByItemId;
                hammerItemId = modProj.hammerItemId;
            }
            if (createdBy != null) {
                createdByItemId = createdBy.type;
                hammerItemId = createdBy.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammerItemTypeOrNone();
                
            }
            if (hammerItemId > ItemID.None) {
                ReforgeHammerRegistry.GetHammerTypeForItemType(hammerItemId).onCreateProjectile(createdByItemId, projectile, source);
            }
        }

        private ReforgeHammerType? Hammer {
            get {
                return ReforgeHammerRegistry.GetHammerTypeForItemType(hammerItemId);
            }
        }

        // The following is for melee hits only. For projectiles, see ReforgeHammerEnhancedGlobalProjectile
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            Hammer?.changeWeaponDealDamageNpc(createdByItemId, Main.player[projectile.owner], target, ref modifiers);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            Hammer?.changeWeaponDealDamagePvp(createdByItemId, Main.player[projectile.owner], target, ref modifiers);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            Hammer?.onWeaponDealDamageNpc(createdByItemId, Main.player[projectile.owner], target, hit, damageDone);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            Hammer?.onWeaponDealDamagePvp(createdByItemId, Main.player[projectile.owner], target, info);
        }

        public int GetHammerItemTypeOrNone() {
            return hammerItemId;
        }

        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) {
            // We only care if the hammer is defined!
            if (hammerItemId > ItemID.None) {
                binaryWriter.Write(true);
                binaryWriter.Write(hammerItemId);
                binaryWriter.Write(createdByItemId);
            } else {
                binaryWriter.Write(false);
            }
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) {
            if (binaryReader.ReadBoolean()) {
                hammerItemId = binaryReader.ReadInt32();
                createdByItemId = binaryReader.ReadInt32();
            }
        }

    }
}
