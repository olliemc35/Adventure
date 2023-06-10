using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter : SeriesOfMovingPlatform_ABWrapAround
    {
        // Controlled by the Player
        // By pressing a Note the Player can EMIT a platform
        // The Player needs to wait framesBetweenEmitting between emitting each platform
        // If we run out of platforms the Player needs to wait until a platform reappears at the start (and framesBetweenEmitting has passed) - this may be the case if we only have one platform i.e. we only want to have one on the screen at a time 

        Vector2 endPointDisplacement;

        public SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform platformEmitter = null, MovingPlatform platformReceiver = null, int numberOfPlatforms = 0) : base(initialPosition, endPoint, filename, speed, stationaryTimes, framesBetweenEmitting, assetManager, colliderManager, screenManager, player, platformEmitter, platformReceiver, numberOfPlatforms)
        {
            if (platformEmitter != null)
            {
                platformEmitter.numberOfFramesHalted = 30;
            }
        }

        public SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter(Vector2 initialPosition, string movementDirection, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform platformEmitter = null, MovingPlatform platformReceiver = null, int numberOfPlatforms = 0) : base(initialPosition, movementDirection, filename, speed, stationaryTimes, framesBetweenEmitting, assetManager, colliderManager, screenManager, player, platformEmitter, platformReceiver, numberOfPlatforms)
        {
            if (platformEmitter != null)
            {
                platformEmitter.numberOfFramesHalted = 30;
            }
        }


        public override void Update(GameTime gameTime)
        {
            platformEmitter?.Update(gameTime);
            platformReceiver?.Update(gameTime);


            if (counter < framesBetweenEmitting)
            {
                counter++;
            }
            else
            {
                counter = framesBetweenEmitting;
            }

            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.Update(gameTime);

                if (platformEmitter != null)
                {
                    if (!platform.movePlatform)
                    {
                        
                        platform.positions[0] += platformEmitter.displacement;
                        platform.positions[1] += platformEmitter.displacement + endPointDisplacement;
                        endPointDisplacement = new Vector2(0,0);
                        platform.position = platform.positions[0];
                        platform.idleHitbox.rectangle.X = FindNearestInteger(platform.position.X) + platform.idleHitbox.offsetX;
                        platform.idleHitbox.rectangle.Y = FindNearestInteger(platform.position.Y) + platform.idleHitbox.offsetY;
                    }
                    else 
                    {
                        platform.positions[0] += platformEmitter.displacement;
                        endPointDisplacement += platformEmitter.displacement;
                    }
                }

               // platform.Update(gameTime);
            }

            
        }

        public void StartAPlatformMoving()
        {
            if (counter == framesBetweenEmitting)
            {          
                counter = 0;
                index = (index + 1) % numberOfPlatforms;
                platforms[index].movePlatform = true;

            }
        }


        public override void HandleNoteTrigger()
        {
            //if (platformEmitter != null)
            //{
            //    platformEmitter.halt = true;
            //}
            StartAPlatformMoving();
        }

        
    }
}
