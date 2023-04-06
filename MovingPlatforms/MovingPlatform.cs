﻿using Microsoft.Xna.Framework.Content;
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
    public class MovingPlatform : MovingGameObject
    {

        public AnimatedSprite animation_Moving;

        public Vector2 startPosition;
        public Vector2 endPosition;
        public Vector2 positionOffset = new Vector2(0, 0);
        public bool verticalMovement = false;
        public bool horizontalMovement = false;
        public bool movingRight = false;
        public bool movingLeft = false;
        public bool movingUp = false;
        public bool movingDown = false;
        public bool firstLoop = true;

        public int timeStationaryAtEndPoints;
        public float timeStationaryCounter = 0;


        public float delay = 0;
        public float delayCounter = 0;
        public bool beforeDelay;

        public float speed;

        // This is a trigger for the derived classes to use
        public bool movePlatform = false;


        public bool movePlayerToo = false;

        public List<GameObject> spritesOnPlatform;



        public MovingPlatform() : base()
        {
            CollisionObject = true;

        }

        public MovingPlatform(Vector2 initialPosition) : base(initialPosition)
        {
            CollisionObject = true;
            beforeDelay = false;

        }

        public MovingPlatform(Vector2 startPosition, string filename, Vector2 endPosition, int timeStationaryAtEndPoints, float speed) : base(startPosition, filename)
        {
            CollisionObject = true;
            beforeDelay = false;

            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.timeStationaryAtEndPoints = timeStationaryAtEndPoints;
            this.speed = speed;

            if (startPosition.X == endPosition.X)
            {
                verticalMovement = true;

                if (startPosition.Y > endPosition.Y)
                {
                    movingUp = true;
                }
                else
                {
                    movingDown = true;
                }
            }
            else
            {
                horizontalMovement = true;

                if (startPosition.X < endPosition.X)
                {
                    movingRight = true;
                }
                else
                {
                    movingLeft = true;
                }

            }


            deltaTime = 1f / 60;

        }

        public MovingPlatform(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, List<GameObject> spritesOnPlatform) : base(initialPosition, filename)
        {
            CollisionObject = true;
            beforeDelay = false;

            this.spritesOnPlatform = spritesOnPlatform;

            startPosition = initialPosition;
            this.endPosition = endPoint;
            this.timeStationaryAtEndPoints = timeStationaryAtEndPoints;
            this.speed = speed;

            if (startPosition.X == endPosition.X)
            {
                verticalMovement = true;

                if (startPosition.Y > endPosition.Y)
                {
                    movingUp = true;
                }
                else
                {
                    movingDown = true;
                }
            }
            else
            {
                horizontalMovement = true;

                if (startPosition.X < endPosition.X)
                {
                    movingRight = true;
                }
                else
                {
                    movingLeft = true;
                }

            }


            deltaTime = 1f / 60;

        }

        public MovingPlatform(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, float delay) : base(initialPosition, filename)
        {
            startPosition = initialPosition;
            beforeDelay = true;

            this.endPosition = endPoint;
            this.timeStationaryAtEndPoints = timeStationaryAtEndPoints;
            this.speed = speed;

            if (startPosition.X == endPosition.X)
            {
                verticalMovement = true;

                if (startPosition.Y > endPosition.Y)
                {
                    movingUp = true;
                }
                else
                {
                    movingDown = true;
                }
            }
            else
            {
                horizontalMovement = true;

                if (startPosition.X < endPosition.X)
                {
                    movingRight = true;
                }
                else
                {
                    movingLeft = true;
                }

            }

            this.delay = delay;

            deltaTime = 1f / 60;

        }



        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            animation_Moving = spriteSheet.CreateAnimatedSprite("Moving");
            CollisionObject = true;
            idleHitbox.isActive = true;


        }

        public override void Update(GameTime gameTime)
        {

            if (beforeDelay)
            {
                if (delayCounter > delay)
                {
                    beforeDelay = false;
                }
                else
                {
                    delayCounter += 1;
                }
            }
            else
            {
                if (horizontalMovement)
                {
                    FindHorizontalVelocityAndDisplacement();
                }
                else
                {
                    FindVerticalVelocityAndDisplacement();
                }
            }



            position.X += displacement.X;
            position.X = FindNearestInteger(position.X);
            position.Y += displacement.Y;
            position.Y = FindNearestInteger(position.Y);

            // This needs to be done BEFORE idleHitbox is updated
            if (colliderManager.CheckForEdgesMeeting(this.idleHitbox, References.player.idleHitbox))
            {
                movePlayerToo = true;
            }
            else
            {
                movePlayerToo = false;
            }

            idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;

            if (movePlayerToo)
            {
                References.player.position.X += displacement.X;
                References.player.position.Y += displacement.Y;
                References.player.velocityOffSetDueToMovingPlatform.X = velocity.X;
                References.player.velocityOffSetDueToMovingPlatform.Y = velocity.Y;
            }
            if (spritesOnPlatform != null)
            {

                foreach (GameObject gameObject in spritesOnPlatform)
                {
                    if (gameObject is Note note)
                    {
                        note.key.position.X += displacement.X;
                        note.key.idleHitbox.rectangle.X = (int)note.key.position.X + note.key.idleHitbox.offsetX;
                        note.key.position.Y += displacement.Y;
                        note.key.idleHitbox.rectangle.Y = (int)note.key.position.Y + note.key.idleHitbox.offsetY;
                    }
                    else if (gameObject is AnimatedGameObject sprite)
                    {
                        sprite.position.X += displacement.X;
                        sprite.idleHitbox.rectangle.X = (int)sprite.position.X + sprite.idleHitbox.offsetX;
                        sprite.position.Y += displacement.Y;
                        sprite.idleHitbox.rectangle.Y = (int)sprite.position.Y + sprite.idleHitbox.offsetY;
                    }
                }
            }


            base.Update(gameTime);
            ManageAnimations();






        }


        public override void Draw(SpriteBatch spriteBatch)
        {



            //if (!drawHitboxes)
            //{
            //foreach (HitboxRectangle hitbox in spriteHitboxes)
            //{
            //    if (hitbox.isActive)
            //    {
            //        spriteBatch.Draw(spriteHitboxTexture, hitbox.rectangle, Color.Red);
            //    }
            //}

            //}
            base.Draw(spriteBatch);
            //animatedSprite_Idle.Render(spriteBatch);


        }


        public void FindHorizontalVelocityAndDisplacement()
        {
            if (position.X != endPosition.X && position.X != startPosition.X)
            {
                if (movingRight)
                {
                    velocity.X = 60 * speed;
                    displacement.X = velocity.X * deltaTime;
                }
                else if (movingLeft)
                {
                    velocity.X = -60 * speed;
                    displacement.X = velocity.X * deltaTime;
                }

                return;
            }

            if (timeStationaryAtEndPoints > 0)
            {

                if ((position.X == endPosition.X || position.X == startPosition.X) && timeStationaryCounter < timeStationaryAtEndPoints)
                {

                    velocity.X = 0;
                    displacement.X = 0;
                    timeStationaryCounter += 1;
                    return;
                }
            }

            if ((position.X == endPosition.X || position.X == startPosition.X) && timeStationaryCounter == timeStationaryAtEndPoints)
            {
                timeStationaryCounter = 0;

                if (firstLoop)
                {
                    if (movingRight)
                    {
                        velocity.X = 60 * speed;
                        displacement.X = velocity.X * deltaTime;
                    }
                    else if (movingLeft)
                    {
                        velocity.X = -60 * speed;
                        displacement.X = velocity.X * deltaTime;
                    }

                    firstLoop = false;

                }
                else
                {
                    if (movingRight)
                    {
                        movingRight = false;
                        movingLeft = true;
                        velocity.X = -60 * speed;
                        displacement.X = velocity.X * deltaTime;
                    }
                    else if (movingLeft)
                    {
                        movingLeft = false;
                        movingRight = true;
                        velocity.X = 60 * speed;
                        displacement.X = velocity.X * deltaTime;
                    }
                }

                return;
            }
        }

        public void FindVerticalVelocityAndDisplacement()
        {
            if (position.Y != endPosition.Y && position.Y != startPosition.Y)
            {
                if (movingDown)
                {
                    positionOffset.Y = 1;
                    velocity.Y = 60 * speed;
                    displacement.Y = velocity.Y * deltaTime;
                }
                else if (movingUp)
                {
                    positionOffset.Y = -1;
                    velocity.Y = -60 * speed;
                    displacement.Y = velocity.Y * deltaTime;

                }


                return;
            }

            if (timeStationaryAtEndPoints > 0)
            {
                if ((position.Y == endPosition.Y || position.Y == startPosition.Y) && timeStationaryCounter < timeStationaryAtEndPoints)
                {
                    positionOffset.Y = 0;
                    velocity.Y = 0;
                    displacement.Y = 0;
                    timeStationaryCounter += 1;
                    return;
                }
            }


            if ((position.Y == endPosition.Y || position.Y == startPosition.Y) && timeStationaryCounter == timeStationaryAtEndPoints)
            {
                timeStationaryCounter = 0;

                if (firstLoop)
                {
                    if (movingDown)
                    {
                        positionOffset.Y = 1;
                        velocity.Y = 60 * speed;
                        displacement.Y = velocity.Y * deltaTime;
                    }
                    else if (movingUp)
                    {
                        positionOffset.Y = -1;
                        velocity.Y = -60 * speed;
                        displacement.Y = velocity.Y * deltaTime;
                    }

                    firstLoop = false;
                }
                else
                {
                    if (movingDown)
                    {
                        movingDown = false;
                        movingUp = true;
                        positionOffset.Y = -1;
                        velocity.Y = -60 * speed;
                        displacement.Y = velocity.Y * deltaTime;
                    }
                    else if (movingUp)
                    {
                        movingUp = false;
                        movingDown = true;
                        positionOffset.Y = 1;
                        velocity.Y = 60 * speed;
                        displacement.Y = velocity.Y * deltaTime;

                    }

                }

                return;
            }
        }





        public override void ManageAnimations()
        {
            if (timeStationaryCounter == 0)
            {
                UpdatePlayingAnimation(animation_Moving);
            }
            else
            {
                UpdatePlayingAnimation(animation_Idle);
            }



        }


    }
}