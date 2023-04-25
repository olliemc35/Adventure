using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class NoteShip : MovingPlatform
    {
        public int displacement;
        public int displacementScaling = 0;
        public bool moveVertically;
        public Vector2 originalPosition;

        // THIS WILL NEED FIXING

        public NoteShip(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int displacement, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, new List<int>() { 0, 1 }, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player)
        {
            this.displacement = displacement;
            originalPosition = initialPosition;

        }

        public override void Update(GameTime gameTime)
        {
            if (!movePlatform && player.position.X < originalPosition.X + 2 * idleHitbox.rectangle.Width)
            {
                if (colliderManager.CheckForCollision(idleHitbox, player.idleHitbox))
                {
                    movePlatform = true;
                }
            }
            else if (!movePlatform && position.Y != originalPosition.Y)
            {
                base.Update(gameTime);

                velocity.Y = Math.Sign(originalPosition.Y - position.Y) * 60 * speed;
                base.displacement.Y = velocity.Y * deltaTime;

                if (position.Y == originalPosition.Y)
                {
                    velocity.Y = 0;
                    base.displacement.Y = 0;
                }
            }



            if (movePlatform)
            {
                base.Update(gameTime);


                if (moveVertically)
                {
                    velocity.Y = Math.Sign(originalPosition.Y + displacementScaling * displacement - position.Y) * 60 * speed * 2;
                    base.displacement.Y = velocity.Y * deltaTime;

                    if (position.Y == originalPosition.Y + displacementScaling * displacement)
                    {
                        velocity.Y = 0;
                        base.displacement.Y = 0;
                        moveVertically = false;
                    }


                }


                if (position.X == positions[1].X)
                {
                    velocity.X = 0;
                    base.displacement.X = 0;
                    movePlatform = false;
                }


            }



        }


        public override void HandleNoteTrigger()
        {
            //displacementScaling = displacementScalingForNoteShip;
            moveVertically = true;
        }


    }
}
