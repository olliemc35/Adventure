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
        public MovingPlatform_ABLoop(Vector2 position1, Vector2 position2, string filename, int timeStationaryAtEndPoints, float speed, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { position1, position2 }, new List<int>() { 0,1}, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player)
        {
        }

    }
}
