using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatform_ABWrapAround : MovingPlatform
    {
        // This may or may not be controlled by the player
        // Platform will move from A to B and then reappear at A
        // We use this type of platform to create an infinite series of platforms which go from one side of the screen to another

        // We add some extra functionality: when the platform reaches the endpoint it can play a sound
        // E.g. we can use this if our platform is an Orb and it hits a Tuning Fork
        public string noteValue;
        public bool hitFlag;
        public SoundEffect noteSound;
        public List<SoundEffectInstance> noteInstances = new List<SoundEffectInstance>();

        

        public MovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, string noteValue = null, SoundManager soundManager = null) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
            movePlatform = false;
            this.soundManager  = soundManager;
            this.noteValue = noteValue; 
        }

        // We adjust Update method with logic to handle the sound effects
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



        // Adjust UpdateAtStationaryPoints so that we reappear at the location we started with.
        // Note that the player may reverse the direction so that instead of going from A to B all the time we may go from B to A (and so we can't just test for position == positions[1]).
        public override void UpdateAtStationaryPoints()
        {
            //Debug.WriteLine(position.Y);
            //Debug.WriteLine(position.X);
            //Debug.WriteLine(positions[indexToMoveTo]);

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
                    idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
                    idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
                    movePlatform = false;

                    if (soundManager != null)
                    {
                        hitFlag = true;
                        noteInstances.Add(noteSound.CreateInstance());
                        noteInstances.Last().Play();
                    }
                }


            }
        }

        public override void HandleNoteTrigger()
        {
            if (!movePlatform)
            {
                movePlatform = true;
            }
        }



        public override void AdjustHorizontally(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X + ints[0], positions[0].Y);
            positions[1] = new Vector2(positions[1].X + ints[1], positions[1].Y);
            position.X += ints[0];


            ints.RemoveRange(0, 2);

        }
        public override void AdjustVertically(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X, positions[0].Y + ints[0]);
            positions[1] = new Vector2(positions[1].X, positions[1].Y + ints[1]);
            position.Y += ints[0];


            ints.RemoveRange(0, 2);

        }

        public override void HandleCollision()
        {
            position = positions[currentIndex];
            idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;

            movePlatform = false;
            flagCollision = true;
        }


    }
}
