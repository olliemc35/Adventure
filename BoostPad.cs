using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using System.Diagnostics;

namespace Adventure
{
    public class BoostPad : AnimatedGameObject
    {
        public AnimatedSprite animation_Activated;
        public float boostMultiplier;
        public BoostPad(Vector2 position, string filename, float boostMultiplier, AssetManager assetManager, ColliderManager colliderManager, Player player): base(position, filename, assetManager)
        {
            this.colliderManager = colliderManager;
            this.player = player;
            this.boostMultiplier = boostMultiplier;
            CollisionObject = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            animation_Activated = spriteSheet.CreateAnimatedSprite("Activated");
            idleHitbox.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (colliderManager.CheckForCollision(idleHitbox, player.idleHitbox) && player.CollidedOnBottom)
            {
                UpdatePlayingAnimation(animation_Activated);
                player.boosted = true; // This is set to false at the end of every frame by Player
                player.boostMultiplier = boostMultiplier;
            }
            else
            {
                UpdatePlayingAnimation(animation_Idle);
            }

        }

    }
}
