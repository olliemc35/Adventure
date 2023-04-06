using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class LaunchPad : MovingGameObject
    {
        public AnimatedSprite animation_Compress;
        public AnimatedSprite animation_Launch;

        public bool compress;
        public bool breakOutOfCompressFlag;

        public bool needToJumpAgain = false;

        public float launchSpeedMultiplier = 1;
        public float launchSpeedMultiplierIncrement = 0.1f;

        public float launchSpeed = 280;
        public bool launchFlag = false;


        public int initialHeight;
        public int initialY;

        public LaunchPad(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            CollisionObject = true;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            animation_Compress = spriteSheet.CreateAnimatedSprite("Compress");
            animation_Launch = spriteSheet.CreateAnimatedSprite("Launch");

            idleHitbox.rectangle.Height = 12;
            idleHitbox.rectangle.Y += 4;
            idleHitbox.isActive = true;

            initialHeight = idleHitbox.rectangle.Height;
            initialY = idleHitbox.rectangle.Y;

            animation_Compress.OnFrameEnd = (hello) =>
            {
                idleHitbox.rectangle.Y += 1;
                idleHitbox.rectangle.Height -= 1;
                References.player.position.Y += 1;
                launchSpeedMultiplier += launchSpeedMultiplierIncrement;
            };
          

            animation_Compress.OnAnimationLoop = (hello) =>
            {
                compress = false;
                UpdatePlayingAnimation(animation_Idle);


                idleHitbox.rectangle.Y = initialY;
                idleHitbox.rectangle.Height = initialHeight;
                References.player.position.Y = idleHitbox.rectangle.Y - References.player.idleHitbox.rectangle.Height - References.player.idleHitbox.offsetY;
                References.player.idleHitbox.rectangle.Y = (int)References.player.position.Y + References.player.idleHitbox.offsetY;
                launchSpeedMultiplier = 1;

                needToJumpAgain = true;

                animation_Compress.OnAnimationLoop = null;
            };

            animation_Launch.OnAnimationLoop = (hello) =>
            {
                UpdatePlayingAnimation(animation_Idle);
                animation_Launch.OnAnimationLoop = null;
            };

        }

        public override void Update(GameTime gameTime)
        {
            ManageAnimations();

            if (needToJumpAgain)
            {
                //Debug.WriteLine("here");

                if (!colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox))
                {
                    needToJumpAgain = false;
                }
            }
            else
            {
                // Not capturing behaviour as player is collided on ground
                if (colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox) && References.player.CollidedOnBottom)
                {
                    compress = true;
                }
                else
                {
                    compress = false;
                    breakOutOfCompressFlag = true;
                }
            }



            if (launchFlag)
            {
                Launch();
                launchFlag = false;
            }

            base.Update(gameTime);
        }

        public void Launch()
        {

            if (colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox) && References.player.CollidedOnBottom)
            {
                References.player.velocity.Y = -launchSpeed * launchSpeedMultiplier;
                References.player.launchFlag = true;
            }
        }



        public override void ManageAnimations()
        {
            if (compress)
            {
                UpdatePlayingAnimation(animation_Compress);
            }
            else if (breakOutOfCompressFlag)
            {
                breakOutOfCompressFlag = false;
                idleHitbox.rectangle.Y = initialY;
                idleHitbox.rectangle.Height = initialHeight;
                launchSpeedMultiplier = 1;

                UpdatePlayingAnimation(animation_Idle);
            }
            else if (launchFlag)
            {
                UpdatePlayingAnimation(animation_Launch);
            }
            else
            {
                UpdatePlayingAnimation(animation_Idle);
            }

            

           

        }

    }
}
