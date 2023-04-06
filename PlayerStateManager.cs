using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Framework.Utilities.Deflate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Adventure
{
    public class PlayerStateManager
    {

        public Player player;

        public List<State> playerStates;

        public NormalState normalState;
        public TeleportState teleportState;
        public SwingingState swingingState;
        public ClimbingLadderState climbingLadderState;
        public SlidingOnWallState slidingOnWallState;
        public ClimbingState climbingState;
        public DeadState deadState;









        public PlayerStateManager(Player player)
        {
            this.player = player;

            normalState = new NormalState(player);
            teleportState = new TeleportState(player);
            swingingState = new SwingingState(player);
            climbingLadderState = new ClimbingLadderState(player);
            slidingOnWallState = new SlidingOnWallState(player);
            climbingState = new ClimbingState(player);
            deadState = new DeadState(player);

            playerStates = new List<State>()
            {
                normalState,
                teleportState,
                swingingState,
                climbingLadderState,
                slidingOnWallState,
                climbingState,
                deadState
            };


            normalState.Active = true;




        }


        public void DeactivatePlayerStates()
        {
            foreach (State state in playerStates)
            {
                state.Deactivate();
            }
        }

        public void Update(GameTime gameTime)
        {

            UpdateWhichStateIsActive();

            UpdateActiveState(gameTime);

            //Debug.WriteLine(player.spriteVelocity.X);
        }


        public void UpdateActiveState(GameTime gameTime)
        {
            foreach (State state in playerStates)
            {
                if (state.Active)
                {
                    state.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (State state in playerStates)
            {
                if (state.Active)
                {
                    state.Draw(spriteBatch);
                }
            }
        }

        public void UpdateWhichStateIsActive()
        {

            foreach (State state in playerStates)
            {
                if (state.Active)
                {
                    state.UpdateExits();

                    switch (state.exits)
                    {
                        case State.Exits.noExit:
                            {
                                break;
                            }
                        case State.Exits.exitToNormalState:
                            {
                                state.Deactivate();
                                normalState.Activate();
                                return;
                            }
                        case State.Exits.exitToSlidingWallStateFacingRight:
                            {
                                state.Deactivate();
                                slidingOnWallState.Activate();
                                slidingOnWallState.facingRight = true;
                                return;
                            }
                        case State.Exits.exitToSlidingWallStateFacingLeft:
                            {
                                state.Deactivate();
                                slidingOnWallState.Activate();
                                slidingOnWallState.facingRight = false;
                                return;
                            }
                    }
                }

            }

            if (deadState.Active)
            {
                return;
            }

            // Check whether I should enter the climbing state
            if (!climbingState.Active)
            {
                if (player.climbButtonPressed)
                {

                    if (References.activeScreen.screenClimables != null)
                    {
                        foreach (AnimatedGameObject platform in References.activeScreen.screenClimables)
                        {
                            if (player.colliderManager.CheckForCollision(player.idleHitbox, platform.idleHitbox))
                            {
                                if (player.CollidedOnTop && player.velocity.Y < 0)
                                {
                                    DeactivatePlayerStates();
                                    climbingState.Activate();
                                    climbingState.platform = platform;
                                    return;
                                }
                                if (player.CollidedOnRight || player.CollidedOnLeft)
                                {
                                    DeactivatePlayerStates();
                                    climbingState.Activate();
                                    climbingState.platform = platform;
                                    return;
                                }
                                else if (player.CollidedOnBottom)
                                {
                                    if ((player.spriteDirectionX == 1 || player.spriteDirectionY == 1) && player.idleHitbox.rectangle.X >= platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - player.distanceCanStartClimbing)
                                    {
                                        player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width;
                                        player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y;
                                        player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                        player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                                        DeactivatePlayerStates();
                                        climbingState.Activate();
                                        climbingState.platform = platform;
                                        return;
                                    }
                                    else if ((player.spriteDirectionX == -1 || player.spriteDirectionY == 1) && player.idleHitbox.rectangle.X <= platform.idleHitbox.rectangle.X - player.distanceCanStartClimbing)
                                    {
                                        player.idleHitbox.rectangle.X = platform.idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                                        player.idleHitbox.rectangle.Y = platform.idleHitbox.rectangle.Y;
                                        player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                                        player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                                        DeactivatePlayerStates();
                                        climbingState.Activate();
                                        climbingState.platform = platform;
                                        return;
                                    }

                                }
                            }

                            //if (player.spriteCollider.CheckForCollision(player.idleHitbox, sprite.idleHitbox) && player.flagClimbButtonPressed)
                            //{
                            //    DeactivatePlayerStates();
                            //    climbingState.Activate();
                            //    climbingState.platform = sprite;
                            //}

                        }

                    }
                }
            }

            // Check whether I should enter the teleport state
            if (!teleportState.Active)
            {
                if (References.activeScreen.screenTeleports != null)
                {
                    foreach (Teleport portal in References.activeScreen.screenTeleports)
                    {
                        if (portal is LocalTeleport)
                        {
                            if (portal.InRange && player.flagTeleportButtonPressed)
                            {
                                DeactivatePlayerStates();
                                teleportState.Activate();
                                teleportState.portal = portal;
                                teleportState.isTeleportGlobal = false;
                                return;
                            }
                        }
                        else if (portal is GlobalTeleport)
                        {
                            if (player.flagTeleportButtonPressed)
                            {
                                DeactivatePlayerStates();
                                teleportState.Activate();
                                teleportState.portal = portal;
                                teleportState.isTeleportGlobal = true;
                                return;
                            }
                        }
                    }

                }
            }

            // Check whether I should enter the climbingLadder state
            if (!climbingLadderState.Active)
            {
                if (References.activeScreen.screenLadders != null)
                {
                    foreach (Ladder ladder in References.activeScreen.screenLadders)
                    {
                        if (player.colliderManager.CheckForCollision(player.idleHitbox, ladder.idleHitbox))
                        {
                            // I'm climbing starting from the top
                            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height <= ladder.positionOfTopLeftCorner.Y && player.spriteDirectionY == 1)
                            {
                                climbingLadderState.ladder = ladder;
                                ladder.idleHitbox.isActive = false;
                                DeactivatePlayerStates();
                                climbingLadderState.Activate();
                                return;
                            }

                            // I'm climbing starting from somewhere below the top
                            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height > ladder.positionOfTopLeftCorner.Y && player.spriteDirectionY != 0)
                            {
                                climbingLadderState.ladder = ladder;
                                DeactivatePlayerStates();
                                climbingLadderState.Activate();
                                return;
                            }

                        }

                    }
                }
            }

            // Check whether I should enter the dead state
            if (!deadState.Active)
            {
                if (References.activeScreen.screenHazards != null)
                {
                    foreach (AnimatedGameObject sprite in References.activeScreen.screenHazards)
                    {
                        if (player.colliderManager.CheckForCollision(player.idleHitbox, sprite.idleHitbox) && sprite.idleHitbox.isActive)
                        {
                            DeactivatePlayerStates();
                            deadState.Activate();
                            return;
                        }
                    }
                }

            }

            // Check whether I should enter the swinging state
            if (!swingingState.Active)
            {
                if (References.activeScreen.screenHookPoints != null)
                {
                    foreach (HookPoint hookPoint in References.activeScreen.screenHookPoints)
                    {
                        if (hookPoint.InRange && player.flagHookButtonPressed)
                        {
                            DeactivatePlayerStates();
                            swingingState.Activate();
                            swingingState.hookPoint = hookPoint;
                        }
                    }
                }

            }




        }













    }
}
