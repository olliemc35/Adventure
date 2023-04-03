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


        public SlidingOnWallState(Player player) : base(player)
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
            player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }


        public override void UpdateExits()
        {
            if (player.SpriteCollidedOnBottom)
            {
                player.spriteVelocity.Y = 0;
                player.spriteDisplacement.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }

            if (facingRight)
            {
                if (player.spriteDirectionX == -1)
                {
                    if (leavingWallCounter >= numberOfFramesToLeaveWall)
                    {
                        player.spriteVelocity.Y = 0;
                        player.spriteDisplacement.Y = 0;
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
                        player.spriteVelocity.Y = 0;
                        player.spriteDisplacement.Y = 0;
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
                    player.spriteVelocity.X = -wallJumpSpeed;
                }
                else
                {
                    player.spriteVelocity.X = wallJumpSpeed;
                }

                player.spriteVelocity.Y = -1 * player.jumpSpeed;
                exits = Exits.exitToNormalState;
                return;
            }
        }


        public void FindVelocityAndDisplacement()
        {
            player.spriteVelocity.X = 0;
            player.spriteDisplacement.X = 0;
            player.spriteVelocity.Y = slideConstant;
            player.spriteDisplacement.Y = player.spriteVelocity.Y * player.deltaTime;
        }


        public override void UpdateAnimations()
        {
            if (facingRight)
            {
                player.animatedSprite.Play("SlideRight");
                player.currentFrame = player.frameAndTag["SlideRight"].From;
                player.tagOfCurrentFrame = "SlideRight";
                player.TurnOffAllHitboxes();
                player.idleHitbox.isActive = true;
            }
            else
            {
                player.animatedSprite.Play("SlideLeft");
                player.currentFrame = player.frameAndTag["SlideLeft"].From;
                player.tagOfCurrentFrame = "SlideLeft";
                player.TurnOffAllHitboxes();
                player.idleHitbox.isActive = true;
            }
        }












    }
}
