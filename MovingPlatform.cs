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
    public class MovingPlatform : MovingSprite
    {

        public AnimatedSprite animatedSprite_Moving;

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

        public List<Sprite> spritesOnPlatform;



        public MovingPlatform() : base()
        {
            CollisionSprite = true;

        }

        public MovingPlatform(Vector2 initialPosition) : base(initialPosition)
        {
            CollisionSprite = true;
            beforeDelay = false;

        }

        public MovingPlatform(Vector2 startPosition, string filename, Vector2 endPosition, int timeStationaryAtEndPoints, float speed) : base(startPosition, filename)
        {
            CollisionSprite = true;
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

        public MovingPlatform(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, List<Sprite> spritesOnPlatform) : base(initialPosition, filename)
        {
            CollisionSprite = true;
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

            animatedSprite_Moving = spriteSheet.CreateAnimatedSprite("Moving");
            animatedSpriteAndTag.Add("Moving", animatedSprite_Moving);

            CollisionSprite = true;
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



            spritePosition.X += spriteDisplacement.X;
            spritePosition.X = DistanceToNearestInteger(spritePosition.X);
            spritePosition.Y += spriteDisplacement.Y;
            spritePosition.Y = DistanceToNearestInteger(spritePosition.Y);

            // This needs to be done BEFORE idleHitbox is updated
            if (spriteCollider.CheckForEdgesMeeting(this.idleHitbox, References.player.idleHitbox))
            {
                movePlayerToo = true;
            }
            else
            {
                movePlayerToo = false;
            }

            idleHitbox.rectangle.X = DistanceToNearestInteger(spritePosition.X) + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = DistanceToNearestInteger(spritePosition.Y) + idleHitbox.offsetY;

            if (movePlayerToo)
            {
                References.player.spritePosition.X += spriteDisplacement.X;
                References.player.spritePosition.Y += spriteDisplacement.Y;
                References.player.velocityOffSetDueToMovingPlatform.X = spriteVelocity.X;
                References.player.velocityOffSetDueToMovingPlatform.Y = spriteVelocity.Y;
            }
            if (spritesOnPlatform != null)
            {

                foreach (Sprite sprite in spritesOnPlatform)
                {
                    if (sprite is Note note)
                    {
                        note.key.spritePosition.X += spriteDisplacement.X;
                        note.key.idleHitbox.rectangle.X = (int)note.key.spritePosition.X + note.key.idleHitbox.offsetX;
                        note.key.spritePosition.Y += spriteDisplacement.Y;
                        note.key.idleHitbox.rectangle.Y = (int)note.key.spritePosition.Y + note.key.idleHitbox.offsetY;
                    }
                    else
                    {

                        sprite.spritePosition.X += spriteDisplacement.X;
                        sprite.idleHitbox.rectangle.X = (int)sprite.spritePosition.X + sprite.idleHitbox.offsetX;
                        sprite.spritePosition.Y += spriteDisplacement.Y;
                        sprite.idleHitbox.rectangle.Y = (int)sprite.spritePosition.Y + sprite.idleHitbox.offsetY;
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
            if (spritePosition.X != endPosition.X && spritePosition.X != startPosition.X)
            {
                if (movingRight)
                {
                    spriteVelocity.X = 60 * speed;
                    spriteDisplacement.X = spriteVelocity.X * deltaTime;
                }
                else if (movingLeft)
                {
                    spriteVelocity.X = -60 * speed;
                    spriteDisplacement.X = spriteVelocity.X * deltaTime;
                }

                return;
            }

            if (timeStationaryAtEndPoints > 0)
            {

                if ((spritePosition.X == endPosition.X || spritePosition.X == startPosition.X) && timeStationaryCounter < timeStationaryAtEndPoints)
                {

                    spriteVelocity.X = 0;
                    spriteDisplacement.X = 0;
                    timeStationaryCounter += 1;
                    return;
                }
            }

            if ((spritePosition.X == endPosition.X || spritePosition.X == startPosition.X) && timeStationaryCounter == timeStationaryAtEndPoints)
            {
                timeStationaryCounter = 0;

                if (firstLoop)
                {
                    if (movingRight)
                    {
                        spriteVelocity.X = 60 * speed;
                        spriteDisplacement.X = spriteVelocity.X * deltaTime;
                    }
                    else if (movingLeft)
                    {
                        spriteVelocity.X = -60 * speed;
                        spriteDisplacement.X = spriteVelocity.X * deltaTime;
                    }

                    firstLoop = false;

                }
                else
                {
                    if (movingRight)
                    {
                        movingRight = false;
                        movingLeft = true;
                        spriteVelocity.X = -60 * speed;
                        spriteDisplacement.X = spriteVelocity.X * deltaTime;
                    }
                    else if (movingLeft)
                    {
                        movingLeft = false;
                        movingRight = true;
                        spriteVelocity.X = 60 * speed;
                        spriteDisplacement.X = spriteVelocity.X * deltaTime;
                    }
                }

                return;
            }
        }

        public void FindVerticalVelocityAndDisplacement()
        {
            if (spritePosition.Y != endPosition.Y && spritePosition.Y != startPosition.Y)
            {
                if (movingDown)
                {
                    positionOffset.Y = 1;
                    spriteVelocity.Y = 60 * speed;
                    spriteDisplacement.Y = spriteVelocity.Y * deltaTime;
                }
                else if (movingUp)
                {
                    positionOffset.Y = -1;
                    spriteVelocity.Y = -60 * speed;
                    spriteDisplacement.Y = spriteVelocity.Y * deltaTime;

                }


                return;
            }

            if (timeStationaryAtEndPoints > 0)
            {
                if ((spritePosition.Y == endPosition.Y || spritePosition.Y == startPosition.Y) && timeStationaryCounter < timeStationaryAtEndPoints)
                {
                    positionOffset.Y = 0;
                    spriteVelocity.Y = 0;
                    spriteDisplacement.Y = 0;
                    timeStationaryCounter += 1;
                    return;
                }
            }


            if ((spritePosition.Y == endPosition.Y || spritePosition.Y == startPosition.Y) && timeStationaryCounter == timeStationaryAtEndPoints)
            {
                timeStationaryCounter = 0;

                if (firstLoop)
                {
                    if (movingDown)
                    {
                        positionOffset.Y = 1;
                        spriteVelocity.Y = 60 * speed;
                        spriteDisplacement.Y = spriteVelocity.Y * deltaTime;
                    }
                    else if (movingUp)
                    {
                        positionOffset.Y = -1;
                        spriteVelocity.Y = -60 * speed;
                        spriteDisplacement.Y = spriteVelocity.Y * deltaTime;
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
                        spriteVelocity.Y = -60 * speed;
                        spriteDisplacement.Y = spriteVelocity.Y * deltaTime;
                    }
                    else if (movingUp)
                    {
                        movingUp = false;
                        movingDown = true;
                        positionOffset.Y = 1;
                        spriteVelocity.Y = 60 * speed;
                        spriteDisplacement.Y = spriteVelocity.Y * deltaTime;

                    }

                }

                return;
            }
        }





        public override void ManageAnimations()
        {
            if (timeStationaryCounter == 0)
            {
                nameOfCurrentAnimationSprite = "Moving";

                //animatedSprite_Idle.Play("Moving");
                //currentFrame = frameAndTag["Moving"].From;
                //tagOfCurrentFrame = "Moving";

            }
            else
            {
                nameOfCurrentAnimationSprite = "Idle";

                //animatedSprite_Idle.Play("Idle");
                //currentFrame = frameAndTag["Idle"].From;
                //tagOfCurrentFrame = "Idle";
            }



        }


    }
}
