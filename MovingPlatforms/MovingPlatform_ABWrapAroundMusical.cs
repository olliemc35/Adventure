using System;
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

        public MovingPlatform_ABWrapAroundMusical(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, string noteValue, SoundManager soundManager, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>(){ initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
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
            base.Update(gameTime);

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

        public override void UpdateAtStationaryPoints()
        {
            if (direction == Direction.stationary)
            {
                if (timeStationaryCounter < stationaryTimes[currentIndex])
                {
                    timeStationaryCounter += 1;
                }
                else
                {
                    timeStationaryCounter = 0;
                    UpdateDirection();
                }
            }
            else
            {
                if (position == positions[indexToMoveTo])
                {
                    position = positions[currentIndex];
                    hitFlag = true;
                    noteInstances.Add(noteSound.CreateInstance());
                    noteInstances.Last().Play();
                    movePlatform = false;
                }


            }
        }

        //public void ReturnToBeginningAtStationaryPointsAndPlaySound()
        //{
        //    if (position == positions[1])
        //    {
        //        if (playerControlled)
        //        {
        //            movePlatform = false;
        //        }
        //        currentIndex = 0;
        //        position = positions[0];
        //        hitFlag = true;
        //        noteInstances.Add(noteSound.CreateInstance());
        //        noteInstances.Last().Play();
        //    }
        //}


    }
}
