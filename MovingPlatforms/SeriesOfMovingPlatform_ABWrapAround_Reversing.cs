using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class SeriesOfMovingPlatform_ABWrapAround_Reversing : SeriesOfMovingPlatform_ABWrapAround
    {
        // Controlled by player
        // This GameObject consists of a series of moving platforms which will move across the screen and wrap around when they reach the end
        // The player is able to REVERSE the direction of the moving platforms by playing a corresponding Note


        public SeriesOfMovingPlatform_ABWrapAround_Reversing(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform platformEmitter = null, MovingPlatform platformReceiver = null, int numberOfPlatforms = 0) : base(initialPosition, endPoint, filename, speed, stationaryTimes, framesBetweenEmitting, assetManager, colliderManager, screenManager, player, platformEmitter, platformReceiver, numberOfPlatforms)
        {
            SetStartingPositions(initialPosition, endPoint);
        }

        public override void Update(GameTime gametime)
        {
            //Debug.WriteLine(indexOfPlatformClosestToStart);


            //if (Vector2.Distance(platforms[indexOfPlatformClosestToStart].position, platforms[0].positions[platforms[0].currentIndex]) >= 16 * spacing)
            //{
            //    if (!platforms[(platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count].movePlatform)
            //    {
            //        platforms[(platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count].movePlatform = true;

            //        indexOfPlatformClosestToStart = (platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count;
            //    }

            //}


            //foreach (MovingPlatform_ABWrapAround platform in platforms)
            //{
            //    platform.Update(gametime);

            //    if (platform.flagCollision)
            //    {
            //        platform.flagCollision = false;
            //        indexOfPlatformClosestToStart = (platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count;
            //    }
            //}

        }

       
        public void ReverseDirection()
        {
            int newIndexOfPlatformClosestToStart = 0;

            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                if (platform.position == platform.positions[platform.currentIndex])
                {
                    platform.position = platform.positions[platform.indexToMoveTo];
                }
            }

            for (int i = 0; i < numberOfPlatforms; i++)
            {
                // As platforms are handled so that they are always precisely a distance 16 * spacing apart and they wrap-around at the end-point
                // there will always be a platform satisfying the inequality below

                if (Vector2.Distance(platforms[i].position, platforms[0].positions[platforms[0].indexToMoveTo]) <= spacing)
                {
                    newIndexOfPlatformClosestToStart = i;
                    break;
                }

            }

            indexOfPlatformClosestToStart = newIndexOfPlatformClosestToStart;

            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.ReverseDirection();
            }

            // When we reverse the platforms we want to move through the list platforms in the opposite direction
            // E.g. if we are going UP through the list we want to go DOWN through the list. (Drawing a picture makes this make sense.)
            shift *= -1;
        }     

        public override void HandleNoteTrigger()
        {
            ReverseDirection();
        }




    }
}
