using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Adventure
{
    public class MovingPlatform : MovingGameObject
    {
        // Every MovingPlatform will have a "moving" animation
        public AnimatedSprite animation_Moving;

        // A MovingPlatform object can either move horizontally or vertically in straight lines (or be stationary)
        public enum Direction
        {
            moveRight,
            moveLeft,
            moveUp,
            moveDown,
            stationary
        };
        public Direction direction;

        // Every MovingPlatform will contain a list of positions it will travel to.
        // There will always be at least two positions and when we reach the end of the list the next position the platform will move to will be the first element i.e. we loop back to the start.
        public List<Vector2> positions = new List<Vector2>();

        // The position WE HAVE JUST LEFT will be kept track of via currentIndex - i.e. we will always have just left positions[currentIndex].
        public int currentIndex;
        // The position WE ARE TRAVELLING TOWARDS will be kept track of via indexToMoveTo - i.e. we will always be travelling to positions[indexToMoveTo]
        public int indexToMoveTo;
        // We keep track of these by moving through the list indexes - in derived classes we may want the player to be able to reverse the direction.
        // This means going through the list in the opposite direction. We keep track of this via the following sign (+1 means go forward through the list, -1 means go backwards).
        public int sign = 1;


        // Every MovingPlatform will have a speed
        public float speed;

        // At each position we may want the platform to remain stationary for a set number of frames
        public List<int> stationaryTimes = new List<int>();
        public int timeStationaryCounter = 0;


        // Certain MovingPlatforms will have behaviour that is triggered by the player and we incorporate a trigger via this bool. (Any derived classes which is controlled by the player will set this to false.)
        public bool movePlatform = true;

        // If the platform is a hazard (e.g. an Orb) we do not want the player to move as in this case we never detect collision with the player's hurtHitbox
        public bool movePlayer = true;

        // If the player reverses the direction of the platform we may want the motion to halt for a set number of frames
        public bool halt = false;
        public int numberOfFramesHalted = 60;
        public int haltCounter = 0;



        public MovingPlatform(Direction direction)
        {
            this.direction = direction;
        }

        public MovingPlatform(List<Vector2> positions, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(positions[0], filename, assetManager)
        {
            CollisionObject = true;
            LoadFirst = true;
            attachedGameObjects = new List<GameObject>();

            this.colliderManager = colliderManager;
            this.positions = positions;
            this.speed = speed;
            this.stationaryTimes = stationaryTimes;
            this.player = player;

            // We configure our starting direction
            currentIndex = 0;
            indexToMoveTo = 1;
            direction = Direction.stationary; // We initialise direction to stationary so that we do not stop by mistake on the first loop 

            //if (stationaryTimes[0] == 0)
            //{
            //    currentIndex = 0;
            //    indexToMoveTo = 1;
            //    UpdateDirection();
            //}
            //else
            //{
            //    currentIndex = 0;
            //    indexToMoveTo = 1;
            //    direction = Direction.stationary;
            //}


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
                    ManageAnimations();
                    BaseUpdate(gameTime);
                    return;
                }
            }

            if (movePlatform)
            {
                MovePlatformAndAttachedGameObjects();
            }

            ManageAnimations();
            base.Update(gameTime);

        }

        public void BaseUpdate(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



        public void MovePlatformAndAttachedGameObjects()
        {
            UpdateAtStationaryPoints();

            // In derived classes we may have set movePlatform to false in the above method call
            // In this case we do not want to update any further and want to break from the loop
            if (!movePlatform)
            {
                return;
            }

            UpdateVelocityAndDisplacement();
            position.X += displacement.X;
            position.Y += displacement.Y;
            // This method needs to be done BEFORE idleHitbox is updated
            MoveAttachedGameObjects();
            idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;
        }



        public virtual void UpdateAtStationaryPoints()
        {
            if (direction == Direction.stationary)
            {
                if (timeStationaryCounter < stationaryTimes[currentIndex])
                {
                    timeStationaryCounter += 1;
                }
                else
                {
                    timeStationaryCounter = 0;
                    UpdateDirection();
                }
            }
            else
            {
                if (position == positions[indexToMoveTo])
                {
                    if (stationaryTimes[indexToMoveTo] == 0)
                    {
                        timeStationaryCounter = 0;
                        UpdateIndices();
                        UpdateDirection();
                    }
                    else
                    {
                        UpdateIndices();
                        direction = Direction.stationary;
                    }
                }
            }

        }

        public void UpdateIndices()
        {
            currentIndex = indexToMoveTo;
            indexToMoveTo = (indexToMoveTo + positions.Count + sign) % positions.Count;
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

            displacement = velocity / 60; // If we multiply by deltaTime we incur floating point errors and must round the position to nearest integer (or half-integer etc) depending on speed
        }




        public override void ManageAnimations()
        {
            if (!movePlatform || direction == Direction.stationary || halt)
            {
                UpdatePlayingAnimation(animation_Idle);
            }
            else
            {
                UpdatePlayingAnimation(animation_Moving);
            }

        }


        public void MoveAttachedGameObjects()
        {
            // Move the player if they are in contact with the platform OR if they are climbing on an object attached to the platform
            if (colliderManager.CheckForEdgesMeeting(idleHitbox, player.idleHitbox) && movePlayer)
            {
                player.MoveManually(displacement);
            }
            else
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {
                    if (gameObject is AnimatedGameObject sprite)
                    {
                        if (sprite.Climable && player.playerStateManager.climbingState.Active && player.playerStateManager.climbingState.platform == sprite)
                        {
                            player.MoveManually(displacement);
                            break;
                        }

                    }
                }
            }

            if (attachedGameObjects != null)
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {
                    gameObject.MoveManually(displacement);
                }
            }
        }

        public void UpdateDirection()
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

            sign *= -1;
            (indexToMoveTo, currentIndex) = (currentIndex, indexToMoveTo);
        }


        // Methods for the ActionScreenBuilder to use
        public override void AdjustHorizontally(ref List<int> ints)
        {
            position.X += ints[0];
            positions[0] = new Vector2(positions[0].X + ints[0], positions[0].Y);
            ints.RemoveAt(0);
        }

        public override void AdjustVertically(ref List<int> ints)
        {
            position.Y += ints[0];
            positions[0] = new Vector2(positions[0].X, positions[0].Y + ints[0]);
            ints.RemoveAt(0);
        }

    }
}
