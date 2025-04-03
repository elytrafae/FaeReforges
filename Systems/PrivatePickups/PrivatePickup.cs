using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems.PrivatePickups {

    // NOTE: No ID system of any kind needed
    public class PrivatePickup : Entity {

        public Asset<Texture2D> texture = null;
        public Rectangle? frame = null;
        public Color color = Color.White;
        public float rotation = 0;
        public bool reactToPlayer = false; // NOTE: Only the local player can react to these!
        public bool reactToProjectiles = false; // NOTE: Only the local player's projectiles can react to these!

        public virtual void SetDefaults() {}

        public virtual void Update() {}

        // The player instance is always the local player, I just added the parameter for convenience and familiarity.
        // Returns if this pickup should be deleted or not.
        public virtual bool OnCollidePlayer(Player player) {
            return true;
        }

        // Returns if this pickup should be deleted or not.
        public virtual bool OnCollideProjectile(Projectile projectile) {
            return true;
        }

        public virtual void PreKill() {}


        // Implementation //

        public PrivatePickup() { active = false; }

        public void Draw(SpriteBatch spriteBatch) {
            if (texture != null) {
                Rectangle f = frame ?? new Rectangle(0, 0, texture.Width(), texture.Height());
                Vector2 scale = new(width/f.Width, height/f.Height);
                spriteBatch.Draw(texture.Value, position - Main.screenPosition, f, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        public void Kill() {
            PreKill();
            active = false;
        }

        public Rectangle getRect() => new Rectangle((int)position.X, (int)position.Y, width, height);

    }
}
