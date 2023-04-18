using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class MovingPlatformLooping2 : MovingPlatform
    {

        // This type of MovingPlatform will alternate between exactly two positions in an infinite loop
        public MovingPlatformLooping2(Vector2 position1, Vector2 position2, string filename, int timeStationaryAtEndPoints, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player, int delay = 0, List<GameObject> spritesOnPlatform = null) : base(new List<Vector2>() { position1, position2 }, new List<int>() { 0,1}, filename, timeStationaryAtEndPoints, speed, assetManager, colliderManager, player, delay, spritesOnPlatform)
        {
        }

    }
}
