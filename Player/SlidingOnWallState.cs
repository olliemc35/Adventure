using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace Adventure
{
    public class SlidingOnWallState : State
    {
        public float slideConstant = 30;
        public float wallJumpSpeed = 120;
        public bool facingRight = false;

        public int leavingWallCounter = 0;
        public int numberOfFramesToLeaveWall = 10;


        public SlidingOnWallState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                enterStateFlag = false;
            }

            UpdateAnimations();
            FindVelocityAndDisplacement();
            player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }


        public override void UpdateExits()
        {
            if (player.CollidedOnBottom)
            {
                player.velocity.Y = 0;
                player.displacement.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }


            if (facingRight && !player.CollidedOnRight)
            {
                player.velocity.Y = 0;
                player.displacement.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }

            if (!facingRight && !player.CollidedOnLeft)
            {
                player.velocity.Y = 0;
                player.displacement.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }

            if (facingRight)
            {
                if (player.spriteDirectionX == -1)
                {
                    if (leavingWallCounter >= numberOfFramesToLeaveWall)
                    {
                        player.velocity.Y = 0;
                        player.displacement.Y = 0;
                        exits = Exits.exitToNormalState;
                        return;
                    }

                    leavingWallCounter += 1;
                }
                else
                {
                    leavingWallCounter = 0;
                }
            }

            if (!facingRight)
            {
                if (player.spriteDirectionX == 1)
                {
                    if (leavingWallCounter >= numberOfFramesToLeaveWall)
                    {
                        player.velocity.Y = 0;
                        player.displacement.Y = 0;
                        exits = Exits.exitToNormalState;
                        return;
                    }

                    leavingWallCounter += 1;
                }
                else
                {
                    leavingWallCounter = 0;
                }
            }


            if (player.flagJumpButtonPressed)
            {
                if (facingRight)
                {
                    player.velocity.X = -wallJumpSpeed;
                }
                else
                {
                    player.velocity.X = wallJumpSpeed;
                }

                player.velocity.Y = -1 * player.jumpSpeed;
                exits = Exits.exitToNormalState;
                return;
            }
        }


        public void FindVelocityAndDisplacement()
        {
            player.velocity.X = 0;
            player.displacement.X = 0;
            player.velocity.Y = slideConstant;
            player.displacement.Y = player.velocity.Y * player.deltaTime;
        }


        public override void UpdateAnimations()
        {
            if (facingRight)
            {
                player.UpdatePlayingAnimation(player.animation_SlideRight);
            }
            else
            {
                player.UpdatePlayingAnimation(player.animation_SlideLeft);
            }
        }












    }
}
