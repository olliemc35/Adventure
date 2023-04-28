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
    public class SeriesOfMovingPlatform_ABWrapAround2 : GameObject
    {
        public List<MovingPlatform_ABWrapAround> platforms = new List<MovingPlatform_ABWrapAround>();
        public int indexOfPlatformClosestToStart = -1;
        public int spacing;
        public int numberOfPlatforms;
        public int indexOfStartPosition = 0;


        // This GameObject consists of a series of moving platforms which will move across the screen and wrap around when they reach the end
        // The player is able to control EMITTING one of the platforms
        public SeriesOfMovingPlatform_ABWrapAround2(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int delay, int numberOfPlatforms, int spacing, AssetManager assetManager, ColliderManager colliderManager, Player player)
        {
            this.spacing = spacing;
            this.numberOfPlatforms = numberOfPlatforms;


            for (int i = 0; i < numberOfPlatforms; i++)
            {
                platforms.Add(new MovingPlatform_ABWrapAround(initialPosition, endPoint, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player));
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

        public void StartAPlatformMoving()
        {
            int indexofFirstPlatformNotMoving = -1;

            for (int i = 0; i < platforms.Count; i++)
            {

                if (platforms[i].position == platforms[i].positions[0])
                {
                    indexofFirstPlatformNotMoving = i;
                    break;
                }

            }

            if (indexofFirstPlatformNotMoving == -1)
            {
                // do nothing - I need to supply more platforms so this doesn't happen
                Debug.WriteLine("SUPPLY MORE PLATFORMS");
            }
            else
            {
                platforms[indexofFirstPlatformNotMoving].movePlatform = true;
            }
        }


        public override void HandleNoteTrigger()
        {
            StartAPlatformMoving();
        }

    }
}
