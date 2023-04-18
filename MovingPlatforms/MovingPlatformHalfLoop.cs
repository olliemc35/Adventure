using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Adventure
{
    public class MovingPlatformHalfLoop : MovingPlatform
    {

        public bool TravellingFromStartPoint = true;

        // This type of MovingPlatform will be controlled by the player.
        // The player will press a corresponding Note and the platform will move from position 1 to position 2
        // When the player presses the Note again, and the platform is at position 2, it will move back to position 1 etc.
        // As always the player trigger is the movePlatform bool

        public MovingPlatformHalfLoop(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player, List<GameObject> attachedGameObjects = null) : base(new List<Vector2>() { initialPosition, endPoint }, new List<int>() { 0, 1 }, filename, 0, speed, assetManager, colliderManager, player, 0, attachedGameObjects)
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
            if (globalDirection == GlobalDirection.horizontal)
            {
                if (position.X == positions[indexes[currentIndex]].X || position.X == positions[indexes[(currentIndex + 1) % indexes.Count]].X)
                {
                    movePlatform = false;
                }
            }
            else
            {
                if (position.Y == positions[indexes[currentIndex]].Y || position.Y == positions[indexes[(currentIndex + 1) % indexes.Count]].Y)
                {
                    movePlatform = false;
                }
            }
        }

    }
}
