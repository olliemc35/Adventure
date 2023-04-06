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
    public class Hook : Rope
    {
        public bool hookAttached = false;
        public Vector2 hookPosition = new Vector2();
        public Player player;

        public Hook(Vector2 hookPosition, Player player) : base(hookPosition)
        {
            this.hookPosition = hookPosition;
            this.player = player;

            float distance = Vector2.Distance(player.ropeAnchor, hookPosition);
            Vector2 displacement = player.ropeAnchor - hookPosition;
            displacement.Normalize();

            LengthBetweenRopeBits = 8;

            for (int i = 0; i < distance / LengthBetweenRopeBits; i++)
            {
                RopeBit ropeBit = new RopeBit(hookPosition + LengthBetweenRopeBits * i * displacement)
                {
                    Enabled = true
                };

                rope.Add(ropeBit);
            }

            NumberOfRopeBits = rope.Count();


            IndexOfFirstEnabledRopeBit = NumberOfRopeBits - 1;
            IndexOfFirstRopeBitAnchor = NumberOfRopeBits - 1;



            frictionConstant = 2f; // This should be between 0 and 1 and dissipates energy from the rope
            groundFrictionConstant = 2f;
            groundFrictionNormalConstant = 0.3f;
            airFrictionConstant = 1f;



            RopeLength = (NumberOfRopeBits - 1) * LengthBetweenRopeBits;
            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = FindNearestInteger(rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits));
            groundFrictionNormalConstant = (float)(distanceStretchRequiredToOvercomeFriction - LengthBetweenRopeBits) / (distanceTilEquilibriumY - LengthBetweenRopeBits);


            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(position);
                ropeBit.Enabled = false;
                ropeBit.filename = "YellowDot";

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


        }


        public override void Update(GameTime gameTime)
        {


            ropeAnchor = player.ropeAnchor;

            rope[0].position.X = hookPosition.X;
            rope[0].position.Y = hookPosition.Y;
            rope[0].velocity.X = 0;
            rope[0].velocity.Y = 0;
            rope[0].displacement.X = 0;
            rope[0].displacement.Y = 0;
            rope[0].idleHitbox.rectangle.X = (int)rope[0].position.X + rope[0].idleHitbox.offsetX;
            rope[0].idleHitbox.rectangle.Y = (int)rope[0].position.Y + rope[0].idleHitbox.offsetY;
            //rope[0].animatedSprite_Idle.Position = rope[0].spritePosition;
            rope[0].attached = true;

            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {
                if (Math.Abs(rope[i].velocity.X) < 0.001)
                {
                    rope[i].FirstLoopX = true;
                }
                if (Math.Abs(rope[i].velocity.Y) < 0.001)
                {
                    rope[i].FirstLoopY = true;
                }
            }


            SimulateRopeWhenAttached();


            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gameTime);
            }


        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < rope.Count(); i++)
            {
                rope[i].animation_Idle.Draw(spriteBatch, rope[i].animationPosition);

            }

        }






        public void SimulateRopeWhenAttached()
        {
            rope[IndexOfFirstRopeBitAnchor].previousPosition.X = rope[IndexOfFirstRopeBitAnchor].position.X;
            rope[IndexOfFirstRopeBitAnchor].previousPosition.Y = rope[IndexOfFirstRopeBitAnchor].position.Y;
            rope[IndexOfFirstRopeBitAnchor].position.X = player.ropeAnchor.X;
            rope[IndexOfFirstRopeBitAnchor].position.Y = player.ropeAnchor.Y;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.X = (int)rope[IndexOfFirstRopeBitAnchor].position.X + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetX;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.Y = (int)rope[IndexOfFirstRopeBitAnchor].position.Y + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetY;
            rope[IndexOfFirstRopeBitAnchor].velocity = player.velocity;
            //rope[IndexOfFirstRopeBitAnchor].animatedSprite_Idle.Position = rope[IndexOfFirstRopeBitAnchor].spritePosition;


            for (int i = 0; i < IndexOfFirstEnabledRopeBit; i++)
            {
                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousVelocity = rope[i].velocity;
                rope[i].previousPosition = rope[i].position;
            }

            for (int i = IndexOfFirstEnabledRopeBit; i >= 0; i--)
            {
                if (i != IndexOfFirstRopeBitAnchor)
                {
                    if (Vector2.Distance(rope[i].position, rope[i + 1].position) >= LengthBetweenRopeBits)
                    {
                        FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                        FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                    }

                }

            }

            for (int i = IndexOfFirstEnabledRopeBit; i >= 1; i--)
            {
                if (i != IndexOfFirstRopeBitAnchor)
                {

                    rope[i].FindGravityForce();
                    FindGroundFrictionWithNormal(rope[i]);
                    FindAirFrictionEULER(rope[i]);
                    rope[i].FindNormalForces();
                    FindVelocityAndDisplacementForRopeBitX(i);
                    FindVelocityAndDisplacementForRopeBitY(i);

                }
            }


            spriteCollider.AdjustForRopeAgainstListOfSpritesBackward(this, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);



        }




        public void DisableRope()
        {
            for (int i = 0; i < rope.Count(); i++)
            {
                rope[i].Enabled = false;
                rope[i].ThisIsTheRopeAnchor = false;
                rope[i].velocity.X = 0;
                rope[i].velocity.Y = 0;
                rope[i].previousVelocity.X = 0;
                rope[i].previousVelocity.Y = 0;
                rope[i].displacement.X = 0;
                rope[i].displacement.Y = 0;
                rope[i].colliderManager.ResetColliderBoolsForHitbox(rope[i].idleHitbox);
                rope[i].colliderManager.ResetGlobalColliderBoolsForHitbox(rope[i].idleHitbox);
                rope[i].colliderManager.ResetColliderBoolsForSprite(rope[i]);
                rope[i].HaveIDeterminedTheDistanceYet = false;
                rope[i].activeAfterRest = false;
                rope[i].FirstLoopX = true;
                rope[i].FirstLoopY = true;
                rope[i].pivotBetweenThisRopeBitandOneMinus.isPivotActive = false;
                rope[i].pivotBetweenThisRopeBitandOnePlus.isPivotActive = false;
                rope[i].pivotBetweenThisRopeBitandOnePlus.ResetOrientationBools();
                rope[i].pivotBetweenThisRopeBitandOneMinus.ResetOrientationBools();
                rope[i].pivotBetweenThisRopeBitandOneMinus.position = new Vector2(0, 0);
                rope[i].pivotBetweenThisRopeBitandOnePlus.position = new Vector2(0, 0);

                rope[i].RemovePivots();


            }


            for (int i = 0; i < ropeBitsDrawnOnScreen.Count(); i++)
            {
                ropeBitsDrawnOnScreen[i].Enabled = false;
            }

        }





        public void DrawLineBetweenTwoPoints(Vector2 startPosition, Vector2 endPosition)
        {
            Vector2 displacement = endPosition - startPosition;
            int maxInt = (int)Math.Floor(displacement.Length());


            for (int k = IndexOfFirstRopeBitInBetweenWhichIsNotEnabled; k <= IndexOfFirstRopeBitInBetweenWhichIsNotEnabled + maxInt; k++)
            {
                if (k < ropeBitsDrawnOnScreen.Count())
                {
                    ropeBitsDrawnOnScreen[k].position = startPosition + ((float)(k - IndexOfFirstRopeBitInBetweenWhichIsNotEnabled) / maxInt) * displacement;
                    ropeBitsDrawnOnScreen[k].position.X = FindNearestInteger(ropeBitsDrawnOnScreen[k].position.X);
                    ropeBitsDrawnOnScreen[k].position.Y = FindNearestInteger(ropeBitsDrawnOnScreen[k].position.Y);
                    ropeBitsDrawnOnScreen[k].position.Y += 0.5f * rope[0].idleHitbox.rectangle.Height;

                    //ropeBitsDrawnOnScreen[k].animatedSprite_Idle.Position = ropeBitsDrawnOnScreen[k].spritePosition;
                    ropeBitsDrawnOnScreen[k].Enabled = true;

                }

            }

            IndexOfFirstRopeBitInBetweenWhichIsNotEnabled += maxInt;
        }

        public void CreateRopeBitsToDrawOnScreen()
        {

            for (int i = 0; i <= ropeBitsDrawnOnScreen.Count() - 1; i++)
            {
                ropeBitsDrawnOnScreen[i].Enabled = false;
            }

            IndexOfFirstRopeBitInBetweenWhichIsNotEnabled = 0;

            for (int i = IndexOfFirstRopeBitAnchor; i >= 1; i--)
            {
                if (rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() == 0)
                {
                    DrawLineBetweenTwoPoints(rope[i].position, rope[i - 1].position);
                }
                else
                {
                    for (int k = 0; k <= rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() - 1; k++)
                    {
                        if (k == 0)
                        {
                            DrawLineBetweenTwoPoints(rope[i].position, rope[i].pivotsBetweenThisRopeBitandOneMinus[0].position);
                        }

                        if (k > 0)
                        {
                            DrawLineBetweenTwoPoints(rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position, rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position);
                        }

                        if (k == rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() - 1)
                        {
                            DrawLineBetweenTwoPoints(rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position, rope[i - 1].position);

                        }


                    }
                }
            }
        }





        public void ResetRopeBitsInBetween()
        {
            ropeBitsDrawnOnScreen.Clear();

        }
    }
}
