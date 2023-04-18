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
    public class MovingPlatform_ABWrapAroundMusical : MovingPlatform
    {
        public string noteValue;
        public bool hitFlag;
        public SoundEffect noteSound;
        public List<SoundEffectInstance> noteInstances = new List<SoundEffectInstance>();

        // This type of MovingPlatform is very similar to MovingPlatformNoLoop except there is extra code to play a sound when the platform reaches the endpoint (and loops back)
        // E.g. used if we have an orb hitting a tuning fork 

        public MovingPlatform_ABWrapAroundMusical(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int delay, string noteValue, SoundManager soundManager, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>(){ initialPosition, endPoint }, new List<int>(){ 0, 1 }, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player)
        {
            this.soundManager = soundManager;
            this.noteValue = noteValue;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            noteSound = soundManager.soundEffects[noteValue];
        }


        public override void Update(GameTime gameTime)
        {
            if (movePlatform)
            {
                base.Update(gameTime);
                ReturnToBeginningAtStationaryPointsAndPlaySound();

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


        public void ReturnToBeginningAtStationaryPointsAndPlaySound()
        {
            if (position == positions[1])
            {
                if (playerControlled)
                {
                    movePlatform = false;
                }
                currentIndex = 0;
                firstLoop = true;
                position = positions[0];
                hitFlag = true;
                noteInstances.Add(noteSound.CreateInstance());
                noteInstances.Last().Play();
            }
        }


    }
}