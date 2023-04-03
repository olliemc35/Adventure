using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class LaunchPad : MovingSprite
    {
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
            CollisionSprite = true;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            idleHitbox.rectangle.Height = 12;
            idleHitbox.rectangle.Y += 4;
            idleHitbox.isActive = true;

            initialHeight = idleHitbox.rectangle.Height;
            initialY = idleHitbox.rectangle.Y;

        }

        public override void Update(GameTime gameTime)
        {
            ManageAnimations();

            if (needToJumpAgain)
            {
                //Debug.WriteLine("here");

                if (!spriteCollider.CheckForCollision(References.player.idleHitbox, idleHitbox))
                {
                    needToJumpAgain = false;
                }
            }
            else
            {
                // Not capturing behaviour as player is collided on ground
                if (spriteCollider.CheckForCollision(References.player.idleHitbox, idleHitbox) && References.player.SpriteCollidedOnBottom)
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

            if (spriteCollider.CheckForCollision(References.player.idleHitbox, idleHitbox) && References.player.SpriteCollidedOnBottom)
            {
                References.player.spriteVelocity.Y = -launchSpeed * launchSpeedMultiplier;
                References.player.launchFlag = true;
            }
        }



        public override void ManageAnimations()
        {
            if (compress)
            {
                animatedSprite_Idle.Play("Compress");
                currentFrame = frameAndTag["Compress"].From;
                tagOfCurrentFrame = "Compress";
            }
            else if (breakOutOfCompressFlag)
            {
                breakOutOfCompressFlag = false;
                idleHitbox.rectangle.Y = initialY;
                idleHitbox.rectangle.Height = initialHeight;
                launchSpeedMultiplier = 1;

                animatedSprite_Idle.Play("Idle");
                currentFrame = frameAndTag["Idle"].From;
                tagOfCurrentFrame = "Idle";
            }
            else if (launchFlag)
            {
                animatedSprite_Idle.Play("Launch");
                currentFrame = frameAndTag["Launch"].From;
                tagOfCurrentFrame = "Launch";
            }
            else
            {
                animatedSprite_Idle.Play("Idle");
                currentFrame = frameAndTag["Idle"].From;
                tagOfCurrentFrame = "Idle";
            }


            animatedSprite_Idle.OnFrameEnd = () =>
            {
                if (tagOfCurrentFrame == "Compress")
                {
                    idleHitbox.rectangle.Y += 1;
                    idleHitbox.rectangle.Height -= 1;
                    References.player.spritePosition.Y += 1;
                    launchSpeedMultiplier += launchSpeedMultiplierIncrement;

                }
            };



            //animatedSprite.OnAnimationEnd = () =>
            //{
            //    Debug.WriteLine("here");


            //    if (tagOfCurrentFrame == "Launch")
            //    {
            //        animatedSprite.Play("Idle");
            //        currentFrame = frameAndTag["Idle"].From;
            //        tagOfCurrentFrame = "Idle";
            //    }

            //    if (tagOfCurrentFrame == "Compress")
            //    {
            //        Debug.WriteLine("here");
            //        compress = false;

            //        animatedSprite.Play("Idle");
            //        currentFrame = frameAndTag["Idle"].From;
            //        tagOfCurrentFrame = "Idle";

            //        idleHitbox.rectangle.Y = initialY;
            //        idleHitbox.rectangle.Height = initialHeight;
            //        References.player.spritePosition.Y = idleHitbox.rectangle.Y - References.player.idleHitbox.rectangle.Height - References.player.idleHitbox.offsetY;
            //        References.player.idleHitbox.rectangle.Y = (int)References.player.spritePosition.Y + References.player.idleHitbox.offsetY;
            //        launchSpeedMultiplier = 1;

            //        needToJumpAgain = true;

            //    }

            //};

            animatedSprite_Idle.OnAnimationLoop = () =>
            {
                if (tagOfCurrentFrame == "Launch")
                {
                    animatedSprite_Idle.Play("Idle");
                    currentFrame = frameAndTag["Idle"].From;
                    tagOfCurrentFrame = "Idle";

                    animatedSprite_Idle.OnAnimationLoop = null;

                }

                if (tagOfCurrentFrame == "Compress")
                {
                    compress = false;

                    animatedSprite_Idle.Play("Idle");
                    currentFrame = frameAndTag["Idle"].From;
                    tagOfCurrentFrame = "Idle";

                    idleHitbox.rectangle.Y = initialY;
                    idleHitbox.rectangle.Height = initialHeight;
                    References.player.spritePosition.Y = idleHitbox.rectangle.Y - References.player.idleHitbox.rectangle.Height - References.player.idleHitbox.offsetY;
                    References.player.idleHitbox.rectangle.Y = (int)References.player.spritePosition.Y + References.player.idleHitbox.offsetY;
                    launchSpeedMultiplier = 1;

                    needToJumpAgain = true;

                    animatedSprite_Idle.OnAnimationLoop = null;

                }


            };

        }

    }
}
