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
        public AnimatedGameObject platform;
        public float climbingSpeed = 120;
        public float wallJumpSpeed = 120;

        public ClimbingState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                player.velocity.X = 0;
                player.velocity.Y = 0;
                enterStateFlag = false;
            }


            UpdateAnimations();

            UpdateVelocityAndDisplacement();

            player.colliderManager.AdjustForCollisionWithClimable(player, platform, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);




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

                if (player.CollidedOnLeft)
                {
                    player.velocity.X = wallJumpSpeed;
                    player.velocity.Y = -1 * player.jumpSpeed;

                }
                else if (player.CollidedOnRight)
                {
                    player.velocity.X = -wallJumpSpeed;
                    player.velocity.Y = -1 * player.jumpSpeed;
                }
                else if (player.CollidedOnTop)
                {
                    player.velocity.Y = 0;
                }

                return;

            }


            if (player.idleHitbox.rectangle.Y <= platform.idleHitbox.rectangle.Y - player.distanceCanStartClimbing)
            {
                if (player.CollidedOnRight && (player.spriteDirectionX == 1 || player.spriteDirectionY == -1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                    player.velocity.Y = 0;
                    exits = Exits.exitToNormalState;
                    return;
                }

                if (player.CollidedOnLeft && (player.spriteDirectionX == -1 || player.spriteDirectionY == -1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                    player.velocity.Y = 0;
                    exits = Exits.exitToNormalState;
                    return;
                }
            }



        }

        public void TransitionFromSidesToUnderneath()
        {
            if (player.idleHitbox.rectangle.Y >= platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing)
            {
                if (player.CollidedOnRight && (player.spriteDirectionX == 1 || player.spriteDirectionY == 1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                }

                if (player.CollidedOnLeft && (player.spriteDirectionX == -1 || player.spriteDirectionY == 1))
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                }
            }

        }

        public void TransitionFromUnderneathToSides()
        {
            if (player.CollidedOnTop)
            {
                if (player.idleHitbox.rectangle.X >= platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;


                }

                if (player.idleHitbox.rectangle.X <= platform.idleHitbox.rectangle.X - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;
                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                }
            }
        }


        public override void UpdateVelocityAndDisplacement()
        {

            if (player.CollidedOnRight)
            {
                player.velocity.Y = climbingSpeed * player.spriteDirectionY;
                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
            }
            else if (player.CollidedOnLeft)
            {
                player.velocity.Y = climbingSpeed * player.spriteDirectionY;
                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
            }
            else if (player.CollidedOnTop)
            {
                player.velocity.X = climbingSpeed * player.spriteDirectionX;
                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
            }

            TransitionFromSidesToUnderneath();
            TransitionFromUnderneathToSides();



            player.displacement = player.velocity * player.deltaTime;

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
            if (player.CollidedOnLeft)
            {
                player.UpdatePlayingAnimation(player.animation_SlideLeft);

            }
            else if (player.CollidedOnTop)
            {
                player.UpdatePlayingAnimation(player.animation_ClimbTop);
            }
            else if (player.CollidedOnRight)
            {
                player.UpdatePlayingAnimation(player.animation_SlideRight);

            }


        }

    }
}
