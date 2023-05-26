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
    public class SeriesOfMovingPlatform_ABWrapAround : GameObject
    {
        // The way in which we use the MovingPlatform_ABWrapAround class is in the following class and derived classes thereof
        // The default behaviour is to have MovingPlatform_ABWrapAround emitted every time framesBetweenEmitting has elapsed, thus giving a constant stream of platforms moving across the screen.

        public List<MovingPlatform_ABWrapAround> platforms = new List<MovingPlatform_ABWrapAround>();

        // The platforms appear from somewhere - we call this the platformEmitter. 
        // The platforms disappear somewhere - we call this the platformReceiver.
        // Both of these are MovingPlatforms, but we may just want them to be animatedSprites (i.e. stationary). In this case we can create a MovingPlatform with speed 0.
        // NOTE: the platformReceiver IS NOT THE SAME as the "platform receptor" which will be used for puzzle logic - the orb must hit the receptor in order to open a gate etc.
        // This is because tying the receptor to the platform is not a good idea - we may want a receptor which requires 4 different orbs to activate it etc.
        // These may be NULL if the platforms simply appear/disappear off-screen
        public MovingPlatform platformEmitter;
        public MovingPlatform platformReceiver;

        // The int index denotes the index of the next platform to be released
        public int index = 0;
        public int counter = 0;
        public int framesBetweenEmitting;

        public int indexOfPlatformClosestToStart = -1;
        public int spacing;
        public int numberOfPlatforms;
        public int shift = 1; // when we change direction we need to change this to -1 

        public bool detectCollisionsWithTerrain = false;


        


        public SeriesOfMovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform platformEmitter = null, MovingPlatform platformReceiver = null, int numberOfPlatforms = 0)
        {

            this.framesBetweenEmitting = framesBetweenEmitting;

            this.platformEmitter = platformEmitter;
            this.platformReceiver = platformReceiver;

            // We may want to set the numberOfPlatforms manually (in this case we would nearly always want to set it to 1 manually). Otherwise we simply set it to be so that we have "enough" platforms
            // As speed = distance / time and here time is measured in frames and speed is measured in pixels / frames 
            // we see that, emitting a platform after timeBetweenPlatforms has passed means there is a spacing of timeBetweenPlatforms * speed between each platform.
            // If N platforms are on screen then they cover a distance of N * (screenWidth) + N * spacing (if moving horizontally, say). We want this to be > Vector2.Distance(initialPosition,endPoint).
            // We can guarantee this by taking N to be the ceiling of this distance over the spacing. This leads to the formulae below.
            // (It is very likely that some platforms are redundant but this does not matter.)

            if (numberOfPlatforms == 0)
            {
                this.numberOfPlatforms = (int)Math.Ceiling(Vector2.Distance(initialPosition, endPoint) / (framesBetweenEmitting * speed));
            }
            else
            {
                this.numberOfPlatforms = numberOfPlatforms;
            }

            spacing = (int)(framesBetweenEmitting * speed);

            for (int i = 0; i < this.numberOfPlatforms; i++)
            {
                platforms.Add(new MovingPlatform_ABWrapAround(initialPosition, endPoint, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player));
            }

            // We set this so that on the first frame we get a platforming emitting.
            counter = framesBetweenEmitting;


        }

        public override void LoadContent()
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.LoadContent();
            }

            // X?.Method() does nothing if X == null and evaluates Method otherwise
            platformEmitter?.LoadContent();          
            platformReceiver?.LoadContent();

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
                counter = 0;
                index = (index + shift + numberOfPlatforms) % numberOfPlatforms;
                platforms[index].movePlatform = true;
            }

            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                if (platformEmitter != null)
                {
                    if (!platform.movePlatform)
                    {
                        for (int i = 0; i < platform.positions.Count; i++)
                        {
                            platform.positions[i] += platformEmitter.displacement;
                            platform.position = platform.positions[0];
                            platform.idleHitbox.rectangle.X = FindNearestInteger(platform.position.X) + platform.idleHitbox.offsetX;
                            platform.idleHitbox.rectangle.Y = FindNearestInteger(platform.position.Y) + platform.idleHitbox.offsetY;
                        }
                    }
                }

                platform.Update(gameTime);

            }

            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                if (platform.movePlatform)
                {
                    platform.Draw(spriteBatch);

                }
            }

            platformEmitter?.Draw(spriteBatch);
            platformReceiver?.Draw(spriteBatch);


        }








        // We call this function so that the screen is already populated with platforms when we first enter it
        public void SetStartingPositions(Vector2 initialPosition, Vector2 endPoint)
        {
            Vector2 direction = endPoint - initialPosition;
            direction.Normalize();

            if (initialPosition.Length() < endPoint.Length())
            {
                for (int i = 0; i < numberOfPlatforms; i++)
                {
                    if ((initialPosition + direction * spacing * i).Length() < endPoint.Length())
                    {
                        platforms[i].movePlatform = true;
                        indexOfPlatformClosestToStart += 1;

                    }
                }
            }
            else
            {
                for (int i = 0; i < numberOfPlatforms; i++)
                {
                    if ((initialPosition + direction * spacing * i).Length() > endPoint.Length())
                    {
                        platforms[i].movePlatform = true;
                        indexOfPlatformClosestToStart += 1;

                    }
                }
            }

            for (int i = 0; i <= indexOfPlatformClosestToStart; i++)
            {
                platforms[i].position = initialPosition + spacing * (indexOfPlatformClosestToStart - i) * direction;
            }
        }


        // Code for ActionScreenBuilder
        public override void AdjustHorizontally(ref List<int> ints)
        {
            
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.positions[0] = new Vector2(platform.positions[0].X + ints[0], platform.positions[0].Y);
                platform.positions[1] = new Vector2(platform.positions[1].X + ints[1], platform.positions[1].Y);
                platform.position.X += ints[0];
            }

            int counter = 0;

            for (int i = 0; i < platformEmitter.positions.Count; i++)
            {
                platformEmitter.positions[i] = new Vector2(platformEmitter.positions[i].X + ints[2 + i], platformEmitter.positions[i].Y);
                counter++;
            }

            for (int i = 0; i < platformReceiver.positions.Count; i++)
            {
                platformReceiver.positions[i] = new Vector2(platformReceiver.positions[i].X + ints[2 + counter + i], platformReceiver.positions[i].Y);
                counter++;
            }


            ints.RemoveRange(0, 2 + counter);

        }
        public override void AdjustVertically(ref List<int> ints)
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.positions[0] = new Vector2(platform.positions[0].X, platform.positions[0].Y + ints[0]);
                platform.positions[1] = new Vector2(platform.positions[1].X, platform.positions[1].Y + ints[1]);
                platform.position.Y += ints[0];
            }

            int counter = 0;

            //for (int i = 0; i < platformEmitter.positions.Count; i++)
            //{
            //    platformEmitter.positions[i] = new Vector2(platformEmitter.positions[i].X, platformEmitter.positions[i].Y + ints[2 + i]);
            //    counter++;
            //}

            //for (int i = 0; i < platformReceiver.positions.Count; i++)
            //{
            //    platformReceiver.positions[i] = new Vector2(platformReceiver.positions[i].X, platformReceiver.positions[i].Y + ints[2 + counter + i]);
            //    counter++;
            //}


            ints.RemoveRange(0, 2 + counter);


        }


        public override void SetBoosters()
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.SetBoosters();
            }
        }



    }
}
