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
        // This is not controlled by the player
        // Platform will alternate between A and B in an infinite loop
        public MovingPlatform_ABLoop(Vector2 position1, Vector2 position2, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, bool receptorBehaviour = false) : base(new List<Vector2>() { position1, position2 }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player, receptorBehaviour)
        {
        }

    }
}
