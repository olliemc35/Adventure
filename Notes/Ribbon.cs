using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Ribbon : Rope
    {
        public List<Note> listOfNotes = new List<Note>();

        public float maxDistanceFromFixedRopeBit = 0;
        public bool maxDistanceFound = false;
        public bool inPlayersHand = false;
        //public bool inPlayersHand = true;
        public bool fixedAtNote = false;

        public bool playNotes = false;

        public int numberOfFramesEachNotePlaysFor = 60;
        public int noteCounter = 0;
        public int indexOfNoteToPlay;


        public Ribbon(Player player, Vector2 initialPosition, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager)
        {
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.assetManager = assetManager;
            this.player = player;

            CollisionObject = true;

            IndexOfRopeBitInPlayersHand = 0;
            IndexOfFirstFixedRopeBit = 0;

            //IndexOfFirstRopeBitInBetweenWhichIsNotEnabled = 0;

            NumberOfRopeBits = 50;

            frictionConstant = 2f; // This should be between 0 and 1 and dissipates energy from the rope
            groundFrictionConstant = 2f;
            groundFrictionNormalConstant = 0.3f;
            airFrictionConstant = 1f;


            // Create them all so we do not have to load content everytime
            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                RopeBit ropeBit = new RopeBit(initialPosition, assetManager);
                ropeBit.Enabled = false;
                rope.Add(ropeBit);

            }

            LengthBetweenRopeBits = 8;
            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = FindNearestInteger(rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits));
            groundFrictionNormalConstant = (float)(distanceStretchRequiredToOvercomeFriction - LengthBetweenRopeBits) / (distanceTilEquilibriumY - LengthBetweenRopeBits);

            acc = 10;

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(initialPosition, assetManager);
                ropeBit.Enabled = false;
                ropeBit.filename = "YellowDot";
                ropeBit.idleHitbox.rectangle.Width = 1;
                ropeBit.idleHitbox.rectangle.Height = 1;
                ropeBit.idleHitbox.offsetX = 3;
                ropeBit.idleHitbox.offsetY = 3;
                ropeBitsDrawnOnScreen.Add(ropeBit);

            }


            rope[0].isFixed = true;
            rope[0].Enabled = true;



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
            }


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

           

                for (int i = 0; i <= IndexOfRopeBitInPlayersHand; i++)
                {
                    rope[i].animation_Idle.Draw(spriteBatch, rope[i].drawPosition);


                    foreach (Pivot pivot in rope[i].pivotsBetweenThisRopeBitandOnePlus)
                    {
                        Rectangle rect = new Rectangle((int)pivot.position.X, (int)pivot.position.Y, 2, 2);
                        spriteBatch.Draw(player.idleHitbox.texture, rect, Color.Blue);
                    }

                }
            




        }


        public override void Update(GameTime gametime)
        {
            //Debug.WriteLine(indexOfNoteToPlay);
           
            if (inPlayersHand)
            {
                bool test = true;

                if (screenManager.activeScreen.screenNotes.Count > 0)
                {
                    foreach (Note note in screenManager.activeScreen.screenNotes)
                    {

                        if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox))
                        {
                            test = false;
                            break;
                        }

                    }

                    if (test)
                    {
                        if (inputManager.OnKeyUp(Keys.R))
                        {
                            if (listOfNotes.Count == 1)
                            {
                                inPlayersHand = false;
                                Enabled = false;
                                player.ribbonInHand = false;
                                ClearNoteBools();

                            }
                            else
                            {
                                inPlayersHand = false;
                                fixedAtNote = true;
                                FixRibbonAtLastNote();
                                player.ribbonInHand = false;
                                player.ribbonIndex = (player.ribbonIndex + 1) % 3;
                            }
                        }
                    }


                }

            }



            SimulateRibbon();

            if (playNotes)
            {
                PlayAtNote2();
            }


            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gametime);
            }

            base.Update(gametime);
        }










        public void SimulateRibbon()
        {
            //Debug.WriteLine(playNotes);

            for (int i = 0; i <= IndexOfRopeBitInPlayersHand; i++)
            {

                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousVelocity = rope[i].velocity;
                rope[i].previousPosition = rope[i].position;

                if (rope[i].isFixed)
                {
                    rope[i].velocity.X = 0;
                    rope[i].velocity.Y = 0;
                    //rope[i].animatedSprite_Idle.Position = rope[i].spritePosition;
                }


            }

            if (!rope[IndexOfRopeBitInPlayersHand].isFixed)
            {
                rope[IndexOfRopeBitInPlayersHand].position = player.ropeAnchor;
                //rope[IndexOfRopeBitInPlayersHand].animatedSprite_Idle.Position = rope[IndexOfRopeBitInPlayersHand].spritePosition;
            }


            for (int i = 0; i < IndexOfRopeBitInPlayersHand; i++)
            {


                if (Vector2.Distance(rope[i].position, rope[i + 1].position) >= LengthBetweenRopeBits)
                {
                    FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                    FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                }

            }

            for (int i = 0; i < IndexOfRopeBitInPlayersHand; i++)
            {
                if (!rope[i].isFixed)
                {

                    rope[i].FindGravityForce();

                    FindGroundFrictionWithNormal(rope[i]);
                    FindAirFrictionEULER(rope[i]);
                    rope[i].FindNormalForces();
                    FindVelocityAndDisplacementForRopeBitX(i);
                    FindVelocityAndDisplacementForRopeBitY(i);
                    colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope[i], screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);


                }
            }




            if (!maxDistanceFound && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].position, rope[IndexOfFirstFixedRopeBit].position) < Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].previousPosition, rope[IndexOfFirstFixedRopeBit].position))
            {
                maxDistanceFromFixedRopeBit = Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].previousPosition, rope[IndexOfFirstFixedRopeBit].position);
                maxDistanceFound = true;
            }


            if (maxDistanceFound && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].position, rope[IndexOfFirstFixedRopeBit].position) > maxDistanceFromFixedRopeBit)
            {
                maxDistanceFromFixedRopeBit = 0;
                maxDistanceFound = false;
            }


            // THE SPRITE COLLIDER METHOD USES INDEXOFFIRSTENABLEDROPEBIT
            //IndexOfFirstEnabledRopeBit = IndexOfRopeBitInPlayersHand;
            //spriteCollider.AdjustForRopeAgainstListOfSpritesForward(this, References.tileHitboxes[References.activeScreen.screenNumber - 1], 1, 10);


            if (IndexOfRopeBitInPlayersHand <= NumberOfRopeBits - 2 && inPlayersHand)
            {
                UpdateIndices();
            }


        }


        public void UpdateIndices()
        {



            if (Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].position, rope[IndexOfRopeBitInPlayersHand - 1].position) >= LengthBetweenRopeBits && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].position, rope[IndexOfFirstFixedRopeBit].position) > maxDistanceFromFixedRopeBit)
            {
                Vector2 displacementVec = rope[IndexOfRopeBitInPlayersHand].position - rope[IndexOfRopeBitInPlayersHand - 1].position;
                int MaxInt = FindNearestInteger(displacementVec.Length() / LengthBetweenRopeBits);
                int count = 0;


                //Debug.WriteLine(MaxInt);

                for (int i = 1; i <= MaxInt; i++)
                {
                    if (IndexOfRopeBitInPlayersHand + i <= NumberOfRopeBits - 1)
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                displacementVec.Normalize();

                if (count >= 1)
                {
                    rope[IndexOfRopeBitInPlayersHand + count].position = rope[IndexOfRopeBitInPlayersHand].position;

                    for (int i = 1; i <= count; i++)
                    {
                        Vector2 vec = displacementVec * LengthBetweenRopeBits * i;

                        rope[IndexOfRopeBitInPlayersHand - 1 + i].Enabled = true;
                        rope[IndexOfRopeBitInPlayersHand - 1 + i].position = rope[IndexOfRopeBitInPlayersHand - 1].position + vec;
                        //rope[IndexOfRopeBitInPlayersHand - 1 + i].animatedSprite_Idle.Position = rope[IndexOfRopeBitInPlayersHand - 1 + i].spritePosition;
                        rope[IndexOfRopeBitInPlayersHand - 1 + i].velocity = rope[IndexOfRopeBitInPlayersHand].velocity;

                    }

                    IndexOfRopeBitInPlayersHand += count;


                }


            }


        }





        public void FixRibbonToNote(Note note)
        {
            listOfNotes.Add(note);
            note.ribbonAttached = true;

            maxDistanceFromFixedRopeBit = 0;

            rope[IndexOfRopeBitInPlayersHand].position = player.position;
            rope[IndexOfRopeBitInPlayersHand].position.X += 0.5f * player.idleHitbox.rectangle.Width;
            rope[IndexOfRopeBitInPlayersHand].position.Y += 0.5f * player.idleHitbox.rectangle.Height;
            rope[IndexOfRopeBitInPlayersHand].velocity.X = 0;
            rope[IndexOfRopeBitInPlayersHand].velocity.Y = 0;


            rope[IndexOfRopeBitInPlayersHand].isFixed = true;

            IndexOfFirstFixedRopeBit = IndexOfRopeBitInPlayersHand;

            if (IndexOfRopeBitInPlayersHand <= NumberOfRopeBits - 2)
            {
                rope[IndexOfRopeBitInPlayersHand + 1].Enabled = true;
                rope[IndexOfRopeBitInPlayersHand + 1].position = player.ropeAnchor;
                IndexOfRopeBitInPlayersHand += 1;
            }
            else
            {
                inPlayersHand = false;
            }

        }

        public void FixRibbonAtLastNote()
        {

            if (IndexOfRopeBitInPlayersHand > IndexOfFirstEnabledRopeBit)
            {
                for (int i = IndexOfFirstEnabledRopeBit + 1; i <= IndexOfRopeBitInPlayersHand; i++)
                {
                    rope[i].Enabled = false;
                }
            }

            IndexOfRopeBitInPlayersHand = IndexOfFirstFixedRopeBit;
            inPlayersHand = false;

        }

        public void ClearNoteBools()
        {
            foreach (Note note in listOfNotes)
            {
                note.ribbonAttached = false;
            }

            playNotes = false;
        }


        public void PlayAtNote()
        {
            if (noteCounter == 0)
            {
                listOfNotes[indexOfNoteToPlay].flagPlayerInteractedWith = true;
                indexOfNoteToPlay = (indexOfNoteToPlay + 1) % listOfNotes.Count;
                noteCounter += 1;
            }
            else if (noteCounter < numberOfFramesEachNotePlaysFor)
            {
                noteCounter += 1;
            }
            else
            {
                noteCounter = 0;
            }

        }
        public void PlayAtNote2()
        {
            if (noteCounter == 0)
            {
                listOfNotes[indexOfNoteToPlay].flagPlayerInteractedWith = true;
                indexOfNoteToPlay = indexOfNoteToPlay + 1;

                if (indexOfNoteToPlay == listOfNotes.Count)
                {
                    playNotes = false;
                    inPlayersHand = false;
                    Enabled = false;
                    player.ribbonInHand = false;
                    indexOfNoteToPlay = 0;
                    ClearNoteBools();
                }
                else
                {
                    noteCounter += 1;
                }
            }
            else if (noteCounter < numberOfFramesEachNotePlaysFor)
            {
                noteCounter += 1;
            }
            else
            {
                noteCounter = 0;
            }

            //if (indexOfNoteToPlay == listOfNotes.Count - 1 && listOfNotes[listOfNotes.Count - 1].keyPlayInteractedAnimation)
            //{
            //    playNotes = false; 
            //    inPlayersHand = false;
            //    Enabled = false;
            //    player.ribbonInHand = false;
            //    indexOfNoteToPlay = 0;
            //    ClearNoteBools();
            //}
            //else if (noteCounter == 0)
            //{
            //    listOfNotes[indexOfNoteToPlay].flagPlayerInteractedWith = true;
            //    indexOfNoteToPlay = (indexOfNoteToPlay + 1) % listOfNotes.Count;

            //    //if (indexOfNoteToPlay < listOfNotes.Count - 1)
            //    //{
            //    //    indexOfNoteToPlay = (indexOfNoteToPlay + 1) % listOfNotes.Count;
            //    //}
            //    noteCounter += 1;
            //}
            //else if (noteCounter < numberOfFramesEachNotePlaysFor)
            //{
            //    noteCounter += 1;
            //}
            //else
            //{
            //    noteCounter = 0;
            //}

        }



    }
}
