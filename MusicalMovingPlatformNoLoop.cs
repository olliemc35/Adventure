﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace Adventure
{
    public class MusicalMovingPlatformNoLoop : MovingPlatform
    {
        public string noteValue;
        public bool hitFlag;
        public SoundEffect noteSound;
        public List<SoundEffectInstance> noteInstances = new List<SoundEffectInstance>();


        public MusicalMovingPlatformNoLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, string noteValue) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed)
        {
            this.noteValue = noteValue;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            noteSound = contentManager.Load<SoundEffect>(noteValue);
        }


        public override void Update(GameTime gameTime)
        {
            if (movePlatform)
            {
                if (delayCounter < delay)
                {
                    delayCounter += 1;
                }
                else
                {
                    base.Update(gameTime);

                    if (horizontalMovement)
                    {
                        if (spritePosition.X == endPosition.X)
                        {
                            movePlatform = false;
                            timeStationaryCounter = 0;
                            firstLoop = true;
                            hitFlag = true;
                            spritePosition.X = startPosition.X;
                            noteInstances.Add(noteSound.CreateInstance());
                            noteInstances.Last().Play();
                        }
                    }
                    else if (verticalMovement)
                    {
                        if (spritePosition.Y == endPosition.Y)
                        {
                            movePlatform = false;
                            timeStationaryCounter = 0;
                            firstLoop = true;
                            hitFlag = true;
                            spritePosition.Y = startPosition.Y;
                            noteInstances.Add(noteSound.CreateInstance());
                            noteInstances.Last().Play();
                        }
                    }
                }


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



        }


    }
}
