using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Ribbon : Rope
    {
        public List<Note> listOfNotes = new List<Note>();
        public Player player;

        public float maxDistanceFromFixedRopeBit = 0;
        public bool maxDistanceFound = false;
        public bool inPlayersHand = true;

        public bool playNotes = false;

        public int numberOfFramesEachNotePlaysFor = 60;
        public int noteCounter = 0;
        public int indexOfNoteToPlay;


        public Ribbon(Player player, Vector2 initialPosition)
        {
            this.player = player;

            CollisionSprite = true;

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
                RopeBit ropeBit = new RopeBit(initialPosition);
                ropeBit.Enabled = false;
                rope.Add(ropeBit);

            }

            LengthBetweenRopeBits = 8;
            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = DistanceToNearestInteger(rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits));
            groundFrictionNormalConstant = (float)(distanceStretchRequiredToOvercomeFriction - LengthBetweenRopeBits) / (distanceTilEquilibriumY - LengthBetweenRopeBits);

            acc = 10;

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(initialPosition);
                ropeBit.Enabled = false;
                ropeBit.spriteFilename = "YellowDot";
                ropeBit.idleHitbox.rectangle.Width = 1;
                ropeBit.idleHitbox.rectangle.Height = 1;
                ropeBit.idleHitbox.offsetX = 3;
                ropeBit.idleHitbox.offsetY = 3;
                ropeBitsDrawnOnScreen.Add(ropeBit);

            }


            rope[0].isFixed = true;
            rope[0].Enabled = true;



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
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                foreach (HitboxRectangle hitbox in spriteHitboxes)
                {
                    if (hitbox.isActive)
                    {

                        spriteBatch.Draw(spriteHitboxTexture, hitbox.rectangle, Color.Red);
                    }
                }
            }


            for (int i = 0; i <= IndexOfRopeBitInPlayersHand; i++)
            {
                rope[i].animatedSprite_Idle.Render(spriteBatch);


                foreach (Pivot pivot in rope[i].pivotsBetweenThisRopeBitandOnePlus)
                {
                    Rectangle rect = new Rectangle((int)pivot.spritePosition.X, (int)pivot.spritePosition.Y, 2, 2);
                    spriteBatch.Draw(player.spriteHitboxTexture, rect, Color.Blue);
                }

            }




        }


        public override void Update(GameTime gametime)
        {
            //Debug.WriteLine(IndexOfRopeBitInPlayersHand);
            //Debug.WriteLine(inPlayersHand);
            Debug.WriteLine(listOfNotes.Count);

            SimulateRibbon();

            if (playNotes)
            {
                PlayAtNote();
            }


            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gametime);
            }

            base.Update(gametime);
        }










        public void SimulateRibbon()
        {



            for (int i = 0; i <= IndexOfRopeBitInPlayersHand; i++)
            {

                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousSpriteVelocity = rope[i].spriteVelocity;
                rope[i].previousSpritePosition = rope[i].spritePosition;

                if (rope[i].isFixed)
                {
                    rope[i].spriteVelocity.X = 0;
                    rope[i].spriteVelocity.Y = 0;
                    rope[i].animatedSprite_Idle.Position = rope[i].spritePosition;
                }


            }

            if (!rope[IndexOfRopeBitInPlayersHand].isFixed)
            {
                rope[IndexOfRopeBitInPlayersHand].spritePosition = player.ropeAnchor;
                rope[IndexOfRopeBitInPlayersHand].animatedSprite_Idle.Position = rope[IndexOfRopeBitInPlayersHand].spritePosition;
            }


            for (int i = 0; i < IndexOfRopeBitInPlayersHand; i++)
            {


                if (Vector2.Distance(rope[i].spritePosition, rope[i + 1].spritePosition) >= LengthBetweenRopeBits)
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
                    spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope[i], References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);


                }
            }




            if (!maxDistanceFound && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].spritePosition, rope[IndexOfFirstFixedRopeBit].spritePosition) < Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].previousSpritePosition, rope[IndexOfFirstFixedRopeBit].spritePosition))
            {
                maxDistanceFromFixedRopeBit = Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].previousSpritePosition, rope[IndexOfFirstFixedRopeBit].spritePosition);
                maxDistanceFound = true;
            }


            if (maxDistanceFound && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].spritePosition, rope[IndexOfFirstFixedRopeBit].spritePosition) > maxDistanceFromFixedRopeBit)
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



            if (Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].spritePosition, rope[IndexOfRopeBitInPlayersHand - 1].spritePosition) >= LengthBetweenRopeBits && Vector2.Distance(rope[IndexOfRopeBitInPlayersHand].spritePosition, rope[IndexOfFirstFixedRopeBit].spritePosition) > maxDistanceFromFixedRopeBit)
            {
                Vector2 displacementVec = rope[IndexOfRopeBitInPlayersHand].spritePosition - rope[IndexOfRopeBitInPlayersHand - 1].spritePosition;
                int MaxInt = DistanceToNearestInteger(displacementVec.Length() / LengthBetweenRopeBits);
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
                    rope[IndexOfRopeBitInPlayersHand + count].spritePosition = rope[IndexOfRopeBitInPlayersHand].spritePosition;

                    for (int i = 1; i <= count; i++)
                    {
                        Vector2 vec = displacementVec * LengthBetweenRopeBits * i;

                        rope[IndexOfRopeBitInPlayersHand - 1 + i].Enabled = true;
                        rope[IndexOfRopeBitInPlayersHand - 1 + i].spritePosition = rope[IndexOfRopeBitInPlayersHand - 1].spritePosition + vec;
                        rope[IndexOfRopeBitInPlayersHand - 1 + i].animatedSprite_Idle.Position = rope[IndexOfRopeBitInPlayersHand - 1 + i].spritePosition;
                        rope[IndexOfRopeBitInPlayersHand - 1 + i].spriteVelocity = rope[IndexOfRopeBitInPlayersHand].spriteVelocity;

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

            rope[IndexOfRopeBitInPlayersHand].spritePosition = player.spritePosition;
            rope[IndexOfRopeBitInPlayersHand].spritePosition.X += 0.5f * player.idleHitbox.rectangle.Width;
            rope[IndexOfRopeBitInPlayersHand].spritePosition.Y += 0.5f * player.idleHitbox.rectangle.Height;
            rope[IndexOfRopeBitInPlayersHand].spriteVelocity.X = 0;
            rope[IndexOfRopeBitInPlayersHand].spriteVelocity.Y = 0;


            rope[IndexOfRopeBitInPlayersHand].isFixed = true;

            IndexOfFirstFixedRopeBit = IndexOfRopeBitInPlayersHand;

            if (IndexOfRopeBitInPlayersHand <= NumberOfRopeBits - 2)
            {
                rope[IndexOfRopeBitInPlayersHand + 1].Enabled = true;
                rope[IndexOfRopeBitInPlayersHand + 1].spritePosition = player.ropeAnchor;
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




    }
}
