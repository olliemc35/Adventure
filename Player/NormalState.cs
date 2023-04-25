using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class NormalState : State
    {

        public int playerControlCounter = 0;
        public int NumberOfFramesBeforePlayerRegainsControl = 5;

        public float TimeTakenToAccelerateToMaxSpeedX = (float)6 / 60;
        public float TimeTakenToDecelerateFromMaxSpeedX = (float)6 / 60;

        public float originalLambdaAccel;
        public float originalLambdaDecel;
        public float lambdaAccel;
        public float lambdaDecel;
        public float fallingLambdaMultiplier = 0.2f;

        public bool landedFlag = false;

        public enum StatesX
        {
            accelerateLeft,
            accelerateRight,
            decelerateLeft,
            decelerateRight,
            constantVelocityLeft,
            constantVelocityRight,
            constantVelocityRightUntilHitGround,
            constantVelocityLeftUntilHitGround,
            atRestX
        };

        public enum StatesY
        {
            falling,
            atRestY
        };

        public StatesX statesX;
        public StatesY statesY;



        public NormalState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {
            statesX = StatesX.atRestX;
            statesY = StatesY.atRestY;

            originalLambdaAccel = player.maxHorizontalSpeed / (TimeTakenToAccelerateToMaxSpeedX * TimeTakenToAccelerateToMaxSpeedX);
            originalLambdaDecel = player.maxHorizontalSpeed / (TimeTakenToDecelerateFromMaxSpeedX * TimeTakenToDecelerateFromMaxSpeedX);

            lambdaAccel = originalLambdaAccel;
            lambdaDecel = originalLambdaDecel;
        }

        public override void Deactivate()
        {
            statesX = StatesX.atRestX;
            statesY = StatesY.atRestY;
            base.Deactivate();
        }

        public override void Update(GameTime gameTime)
        {

            if (enterStateFlag)
            {
                enterStateFlag = false;

                UpdateAnimations();

                if (player.velocity.X > 0)
                {
                    statesX = StatesX.constantVelocityRightUntilHitGround;
                }
                else if (player.velocity.X < 0)
                {
                    statesX = StatesX.constantVelocityLeftUntilHitGround;
                }
                else
                {
                    if (player.directionX == 1)
                    {
                        statesX = StatesX.accelerateRight;

                    }
                    else if (player.directionX == -1)
                    {
                        statesX = StatesX.accelerateLeft;
                    }
                    else
                    {
                        statesX = StatesX.atRestX;
                    }
                }

                if (player.CollidedOnBottom && player.velocity.Y == 0)
                {
                    statesY = StatesY.atRestY;
                }
                else
                {
                    statesY = StatesY.falling;
                }


                UpdateVelocityAndDisplacement();
                //player.colliderManager.AdjustForCollisionsPlayer(player, screenManager.activeScreen.terrainHitboxes, screenManager.activeScreen.hazardHitboxes, 1, 10);
                player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
                return;

            }



            UpdateAnimations();
            UpdateStatesX();
            UpdateStatesY();
            UpdateVelocityAndDisplacement();
            //player.colliderManager.AdjustForCollisionsPlayer(player, screenManager.activeScreen.terrainHitboxes, screenManager.activeScreen.hazardHitboxes, 1, 10);

            player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
        }

        public override void UpdateVelocityAndDisplacement()
        {
            UpdateVelocityAndDisplacementX();
            UpdateVelocityAndDisplacementY();
        }

        public override void UpdateExits()
        {
            if (!player.CollidedOnBottom)
            {
                if (player.CollidedOnRight && player.directionX == 1)
                {
                    exits = Exits.exitToSlidingWallStateFacingRight;
                    return;
                }
                if (player.CollidedOnLeft && player.directionX == -1)
                {
                    exits = Exits.exitToSlidingWallStateFacingLeft;
                    return;
                }
            }



        }

        public void UpdateStatesX()
        {

            if (statesX == StatesX.constantVelocityRightUntilHitGround)
            {
                if (player.directionX == -1)
                {
                    if (playerControlCounter >= NumberOfFramesBeforePlayerRegainsControl)
                    {
                        playerControlCounter = 0;
                        statesX = StatesX.accelerateLeft;
                        return;
                    }
                }

                //if (player.spriteDirectionX == 0)
                //{
                //    if (playerControlCounter >= NumberOfFramesBeforePlayerRegainsControl)
                //    {
                //        playerControlCounter = 0;
                //        statesX = StatesX.decelerateRight;
                //        return;
                //    }
                //}

                playerControlCounter += 1;

                if (player.CollidedOnBottom)
                {
                    if (player.directionX == 1)
                    {
                        statesX = StatesX.constantVelocityRight;
                    }
                    else if (player.directionX == -1)
                    {
                        statesX = StatesX.accelerateLeft;
                    }
                    else
                    {
                        statesX = StatesX.decelerateRight;
                    }
                }

                return;
            }

            if (statesX == StatesX.constantVelocityLeftUntilHitGround)
            {

                if (player.directionX == 1)
                {
                    if (playerControlCounter >= NumberOfFramesBeforePlayerRegainsControl)
                    {
                        playerControlCounter = 0;
                        statesX = StatesX.accelerateRight;
                        return;
                    }
                }

                //if (player.spriteDirectionX == 0)
                //{
                //    if (playerControlCounter >= NumberOfFramesBeforePlayerRegainsControl)
                //    {
                //        playerControlCounter = 0;
                //        statesX = StatesX.decelerateLeft;
                //        return;
                //    }
                //}

                playerControlCounter += 1;

                if (player.CollidedOnBottom)
                {

                    if (player.directionX == -1)
                    {
                        statesX = StatesX.constantVelocityLeft;
                    }
                    else if (player.directionX == 1)
                    {
                        statesX = StatesX.accelerateRight;
                    }
                    else
                    {
                        statesX = StatesX.decelerateLeft;
                    }
                }

                return;
            }


            if (player.CollidedOnRight && player.directionX == 1)
            {
                statesX = StatesX.constantVelocityRight;
                return;
            }

            if (player.CollidedOnLeft && player.directionX == -1)
            {
                statesX = StatesX.constantVelocityLeft;
                return;
            }

            if (statesX == StatesX.atRestX && player.DirectionChangedX && player.directionX == 1)
            {
                statesX = StatesX.accelerateRight;
                return;
            }

            if (statesX == StatesX.atRestX && player.DirectionChangedX && player.directionX == -1)
            {
                statesX = StatesX.accelerateLeft;
                return;
            }

            if ((statesX == StatesX.accelerateRight || statesX == StatesX.constantVelocityRight) && player.DirectionChangedX && player.directionX == 0)
            {
                statesX = StatesX.decelerateRight;
                return;
            }

            if ((statesX == StatesX.accelerateLeft || statesX == StatesX.constantVelocityLeft) && player.DirectionChangedX && player.directionX == 0)
            {
                statesX = StatesX.decelerateLeft;
                return;
            }

            if ((statesX == StatesX.decelerateRight || statesX == StatesX.decelerateLeft || statesX == StatesX.accelerateLeft || statesX == StatesX.constantVelocityLeft) && player.DirectionChangedX && player.directionX == 1)
            {
                statesX = StatesX.accelerateRight;
                return;
            }

            if ((statesX == StatesX.decelerateRight || statesX == StatesX.decelerateLeft || statesX == StatesX.accelerateRight || statesX == StatesX.constantVelocityRight) && player.DirectionChangedX && player.directionX == -1)
            {
                statesX = StatesX.accelerateLeft;
                return;
            }

            if (statesX == StatesX.accelerateRight && player.velocity.X >= player.maxHorizontalSpeed)
            {
                player.velocity.X = player.maxHorizontalSpeed;
                statesX = StatesX.constantVelocityRight;
                return;
            }

            if (statesX == StatesX.accelerateLeft && player.velocity.X <= -player.maxHorizontalSpeed)
            {
                player.velocity.X = -player.maxHorizontalSpeed;
                statesX = StatesX.constantVelocityLeft;
                return;
            }

            if ((statesX == StatesX.decelerateLeft || statesX == StatesX.decelerateRight || statesX == StatesX.constantVelocityRightUntilHitGround || statesX == StatesX.constantVelocityLeftUntilHitGround) && Math.Abs(player.velocity.X) <= 0.01)
            {
                player.velocity.X = 0;
                statesX = StatesX.atRestX;
                return;
            }




        }


        public void UpdateVelocityAndDisplacementX()
        {

            if (statesY == StatesY.falling && lambdaAccel == originalLambdaAccel)
            {
                lambdaAccel *= fallingLambdaMultiplier;
                //lambdaDecel *= fallingLambdaMultiplier;
            }

            if (statesY == StatesY.atRestY && lambdaAccel != originalLambdaAccel)
            {
                lambdaAccel = originalLambdaAccel;
                //lambdaDecel = originalLambdaDecel;
            }



            switch (statesX)
            {
                case StatesX.constantVelocityLeft:
                    {
                        player.velocity.X += 0;
                        break;
                    }

                case StatesX.constantVelocityRight:
                    {
                        player.velocity.X += 0;
                        break;
                    }

                case StatesX.accelerateLeft:
                    {
                        player.velocity.X += -lambdaAccel * player.deltaTime;
                        player.velocity.X = Math.Max(player.velocity.X, -player.maxHorizontalSpeed);
                        break;
                    }
                case StatesX.accelerateRight:
                    {
                        player.velocity.X += lambdaAccel * player.deltaTime;
                        player.velocity.X = Math.Min(player.velocity.X, player.maxHorizontalSpeed);
                        break;
                    }
                case StatesX.decelerateLeft:
                    {
                        player.velocity.X += lambdaDecel * player.deltaTime;
                        player.velocity.X = Math.Min(player.velocity.X, 0);
                        break;
                    }
                case StatesX.decelerateRight:
                    {
                        player.velocity.X += -lambdaDecel * player.deltaTime;
                        player.velocity.X = Math.Max(player.velocity.X, 0);
                        break;
                    }
                case StatesX.constantVelocityLeftUntilHitGround:
                    {
                        player.velocity.X += 0;
                        break;
                    }
                case StatesX.constantVelocityRightUntilHitGround:
                    {
                        player.velocity.X += 0;
                        break;
                    }
                case StatesX.atRestX:
                    {
                        player.velocity.X = 0;
                        break;
                    }

            }

            player.displacement.X = player.velocity.X * player.deltaTime;


        }


        public void UpdateStatesY()
        {
            // Launched from bounce pad 
            if (player.launchFlag)
            {
                player.launchFlag = false;
                statesY = StatesY.falling;
                return;
            }

            // On the ground and press jump
            if (statesY == StatesY.atRestY && player.flagJumpButtonPressed)
            {
                statesY = StatesY.falling;
                player.velocity.Y = -player.jumpSpeed;
                return;
            }

            // On the ground and walk off an edge
            if (statesY == StatesY.atRestY && !player.CollidedOnBottom)
            {
                statesY = StatesY.falling;
                player.velocity.Y = 0;
                return;
            }

            // Falling in middair and hit ground
            if (statesY == StatesY.falling && player.CollidedOnBottom)
            {
                statesY = StatesY.atRestY;
                player.velocity.Y = 0;
                landedFlag = true;
                return;
            }



        }


        public void UpdateVelocityAndDisplacementY()
        {

            switch (statesY)
            {

                case StatesY.falling:
                    {
                        player.velocity.Y += player.gravityConstant * player.deltaTime;
                        player.velocity.Y = Math.Min(player.velocity.Y, player.maxVerticalSpeed);
                        break;
                    }
                case StatesY.atRestY:
                    {
                        player.velocity.Y = 0;
                        break;
                    }
            }

            player.displacement.Y = player.velocity.Y * player.deltaTime;


        }



        public override void UpdateAnimations()
        {

            if (statesY == StatesY.falling)
            {
                if (player.velocity.Y < 0)
                {
                    if (player.directionX == -1)
                    {
                        player.UpdatePlayingAnimation(player.animation_JumpLeft);
                    }
                    else if (player.directionX == 1) 
                    {
                        player.UpdatePlayingAnimation(player.animation_JumpRight);
                    }
                    else if (player.directionX == 0)
                    {
                        if (player.previousDirectionX == -1)
                        {
                            player.UpdatePlayingAnimation(player.animation_JumpLeft);
                        }
                        else if (player.previousDirectionX == 1)
                        {
                            player.UpdatePlayingAnimation(player.animation_JumpRight);
                        }
                    }
                }
                else
                {
                    if (player.directionX == -1)
                    {
                        player.UpdatePlayingAnimation(player.animation_FallingLeft);
                    }
                    else if (player.directionX == 1)
                    {
                        player.UpdatePlayingAnimation(player.animation_FallingRight);
                    }
                    else if (player.directionX == 0)
                    {
                        if (player.previousDirectionX == -1)
                        {
                            player.UpdatePlayingAnimation(player.animation_FallingLeft);
                        }
                        else if (player.previousDirectionX == 1)
                        {
                            player.UpdatePlayingAnimation(player.animation_FallingRight);
                        }
                    }
                }
            }
            else if (statesY == StatesY.atRestY)
            {
                if (landedFlag)
                {
                    landedFlag = false;

                    if (player.directionX == -1)
                    {
                        player.UpdatePlayingAnimation(player.animation_LandedLeft, 1);
                    }
                    else if (player.directionX == 1)
                    {
                        player.UpdatePlayingAnimation(player.animation_Landed, 1);
                    }
                    else if (player.directionX == 0)
                    {
                        if (player.previousDirectionX == -1)
                        {
                            player.UpdatePlayingAnimation(player.animation_LandedLeft, 1);
                        }
                        else if (player.previousDirectionX == 1)
                        {
                            player.UpdatePlayingAnimation(player.animation_Landed, 1);
                        }
                    }

                }

                if (!(player.animation_Landed.IsAnimating || player.animation_LandedLeft.IsAnimating))
                {
                    if (player.directionX == 1)
                    {
                        player.UpdatePlayingAnimation(player.animation_MoveRight);
                    }
                    else if (player.directionX == -1)
                    {
                        player.UpdatePlayingAnimation(player.animation_MoveLeft);
                    }
                    else if (player.directionX == 0)
                    {
                        if (player.previousDirectionX == 1)
                        {
                            player.UpdatePlayingAnimation(player.animation_Idle);
                        }
                        else if (player.previousDirectionX == -1)
                        {
                            player.UpdatePlayingAnimation(player.animation_IdleLeft);

                        }
                    }

                }






            }

            
       

        }

       

        

    }
}
