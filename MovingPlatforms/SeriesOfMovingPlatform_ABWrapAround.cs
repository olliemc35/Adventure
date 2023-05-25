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

        // These may be NULL if the platforms simply appear/disappear off-screen
        public Emitter platformEmitter;
        public Emitter platformReceiver;

        // The int index denotes the index of the next platform to be released
        public int index = 0;
        public int counter = 0;
        public int framesBetweenEmitting;

        public int indexOfPlatformClosestToStart = -1;
        public int spacing;
        public int numberOfPlatforms;
        public int shift = 1; // when we change direction we need to change this to -1 

        public bool detectCollisionsWithTerrain = false;


       


        


        public SeriesOfMovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, Emitter platformEmitter = null, Emitter platformReceiver = null, int numberOfPlatforms = 0)
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
                platform.Update(gameTime);

            }

            platformEmitter?.Update(gameTime);
            platformReceiver?.Update(gameTime);
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

            ints.RemoveRange(0, 2);

        }
        public override void AdjustVertically(ref List<int> ints)
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.positions[0] = new Vector2(platform.positions[0].X, platform.positions[0].Y + ints[0]);
                platform.positions[1] = new Vector2(platform.positions[1].X, platform.positions[1].Y + ints[1]);
                platform.position.Y += ints[0];
            }

            ints.RemoveRange(0, 2);

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
