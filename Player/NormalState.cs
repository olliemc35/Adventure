using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class NormalState : State
    {

        public int playerControlCounter = 0;
        public int NumberOfFramesBeforePlayerRegainsControl = 13;

        public float framesTakenToAccelerateToMaxSpeedX = (float)6;
        public float framesTakenToDecelerateFromMaxSpeedX = (float)6;

        // The idea was to have different accel/decel rates depending on whether we were on the ground or in the air
        // So we would move less horizontally in the air say
        // Don't think this was really necessary in the end, but leave in for now...
        // Still might incorporate it - something about stopping (horizontally) virtually instantly when the direction key is released feels strange ... (try with wall jump)
        public float lambdaAccelOnGround;
        public float lambdaDecelOnGround;
        public float lambdaAccelFalling;
        public float lambdaDecelFalling;
        public float lambdaAccel;
        public float lambdaDecel;
        public float maxHorizontalSpeedOnGround = 180;
        public float maxHorizontalSpeedFalling = 180;
        public float maxHorizontalSpeed;



        public bool landedFlag = false;

        // If the player holds the jump button for longer than framesForMaxJump they will perform a bigger jump
        public int jumpButtonHeldCounter = 0;
        public int framesWhenJumpButtonReleased = 0;
        public int framesForMaxJump = 10;
        public bool flagJumpButtonReleased;


        // JumpBuffer - if the player has pressed the jump button within framesForJumpBuffer of touching the floor they will jump on the frame they land
        public int jumpBufferCounter = 0;
        public int framesForJumpBuffer = 3;
        public bool flagJumpBuffer;


        // CoyoteTime - the player can still jump if they are within framesForCoyoteTime of falling off a ledge
        public int coyoteTimeCounter = 0;
        public int framesForCoyoteTime = 5;
        public bool flagCoyoteTimeEnded = false;


        public float gravityConstant;

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
            pushingAgainstWallRight,
            pushingAgainstWallLeft,
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

            maxHorizontalSpeed = maxHorizontalSpeedOnGround;

            lambdaAccelOnGround = 60 * 60 * maxHorizontalSpeed / (framesTakenToAccelerateToMaxSpeedX * framesTakenToAccelerateToMaxSpeedX);
            lambdaDecelOnGround = 60 * 60 * maxHorizontalSpeed / (framesTakenToDecelerateFromMaxSpeedX * framesTakenToDecelerateFromMaxSpeedX);

            lambdaAccelFalling = lambdaAccelOnGround;
            lambdaDecelFalling = lambdaDecelOnGround;


            lambdaAccel = lambdaAccelOnGround;
            lambdaDecel = lambdaDecelOnGround;

            gravityConstant = player.gravityConstant;

        }

        public override void Deactivate()
        {
            statesX = StatesX.atRestX;
            statesY = StatesY.atRestY;
            base.Deactivate();
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine(playerControlCounter);
            //Debug.WriteLine(statesX);
            //Debug.WriteLine(player.displacement.X);

            //Debug.WriteLine(statesY);
            //Debug.WriteLine(framesWhenJumpButtonReleased);
            //Debug.WriteLine(lambdaDecel);
            //Debug.WriteLine(player.velocity.X);

            //if (statesY == StatesY.falling)
            //{
            //    Debug.WriteLine("here");
            //}

            if (enterStateFlag)
            {
                enterStateFlag = false;

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
                        if (!player.CollidedOnRight)
                        {
                            statesX = StatesX.accelerateRight;
                        }
                        else
                        {
                            statesX = StatesX.pushingAgainstWallRight;
                        }

                    }
                    else if (player.directionX == -1)
                    {
                        if (!player.CollidedOnLeft)
                        {
                            statesX = StatesX.accelerateLeft;
                        }
                        else
                        {
                            statesX = StatesX.pushingAgainstWallLeft;
                        }
                    }
                    else
                    {
                        statesX = StatesX.atRestX;
                    }
                }

                if (player.CollidedOnBottom && player.velocity.Y == 0)
                {
                    statesY = StatesY.atRestY;
                    SetHorizontalMovementValues();
                }
                else
                {
                    statesY = StatesY.falling;
                    SetHorizontalMovementValues();
                }

                UpdateAnimations();

                UpdateVelocityAndDisplacement();
                player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
                return;

            }



            UpdateStatesX();
            UpdateStatesY();
            UpdateAnimations();

            //Debug.WriteLine(statesX);

            //Debug.WriteLine(player.velocity.X);


            UpdateVelocityAndDisplacement();

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
                if (player.CollidedOnRight && player.directionX == 1 && player.velocity.Y >= 0)
                {
                    playerControlCounter = 0;
                    jumpButtonHeldCounter = 0;
                    jumpBufferCounter = 0;
                    framesWhenJumpButtonReleased = 0;
                    coyoteTimeCounter = 0;
                    gravityConstant = player.gravityConstant;
                    exits = Exits.exitToSlidingWallStateFacingRight;
                    return;
                }
                if (player.CollidedOnLeft && player.directionX == -1 && player.velocity.Y >= 0)
                {
                    playerControlCounter = 0;
                    jumpButtonHeldCounter = 0;
                    jumpBufferCounter = 0;
                    framesWhenJumpButtonReleased = 0;
                    coyoteTimeCounter = 0;
                    gravityConstant = player.gravityConstant;
                    exits = Exits.exitToSlidingWallStateFacingLeft;
                    return;
                }
            }



        }

        public void UpdateStatesX()
        {

            if (statesX == StatesX.constantVelocityRightUntilHitGround)
            {

                if (playerControlCounter < NumberOfFramesBeforePlayerRegainsControl)
                {
                    playerControlCounter += 1;
                    return;
                }
                else
                {



                    if (player.CollidedOnBottom)
                    {
                        playerControlCounter = 0;

                        if (player.directionX == 1)
                        {
                            statesX = StatesX.accelerateRight;
                            return;
                        }
                        else if (player.directionX == -1)
                        {
                            statesX = StatesX.accelerateLeft;
                            return;
                        }
                        else
                        {
                            statesX = StatesX.atRestX;
                            return;
                        }
                    }
                    else
                    {
                        if (player.directionX == -1)
                        {
                            statesX = StatesX.accelerateLeft;
                            playerControlCounter = 0;
                            return;
                        }

                        if (player.directionX == 0)
                        {
                            statesX = StatesX.decelerateRight;
                            playerControlCounter = 0;
                            return;
                        }
                    }


                    return;

                }


            }

            if (statesX == StatesX.constantVelocityLeftUntilHitGround)
            {

                if (playerControlCounter < NumberOfFramesBeforePlayerRegainsControl)
                {
                    playerControlCounter += 1;
                    return;
                }
                else
                {


                    if (player.CollidedOnBottom)
                    {
                        playerControlCounter = 0;

                        if (player.directionX == 1)
                        {
                            statesX = StatesX.accelerateRight;
                            return;
                        }
                        else if (player.directionX == -1)
                        {
                            statesX = StatesX.accelerateLeft;
                            return;
                        }
                        else
                        {
                            statesX = StatesX.atRestX;
                            return;
                        }
                    }
                    else
                    {
                        if (player.directionX == 1)
                        {
                            statesX = StatesX.accelerateRight;
                            playerControlCounter = 0;
                            return;
                        }

                        if (player.directionX == 0)
                        {
                            statesX = StatesX.decelerateLeft;
                            playerControlCounter = 0;
                            return;
                        }
                    }


                    return;

                }

            }

            if (player.CollidedOnBottom && player.CollidedOnRight && player.directionX == 1)
            {
                statesX = StatesX.pushingAgainstWallRight;
                return;
            }

            if (player.CollidedOnBottom && player.CollidedOnLeft && player.directionX == -1)
            {
                statesX = StatesX.pushingAgainstWallLeft;
                return;
            }

            if (statesX == StatesX.pushingAgainstWallRight)
            {
                if (player.directionX == -1)
                {
                    statesX = StatesX.accelerateLeft;
                    return;
                }

                if (player.directionX == 0)
                {
                    statesX = StatesX.atRestX;
                    return;
                }

                if (!player.CollidedOnRight)
                {
                    if (player.directionX == 1)
                    {
                        statesX = StatesX.accelerateRight;

                    }
                    else
                    {
                        statesX = StatesX.atRestX;
                    }

                    return;

                }
            }

            if (statesX == StatesX.pushingAgainstWallLeft)
            {
                if (player.directionX == 1)
                {
                    statesX = StatesX.accelerateRight;
                    return;
                }

                if (player.directionX == 0)
                {
                    statesX = StatesX.atRestX;
                    return;
                }

                if (!player.CollidedOnLeft)
                {

                    if (player.directionX == -1)
                    {
                        statesX = StatesX.accelerateLeft;

                    }
                    else
                    {
                        statesX = StatesX.atRestX;
                    }

                }
            }

            //if (player.CollidedOnRight && player.directionX == 1)
            //{
            //    //statesX = StatesX.atRestX;
            //    statesX = StatesX.constantVelocityRight;
            //    return;
            //}

            //if (player.CollidedOnLeft && player.directionX == -1)
            //{
            //    statesX = StatesX.constantVelocityLeft;
            //    return;
            //}

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

            if (statesX == StatesX.accelerateRight && player.velocity.X >= maxHorizontalSpeed)
            {
                player.velocity.X = maxHorizontalSpeed;
                statesX = StatesX.constantVelocityRight;
                return;
            }

            if (statesX == StatesX.accelerateLeft && player.velocity.X <= -maxHorizontalSpeed)
            {
                player.velocity.X = -maxHorizontalSpeed;
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
                        player.velocity.X = Math.Max(player.velocity.X, -maxHorizontalSpeed);
                        break;
                    }
                case StatesX.accelerateRight:
                    {
                        player.velocity.X += lambdaAccel * player.deltaTime;
                        player.velocity.X = Math.Min(player.velocity.X, maxHorizontalSpeed);
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
                case StatesX.pushingAgainstWallRight:
                    {
                        player.velocity.X = 0;
                        break;
                    }
                case StatesX.pushingAgainstWallLeft:
                    {
                        player.velocity.X = 0;
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
                SetHorizontalMovementValues();

                return;
            }

            if (jumpBufferCounter > 0)
            {

                if (player.CollidedOnBottom)
                {
                    flagJumpBuffer = true;
                }
                else
                {
                    if (jumpBufferCounter < framesForJumpBuffer)
                    {
                        jumpBufferCounter++;
                    }
                    else
                    {
                        jumpBufferCounter = 0;
                    }
                }

            }

            if (statesY == StatesY.falling && player.flagJumpButtonPressed)
            {
                jumpBufferCounter++;
            }


            // On the ground and press jump
            if ((statesY == StatesY.atRestY && player.flagJumpButtonPressed) || (statesY == StatesY.falling && coyoteTimeCounter > 0 && player.flagJumpButtonPressed) || flagJumpBuffer)
            {
                flagJumpBuffer = false;
                coyoteTimeCounter = 0;

                jumpButtonHeldCounter = 1;
                statesY = StatesY.falling;
                SetHorizontalMovementValues();


                if (player.boosted)
                {
                    player.velocity.Y = -player.boostMultiplier * player.jumpSpeed;
                }
                else
                {
                    player.velocity.Y = -player.jumpSpeed;
                }
                return;
            }

            if (statesY == StatesY.falling && jumpButtonHeldCounter > 0 && !player.CollidedOnBottom)
            {
                if (player.jumpButtonPressed)
                {
                    jumpButtonHeldCounter++;
                }
                else
                {
                    flagJumpButtonReleased = true;
                    framesWhenJumpButtonReleased = jumpButtonHeldCounter;
                    jumpButtonHeldCounter = 0;
                }

                if (player.velocity.Y >= 0)
                {
                    // Implementing half-gravity at the top of the jump like Celeste, but feels too floaty
                    //gravityConstant = 0.5f * player.gravityConstant;
                }

                return;
            }



            // On the ground and walk off an edge - transition to falling and start coyote time counter
            if (statesY == StatesY.atRestY && !player.CollidedOnBottom)
            {
                coyoteTimeCounter = 1;


                statesY = StatesY.falling;
                SetHorizontalMovementValues();

                player.velocity.Y = 0;
                return;
            }

            if (statesY == StatesY.falling && coyoteTimeCounter > 0 && !player.CollidedOnBottom)
            {
                if (coyoteTimeCounter < framesForCoyoteTime)
                {
                    coyoteTimeCounter++;
                }
                else
                {
                    coyoteTimeCounter = 0;
                }

                return;
            }

            // Falling in middair and hit ground
            if (statesY == StatesY.falling && player.CollidedOnBottom)
            {
                jumpButtonHeldCounter = 0;
                framesWhenJumpButtonReleased = 0;
                coyoteTimeCounter = 0;
                statesY = StatesY.atRestY;
                gravityConstant = player.gravityConstant;
                SetHorizontalMovementValues();

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
                        player.velocity.Y += gravityConstant * player.deltaTime;

                        if (flagJumpButtonReleased)
                        {
                            flagJumpButtonReleased = false;

                            if (framesWhenJumpButtonReleased < framesForMaxJump)
                            {
                                player.velocity.Y *= 0.2f;
                            }
                        }


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
                    if (statesX == StatesX.pushingAgainstWallRight)
                    {
                        player.UpdatePlayingAnimation(player.animation_PushRight);

                    }
                    else if (statesX == StatesX.pushingAgainstWallLeft)
                    {
                        player.UpdatePlayingAnimation(player.animation_PushLeft);

                    }
                    else
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



        public void SetHorizontalMovementValues()
        {
            if (statesY == StatesY.atRestY)
            {
                lambdaAccel = lambdaAccelOnGround;
                lambdaDecel = lambdaDecelOnGround;
                maxHorizontalSpeed = maxHorizontalSpeedOnGround;
            }
            else if (statesY == StatesY.falling)
            {
                lambdaAccel = lambdaAccelFalling;
                lambdaDecel = lambdaDecelFalling;
                maxHorizontalSpeed = Math.Max(maxHorizontalSpeedFalling, Math.Abs(player.velocity.X));

                //if (player.velocity.X == 0)
                //{
                //    maxHorizontalSpeed = maxHorizontalSpeedFalling;
                //}
                //else
                //{
                //    maxHorizontalSpeed = Math.Abs(player.velocity.X);
                //}
            }
        }



    }
}
