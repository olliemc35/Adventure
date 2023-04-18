using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Adventure
{
    public class MovingPlatform_AB : MovingPlatform
    {
        // This type of MovingPlatform will be controlled by the player.
        // The player will press a corresponding Note and the platform will move from position A to position B
        // When the player presses the Note again AND the platform is at position B, it will move back to position A etc.
        // As always the player trigger is the movePlatform bool

        public MovingPlatform_AB(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, new List<int>() { 0, 1 }, filename, 0, speed, 0, assetManager, colliderManager, player)
        {

        }

        public override void Update(GameTime gameTime)
        {

            if (movePlatform)
            {
                base.Update(gameTime);
                StopAtStationaryPoints();
            }

        }


        public void StopAtStationaryPoints()
        {
            if (position == positions[0] || position == positions[1])
            {
                movePlatform = false;
            }

        }

    }
}
