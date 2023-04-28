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
    public class RopeForPlayer : Rope
    {

        public bool HaveIDeterminedNewLengthYet = false;

        public bool hookLanded = false;
        public bool atRestTest = false;
        public bool ropeAtRest = false;
        public bool readyToAttach = true;

        public bool ropeActive = false;
        public bool pullBackRope = false;

        // Smoothing code which is currently not used
        public int smoothingParam = 1;
        public int previousPositionsCounter = 0;
        public int previousPositionsCounterTimer = 0;
        public int numberOfPreviousPositions = 30;
        public bool previousPositionsFilled = false;
        public bool stutteringX = false;
        public bool stutteringY = false;

        public bool TurnOffLastRopeBit = false;
        public bool foundDistanceFromRopeHookWhenAtRest = false;

        public float counterForPullBack = 0;
        public int counterForStutteringFix = 0;


        public float maxForce = 0;



        public bool noMoreIndices = false;

        public int IndexWhenRopeAttached;

        public RopeForPlayer(Vector2 spritePosition, Player player, AssetManager assetManager, ScreenManager screenManager) : base(spritePosition, assetManager, screenManager)
        {
            IndexOfFirstEnabledRopeBit = 0;
            IndexOfFirstRopeBitAnchor = 1;
            IndexOfFirstRopeBitInBetweenWhichIsNotEnabled = 0;

            // previously 10
            NumberOfRopeBits = 8;
            LengthBetweenRopeBits = 8; // currently we move 2 pixels horizontally every frame so make it even so when we update indices we can detect exactly when a new one should be enabled
            acc = 10;

            frictionConstant = 2f; // This should be between 0 and 1 and dissipates energy from the rope
            groundFrictionConstant = 2f;
            groundFrictionNormalConstant = 0.3f;
            airFrictionConstant = 1f;

            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                RopeBit ropeBit = new RopeBit(spritePosition, numberOfPreviousPositions, assetManager);
                ropeBit.Enabled = false;
                ropeBit.MaxDistanceFromRopeAnchor = LengthBetweenRopeBits * (NumberOfRopeBits - 1 - i);
                rope.Add(ropeBit);


            }

           

            // Highlight means will always show up ... this is used for debugging code in sprite collider
            //rope[0].Highlight = true;
            //rope[0].Highlight = true;
            rope[0].Enabled = true;
            //rope[0].mass *= 1.5f;
            IndexOfFirstEnabledRopeBit = 0;
            rope[1].ThisIsTheRopeAnchor = true;
            RopeLength = (NumberOfRopeBits - 1) * LengthBetweenRopeBits;
            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = FindNearestInteger(rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits));
            groundFrictionNormalConstant = (float)(distanceStretchRequiredToOvercomeFriction - LengthBetweenRopeBits) / (distanceTilEquilibriumY - LengthBetweenRopeBits);






            this.player = player;
            maxForce = 4 * springConstant * LengthBetweenRopeBits;

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(spritePosition, assetManager);
                ropeBit.Enabled = false;
                ropeBit.filename = "YellowDot";

                ropeBitsDrawnOnScreen.Add(ropeBit);

            }

        }

        public override void LoadContent()
        {
            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                rope[i].LoadContent();

            }

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                ropeBitsDrawnOnScreen[i].LoadContent();
                ropeBitsDrawnOnScreen[i].idleHitbox.rectangle.Width = 1;
                ropeBitsDrawnOnScreen[i].idleHitbox.rectangle.Height = 1;
                ropeBitsDrawnOnScreen[i].idleHitbox.offsetX = 3;
                ropeBitsDrawnOnScreen[i].idleHitbox.offsetY = 3;
            }


        }


        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine(rope[1].pivotPointBetweenThisRopeBitandOneMinus.spritePosition.X);
            //Debug.WriteLine("0 position is " + rope[0].spritePosition.X);
            //Debug.WriteLine("1 position is " + rope[1].spritePosition.X);
            if (ropeActive)
            {
                //Debug.WriteLine(rope[0].totalForce.X);
                //if (rope[0].SpriteCollidedOnBottom)
                //{
                //    Debug.WriteLine("here");
                //}
                ropeAnchor = player.ropeAnchor;

                //Debug.WriteLine("x" + rope[3].spriteVelocity.X);
                //Debug.WriteLine("y" + rope[3].spriteVelocity.Y);

                if (screenManager.activeScreen.screenGameObjects != null)
                {
                    bool resetReadyToAttach = true;

                    foreach (GameObject gameObject in screenManager.activeScreen.screenGameObjects)
                    {
                        if (gameObject is AnimatedGameObject sprite)
                        {

                            if ((sprite.filename == "Post" || sprite.filename == "PostDown") && rope[0].colliderManager.CheckForCollision(rope[0].idleHitbox, sprite.idleHitbox))
                            {
                                resetReadyToAttach = false;

                                if (sprite.filename == "PostDown")
                                {
                                    player.attachedToASwingingPivot = true;
                                }

                                if (readyToAttach)
                                {
                                    rope[0].position.X = sprite.position.X;
                                    rope[0].position.Y = sprite.position.Y;
                                    rope[0].velocity.X = 0;
                                    rope[0].velocity.Y = 0;
                                    rope[0].displacement.X = 0;
                                    rope[0].displacement.Y = 0;
                                    rope[0].idleHitbox.rectangle.X = (int)rope[0].position.X + rope[0].idleHitbox.offsetX;
                                    rope[0].idleHitbox.rectangle.Y = (int)rope[0].position.Y + rope[0].idleHitbox.offsetY;
                                    //rope[0].animatedSprite_Idle.Position = rope[0].spritePosition;
                                    rope[0].attached = true;
                                }
                            }
                        }

                    }

                    if (!readyToAttach && resetReadyToAttach)
                    {
                        readyToAttach = true;
                    }
                }

                if (!pullBackRope)
                {

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

                    if (!rope[0].attached && IndexOfFirstEnabledRopeBit < rope.Count() - 1)
                    {
                        SimulateRopeBeingThrown();
                    }
                    else if (!rope[0].attached && IndexOfFirstEnabledRopeBit == rope.Count() - 1)
                    {
                        SimulateRopeAllRopeBitsOut();
                    }
                    else if (rope[0].attached)
                    {
                        if (!foundDistanceFromRopeHookWhenAtRest)
                        {
                            //
                            //IndexWhenRopeAttached = IndexOfFirstEnabledRopeBit;
                            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
                            {

                                if (i == IndexOfFirstEnabledRopeBit)
                                {
                                    player.distanceFromRopeHookWhenAttached += Vector2.Distance(rope[i].position, ropeAnchor);

                                }
                                else
                                {
                                    player.distanceFromRopeHookWhenAttached += Vector2.Distance(rope[i].position, rope[i + 1].position);
                                }
                            }

                            foundDistanceFromRopeHookWhenAtRest = true;
                        }

                        SimulateRopeWhenAttached();


                    }
                }
                else if (pullBackRope)
                {
                    ropeAtRest = false;
                    SimulateRopePullBackNOPHYSICS();
                }




                if (rope[0].CollidedOnBottom)
                {
                    previousPositionsCounter += 1;
                    // previousPositionsCounterTimer += 1;

                    if (previousPositionsCounter > numberOfPreviousPositions - 1)
                    {
                        previousPositionsCounter = 0;
                        previousPositionsFilled = true;
                    }
                }


            }

            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gameTime);
            }

            //base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                foreach (HitboxRectangle hitbox in hitboxes)
                {
                    if (hitbox.isActive)
                    {

                        spriteBatch.Draw(idleHitbox.texture, hitbox.rectangle, Color.Red);
                    }
                }
            }

            if (ropeActive)
            {
                //for (int i = 0; i <= 1; i++)
                //{

                //    rope[i].animatedSprite.Render(spriteBatch);

                //}

                for (int i = 0; i <= IndexOfFirstRopeBitAnchor; i++)
                {
                    rope[i].animation_Idle.Draw(spriteBatch, rope[i].drawPosition);


                    foreach (Pivot pivot in rope[i].pivotsBetweenThisRopeBitandOnePlus)
                    {
                        Rectangle rect = new Rectangle((int)pivot.position.X, (int)pivot.position.Y, 2, 2);
                        spriteBatch.Draw(player.idleHitbox.texture, rect, Color.Blue);
                    }
                    //if (rope[i].pivotBetweenThisRopeBitandOnePlus.isPivotActive)
                    //{
                    //    Rectangle rect = new Rectangle((int)rope[i].pivotBetweenThisRopeBitandOnePlus.spritePosition.X, (int)rope[i].pivotBetweenThisRopeBitandOnePlus.spritePosition.Y, 2, 2);
                    //    spriteBatch.Draw(player.spriteHitboxTexture, rect, Color.Blue);
                    //}
                }

                //for (int i = 0; i<= ropeBitsDrawnOnScreen.Count()-1; i++)
                //{
                //    if (ropeBitsDrawnOnScreen[i].Enabled)
                //    {
                //        ropeBitsDrawnOnScreen[i].animatedSprite.Render(spriteBatch);
                //    }

                //}

            }
        }




        public void SimulateRopeBeingThrown()
        {
            // Whichever ropeBit is the anchor will never move and be fixed to the ropeAnchor point
            rope[IndexOfFirstRopeBitAnchor].previousPosition.X = rope[IndexOfFirstRopeBitAnchor].position.X;
            rope[IndexOfFirstRopeBitAnchor].previousPosition.Y = rope[IndexOfFirstRopeBitAnchor].position.Y;
            rope[IndexOfFirstRopeBitAnchor].position.X = ropeAnchor.X;
            rope[IndexOfFirstRopeBitAnchor].position.Y = ropeAnchor.Y;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.X = (int)rope[IndexOfFirstRopeBitAnchor].position.X + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetX;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.Y = (int)rope[IndexOfFirstRopeBitAnchor].position.Y + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetY;
            //rope[IndexOfFirstRopeBitAnchor].animatedSprite_Idle.Position = rope[IndexOfFirstRopeBitAnchor].spritePosition;

            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {

                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousVelocity = rope[i].velocity;
                rope[i].previousPosition = rope[i].position;


            }

            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {

                if (i != IndexOfFirstRopeBitAnchor)
                {
                    if (rope[i + 1].Enabled)
                    {
                        FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                        FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                    }

                }
            }

            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {
                if (i != IndexOfFirstRopeBitAnchor)
                {

                    rope[i].FindGravityForce();
                    FindGroundFrictionWithNormal(rope[i]);
                    FindAirFrictionEULER(rope[i]);
                    rope[i].FindNormalForces();

                    if (rope[i].FirstLoopX && rope[i].FirstLoopY && i > 0)
                    {
                        int j = i - 1;
                        int ctr = 0;


                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (rope[k].FirstLoopX && rope[k].FirstLoopY)
                            {
                                ctr += 1;
                            }
                            else
                            {
                                break;
                            }
                        }


                        j -= ctr;


                        Vector2 direction = rope[j].position - rope[i].position;
                        direction.Normalize();
                        rope[i].velocity = Vector2.Dot(rope[j].velocity, direction) * direction;

                    }

                    FindVelocityAndDisplacementForRopeBitX(i);
                    FindVelocityAndDisplacementForRopeBitY(i);


                }
            }


            colliderManager.AdjustForRopeAgainstListOfSpritesForward(this, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);


            if (IndexOfFirstEnabledRopeBit <= NumberOfRopeBits - 2)
            {
                UpdateIndicesThrowing();
            }
            else
            {
                noMoreIndices = true;
            }


        }

        public void SimulateRopeAllRopeBitsOut()
        {
            rope[IndexOfFirstRopeBitAnchor].previousPosition.X = rope[IndexOfFirstRopeBitAnchor].position.X;
            rope[IndexOfFirstRopeBitAnchor].previousPosition.Y = rope[IndexOfFirstRopeBitAnchor].position.Y;
            rope[IndexOfFirstRopeBitAnchor].position.X = ropeAnchor.X;
            rope[IndexOfFirstRopeBitAnchor].position.Y = ropeAnchor.Y;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.X = (int)rope[IndexOfFirstRopeBitAnchor].position.X + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetX;
            rope[IndexOfFirstRopeBitAnchor].idleHitbox.rectangle.Y = (int)rope[IndexOfFirstRopeBitAnchor].position.Y + rope[IndexOfFirstRopeBitAnchor].idleHitbox.offsetY;
            rope[IndexOfFirstRopeBitAnchor].velocity = player.velocity; // this is important so the the rope frictional force behaves properly
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
                    if (rope[i + 1].Enabled)
                    {

                        if (Vector2.Distance(rope[i].position, rope[i + 1].position) >= LengthBetweenRopeBits)
                        {
                            FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                            FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                        }

                    }
                }

            }

            for (int i = IndexOfFirstEnabledRopeBit; i >= 0; i--)
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

            colliderManager.AdjustForRopeAgainstListOfSpritesBackward(this, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }






        public void SimulateRopeWhenAttached()
        {
            rope[IndexOfFirstRopeBitAnchor].previousPosition.X = rope[IndexOfFirstRopeBitAnchor].position.X;
            rope[IndexOfFirstRopeBitAnchor].previousPosition.Y = rope[IndexOfFirstRopeBitAnchor].position.Y;
            rope[IndexOfFirstRopeBitAnchor].position.X = ropeAnchor.X;
            rope[IndexOfFirstRopeBitAnchor].position.Y = ropeAnchor.Y;
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
                    if (rope[i + 1].Enabled)
                    {

                        if (Vector2.Distance(rope[i].position, rope[i + 1].position) >= LengthBetweenRopeBits)
                        {
                            FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                            FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                        }

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


            colliderManager.AdjustForRopeAgainstListOfSpritesBackward(this, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);



            // Here we update the index
            //if (player.spriteVelocity.Length() > 0.01)
            //{


            //    int ctr = 0;

            //    for (int i = IndexOfFirstEnabledRopeBit; i >= 0; i--)
            //    {
            //        if (i != IndexOfFirstRopeBitAnchor)
            //        {
            //            Vector2 displacement = rope[i].spritePosition - ropeAnchor;


            //            if (displacement.Length() <= LengthBetweenRopeBits && (Math.Abs(displacement.X) < 1 || Math.Abs(displacement.Y) < 1))
            //            {

            //                //rope[i].Enabled = false;
            //                ctr += 1;

            //            }

            //        }
            //    }

            //    if (ctr > 0)
            //    {


            //        IndexOfFirstEnabledRopeBit -= ctr;
            //        IndexOfFirstEnabledRopeBit = Math.Max(IndexOfFirstEnabledRopeBit, 0);
            //        IndexOfFirstRopeBitAnchor = IndexOfFirstEnabledRopeBit + 1;

            //        //for (int i = IndexOfFirstRopeBitAnchor; i< rope.Count(); i++)
            //        //{
            //        //    rope[i].Enabled = false;
            //        //}
            //        //foundDistanceFromRopeHookWhenAtRest = false;
            //        //player.distanceFromRopeHookWhenAttached = 0;


            //    }


            //    if (IndexOfFirstEnabledRopeBit < IndexWhenRopeAttached)
            //    {
            //        if (Vector2.Distance(ropeAnchor, rope[IndexOfFirstEnabledRopeBit].spritePosition) > LengthBetweenRopeBits)
            //        {
            //            IndexOfFirstEnabledRopeBit += 1;
            //            rope[IndexOfFirstEnabledRopeBit].Enabled = true;
            //            rope[IndexOfFirstEnabledRopeBit].spritePosition = ropeAnchor;
            //            rope[IndexOfFirstEnabledRopeBit].animationPosition.X = DistanceToNearestInteger(rope[IndexOfFirstEnabledRopeBit].spritePosition.X);
            //            rope[IndexOfFirstEnabledRopeBit].animationPosition.Y = DistanceToNearestInteger(rope[IndexOfFirstEnabledRopeBit].spritePosition.Y);
            //            rope[IndexOfFirstEnabledRopeBit].animatedSprite.Position = rope[IndexOfFirstEnabledRopeBit].animationPosition;


            //            //rope[IndexOfFirstEnabledRopeBit].idleHitbox.rectangle.X = rope[IndexOfFirstEnabledRopeBit].spritePosition.X + rope[IndexOfFirstEnabledRopeBit].idleHitbox.offsetX;


            //            if (IndexOfFirstEnabledRopeBit == rope.Count() - 1)
            //            {
            //                IndexOfFirstRopeBitAnchor = IndexOfFirstEnabledRopeBit;
            //                rope[IndexOfFirstRopeBitAnchor].Enabled = true;
            //            }
            //            else
            //            {
            //                IndexOfFirstRopeBitAnchor = IndexOfFirstEnabledRopeBit + 1;
            //                rope[IndexOfFirstRopeBitAnchor].Enabled = true;
            //            }
            //        }
            //    }

            //}


        }



        public void FixStuttering(int error)
        {




            for (int i = 1; i <= IndexOfFirstEnabledRopeBit; i++)
            {
                bool stutterTestX = true;
                bool stutterTestY = true;

                bool atRest = true;

                for (int k = 0; k < numberOfPreviousPositions; k++)
                {
                    if (!(rope[i].position.X == rope[i].previousPositions[k].X && rope[i].position.Y == rope[i].previousPositions[k].Y))
                    {
                        atRest = false;
                    }
                    if (Math.Abs(rope[i].position.X - rope[i].previousPositions[k].X) > error)
                    {
                        stutterTestX = false;
                    }
                    if (Math.Abs(rope[i].position.Y - rope[i].previousPositions[k].Y) > error)
                    {
                        stutterTestY = false;
                    }

                }

                if ((!stutterTestX && !stutterTestY) || atRest)
                {
                    continue;
                }

                float testX = 0;
                float testY = 0;


                //if (i == 2)
                //{
                //    Debug.WriteLine("here");
                //}
                for (int k = 0; k < numberOfPreviousPositions; k++)
                {
                    testX += rope[i].previousPositions[k].X;
                    testY += rope[i].previousPositions[k].Y;
                }

                if (stutterTestX)
                {
                    rope[i].position.X = FindNearestInteger((float)testX / numberOfPreviousPositions);

                }

                if (stutterTestY)
                {

                    rope[i].position.Y = FindNearestInteger((float)testY / numberOfPreviousPositions);

                }

                rope[i].idleHitbox.rectangle.X = (int)rope[i].position.X + rope[i].idleHitbox.offsetX;
                rope[i].idleHitbox.rectangle.Y = (int)rope[i].position.Y + rope[i].idleHitbox.offsetY;


            }

        }


        public void FixStutteringTest()
        {
            //Is this necessary ???
            //This is doing the average every frame
            // Stuttering and the dragging effect are interlinked - sort this out
            if ((rope[0].CollidedOnBottom || Vector2.Distance(rope[0].position, ropeAnchor) >= rope[0].MaxDistanceFromRopeAnchor || rope[0].attached) && player.velocity.X == 0)
            {
                counterForStutteringFix += 1;

                if (counterForStutteringFix <= 60)
                {
                    FixStuttering(1);
                }
                else if (counterForStutteringFix <= 120)
                {
                    FixStuttering(2);
                }
                else
                {
                    FixStuttering(3);
                }

            }
            else
            {
                counterForStutteringFix = 0;
            }
        }

        public void SimulateRopePullBackNOPHYSICS()
        {

            if (counterForPullBack < 0.001)
            {
                if (TurnOffLastRopeBit)
                {
                    rope[IndexOfFirstRopeBitAnchor].ThisIsTheRopeAnchor = false;
                    rope[IndexOfFirstRopeBitAnchor].Enabled = false;
                    IndexOfFirstRopeBitAnchor = 1;
                    TurnOffLastRopeBit = false;
                }

                if (IndexOfFirstEnabledRopeBit == 0)
                {
                    rope[0].Enabled = false;
                    TurnOffLastRopeBit = true;
                }
                else
                {

                    rope[IndexOfFirstEnabledRopeBit].Enabled = false;

                    for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
                    {
                        if (IndexOfFirstEnabledRopeBit < NumberOfRopeBits - 1)
                        {
                            rope[i].position = rope[i + 1].position;
                        }
                        else
                        {
                            rope[i].position = ropeAnchor;
                        }

                        //rope[i].animatedSprite_Idle.Position = rope[i].spritePosition;
                    }


                    IndexOfFirstEnabledRopeBit -= 1;

                }

            }



            counterForPullBack += rope[0].deltaTime;


            if (counterForPullBack > 3 * rope[0].deltaTime)
            {
                counterForPullBack = 0;
            }


        }




        public void OnlyUpdateVelocityAndDisplacementLEFT(int i)
        {
            if (rope[i].velocity.X > 0)
            {
                rope[i].velocity.X = 0;
                rope[i].displacement.X = 0;
            }


            if (rope[i].totalForce.X <= 0)
            {
                rope[i].displacement.X += rope[i].totalForce.X * rope[i].deltaTime * rope[i].deltaTime;
                rope[i].velocity.X += rope[i].totalForce.X * rope[i].deltaTime;

            }
        }

        public void OnlyUpdateVelocityAndDisplacementRIGHT(int i)
        {
            if (rope[i].velocity.X < 0)
            {
                rope[i].velocity.X = 0;
                rope[i].displacement.X = 0;
            }


            if (rope[i].totalForce.X >= 0)
            {
                rope[i].displacement.X += rope[i].totalForce.X * rope[i].deltaTime * rope[i].deltaTime;
                rope[i].velocity.X += rope[i].totalForce.X * rope[i].deltaTime;

            }
        }

        public void OnlyUpdateVelocityAndDisplacementUP(int i)
        {
            if (rope[i].velocity.Y > 0)
            {
                rope[i].velocity.Y = 0;
                rope[i].displacement.Y = 0;
            }

            if (rope[i].totalForce.Y <= 0)
            {
                rope[i].displacement.Y += rope[i].totalForce.Y * rope[i].deltaTime * rope[i].deltaTime;
                rope[i].velocity.Y += rope[i].totalForce.Y * rope[i].deltaTime;
            }
        }


        public void OnlyUpdateVelocityAndDisplacementDOWN(int i)
        {
            if (rope[i].velocity.Y < 0)
            {
                rope[i].velocity.Y = 0;
                rope[i].displacement.Y = 0;
            }


            if (rope[i].totalForce.Y >= 0)
            {
                rope[i].displacement.Y += rope[i].totalForce.Y * rope[i].deltaTime * rope[i].deltaTime;
                rope[i].velocity.Y += rope[i].totalForce.Y * rope[i].deltaTime;

            }
        }





        public void UpdateIndicesThrowing()
        {



            if (Vector2.Distance(rope[IndexOfFirstEnabledRopeBit].position, ropeAnchor) >= LengthBetweenRopeBits)
            {
                Vector2 displacementVec = rope[IndexOfFirstEnabledRopeBit].position - ropeAnchor;
                int MaxInt = FindNearestInteger(Vector2.Distance(rope[IndexOfFirstEnabledRopeBit].position, ropeAnchor) / LengthBetweenRopeBits);
                int count = 0;



                for (int i = 1; i <= MaxInt; i++)
                {
                    if (IndexOfFirstEnabledRopeBit + i <= NumberOfRopeBits - 1)
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count >= 1)
                {
                    for (int i = 1; i <= count; i++)
                    {

                        rope[IndexOfFirstEnabledRopeBit + i].Enabled = true;
                        rope[IndexOfFirstEnabledRopeBit + i].position = ropeAnchor + displacementVec * ((float)(count - i) / count);
                        //rope[IndexOfFirstEnabledRopeBit + i].animatedSprite_Idle.Position = rope[IndexOfFirstEnabledRopeBit + i].spritePosition;


                        // Update idleHitbox position, but this is probably unnecessary as this is done in spriteCollider
                        // Keeping everything as floats at this stage just improves accuracy
                        rope[IndexOfFirstEnabledRopeBit + i].idleHitbox.rectangle.X = (int)rope[IndexOfFirstEnabledRopeBit + i].position.X + rope[IndexOfFirstEnabledRopeBit + i].idleHitbox.offsetX;
                        rope[IndexOfFirstEnabledRopeBit + i].idleHitbox.rectangle.Y = (int)rope[IndexOfFirstEnabledRopeBit + i].position.Y + rope[IndexOfFirstEnabledRopeBit + i].idleHitbox.offsetY;
                        //rope[IndexOfFirstEnabledRopeBit + i].spriteVelocity = rope[IndexOfFirstEnabledRopeBit].spriteVelocity;

                        //float speed = rope[IndexOfFirstEnabledRopeBit].spriteVelocity.Length();
                        //Vector2 direction = rope[IndexOfFirstEnabledRopeBit].spritePosition - rope[IndexOfFirstEnabledRopeBit + i].spritePosition;
                        //direction.Normalize();

                        //rope[IndexOfFirstEnabledRopeBit + i].spriteVelocity = 0.8f * Vector2.Dot(rope[IndexOfFirstEnabledRopeBit].spriteVelocity, direction) * direction;

                    }

                    IndexOfFirstEnabledRopeBit += count;

                    IndexOfFirstRopeBitAnchor = Math.Min(IndexOfFirstEnabledRopeBit + 1, NumberOfRopeBits - 1);

                    if (IndexOfFirstRopeBitAnchor < NumberOfRopeBits - 1)
                    {
                        rope[IndexOfFirstRopeBitAnchor].Enabled = false;
                    }
                    else
                    {
                        rope[IndexOfFirstRopeBitAnchor].Enabled = true;
                    }



                }


            }






        }


        public void UpdateIndicesPullBack()
        {

            int ctr = 0;

            for (int i = IndexOfFirstEnabledRopeBit; i >= 0; i--)
            {
                if (Vector2.Distance(rope[i].position, ropeAnchor) <= LengthBetweenRopeBits)
                {
                    rope[i].Enabled = false;
                    ctr += 1;
                }
            }

            if (IndexOfFirstEnabledRopeBit - ctr > 0)
            {
                IndexOfFirstEnabledRopeBit -= ctr;
            }
            else
            {
                IndexOfFirstEnabledRopeBit = 0;
            }
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



                for (int k = 0; k < numberOfPreviousPositions; k++)
                {
                    rope[i].previousPositions[k] = new Vector2(0, 0);
                }
            }


            for (int i = 0; i < ropeBitsDrawnOnScreen.Count(); i++)
            {
                ropeBitsDrawnOnScreen[i].Enabled = false;
            }


            //ResetRopeBitsInBetween();
            rope[0].attached = false;
            hookLanded = false;
            atRestTest = false;
            ropeAtRest = false;
            TurnOffLastRopeBit = false;
            HaveIDeterminedNewLengthYet = false;
            previousPositionsFilled = false;
            noMoreIndices = false;
            foundDistanceFromRopeHookWhenAtRest = false;
            previousPositionsCounter = 0;
            previousPositionsCounterTimer = 0;
            counterForStutteringFix = 0;
            //IndexOfFirstEnabledRopeBit = 0;
            //IndexOfFirstRopeBitAnchor = 1;
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
