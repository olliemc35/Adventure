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
        public List<MovingPlatform_ABWrapAround> platforms = new List<MovingPlatform_ABWrapAround>();
        public int indexOfPlatformClosestToStart = -1;
        public int spacing;
        public int numberOfPlatforms;
        public int indexOfStartPosition = 0;
        public int shift = 1; // when we change direction we need to change this to -1 to make the modular arithmetic order work


        // This GameObject consists of a series of moving platforms which will move across the screen and wrap around when they reach the end
        // The player is able to control the DIRECTION of the moving platforms by playing a corresponding Note

        public SeriesOfMovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int numberOfPlatforms, int spacing, AssetManager assetManager, ColliderManager colliderManager, Player player)
        {
            this.spacing = spacing;
            this.numberOfPlatforms = numberOfPlatforms;

            for (int i = 0; i < numberOfPlatforms; i++)
            {
                platforms.Add(new MovingPlatform_ABWrapAround(initialPosition, endPoint, filename, speed, stationaryTimes, assetManager, colliderManager, player));
            }

            SetStartingPositions(initialPosition, endPoint);          

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

            if (Vector2.Distance(platforms[indexOfPlatformClosestToStart].position, platforms[0].positions[platforms[0].currentIndex]) >= 8 * spacing)
            {
                if (!platforms[(platforms.Count+indexOfPlatformClosestToStart + shift) % platforms.Count].movePlatform)
                {
                    platforms[(platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count].movePlatform = true;

                    indexOfPlatformClosestToStart = (platforms.Count + indexOfPlatformClosestToStart + shift) % platforms.Count;
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

        public void ReverseDirection()
        {
            int newIndexOfPlatformClosestToStart = 0;

            for (int i = 0; i < numberOfPlatforms; i++)
            {
                // As platforms are handled so that they are always precisely a distance 8 * spacing apart and they wrap-around at the end-point
                // there will always be a platform satisfying the inequality below

                if (Vector2.Distance(platforms[i].position, platforms[0].positions[platforms[0].indexToMoveTo]) <= 8 * spacing)
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


        // We call this function so that the screen is already populated with platforms when we first enter it
        public void SetStartingPositions(Vector2 initialPosition, Vector2 endPoint)
        {

            Vector2 direction = endPoint - initialPosition;
            direction.Normalize();

            if (initialPosition.Length() < endPoint.Length())
            {
                for (int i = 0; i < numberOfPlatforms; i++)
                {
                    if ((initialPosition + direction * 8 * spacing * i).Length() < endPoint.Length())
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
                    if ((initialPosition + direction * 8 * spacing * i).Length() > endPoint.Length())
                    {
                        platforms[i].movePlatform = true;
                        indexOfPlatformClosestToStart += 1;

                    }
                }
            }

            for (int i = 0; i <= indexOfPlatformClosestToStart; i++)
            {
                platforms[i].position = initialPosition + 8 * spacing * (indexOfPlatformClosestToStart - i) * direction;
            }
        }


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

        public override void HandleNoteTrigger()
        {
            ReverseDirection();
        }

    }
}
