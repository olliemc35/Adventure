using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class SeriesOfMovingPlatformNoLoop : GameObject
    {
        public List<MovingPlatformNoLoop> platforms;

        public SeriesOfMovingPlatformNoLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, int number)
        {
            for (int i = 0; i < number; i++)
            {
                platforms.Add(new MovingPlatformNoLoop(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed));
            }
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            foreach (MovingPlatformNoLoop platform in platforms)
            {
                platform.LoadContent(contentManager, graphicsDevice);
            }
        }

    }
}
