using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using static Adventure.NormalState;

namespace Adventure
{
    public class ClimbingState : State
    {
        public AnimatedGameObject platform;
        public float climbingSpeed = 120;
        public float wallJumpSpeed = 120;
        public int numberOfFramesToLeave = 10;
        public int leavingCounter = 0;

        public enum Orientation
        {
            left,
            right,
            top
        };

        public Orientation orientation;

        public ClimbingState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                player.velocity.X = 0;
                player.velocity.Y = 0;
                enterStateFlag = false;

                // Set starting positions
                switch (orientation)
                {
                    case Orientation.right:
                        {
                            player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                            player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                            break;
                        }
                    case Orientation.left:
                        {
                            player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                            player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                            break;
                        }
                    case Orientation.top:
                        {
                            player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;
                            player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                            break;
                        }
                }
            }

            UpdateAnimations();
            UpdateVelocityAndDisplacement();
            player.colliderManager.AdjustForCollisionWithClimable(player, platform, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);
            //player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);




        }

        public override void UpdateExits()
        {
            switch (orientation)
            {
                case Orientation.right:
                    {

                        // Leave if I am the bottom and press the down-key
                        if (player.idleHitbox.rectangle.Y > platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height)
                        {
                            if (player.CollidedOnBottom && ((player.directionY == 1 && player.directionX != 1) || player.directionX == -1))
                            {
                                player.velocity.X = 0;
                                player.velocity.Y = 0;
                                exits = Exits.exitToNormalState;
                                return;
                            }
                            //if (player.CollidedOnBottom && (player.spriteDirectionY == 1 && player.spriteDirectionX != 1) || player.spriteDirectionX == -1)
                            //{
                            //    player.velocity.X = 0;
                            //    player.velocity.Y = 0;
                            //    exits = Exits.exitToNormalState;
                            //    return;
                            //}
                        }

                        // Leave if I hold the opposite X direction for a certain number of frames
                        if (player.directionX == -1)
                        {
                            if (leavingCounter >= numberOfFramesToLeave)
                            {
                                player.velocity.Y = 0;
                                player.displacement.Y = 0;
                                player.velocity.X = -wallJumpSpeed;
                                exits = Exits.exitToNormalState;
                                return;
                            }

                            leavingCounter += 1;
                        }
                        else
                        {
                            leavingCounter = 0;
                        }

                        // Leave if I hold the opposite X direction and press the jump button 
                        if (player.directionX == -1 && player.flagJumpButtonPressed)
                        {
                            player.velocity.X = -wallJumpSpeed;
                            player.velocity.Y = -1 * player.jumpSpeed;
                            exits = Exits.exitToNormalState;
                            return;

                        }

                   


                        // Leave if I reach the top of the platform and holding correct dirction keys
                        if (player.idleHitbox.rectangle.Y <= platform.idleHitbox.rectangle.Y - player.distanceCanStartClimbing)
                        {
                            if (player.directionX == 1 || player.directionY == -1)
                            {
                                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;

                                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                                //player.position.X = platform.idleHitbox.rectangle.X;
                                //player.position.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height - player.idleHitbox.offsetY;


                                player.velocity.Y = 0;

                                exits = Exits.exitToNormalState;
                                return;
                            }
                        }


                        break;
                    }
                case Orientation.left:
                    {
                        if (player.directionX == 1)
                        {
                            if (leavingCounter >= numberOfFramesToLeave)
                            {
                                player.velocity.Y = 0;
                                player.displacement.Y = 0;
                                player.velocity.X = wallJumpSpeed;
                                exits = Exits.exitToNormalState;
                                return;
                            }

                            leavingCounter += 1;
                        }
                        else
                        {
                            leavingCounter = 0;
                        }

                        if (player.directionX == 1 && player.flagJumpButtonPressed)
                        {
                            player.velocity.X = wallJumpSpeed;
                            player.velocity.Y = -1 * player.jumpSpeed;
                            exits = Exits.exitToNormalState;
                            return;
                        }

                        if (player.idleHitbox.rectangle.Y <= platform.idleHitbox.rectangle.Y - player.distanceCanStartClimbing)
                        {
                            if (player.directionX == -1 || player.directionY == -1)
                            {
                                //player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                                //player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;
                                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + (int) 0.5f * platform.idleHitbox.rectangle.Width;
                                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y - player.idleHitbox.rectangle.Height;

                                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                                player.velocity.Y = 0;

                                exits = Exits.exitToNormalState;
                                return;
                            }
                        }

                        break;
                    }
                case Orientation.top:
                    {
                        if (player.directionY == 1)
                        {
                            if (leavingCounter >= numberOfFramesToLeave)
                            {
                                player.velocity.Y = 0;
                                player.displacement.Y = 0;
                                exits = Exits.exitToNormalState;
                                return;
                            }

                            leavingCounter += 1;
                        }
                        else
                        {
                            leavingCounter = 0;
                        }

                        if (player.directionY == 1 && player.flagJumpButtonPressed)
                        {
                            player.velocity.Y = 0;
                        }

                        break;
                    }

            }


           
          

        }

        public void TransitionFromSidesToUnderneath()
        {
            if (player.idleHitbox.rectangle.Y >= platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing)
            {

                switch (orientation)
                {
                    case Orientation.right:
                        {

                            if (player.directionX == 1 || player.directionY == 1)
                            {
                                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X;
                                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;

                                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                                orientation = Orientation.top;
                            }

                            break;
                        }
                    case Orientation.left:
                        {
                            if (player.directionX == -1 || player.directionY == 1)
                            {
                                player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.idleHitbox.rectangle.Width;
                                player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height;

                                player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                                orientation = Orientation.top;

                            }

                            break;
                        }
                    case Orientation.top:
                        {                         
                            break;
                        }

                }


 
            }

        }

        public void TransitionFromUnderneathToSides()
        {
            if (orientation == Orientation.top)
            {
                if (player.idleHitbox.rectangle.X >= platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;

                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                    orientation = Orientation.right;


                }

                if (player.idleHitbox.rectangle.X <= platform.idleHitbox.rectangle.X - player.distanceCanStartClimbing)
                {
                    player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                    player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y + platform.idleHitbox.rectangle.Height - player.distanceCanStartClimbing;

                    player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                    player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;

                    orientation = Orientation.left;

                }
            }
        }


        public override void UpdateVelocityAndDisplacement()
        {
            switch (orientation)
            {
                case Orientation.right:
                    {
                        player.velocity.Y = climbingSpeed * player.directionY;
                        player.velocity.X = 0;
                        break;
                    }
                case Orientation.left:
                    {
                        player.velocity.Y = climbingSpeed * player.directionY;
                        player.velocity.X = 0;
                        break;
                    }
                case Orientation.top:
                    {
                        player.velocity.X = climbingSpeed * player.directionX;
                        player.velocity.Y = 0;
                        break;
                    }

            }

            if (platform.idleHitbox.rectangle.Width > 8)
            {
                TransitionFromSidesToUnderneath();
                TransitionFromUnderneathToSides();
            }
            



            player.displacement = player.velocity * player.deltaTime;

        }


        public override void UpdateAnimations()
        {
            switch (orientation)
            {
                case Orientation.right:
                    {
                        player.UpdatePlayingAnimation(player.animation_SlideRight);
                        break;
                    }
                case Orientation.left:
                    {
                        player.UpdatePlayingAnimation(player.animation_SlideLeft);
                        break;
                    }
                case Orientation.top:
                    {
                        player.UpdatePlayingAnimation(player.animation_ClimbTop);
                        break;
                    }
            }



        }

    }
}
