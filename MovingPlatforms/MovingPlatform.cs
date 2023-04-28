﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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

        bool test = true;

        public MovingPlatform(Direction direction)
        {
            this.direction = direction;
        }

        public MovingPlatform(List<Vector2> positions, List<int> indexes, string filename, int timeStationaryAtEndPoints, float speed, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(positions[0], filename, assetManager)
        {
            CollisionObject = true;
            LoadFirst = true;
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

            //if (test)
            //{
            //    position.X += 2 * displacement.X;
            //    position.Y += 2 * displacement.Y;
            //    MoveAttachedGameObjects();
            //    if (colliderManager.CheckForEdgesMeeting(idleHitbox, player.idleHitbox))
            //    {
            //        player.MoveManually(displacement);
            //    }
            //    else
            //    {
            //        foreach (GameObject gameObject in attachedGameObjects)
            //        {
            //            if (gameObject is AnimatedGameObject sprite)
            //            {
            //                if (sprite.Climable && player.playerStateManager.climbingState.Active && player.playerStateManager.climbingState.platform == sprite)
            //                {
            //                    player.MoveManually(displacement);
            //                    break;
            //                }

            //            }
            //        }
            //    }
            //    test = false;
            //}
            //else
            //{
            //    test = true;
            //}

            position.X += displacement.X;
            position.Y += displacement.Y;

            // This check needs to be done BEFORE idleHitbox is updated
            if (colliderManager.CheckForEdgesMeeting(idleHitbox, player.idleHitbox))
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

            MoveAttachedGameObjects();


            idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;

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

            displacement = velocity / 60; // If we multiply by deltaTime we incur floating point errors and must round the position to nearest integer (or half-integer etc) depending on speed
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


        // Give each GameObject a MoveOnPlatform method which it can override? So don't need to adjust method according to gameObject Type
        public void MoveAttachedGameObjects()
        {
            if (attachedGameObjects != null)
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {
                    gameObject.MoveManually(displacement);
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
