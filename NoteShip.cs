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


        public NoteShip(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public NoteShip(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, int displacement) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed)
        {
            horizontalMovement = true;
            this.displacement = displacement;
            originalPosition = initialPosition;

        }

        public override void Update(GameTime gameTime)
        {
            if (!movePlatform && References.player.spritePosition.X < originalPosition.X + 2 * idleHitbox.rectangle.Width)
            {
                if (spriteCollider.CheckForCollision(this.idleHitbox, References.player.idleHitbox))
                {
                    movePlatform = true;
                }
            }
            else if (!movePlatform && spritePosition.Y != originalPosition.Y)
            {
                base.Update(gameTime);

                spriteVelocity.Y = Math.Sign(originalPosition.Y - spritePosition.Y) * 60 * speed;
                spriteDisplacement.Y = spriteVelocity.Y * deltaTime;

                if (spritePosition.Y == originalPosition.Y)
                {
                    spriteVelocity.Y = 0;
                    spriteDisplacement.Y = 0;
                }
            }



            if (movePlatform)
            {
                base.Update(gameTime);

                //spritePosition.Y += spriteDisplacement.Y;
                //spritePosition.Y = DistanceToNearestInteger(spritePosition.Y);

                if (moveVertically)
                {
                    spriteVelocity.Y = Math.Sign(originalPosition.Y + displacementScaling * displacement - spritePosition.Y) * 60 * speed * 2;
                    spriteDisplacement.Y = spriteVelocity.Y * deltaTime;

                    if (spritePosition.Y == originalPosition.Y + displacementScaling * displacement)
                    {
                        spriteVelocity.Y = 0;
                        spriteDisplacement.Y = 0;
                        moveVertically = false;
                    }


                }


                if (spritePosition.X == endPosition.X)
                {
                    spriteVelocity.X = 0;
                    spriteDisplacement.X = 0;
                    movePlatform = false;
                    horizontalMovement = false;
                }


            }



        }


    }
}
