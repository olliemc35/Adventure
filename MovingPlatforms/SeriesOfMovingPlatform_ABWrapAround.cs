using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Adventure.MovingPlatform;

namespace Adventure
{
    public class SeriesOfMovingPlatform_ABWrapAround : GameObject
    {
        public List<MovingPlatform_ABWrapAround> platforms = new List<MovingPlatform_ABWrapAround>();
        public int indexOfPlatformClosestToStart = -1;
        public int horizontalSpacing;
        public int numberOfPlatforms;

        public enum DirectionModes
        {
            leftToRight,
            rightToLeft
        };
        public DirectionModes directionMode;


        // This GameObject consists of a series of moving platforms which will move across the screen and loop back every time
        // We control this behaviour using the movePlatform bool of each MovingPlatformNoLoop

        public SeriesOfMovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int delay, int number, int horizontalSpacing, AssetManager assetManager, ColliderManager colliderManager, Player player)
        {
            this.horizontalSpacing = horizontalSpacing;
            numberOfPlatforms = number;

            directionMode = DirectionModes.leftToRight;

            for (int i = 0; i < number; i++)
            {
                platforms.Add(new MovingPlatform_ABWrapAround(initialPosition,  endPoint, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player));
            }

            for (int i = 0; i < number; i++)
            {
                // we're assuming here initialPosition.X < endPoint.X
                if (initialPosition.X + 8 * horizontalSpacing * i < endPoint.X)
                {
                    platforms[i].movePlatform = true;
                    indexOfPlatformClosestToStart += 1;

                }
            }

            for (int i = 0; i <= indexOfPlatformClosestToStart; i++)
            {
                platforms[i].position.X = initialPosition.X + 8 * horizontalSpacing * (indexOfPlatformClosestToStart - i);
            }



        }

        public override void LoadContent()
        {
            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.LoadContent();
            }
        }

        public override void Update(GameTime gametime)
        {


            if (Vector2.Distance(platforms[indexOfPlatformClosestToStart].position, platforms[indexOfPlatformClosestToStart].positions[platforms[indexOfPlatformClosestToStart].indexes[0]]) >= 8 * horizontalSpacing)
            {
                if (directionMode == DirectionModes.leftToRight)
                {
                    if (!platforms[(indexOfPlatformClosestToStart + 1) % platforms.Count].movePlatform)
                    {
                        platforms[(indexOfPlatformClosestToStart + 1) % platforms.Count].movePlatform = true;

                        indexOfPlatformClosestToStart = (indexOfPlatformClosestToStart + 1) % platforms.Count;
                    }
                }
                else
                {
                    if (!platforms[(platforms.Count + indexOfPlatformClosestToStart - 1) % platforms.Count].movePlatform)
                    {
                        platforms[(platforms.Count + indexOfPlatformClosestToStart - 1) % platforms.Count].movePlatform = true;
                        indexOfPlatformClosestToStart = (platforms.Count + indexOfPlatformClosestToStart - 1) % platforms.Count;
                    }
                }
            }


            foreach (MovingPlatform_ABWrapAround platform in platforms)
            {
                platform.Update(gametime);
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
        }

    }
}
