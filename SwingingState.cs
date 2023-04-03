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
    public class SwingingState : State
    {
        public HookPoint hookPoint;
        public Hook hook;

        public float swingRadius = 0;

        public float swingDirection = 0;
        public float swingDrivingForce = 0;
        public float swingForceMaximum = 5f;
        public float swingForceDuration = 0.5f;
        public float swingFrictionConstant = 0.5f;
        public float timeAngle = 0;
        public float impulseAngle = 0;
        public bool impulseWindow = false;
        public bool firstLoopOnSwing = true;



        public bool swinging = false;
        public SwingingState(Player player) : base(player)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                enterStateFlag = false;
                player.swingAngle = 0;
                player.swingAngleDot = 0;
                firstLoopOnSwing = true;
                timeAngle = 0;

                hook = new Hook(hookPoint.spritePosition, player);
                hook.LoadContent(References.content, References.graphicsDevice);
                swingRadius = Vector2.Distance(hookPoint.spritePosition, player.ropeAnchor);
            }

            UpdateAnimations();
            hook.Update(gameTime);
            UpdateVelocityAndDisplacement();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            hook.Draw(spriteBatch);
        }


        public override void UpdateExits()
        {
            if (player.flagJumpButtonPressed)
            {
                //hook.DisableRope();
                exits = Exits.exitToNormalState;

            }
        }



        public override void UpdateVelocityAndDisplacement()
        {

            DetermineSwingImpulseForce();
            Swing();

        }






        public void Swing()
        {

            if (firstLoopOnSwing)
            {
                int sign;

                if (player.ropeAnchor.X <= hookPoint.spritePosition.X)
                {
                    sign = 1;
                }
                else
                {
                    sign = -1;
                }

                Vector2 direction1 = player.ropeAnchor - hookPoint.spritePosition;
                direction1.Normalize();
                Vector2 direction2 = new Vector2(0, 1);
                float initialSwingAngle = sign * (float)Math.Acos(Vector2.Dot(direction1, direction2));
                float initialAngleDot = -sign * player.spriteVelocity.Length() / swingRadius;

                player.swingAngleDot = initialAngleDot + player.deltaTime * (-player.gravityConstant * (float)Math.Sin(player.swingAngle) / swingRadius - swingDrivingForce * (float)Math.Cos(player.swingAngle) - (swingFrictionConstant / player.mass) * player.swingAngleDot);
                player.swingAngle = initialSwingAngle + player.deltaTime * player.swingAngleDot;


                player.spriteVelocity.X = -swingRadius * player.swingAngleDot * (float)Math.Cos(player.swingAngle);
                player.spriteVelocity.Y = -swingRadius * player.swingAngleDot * (float)Math.Sin(player.swingAngle);

                player.spriteDisplacement.X = hookPoint.spritePosition.X - swingRadius * (float)Math.Sin(player.swingAngle) - player.ropeAnchor.X;
                player.spriteDisplacement.Y = hookPoint.spritePosition.Y + swingRadius * (float)Math.Cos(player.swingAngle) - player.ropeAnchor.Y;
                player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

                firstLoopOnSwing = false;

            }
            else
            {

                player.swingAngleDot += player.deltaTime * (-player.gravityConstant * (float)Math.Sin(player.swingAngle) / swingRadius - swingDrivingForce * (float)Math.Cos(player.swingAngle) - (swingFrictionConstant / player.mass) * player.swingAngleDot);
                player.swingAngle += player.deltaTime * player.swingAngleDot;

                player.spriteVelocity.X = -swingRadius * player.swingAngleDot * (float)Math.Cos(player.swingAngle);
                player.spriteVelocity.Y = -swingRadius * player.swingAngleDot * (float)Math.Sin(player.swingAngle);
                player.spriteDisplacement.X = hookPoint.spritePosition.X - swingRadius * (float)Math.Sin(player.swingAngle) - player.ropeAnchor.X;
                player.spriteDisplacement.Y = hookPoint.spritePosition.Y + swingRadius * (float)Math.Cos(player.swingAngle) - player.ropeAnchor.Y;
                player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

            }

        }

        public void DetermineSwingImpulseForce()
        {


            if (!impulseWindow)
            {

                if (Math.Sign(player.spriteVelocity.X) != Math.Sign(player.previousSpriteVelocity.X) || Math.Abs(player.spriteVelocity.X) <= 0.5)
                {
                    impulseWindow = true;
                    Vector2 direction1 = player.ropeAnchor - hookPoint.spritePosition;
                    direction1.Normalize();
                    Vector2 direction2 = new Vector2(0, 1);
                    impulseAngle = (float)Math.Acos(Vector2.Dot(direction1, direction2));
                    timeAngle = 0;
                }


            }
            else
            {
                Vector2 direction1 = player.ropeAnchor - hookPoint.spritePosition;
                direction1.Normalize();
                Vector2 direction2 = new Vector2(0, 1);
                float angle = (float)Math.Acos(Vector2.Dot(direction1, direction2));

                if (Math.Abs(impulseAngle) < 0.03)
                {
                    if (Math.Abs(angle) >= 0.03)
                    {
                        impulseAngle = 0;
                        impulseWindow = false;
                    }

                }
                else
                {
                    if (Math.Abs(angle) < Math.Abs(impulseAngle) / 2)
                    {
                        impulseAngle = 0;
                        impulseWindow = false;
                    }
                }

            }

            // This code chunk determines the actual impulse force
            if (impulseWindow)
            {
                if (timeAngle >= swingForceDuration)
                {
                    swingDirection = 0;
                    swingDrivingForce = 0;
                    timeAngle = 0;
                    impulseWindow = false;
                }
                else if (timeAngle > 0)
                {
                    swingDrivingForce = swingForceMaximum * (swingForceDuration - timeAngle) * swingDirection;
                    timeAngle += player.deltaTime;
                }
                else
                {
                    if (Math.Abs(impulseAngle) < 0.03)
                    {
                        if (player.spriteDirectionX != 0)
                        {
                            swingDirection = player.spriteDirectionX;
                            swingDrivingForce = swingForceMaximum * swingForceDuration * swingDirection;
                            timeAngle = player.deltaTime;
                        }
                        else
                        {
                            swingDirection = 0;
                            swingDrivingForce = 0;
                            timeAngle = 0;
                        }

                    }
                    else
                    {
                        if (player.spriteDirectionX != 0 && player.spriteDirectionX == Math.Sign(player.spriteVelocity.X))
                        {
                            swingDirection = player.spriteDirectionX;
                            swingDrivingForce = swingForceMaximum * swingForceDuration * swingDirection;
                            timeAngle = player.deltaTime;
                        }
                        else
                        {
                            swingDirection = 0;
                            swingDrivingForce = 0;
                            timeAngle = 0;
                        }

                    }


                }
            }
        }


        public override void UpdateAnimations()
        {
            player.animatedSprite.Play("Idle");
            player.currentFrame = player.frameAndTag["Idle"].From;
            player.tagOfCurrentFrame = "Idle";
            player.TurnOffAllHitboxes();
            player.idleHitbox.isActive = true;
        }




    }
}
