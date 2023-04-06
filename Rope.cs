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
    public class Rope : AnimatedGameObject
    {
        public List<RopeBit> rope = new List<RopeBit>();
        public List<RopeBit> ropeBitsDrawnOnScreen = new List<RopeBit>();
        public Vector2 ropeAnchor = new Vector2();

        public ColliderManager spriteCollider = new ColliderManager();

        public int NumberOfRopeBits = 8;
        public int LengthBetweenRopeBits = 8; // currently we move 2 pixels horizontally every frame so make it even so when we update indices we can detect exactly when a new one should be enabled
        public int acc = 10;
        public int RopeLength;

        public int IndexOfFirstEnabledRopeBit;
        public int IndexOfFirstRopeBitAnchor;
        public int IndexOfRopeBitInPlayersHand;
        public int IndexOfFirstFixedRopeBit;

        public int IndexOfFirstRopeBitInBetweenWhichIsNotEnabled;

        public float springConstant;
        public float distanceTilEquilibriumY;
        public float distanceStretchRequiredToOvercomeFriction;



        public float frictionConstant = 2f; // This should be between 0 and 1 and dissipates energy from the rope
        public float groundFrictionNormalConstant = 0.3f;
        public float airFrictionConstant = 1f;
        public float groundFrictionConstant = 2f;



        public Rope() : base()
        {

        }

        public Rope(Vector2 initialPosition) : base(initialPosition)
        {
        }

        public override void Update(GameTime gametime)
        {
            // base.Update(gametime);
        }






        public void FindVelocityAndDisplacementForRopeBitX(int i)
        {

            if (rope[i].FirstLoopX)
            {
                rope[i].displacement.X = 0.5f * rope[i].totalForce.X * rope[i].deltaTime * rope[i].deltaTime + rope[i].velocity.X * rope[i].deltaTime;

            }
            else
            {
                rope[i].displacement.X += rope[i].totalForce.X * rope[i].deltaTime * rope[i].deltaTime;
            }

            rope[i].velocity.X += rope[i].totalForce.X * rope[i].deltaTime;

            rope[i].FirstLoopX = false;


        }

        public void FindVelocityAndDisplacementForRopeBitY(int i)
        {


            if (rope[i].FirstLoopY)
            {

                rope[i].displacement.Y = 0.5f * rope[i].totalForce.Y * rope[i].deltaTime * rope[i].deltaTime + rope[i].velocity.Y * rope[i].deltaTime;

            }
            else
            {
                rope[i].displacement.Y += rope[i].totalForce.Y * rope[i].deltaTime * rope[i].deltaTime;
            }

            rope[i].velocity.Y += rope[i].totalForce.Y * rope[i].deltaTime;

            rope[i].FirstLoopY = false;


        }





        public void FindSpringForcesPairWise(RopeBit ropeBit1, RopeBit ropeBit2, float length)
        {

            Vector2 displacement1to2 = ropeBit2.position - ropeBit1.position;


            float springForce = -springConstant * (displacement1to2.Length() - length);




            if (displacement1to2.Length() > 0.001)
            {
                displacement1to2.Normalize();
            }

            ropeBit1.totalForce.X += -springForce * displacement1to2.X;
            ropeBit1.totalForce.Y += -springForce * displacement1to2.Y;
            ropeBit2.totalForce.X += springForce * displacement1to2.X;
            ropeBit2.totalForce.Y += springForce * displacement1to2.Y;


        }



        public void FindSpringFrictionPairWise(RopeBit ropeBit1, RopeBit ropeBit2)
        {

            Vector2 relativeVelocity = new Vector2(ropeBit2.velocity.X - ropeBit1.velocity.X, ropeBit2.velocity.Y - ropeBit1.velocity.Y);
            Vector2 frictionForce = -frictionConstant * relativeVelocity;

            //if (ropeBit2 == rope[4])
            //{
            //    Debug.WriteLine(frictionForce.X);
            //}
            ropeBit1.totalForce += -frictionForce;
            ropeBit2.totalForce += frictionForce;


        }


        public void FindGroundFriction(RopeBit ropeBit1)
        {

            if (ropeBit1.CollidedOnBottom)
            {
                Vector2 frictionForce = -groundFrictionConstant * ropeBit1.velocity;
                ropeBit1.totalForce += frictionForce;
            }


        }

        public void FindWallFrictionWithNormal(RopeBit ropeBit1)
        {

            if (ropeBit1.CollidedOnRight)
            {
                float frictionForce;

                if (ropeBit1.totalForce.X >= 0)
                {

                    frictionForce = groundFrictionNormalConstant * ropeBit1.totalForce.X;
                }
                else
                {
                    frictionForce = 0;
                }



                if (Math.Abs(ropeBit1.totalForce.Y) < frictionForce)
                {
                    ropeBit1.FirstLoopY = true;
                    ropeBit1.displacement.Y = 0;
                    ropeBit1.velocity.Y = 0;
                    ropeBit1.totalForce.Y = 0;
                }
                else
                {
                    if (ropeBit1.totalForce.Y > 0)
                    {
                        ropeBit1.totalForce.Y -= frictionForce;
                    }
                    else
                    {
                        ropeBit1.totalForce.Y += frictionForce;
                    }
                }

                return;
            }

            if (ropeBit1.CollidedOnLeft)
            {
                float frictionForce;

                if (ropeBit1.totalForce.X <= 0)
                {

                    frictionForce = Math.Abs(groundFrictionNormalConstant * ropeBit1.totalForce.X);
                }
                else
                {
                    frictionForce = 0;
                }



                if (Math.Abs(ropeBit1.totalForce.Y) < frictionForce)
                {
                    ropeBit1.FirstLoopY = true;
                    ropeBit1.displacement.Y = 0;
                    ropeBit1.velocity.Y = 0;
                    ropeBit1.totalForce.Y = 0;
                }
                else
                {
                    if (ropeBit1.totalForce.Y > 0)
                    {
                        ropeBit1.totalForce.Y -= frictionForce;
                    }
                    else
                    {
                        ropeBit1.totalForce.Y += frictionForce;
                    }
                }

                return;
            }


        }



        public void FindGroundFrictionWithNormal(RopeBit ropeBit1)
        {

            if (ropeBit1.CollidedOnBottom)
            {
                //float frictionForce = groundFrictionNormalConstant * ropeBit1.mass * ropeBit1.gravityConstant;
                float frictionForce;
                if (ropeBit1.totalForce.Y >= 0)
                {

                    frictionForce = groundFrictionNormalConstant * ropeBit1.totalForce.Y;
                }
                else
                {
                    frictionForce = 0;
                }



                if (Math.Abs(ropeBit1.totalForce.X) < frictionForce)
                {
                    ropeBit1.FirstLoopX = true;
                    ropeBit1.displacement.X = 0;
                    ropeBit1.velocity.X = 0;
                    ropeBit1.totalForce.X = 0;
                }
                else
                {
                    if (ropeBit1.totalForce.X > 0)
                    {
                        ropeBit1.totalForce.X -= frictionForce;
                    }
                    else
                    {
                        ropeBit1.totalForce.X += frictionForce;
                    }
                }
            }


        }


        public void FindAirFrictionEULER(RopeBit ropeBit1)
        {
            Vector2 frictionForce = -airFrictionConstant * ropeBit1.velocity;
            ropeBit1.totalForce += frictionForce;
        }


    }
}
