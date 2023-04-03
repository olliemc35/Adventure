using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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



        public NormalState(Player player) : base(player)
        {
            statesX = StatesX.atRestX;
            statesY = StatesY.atRestY;

            originalLambdaAccel = player.spriteMaxHorizontalSpeed / (TimeTakenToAccelerateToMaxSpeedX * TimeTakenToAccelerateToMaxSpeedX);
            originalLambdaDecel = player.spriteMaxHorizontalSpeed / (TimeTakenToDecelerateFromMaxSpeedX * TimeTakenToDecelerateFromMaxSpeedX);

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

                if (player.spriteVelocity.X > 0)
                {
                    statesX = StatesX.constantVelocityRightUntilHitGround;
                }
                else if (player.spriteVelocity.X < 0)
                {
                    statesX = StatesX.constantVelocityLeftUntilHitGround;
                }
                else
                {
                    if (player.spriteDirectionX == 1)
                    {
                        statesX = StatesX.accelerateRight;

                    }
                    else if (player.spriteDirectionX == -1)
                    {
                        statesX = StatesX.accelerateLeft;
                    }
                    else
                    {
                        statesX = StatesX.atRestX;
                    }
                }

                if (player.SpriteCollidedOnBottom && player.spriteVelocity.Y == 0)
                {
                    statesY = StatesY.atRestY;
                }
                else
                {
                    statesY = StatesY.falling;
                }


                UpdateVelocityAndDisplacement();
                player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
                return;

            }



            UpdateAnimations();
            UpdateStatesX();
            UpdateStatesY();
            UpdateVelocityAndDisplacement();
            player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
        }

        public override void UpdateVelocityAndDisplacement()
        {
            UpdateVelocityAndDisplacementX();
            UpdateVelocityAndDisplacementY();
        }

        public override void UpdateExits()
        {
            if (!player.SpriteCollidedOnBottom)
            {
                if (player.SpriteCollidedOnRight && player.spriteDirectionX == 1)
                {
                    exits = Exits.exitToSlidingWallStateFacingRight;
                    return;
                }
                if (player.SpriteCollidedOnLeft && player.spriteDirectionX == -1)
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
                if (player.spriteDirectionX == -1)
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

                if (player.SpriteCollidedOnBottom)
                {
                    if (player.spriteDirectionX == 1)
                    {
                        statesX = StatesX.constantVelocityRight;
                    }
                    else if (player.spriteDirectionX == -1)
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

                if (player.spriteDirectionX == 1)
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

                if (player.SpriteCollidedOnBottom)
                {

                    if (player.spriteDirectionX == -1)
                    {
                        statesX = StatesX.constantVelocityLeft;
                    }
                    else if (player.spriteDirectionX == 1)
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


            if (player.SpriteCollidedOnRight && player.spriteDirectionX == 1)
            {
                statesX = StatesX.constantVelocityRight;
                return;
            }

            if (player.SpriteCollidedOnLeft && player.spriteDirectionX == -1)
            {
                statesX = StatesX.constantVelocityLeft;
                return;
            }

            if (statesX == StatesX.atRestX && player.DirectionChangedX && player.spriteDirectionX == 1)
            {
                statesX = StatesX.accelerateRight;
                return;
            }

            if (statesX == StatesX.atRestX && player.DirectionChangedX && player.spriteDirectionX == -1)
            {
                statesX = StatesX.accelerateLeft;
                return;
            }

            if ((statesX == StatesX.accelerateRight || statesX == StatesX.constantVelocityRight) && player.DirectionChangedX && player.spriteDirectionX == 0)
            {
                statesX = StatesX.decelerateRight;
                return;
            }

            if ((statesX == StatesX.accelerateLeft || statesX == StatesX.constantVelocityLeft) && player.DirectionChangedX && player.spriteDirectionX == 0)
            {
                statesX = StatesX.decelerateLeft;
                return;
            }

            if ((statesX == StatesX.decelerateRight || statesX == StatesX.decelerateLeft || statesX == StatesX.accelerateLeft || statesX == StatesX.constantVelocityLeft) && player.DirectionChangedX && player.spriteDirectionX == 1)
            {
                statesX = StatesX.accelerateRight;
                return;
            }

            if ((statesX == StatesX.decelerateRight || statesX == StatesX.decelerateLeft || statesX == StatesX.accelerateRight || statesX == StatesX.constantVelocityRight) && player.DirectionChangedX && player.spriteDirectionX == -1)
            {
                statesX = StatesX.accelerateLeft;
                return;
            }

            if (statesX == StatesX.accelerateRight && player.spriteVelocity.X >= player.spriteMaxHorizontalSpeed)
            {
                player.spriteVelocity.X = player.spriteMaxHorizontalSpeed;
                statesX = StatesX.constantVelocityRight;
                return;
            }

            if (statesX == StatesX.accelerateLeft && player.spriteVelocity.X <= -player.spriteMaxHorizontalSpeed)
            {
                player.spriteVelocity.X = -player.spriteMaxHorizontalSpeed;
                statesX = StatesX.constantVelocityLeft;
                return;
            }

            if ((statesX == StatesX.decelerateLeft || statesX == StatesX.decelerateRight || statesX == StatesX.constantVelocityRightUntilHitGround || statesX == StatesX.constantVelocityLeftUntilHitGround) && Math.Abs(player.spriteVelocity.X) <= 0.01)
            {
                player.spriteVelocity.X = 0;
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
                        player.spriteVelocity.X += 0;
                        break;
                    }

                case StatesX.constantVelocityRight:
                    {
                        player.spriteVelocity.X += 0;
                        break;
                    }

                case StatesX.accelerateLeft:
                    {
                        player.spriteVelocity.X += -lambdaAccel * player.deltaTime;
                        player.spriteVelocity.X = Math.Max(player.spriteVelocity.X, -player.spriteMaxHorizontalSpeed);
                        break;
                    }
                case StatesX.accelerateRight:
                    {
                        player.spriteVelocity.X += lambdaAccel * player.deltaTime;
                        player.spriteVelocity.X = Math.Min(player.spriteVelocity.X, player.spriteMaxHorizontalSpeed);
                        break;
                    }
                case StatesX.decelerateLeft:
                    {
                        player.spriteVelocity.X += lambdaDecel * player.deltaTime;
                        player.spriteVelocity.X = Math.Min(player.spriteVelocity.X, 0);
                        break;
                    }
                case StatesX.decelerateRight:
                    {
                        player.spriteVelocity.X += -lambdaDecel * player.deltaTime;
                        player.spriteVelocity.X = Math.Max(player.spriteVelocity.X, 0);
                        break;
                    }
                case StatesX.constantVelocityLeftUntilHitGround:
                    {
                        player.spriteVelocity.X += 0;
                        break;
                    }
                case StatesX.constantVelocityRightUntilHitGround:
                    {
                        player.spriteVelocity.X += 0;
                        break;
                    }
                case StatesX.atRestX:
                    {
                        player.spriteVelocity.X = 0;
                        break;
                    }

            }

            player.spriteDisplacement.X = player.spriteVelocity.X * player.deltaTime;


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
                player.spriteVelocity.Y = -player.jumpSpeed;
                return;
            }

            // On the ground and walk off an edge
            if (statesY == StatesY.atRestY && !player.SpriteCollidedOnBottom)
            {
                statesY = StatesY.falling;
                player.spriteVelocity.Y = 0;
                return;
            }

            // Falling in middair and hit ground
            if (statesY == StatesY.falling && player.SpriteCollidedOnBottom)
            {
                statesY = StatesY.atRestY;
                player.spriteVelocity.Y = 0;
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
                        player.spriteVelocity.Y += player.gravityConstant * player.deltaTime;
                        player.spriteVelocity.Y = Math.Min(player.spriteVelocity.Y, player.maxFallSpeed);
                        //player.spriteDisplacement.Y = player.spriteVelocity.Y * player.deltaTime + 0.5f * player.gravityConstant * player.deltaTime * player.deltaTime;
                        break;
                    }
                case StatesY.atRestY:
                    {
                        player.spriteVelocity.Y = 0;
                        //player.spriteDisplacement.Y = 0;
                        break;
                    }
            }

            player.spriteDisplacement.Y = player.spriteVelocity.Y * player.deltaTime;


        }



        public override void UpdateAnimations()
        {
            if (statesY == StatesY.falling)
            {
                if (player.spriteVelocity.Y < 0)
                {
                    if (player.spriteDirectionX == -1)
                    {
                        player.nameOfCurrentAnimationSprite = "JumpLeft";

                        //player.animatedSprite_Idle.Play("JumpLeft");
                        //player.currentFrame = player.frameAndTag["JumpLeft"].From;
                        //player.tagOfCurrentFrame = "JumpLeft";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;
                    }
                    else
                    {
                        player.nameOfCurrentAnimationSprite = "JumpRight";


                        //player.animatedSprite_Idle.Play("JumpRight");
                        //player.currentFrame = player.frameAndTag["JumpRight"].From;
                        //player.tagOfCurrentFrame = "JumpRight";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;

                    }
                }
                else
                {
                    if (player.spriteDirectionX == -1)
                    {
                        player.nameOfCurrentAnimationSprite = "FallingLeft";

                        //player.animatedSprite_Idle.Play("FallingLeft");
                        //player.currentFrame = player.frameAndTag["FallingLeft"].From;
                        //player.tagOfCurrentFrame = "FallingLeft";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;
                    }
                    else
                    {
                        player.nameOfCurrentAnimationSprite = "FallingRight";


                        //player.animatedSprite_Idle.Play("FallingRight");
                        //player.currentFrame = player.frameAndTag["FallingRight"].From;
                        //player.tagOfCurrentFrame = "FallingRight";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;
                    }
                }
            }
            else if (statesY == StatesY.atRestY)
            {
                if (landedFlag)
                {
                    player.nameOfCurrentAnimationSprite = "Landed";


                    //player.animatedSprite_Idle.Play("Landed");
                    //player.currentFrame = player.frameAndTag["Landed"].From;
                    //player.tagOfCurrentFrame = "Landed";
                    //player.TurnOffAllHitboxes();
                    //player.idleHitbox.isActive = true;

                }
                else if (player.spriteDirectionX == 1)
                {
                    player.nameOfCurrentAnimationSprite = "MoveRight";

                    //player.animatedSprite_Idle.Play("MoveRight");
                    //player.currentFrame = player.frameAndTag["MoveRight"].From;
                    //player.tagOfCurrentFrame = "MoveRight";
                    //player.TurnOffAllHitboxes();
                    //player.idleHitbox.isActive = true;

                }
                else if (player.spriteDirectionX == -1)
                {
                    player.nameOfCurrentAnimationSprite = "MoveLeft";


                    //player.animatedSprite_Idle.Play("MoveLeft");
                    //player.currentFrame = player.frameAndTag["MoveLeft"].From;
                    //player.tagOfCurrentFrame = "MoveLeft";
                    //player.TurnOffAllHitboxes();
                    //player.idleHitbox.isActive = true;
                }
                else if (player.spriteDirectionX == 0)
                {
                    if (player.previousSpriteDirectionX == 0 || player.previousSpriteDirectionX == 1)
                    {
                        player.nameOfCurrentAnimationSprite = "Idle";

                        //player.animatedSprite_Idle.Play("Idle");
                        //player.currentFrame = player.frameAndTag["Idle"].From;
                        //player.tagOfCurrentFrame = "Idle";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;
                    }
                    else if (player.previousSpriteDirectionX == -1)
                    {
                        player.nameOfCurrentAnimationSprite = "IdleLeft";

                        //player.animatedSprite_Idle.Play("IdleLeft");
                        //player.currentFrame = player.frameAndTag["IdleLeft"].From;
                        //player.tagOfCurrentFrame = "IdleLeft";
                        //player.TurnOffAllHitboxes();
                        //player.idleHitbox.isActive = true;
                    }
                }


            }

            //player.animatedSprite_Idle.OnAnimationLoop = () =>
            //{
            //    if (player.tagOfCurrentFrame == "Landed")
            //    {
            //        landedFlag = false;
            //        player.animatedSprite_Idle.OnAnimationLoop = null;
            //    }
            //    //if (player.previousSpriteDirectionX == 0 || player.previousSpriteDirectionX == 1)
            //    //{
            //    //    player.animatedSprite.Play("Idle");
            //    //    player.currentFrame = player.frameAndTag["Idle"].From;
            //    //    player.tagOfCurrentFrame = "Idle";
            //    //    player.TurnOffAllHitboxes();
            //    //    player.idleHitbox.isActive = true;
            //    //    player.animatedSprite.OnAnimationLoop = null;
            //    //}
            //    //else if (player.previousSpriteDirectionX == -1)
            //    //{
            //    //    player.animatedSprite.Play("IdleLeft");
            //    //    player.currentFrame = player.frameAndTag["IdleLeft"].From;
            //    //    player.tagOfCurrentFrame = "IdleLeft";
            //    //    player.TurnOffAllHitboxes();
            //    //    player.idleHitbox.isActive = true;
            //    //    player.animatedSprite.OnAnimationLoop = null;
            //    //}

            //};

        }

        //if (player.jump < 0)
        //{
        //    if (player.previousSpriteDirectionX == 1)
        //    {
        //        player.animatedSprite.Play("JumpRight");
        //        player.currentFrame = player.frameAndTag["JumpRight"].From;
        //        player.tagOfCurrentFrame = "JumpRight";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //    else if (player.previousSpriteDirectionX == -1)
        //    {
        //        player.animatedSprite.Play("JumpLeft");
        //        player.currentFrame = player.frameAndTag["JumpLeft"].From;
        //        player.tagOfCurrentFrame = "JumpLeft";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //    else
        //    {
        //        player.animatedSprite.Play("JumpRight");
        //        player.currentFrame = player.frameAndTag["JumpRight"].From;
        //        player.tagOfCurrentFrame = "JumpRight";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //}

        //if (player.spriteDirectionX == 1)
        //{
        //    if (player.jump < 0)
        //    {
        //        player.animatedSprite.Play("JumpRight");
        //        player.currentFrame = player.frameAndTag["JumpRight"].From;
        //        player.tagOfCurrentFrame = "JumpRight";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //    else
        //    {
        //        if (player.runButtonPressed)
        //        {
        //            player.animatedSprite.Play("MoveRightRun");
        //            player.currentFrame = player.frameAndTag["MoveRightRun"].From;
        //            player.tagOfCurrentFrame = "MoveRightRun";
        //            player.TurnOffAllHitboxes();
        //            player.idleHitbox.isActive = true;
        //        }
        //        else
        //        {
        //            player.animatedSprite.Play("MoveRight");
        //            player.currentFrame = player.frameAndTag["MoveRight"].From;
        //            player.tagOfCurrentFrame = "MoveRight";
        //            player.TurnOffAllHitboxes();
        //            player.idleHitbox.isActive = true;
        //        }

        //    }
        //}

        //if (player.spriteDirectionX == -1)
        //{
        //    if (player.jump < 0)
        //    {
        //        player.animatedSprite.Play("JumpLeft");
        //        player.currentFrame = player.frameAndTag["JumpLeft"].From;
        //        player.tagOfCurrentFrame = "JumpLeft";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //    else
        //    {
        //        if (player.runButtonPressed)
        //        {
        //            player.animatedSprite.Play("MoveLeftRun");
        //            player.currentFrame = player.frameAndTag["MoveLeftRun"].From;
        //            player.tagOfCurrentFrame = "MoveLeftRun";
        //            player.TurnOffAllHitboxes();
        //            player.idleHitbox.isActive = true;
        //        }
        //        else
        //        {
        //            player.animatedSprite.Play("MoveLeft");
        //            player.currentFrame = player.frameAndTag["MoveLeft"].From;
        //            player.tagOfCurrentFrame = "MoveLeft";
        //            player.TurnOffAllHitboxes();
        //            player.idleHitbox.isActive = true;
        //        }

        //    }
        //}

        //if (player.spriteDirectionX == 0)
        //{
        //    if (player.previousSpriteDirectionX == 0 || player.previousSpriteDirectionX == 1)
        //    {
        //        player.animatedSprite.Play("Idle");
        //        player.currentFrame = player.frameAndTag["Idle"].From;
        //        player.tagOfCurrentFrame = "Idle";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //    else if (player.previousSpriteDirectionX == -1)
        //    {
        //        player.animatedSprite.Play("IdleLeft");
        //        player.currentFrame = player.frameAndTag["IdleLeft"].From;
        //        player.tagOfCurrentFrame = "IdleLeft";
        //        player.TurnOffAllHitboxes();
        //        player.idleHitbox.isActive = true;
        //    }
        //}









        //}




    }
}
