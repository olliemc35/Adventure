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
    public class MovingPlatform_ABLoop : MovingPlatform
    {

        // This type of MovingPlatform will alternate between exactly two positions in an infinite loop
        public MovingPlatform_ABLoop(Vector2 position1, Vector2 position2, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { position1, position2 }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
        }

    }
}
