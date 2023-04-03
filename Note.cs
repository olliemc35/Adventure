using Microsoft.Xna.Framework.Audio;
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
    public class Note : Sprite
    {
        public MovingSprite key;


        public string symbolFilename;

        public List<MovingPlatformOneLoop> movingPlatformsOneLoop;
        public List<MovingPlatformNoLoop> movingPlatformsNoLoop;
        public List<MovingPlatformHalfLoop> movingPlatformsHalfLoop;
        public NoteShip noteShip;
        public int displacementScalingForNoteShip;

        public List<Gate> gates;
        public LaunchPad launchPad;

        public float orbSpeed;
        public string orbFilename;


        public List<int> indicesOfAttachedGates = new List<int>();

        public bool ribbonAttached = false;
        public Vector2 ribbonPosition = new Vector2();

        public string noteValue;
        public SoundEffect noteSound;

        public List<SoundEffectInstance> noteInstances = new List<SoundEffectInstance>();

        public float duration;
        public bool playerInteractedWith = false;
        public bool keyPlayInteractedAnimation = false;
        public bool flagPlayerInteractedWith = false;
        public bool movePlatform = false;
        public bool symbolTurnedOn = false;
        int counter = 0;

        public List<int> indicesToRemove = new List<int>();

        int numberOfFramesBetweenPlayerInteractions = 60;

        public Note(Vector2 keyPosition, string keyFilename, string symbolFilename, string noteValue)
        {
            this.noteValue = noteValue;
            this.symbolFilename = symbolFilename;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }


        public Note(Vector2 keyPosition, string keyFilename, string symbolFilename, string noteValue, List<Gate> gates)
        {
            this.noteValue = noteValue;
            this.symbolFilename = symbolFilename;
            this.gates = gates;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }

        public Note(Vector2 keyPosition, string keyFilename, string symbolFilename, string noteValue, List<MovingPlatformNoLoop> movingPlatformsNoLooping)
        {
            this.noteValue = noteValue;
            this.symbolFilename = symbolFilename;
            this.movingPlatformsNoLoop = movingPlatformsNoLooping;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }

        public Note(Vector2 keyPosition, string keyFilename, string noteValue, List<MovingPlatformOneLoop> movingPlatformsOneLoop)
        {
            this.noteValue = noteValue;
            this.movingPlatformsOneLoop = movingPlatformsOneLoop;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }

        public Note(Vector2 keyPosition, string keyFilename, string noteValue, List<MovingPlatformHalfLoop> movingPlatformsHalfLoop)
        {
            this.noteValue = noteValue;
            this.movingPlatformsHalfLoop = movingPlatformsHalfLoop;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }


        public Note(Vector2 keyPosition, string keyFilename, string noteValue, NoteShip noteShip, int displacementScalingForNoteShip)
        {
            this.noteValue = noteValue;
            this.noteShip = noteShip;
            this.displacementScalingForNoteShip = displacementScalingForNoteShip;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }



        public Note(Vector2 keyPosition, string keyFilename, string noteValue, LaunchPad launchPad)
        {
            this.noteValue = noteValue;
            this.launchPad = launchPad;
            key = new MovingSprite(keyPosition, keyFilename);
            spriteFilename = null;

        }


        public Note(Vector2 keyPosition, string keyFilename, string symbolFilename, string noteValue, string orbFilename, float speedOfOrb)
        {
            this.noteValue = noteValue;
            this.symbolFilename = symbolFilename;
            key = new MovingSprite(keyPosition, keyFilename);

            orbSpeed = speedOfOrb;
            this.orbFilename = orbFilename;

            spriteFilename = null;
        }


        // If I want bell keys think about how I will implement it - was very messy 
        //public Note(string keyFilename, float impulseForceMax, float impulseForceDuration, Vector2 ropeAnchor, int NumberOfRopeBits, int LengthBetweenRopeBits, string symbolFilename, string noteValue, string orbFilename, float speedOfOrb)
        //{
        //    this.noteValue = noteValue;
        //    key = new HangingRopeWithWeightAttached(keyFilename, impulseForceMax, impulseForceDuration, ropeAnchor, NumberOfRopeBits, LengthBetweenRopeBits);
        //    this.symbolFilename = symbolFilename;

        //    orbSpeed = speedOfOrb;
        //    this.orbFilename = orbFilename;

        //    CollisionSprite = true;
        //    spriteFilename = null;
        //}


        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {

            key.LoadContent(contentManager, graphicsDevice);



            noteSound = contentManager.Load<SoundEffect>(noteValue);

            //Debug.WriteLine(noteSound.Duration.TotalSeconds);



        }


        public override void Update(GameTime gameTime)
        {
            if (flagPlayerInteractedWith)
            {
                flagPlayerInteractedWith = false;
                CreateAndPlayNote();

            }

            if (playerInteractedWith && counter < numberOfFramesBetweenPlayerInteractions)
            {
                counter += 1;
            }
            else if (playerInteractedWith && counter >= numberOfFramesBetweenPlayerInteractions)
            {
                playerInteractedWith = false;
                counter = 0;
            }

            if (noteInstances.Count() > 0)
            {
                for (int i = noteInstances.Count - 1; i >= 0; i--)
                {
                    if (noteInstances[i].State == SoundState.Stopped)
                    {
                        noteInstances.RemoveAt(i);
                    }
                }
            }

            if (noteInstances.Count() == 0)
            {
                keyPlayInteractedAnimation = false;
            }


            ManageAnimations();


            key.Update(gameTime);





        }

        public void CreateAndPlayNote()
        {
            playerInteractedWith = true;
            keyPlayInteractedAnimation = true;
            noteInstances.Add(noteSound.CreateInstance());
            noteInstances.Last().Play();

            if (gates != null)
            {
                foreach (Gate gate in gates)
                {
                    if (gate.open)
                    {
                        gate.open = false;
                    }
                    else
                    {
                        gate.open = true;
                    }
                }
            }

            if (movingPlatformsOneLoop != null)
            {
                foreach (MovingPlatformOneLoop movingPlatform in movingPlatformsOneLoop)
                {
                    if (!movingPlatform.movePlatform)
                    {
                        movingPlatform.movePlatform = true;
                    }
                }

            }


            if (movingPlatformsHalfLoop != null)
            {
                foreach (MovingPlatformHalfLoop movingPlatform in movingPlatformsHalfLoop)
                {
                    if (!movingPlatform.movePlatform)
                    {
                        movingPlatform.movePlatform = true;
                    }
                }

            }

            if (noteShip != null)
            {
                // Do a !noteShip.moveVertically check here perhaps?
                noteShip.displacementScaling = displacementScalingForNoteShip;
                noteShip.moveVertically = true;
            }

            if (movingPlatformsNoLoop != null)
            {
                int indexofFirstPlatformNotMoving = -1;

                for (int i = 0; i < movingPlatformsNoLoop.Count; i++)
                {
                    if (movingPlatformsNoLoop[i].horizontalMovement)
                    {
                        if (movingPlatformsNoLoop[i].spritePosition.X == movingPlatformsNoLoop[i].startPosition.X)
                        {
                            indexofFirstPlatformNotMoving = i;
                            break;
                        }
                    }
                    else
                    {
                        if (movingPlatformsNoLoop[i].spritePosition.Y == movingPlatformsNoLoop[i].startPosition.Y)
                        {
                            indexofFirstPlatformNotMoving = i;
                            break;
                        }
                    }
                }

                if (indexofFirstPlatformNotMoving == -1)
                {
                    // do nothing - I need to supply more platforms so this doesn't happen
                }
                else
                {
                    //Debug.WriteLine(indexofFirstPlatformNotMoving);
                    movingPlatformsNoLoop[indexofFirstPlatformNotMoving].movePlatform = true;
                }


            }


            if (launchPad != null)
            {
                launchPad.launchFlag = true;
            }





        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            key.Draw(spriteBatch);
            //spriteBatch.Draw(References.player.spriteHitboxTexture, key.idleHitbox.rectangle, Color.Blue);



        }


        public void ManageAnimations()
        {
            //if (key is HangingRopeWithWeightAttached bell)
            //{
            //    if (keyPlayInteractedAnimation)
            //    {
            //        bell.weight.animatedSprite.Play("Interacted");
            //        bell.weight.currentFrame = bell.weight.frameAndTag["Interacted"].From;
            //        bell.weight.tagOfCurrentFrame = "Interacted";
            //    }
            //    else
            //    {
            //        bell.weight.animatedSprite.Play("Idle");
            //        bell.weight.currentFrame = bell.weight.frameAndTag["Idle"].From;
            //        bell.weight.tagOfCurrentFrame = "Idle";
            //    }
            //}
            //else
            //{

            if (keyPlayInteractedAnimation)
            {
                key.animatedSprite.Play("Interacted");
                key.currentFrame = key.frameAndTag["Interacted"].From;
                key.tagOfCurrentFrame = "Interacted";
            }
            else
            {
                key.animatedSprite.Play("Idle");
                key.currentFrame = key.frameAndTag["Idle"].From;
                key.tagOfCurrentFrame = "Idle";
            }



        }


    }
}
