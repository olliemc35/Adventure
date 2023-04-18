using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System.Collections.Generic;

namespace Adventure
{
    public class MovingPlatform : MovingGameObject
    {
        // Every MovingPlatform will have a "moving" animation
        public AnimatedSprite animation_Moving;

        // A MovingPlatform object can either move horizontally or vertically in straight lines
        public enum Direction
        {
            moveRight,
            moveLeft,
            moveUp,
            moveDown,
            stationary
        };
        public Direction direction;

        // Every MovingPlatform will contain a list of positions it will travel to. There will always be at least two positions.
        public List<Vector2> positions = new List<Vector2>();

        // We will also give a list of ints corresponding to the indices we move to. At the end of the list we loop back to the start.
        // E.g. suppose there are 3 positions. Then indexes = {0,1,2,1,2} means we go 0 - 1 - 2 - 1 - 2 - 0 - ... etc.
        public List<int> indexes = new List<int>();
        public int currentIndex = 0;

        // Every MovingPlatform will have a speed
        public float speed;

        // We may want the MovingPlatform to remain stationary for a few frames when it reaches the next position in the sequence
        public int timeStationaryAtEndPoints; // counted in frames
        public int timeStationaryCounter = 0;
        public bool thereIsStationaryTime;

        // We may want to delay the movement of the MovingPlatform for a few frames before any movement begins
        public int delay = 0; // counted in frames
        public int delayCounter = 0;
        public bool thereIsADelay;

        // There is a slightly annoying technicality with the code where we must treat the first time we UpdateStationaryPoints separately to the rest, hence we use this bool
        public bool firstLoop = true;

        // Certain MovingPlatforms will have behaviour that is triggered by the player and we incorporate a trigger via this bool. (This is for derived classes to use.)
        public bool movePlatform = false;



        public MovingPlatform(List<Vector2> positions, List<int> indexes, string filename, int timeStationaryAtEndPoints, float speed, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(positions[0], filename, assetManager)
        {
            CollisionObject = true;
            attachedGameObjects = new List<GameObject>();

            this.colliderManager = colliderManager;          
            this.positions = positions;
            this.indexes = indexes;
            this.timeStationaryAtEndPoints = timeStationaryAtEndPoints;
            this.speed = speed;
            this.delay = delay;
            this.player = player;

            if (delay == 0)
            {
                thereIsADelay = false;
            }
            else
            {
                thereIsADelay = true;
            }

            if (timeStationaryAtEndPoints == 0)
            {
                thereIsStationaryTime = false;
            }
            else
            {
                thereIsStationaryTime = true;
            }

            // We configure our starting direction
            UpdateDirection(indexes[0], indexes[1]);

            deltaTime = 1f / 60;
            
        }





        public override void LoadContent()
        {
            base.LoadContent();
            animation_Moving = spriteSheet.CreateAnimatedSprite("Moving");
            idleHitbox.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            
            if (thereIsADelay)
            {
                if (delayCounter > delay)
                {
                    thereIsADelay = false;
                }
                else
                {
                    delayCounter += 1;
                }

                return;
            }

            UpdateAtStationaryPoints();
            UpdateVelocityAndDisplacement();
            position.X += displacement.X;
            position.X = FindNearestInteger(position.X);
            position.Y += displacement.Y;
            position.Y = FindNearestInteger(position.Y);

            // The player check needs to be done BEFORE idleHitbox is updated
            if (colliderManager.CheckForEdgesMeeting(idleHitbox, player.idleHitbox))
            {
                player.position.X += displacement.X;
                player.position.Y += displacement.Y;
                player.velocityOffSetDueToMovingPlatform.X = velocity.X;
                player.velocityOffSetDueToMovingPlatform.Y = velocity.Y;
            }

            idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;

            MoveAttachedGameObjects();
            ManageAnimations();

            base.Update(gameTime);

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void UpdateAtStationaryPoints()
        {
            if (position == positions[indexes[currentIndex]] || position == positions[indexes[(currentIndex + 1) % indexes.Count]])
            {
                if (thereIsStationaryTime && timeStationaryCounter < timeStationaryAtEndPoints)
                {
                    timeStationaryCounter += 1;
                    direction = Direction.stationary;
                }
                else if (!thereIsStationaryTime || (thereIsStationaryTime && timeStationaryCounter == timeStationaryAtEndPoints))
                {
                    timeStationaryCounter = 0;
                    if (firstLoop)
                    {
                        firstLoop = false;
                    }
                    else
                    {
                        currentIndex = (currentIndex + 1) % indexes.Count;
                    }
                    UpdateDirection(currentIndex, (currentIndex + 1) % indexes.Count);
                }

            }

        }


        public void UpdateVelocityAndDisplacement()
        {
            switch (direction)
            {
                case Direction.moveRight:
                    {
                        velocity.X = 60 * speed;
                        velocity.Y = 0;
                        break;
                    }
                case Direction.moveLeft:
                    {
                        velocity.X = -60 * speed;
                        velocity.Y = 0;
                        break;
                    }
                case Direction.moveUp:
                    {
                        velocity.X = 0;
                        velocity.Y = -60 * speed;
                        break;
                    }
                case Direction.moveDown:
                    {
                        velocity.X = 0;
                        velocity.Y = 60 * speed;
                        break;
                    }
                case Direction.stationary:
                    {
                        velocity.X = 0;
                        velocity.Y = 0;
                        break;
                    }

            }

            displacement = velocity * deltaTime;
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


        public void MoveAttachedGameObjects()
        {
            if (attachedGameObjects != null)
            {

                foreach (GameObject gameObject in attachedGameObjects)
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
        }

        public void UpdateDirection(int currentIndex, int indexToMoveTo)
        {
            if (positions[currentIndex].X == positions[indexToMoveTo].X)
            {
                if (positions[currentIndex].Y > positions[indexToMoveTo].Y)
                {
                    direction = Direction.moveUp;
                }
                else
                {
                    direction = Direction.moveDown;
                }
            }
            else
            {

                if (positions[currentIndex].X < positions[indexToMoveTo].X)
                {
                    direction = Direction.moveRight;
                }
                else
                {
                    direction = Direction.moveLeft;
                }

            }
        }

        public virtual void ReverseDirection()
        {
            currentIndex = indexes[(currentIndex + 1) % indexes.Count];

            switch (direction)
            {
                case Direction.moveRight:
                    {
                        direction = Direction.moveLeft;
                        break;
                    }
                case Direction.moveLeft:
                    {
                        direction = Direction.moveRight;

                        break;
                    }
                case Direction.moveUp:
                    {
                        direction = Direction.moveDown;

                        break;
                    }
                case Direction.moveDown:
                    {
                        direction = Direction.moveUp;
                        break;
                    }
                case Direction.stationary:
                    {
                        break;
                    }

            }


            indexes.Reverse();

        }

    }
}
