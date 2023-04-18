using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class SeriesOfMovingPlatformNoLoop : GameObject
    {
        public List<MovingPlatformNoLoop> platforms = new List<MovingPlatformNoLoop>();
        public int indexOfPlatformClosestToStart = 0;
        public int horizontalSpacing;
        public int numberOfPlatforms;
        public SeriesOfMovingPlatformNoLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, int number, int horizontalSpacing, AssetManager assetManager, ColliderManager colliderManager, Player player, float delay = 0, List<GameObject> attachedGameObjects = null)
        {
            this.horizontalSpacing = horizontalSpacing;
            numberOfPlatforms = number;

            for (int i = 0; i < number; i++)
            {
                platforms.Add(new MovingPlatformNoLoop(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed, assetManager, colliderManager, player, delay, attachedGameObjects));
            }
            platforms[0].movePlatform = true;
        }

        public override void LoadContent()
        {
            foreach (MovingPlatformNoLoop platform in platforms)
            {
                platform.LoadContent();
            }
        }

        public override void Update(GameTime gametime)
        {
            //Debug.WriteLine(indexOfPlatformClosestToStart);

            if (Vector2.Distance(platforms[indexOfPlatformClosestToStart].position, platforms[indexOfPlatformClosestToStart].startPosition) >= 8 * horizontalSpacing)
            {
                if (!platforms[(indexOfPlatformClosestToStart + 1) % platforms.Count].movePlatform)
                {
                    platforms[(indexOfPlatformClosestToStart + 1) % platforms.Count].movePlatform = true;
                    indexOfPlatformClosestToStart = (indexOfPlatformClosestToStart + 1) % platforms.Count;
                }
            }


            foreach (MovingPlatformNoLoop platform in platforms)
            {
                platform.Update(gametime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatformNoLoop platform in platforms)
            {
                if (platform.movePlatform)
                {
                    platform.Draw(spriteBatch);

                }
            }
        }

    }
}
