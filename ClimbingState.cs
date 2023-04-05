using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class ClimbingState : State
    {
        public Sprite platform;
        public float climbingSpeed = 120;
        public float wallJumpSpeed = 120;

        public ClimbingState(Player player) : base(player)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                player.spriteVelocity.X = 0;
                player.spriteVelocity.Y = 0;
                enterStateFlag = false;
            }


            UpdateAnimations();

            UpdateVelocityAndDisplacement();

            player.spriteCollider.AdjustForCollisionWithClimable(player, platform, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);




        }

        public override void UpdateExits()
        {
            if (!player.climbButtonPressed)
            {
                exits = Exits.exitToNormalState;
                return;
            }

            if (player.flagJumpButtonPressed)
            {
                exits = Exits.exitToNormalState;

                if (player.SpriteCollidedOnLeft)
                {
                    player.spriteVelocity.X = wallJumpSpeed;
                    player.spriteVelocity.Y = -1 * player.jumpSpeed;

                }
                else if (player.SpriteCollidedOnRight)
                {
                    player.spriteVelocity.X = -wallJumpSpeed;
                    player.spriteVelocity.Y = -1 * player.jumpSpeed;
                }
                else if (player.SpriteCollidedOnTop)
                {
                    player.spriteVelocity.Y = 0;
                }

                return;

            }


            if (player.idleHitbox.rectangle.Y <= platform.idleHitbox.rectangle.Y - player.distanceCanStartClimbing)
            {
                if (player.SpriteCollidedOnRight && (player.spriteDirectionX == 1 || player.spriteDirectionY == -1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                    player.spriteVelocity.Y = 0;
                    exits = Exits.exitToNormalState;
                    return;
                }

                if (player.SpriteCollidedOnLeft && (player.spriteDirectionX == -1 || player.spriteDirectionY == -1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                    player.spriteVelocity.Y = 0;
                    exits = Exits.exitToNormalState;
                    return;
                }
            }



        }

        public void TransitionFromSidesToUnderneath()
        {
            if (player.idleHitbox.rectangle.Y >= platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing)
            {
                if (player.SpriteCollidedOnRight && (player.spriteDirectionX == 1 || player.spriteDirectionY == 1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                }

                if (player.SpriteCollidedOnLeft && (player.spriteDirectionX == -1 || player.spriteDirectionY == 1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                }
            }

        }

        public void TransitionFromUnderneathToSides()
        {
            if (player.SpriteCollidedOnTop)
            {
                if (player.idleHitbox.rectangle.X >= platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;


                }

                if (player.idleHitbox.rectangle.X <= platform.idleHitbox.rectangle.X - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;
                    player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                }
            }
        }


        public override void UpdateVelocityAndDisplacement()
        {

            if (player.SpriteCollidedOnRight)
            {
                player.spriteVelocity.Y = climbingSpeed * player.spriteDirectionY;
                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
            }
            else if (player.SpriteCollidedOnLeft)
            {
                player.spriteVelocity.Y = climbingSpeed * player.spriteDirectionY;
                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                player.spritePosition.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
            }
            else if (player.SpriteCollidedOnTop)
            {
                player.spriteVelocity.X = climbingSpeed * player.spriteDirectionX;
                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                player.spritePosition.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
            }

            TransitionFromSidesToUnderneath();
            TransitionFromUnderneathToSides();



            player.spriteDisplacement = player.spriteVelocity * player.deltaTime;

        }


        public override void UpdateAnimations()
        {
            //if (player.SpriteCollidedOnBottom)
            //{
            //    player.animatedSprite.Play("ClimbBottom");
            //    player.currentFrame = player.frameAndTag["ClimbBottom"].From;
            //    player.tagOfCurrentFrame = "ClimbBottom";
            //    player.TurnOffAllHitboxes();
            //    player.idleHitbox.isActive = true;
            //}
            //else 
            if (player.SpriteCollidedOnLeft)
            {
                player.nameOfCurrentAnimationSprite = "SlideLeft";
                //player.animatedSprite_Idle.Play("SlideLeft");
                //player.currentFrame = player.frameAndTag["SlideLeft"].From;
                //player.tagOfCurrentFrame = "SlideLeft";
                //player.TurnOffAllHitboxes();
                //player.idleHitbox.isActive = true;
            }
            else if (player.SpriteCollidedOnTop)
            {
                player.nameOfCurrentAnimationSprite = "ClimbTop";

                //player.animatedSprite_Idle.Play("ClimbTop");
                //player.currentFrame = player.frameAndTag["ClimbTop"].From;
                //player.tagOfCurrentFrame = "ClimbTop";
                //player.TurnOffAllHitboxes();
                //player.idleHitbox.isActive = true;
            }
            else if (player.SpriteCollidedOnRight)
            {
                player.nameOfCurrentAnimationSprite = "SlideRight";

                //player.animatedSprite_Idle.Play("SlideRight");
                //player.currentFrame = player.frameAndTag["SlideRight"].From;
                //player.tagOfCurrentFrame = "SlideRight";
                //player.TurnOffAllHitboxes();
                //player.idleHitbox.isActive = true;

            }


        }

    }
}
