using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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

        public bool halt = false;
        public int numberOfFramesHalted = 60;
        public int haltCounter = 0;

        public MovingPlatform_AB(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationary, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, new List<int>() { 0, 1 }, filename, timeStationary, speed, 0, assetManager, colliderManager, player)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (halt)
            {
                if (haltCounter == numberOfFramesHalted)
                {
                    haltCounter = 0;
                    halt = false;
                }
                else
                {
                    haltCounter += 1;
                    return;
                }
            }

            if (movePlatform)
            {
                base.Update(gameTime);
                StopAtStationaryPoints();

            }
            else
            {
                if (Climable)
                {
                    UpdateClimable();
                }
            }

        }


        public void StopAtStationaryPoints()
        {
            if (position == positions[indexToMoveTo])
            {
                movePlatform = false;
            }

        }


        public override void HandleNoteTrigger()
        {
            if (movePlatform)
            {
                halt = true;
                ReverseDirection();
            }

            movePlatform = true;
        }

    }
}
