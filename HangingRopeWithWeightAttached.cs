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
    public class HangingRopeWithWeightAttached : Rope
    {
        public MovingSprite weight;
        public bool giveImpulse = false;
        public int impulseDirection;
        public Vector2 weightAttachmentPoint = new Vector2();
        public float lengthOfTheRopeAtRest = 0;


        public bool stopRadialMovement = false;

        public float impulseForceMax;
        public float impulseForceDuration;
        public float impulseTimer = 0;

        public float impulseAngle = 0;


        public HangingRopeWithWeightAttached(string filename, float impulseForceMax, float impulseForceDuration, Vector2 ropeAnchor, int NumberOfRopeBits, int LengthBetweenRopeBits)
        {
            this.NumberOfRopeBits = NumberOfRopeBits;
            this.LengthBetweenRopeBits = LengthBetweenRopeBits;
            this.ropeAnchor = ropeAnchor;
            RopeLength = (NumberOfRopeBits - 1) * LengthBetweenRopeBits;



            this.impulseForceMax = impulseForceMax;
            this.impulseForceDuration = impulseForceDuration;


            this.impulseForceMax = 1000;
            this.impulseForceDuration = 1f;

            rope.Add(new RopeBit(ropeAnchor, "RedDot"));

            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits);

            for (int i = 1; i < NumberOfRopeBits; i++)
            {
                float adjustment = distanceTilEquilibriumY + (distanceTilEquilibriumY - LengthBetweenRopeBits) * (NumberOfRopeBits - 1 - i);

                Vector2 position = new Vector2
                {
                    X = ropeAnchor.X,
                    Y = rope[i - 1].spritePosition.Y + adjustment
                };

                RopeBit ropeBit = new RopeBit(position, "RedDot");
                rope.Add(ropeBit);

                lengthOfTheRopeAtRest += adjustment;

            }

            weight = new MovingSprite(rope[NumberOfRopeBits - 1].spritePosition, filename);






            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(spritePosition);
                ropeBit.Enabled = false;
                ropeBit.spriteFilename = "YellowDot";
                ropeBitsDrawnOnScreen.Add(ropeBit);
            }

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {


            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                rope[i].LoadContent(contentManager, graphicsDevice);

            }

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                ropeBitsDrawnOnScreen[i].LoadContent(contentManager, graphicsDevice);
                ropeBitsDrawnOnScreen[i].idleHitbox.rectangle.Width = 1;
                ropeBitsDrawnOnScreen[i].idleHitbox.rectangle.Height = 1;
                ropeBitsDrawnOnScreen[i].idleHitbox.offsetX = 3;
                ropeBitsDrawnOnScreen[i].idleHitbox.offsetY = 3;
            }

            weight.LoadContent(contentManager, graphicsDevice);
            //weight.CreateHitboxesForGunLine();
            weight.EnableGunlineHitboxes();

        }


        public override void Update(GameTime gameTime)
        {
            // The weight is essentially only used to detect collisions - the main physics is in the behaviour of the rope
            weight.spritePosition.X = rope[NumberOfRopeBits - 1].spritePosition.X;
            weight.spritePosition.Y = rope[NumberOfRopeBits - 1].spritePosition.Y;


            //if (weight.animatedSprite_Idle.CurrentFrameIndex != weight.previousFrameNumber)
            //{
            //    weight.UpdateGunlineHitboxes();
            //    weight.previousFrameNumber = weight.animatedSprite_Idle.CurrentFrameIndex;
            //}


            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {
                if (Math.Abs(rope[i].spriteVelocity.X) < 0.001)
                {
                    rope[i].FirstLoopX = true;
                }
                if (Math.Abs(rope[i].spriteVelocity.Y) < 0.001)
                {
                    rope[i].FirstLoopY = true;
                }
            }

            SimulateRopeHangingWithWeight();

            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gameTime);
            }

            //base.Update(gameTime);
            weight.Update(gameTime);

            for (int k = weight.frameAndTag[weight.tagOfCurrentFrame].From; k <= weight.frameAndTag[weight.tagOfCurrentFrame].To; k++)
            {
                foreach (HitboxRectangle hitbox in weight.hitboxesForGunlineForEachFrame[k])
                {
                    hitbox.rectangle.X = (int)weight.spritePosition.X + hitbox.offsetX;
                    hitbox.rectangle.Y = (int)weight.spritePosition.Y + hitbox.offsetY;
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                rope[i].animation_Idle.Draw(spriteBatch, rope[i].animationPosition);
            }

            weight.animation_Idle.Draw(spriteBatch, weight.animationPosition);

            //foreach (HitboxRectangle hitbox in weight.hitboxesForGunlineForEachFrame[1])
            //{
            //    spriteBatch.Draw(References.player.spriteHitboxTexture, hitbox.rectangle, Color.Red);
            //}
        }


        public void SimulateRopeHangingWithWeight()
        {

            rope[0].spritePosition = ropeAnchor;
            rope[0].idleHitbox.rectangle.X = (int)rope[0].spritePosition.X + rope[0].idleHitbox.offsetX;
            rope[0].idleHitbox.rectangle.Y = (int)rope[0].spritePosition.Y + rope[0].idleHitbox.offsetY;
            rope[0].spriteVelocity.X = 0;
            rope[0].spriteVelocity.Y = 0;
            //rope[0].animatedSprite_Idle.Position = rope[0].spritePosition;

            for (int i = 1; i <= NumberOfRopeBits - 1; i++)
            {

                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousSpriteVelocity = rope[i].spriteVelocity;
                rope[i].previousSpritePosition = rope[i].spritePosition;

            }

            FindImpulseForce();

            for (int i = NumberOfRopeBits - 2; i >= 0; i--)
            {
                if (Vector2.Distance(rope[i].spritePosition, rope[i + 1].spritePosition) >= LengthBetweenRopeBits)
                {
                    FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                    FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                }

            }

            for (int i = NumberOfRopeBits - 1; i > 0; i--)
            {

                rope[i].FindGravityForce();
                FindAirFrictionEULER(rope[i]);
                rope[i].FindNormalForces();
                FindVelocityAndDisplacementForRopeBitX(i);
                FindVelocityAndDisplacementForRopeBitY(i);
                spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope[i], References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

            }

        }

        public void FindImpulseForce()
        {
            if (giveImpulse)
            {

                if (impulseTimer >= impulseForceDuration)
                {
                    impulseTimer = 0;
                    giveImpulse = false;

                }
                else if (impulseTimer > 0)
                {
                    rope[NumberOfRopeBits - 1].totalForce.X += impulseForceMax * (impulseForceDuration - impulseTimer) * (float)Math.Sin(impulseAngle);
                    rope[NumberOfRopeBits - 1].totalForce.Y += -impulseForceMax * (impulseForceDuration - impulseTimer) * (float)Math.Cos(impulseAngle);
                    impulseTimer += weight.deltaTime;
                }
                else
                {
                    rope[NumberOfRopeBits - 1].totalForce.X += impulseForceMax * impulseForceDuration * (float)Math.Sin(impulseAngle);
                    rope[NumberOfRopeBits - 1].totalForce.Y += -impulseForceMax * impulseForceDuration * (float)Math.Cos(impulseAngle);
                    impulseTimer = weight.deltaTime;
                }



            }
        }








        // /////////////////////////////////////////////////////////////////////////////////////////////
        // THIS CODE BELOW CORRESPONDS TO A PENDULUM
        // BUT IF WE WANT TO CODE A PENDULUM THERE IS NO NEED TO USE A ROPE 
        // THE DISTANCE IS FIXED AND THE MOTION IS DESCRIBED WELL BY THE SIMPLE HARMONIC MOTION EQUATIONS

        public void Swing()
        {

            weight.previousSpritePosition = weight.spritePosition;

            weight.swingAngleDot += weight.deltaTime * (-weight.gravityConstant * (float)Math.Sin(weight.swingAngle) / weight.length - weight.swingDrivingForce * (float)Math.Cos(weight.swingAngle) - (weight.swingFrictionConstant / weight.mass) * weight.swingAngleDot);
            weight.swingAngle += weight.deltaTime * weight.swingAngleDot;

            weight.lengthDot += weight.deltaTime * weight.lengthForce;
            weight.length += weight.deltaTime * weight.lengthDot;




            weight.spriteVelocity.X = -weight.length * weight.swingAngleDot * (float)Math.Cos(weight.swingAngle);
            weight.spriteVelocity.Y = -weight.length * weight.swingAngleDot * (float)Math.Sin(weight.swingAngle);
            weight.spriteDisplacement.X = rope[0].spritePosition.X - weight.length * (float)Math.Sin(weight.swingAngle) - weight.spritePosition.X;
            weight.spriteDisplacement.Y = rope[0].spritePosition.Y + weight.length * (float)Math.Cos(weight.swingAngle) - weight.spritePosition.Y;
            spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(weight, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);



            DetermineSwingImpulseForceold();
        }

        public void DetermineSwingImpulseForceoldv2()
        {
            // This code chunk determines whether the impulseWindow is active or not
            // Note that we only use angle magnitudes here so the sign of the angles are not important
            //Debug.WriteLine("impulseWindow " + impulseWindow);
            //Debug.WriteLine("timeAngle " + timeAngle);

            if (giveImpulse)
            {

                if (weight.timeAngle >= weight.swingForceDuration)
                {
                    weight.swingDrivingForce = 0;
                }
                else if (weight.timeAngle > 0)
                {
                    weight.swingDrivingForce = weight.swingForceMaximum * (weight.swingForceDuration - weight.timeAngle) * (float)Math.Sin(impulseAngle);
                    weight.timeAngle += weight.deltaTime;
                }
                else
                {
                    weight.swingDrivingForce = weight.swingForceMaximum * weight.swingForceDuration * (float)Math.Sin(impulseAngle);
                    weight.timeAngle = weight.deltaTime;

                }

                if (weight.timeLength >= 3 * weight.lengthForceDuration)
                {
                    impulseAngle = 0;
                    weight.length = lengthOfTheRopeAtRest;
                    weight.timeLength = 0;
                    weight.timeAngle = 0;
                    giveImpulse = false;
                }
                else if (weight.timeLength > 0)
                {
                    weight.length = lengthOfTheRopeAtRest + weight.lengthForceMaximum * weight.timeLength * (weight.lengthForceDuration / 2 - weight.timeLength / 6) * (float)Math.Cos(impulseAngle);
                    weight.timeLength += weight.deltaTime;
                }
                else
                {
                    weight.length = lengthOfTheRopeAtRest;
                    weight.timeLength = weight.deltaTime;

                }


            }


        }




        public void Swingold()
        {

            weight.previousSpritePosition = weight.spritePosition;

            weight.swingAngleDot += weight.deltaTime * (-weight.gravityConstant * (float)Math.Sin(weight.swingAngle) / lengthOfTheRopeAtRest - weight.swingDrivingForce * (float)Math.Cos(weight.swingAngle) - (weight.swingFrictionConstant / weight.mass) * weight.swingAngleDot);
            weight.swingAngle += weight.deltaTime * weight.swingAngleDot;

            weight.spriteVelocity.X = -RopeLength * weight.swingAngleDot * (float)Math.Cos(weight.swingAngle);
            weight.spriteVelocity.Y = -RopeLength * weight.swingAngleDot * (float)Math.Sin(weight.swingAngle);
            weight.spriteDisplacement.X = rope[0].spritePosition.X - lengthOfTheRopeAtRest * (float)Math.Sin(weight.swingAngle) - weight.spritePosition.X;
            weight.spriteDisplacement.Y = rope[0].spritePosition.Y + lengthOfTheRopeAtRest * (float)Math.Cos(weight.swingAngle) - weight.spritePosition.Y;
            spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(weight, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);



            DetermineSwingImpulseForceold();
        }


        public void DetermineSwingImpulseForceold()
        {
            // This code chunk determines whether the impulseWindow is active or not
            // Note that we only use angle magnitudes here so the sign of the angles are not important
            //Debug.WriteLine("impulseWindow " + impulseWindow);
            //Debug.WriteLine("timeAngle " + timeAngle);

            if (giveImpulse)
            {

                if (weight.timeAngle >= weight.swingForceDuration)
                {
                    impulseDirection = 0;
                    weight.swingDrivingForce = 0;
                    weight.timeAngle = 0;
                    giveImpulse = false;
                }
                else if (weight.timeAngle > 0)
                {
                    weight.swingDrivingForce = weight.swingForceMaximum * (weight.swingForceDuration - weight.timeAngle) * impulseDirection;
                    weight.timeAngle += weight.deltaTime;
                }
                else
                {
                    //weight.swingDirection = spriteDirectionX;
                    weight.swingDrivingForce = weight.swingForceMaximum * weight.swingForceDuration * impulseDirection;
                    weight.timeAngle = weight.deltaTime;

                }


            }


        }

    }
}
