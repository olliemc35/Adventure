using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace Adventure
{
    public class ClimbingLadderState : State
    {
        public Ladder ladder;
        public float climbingSpeed = 60;

        public ClimbingLadderState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {
        }


        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                enterStateFlag = false;
            }

            UpdateAnimations();
            UpdateVelocityAndDisplacement();
            player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }



        public override void UpdateExits()
        {
            // If player presses jump button want to exit ladder climbing state
            if (player.flagJumpButtonPressed)
            {
                exits = Exits.exitToNormalState;
                return;
            }

            // I've climbed the ladder
            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height <= ladder.positionOfTopLeftCorner.Y && player.directionY != 1)
            {
                player.velocity.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }

            // I've hit the bottom and I'm not going up
            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height > ladder.positionOfTopLeftCorner.Y && player.CollidedOnBottom && player.directionY != -1)
            {
                player.velocity.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }
        }


        public override void UpdateVelocityAndDisplacement()
        {
            player.velocity.X = 0;
            player.displacement.X = 0;
            player.velocity.Y = climbingSpeed * player.directionY;
            player.displacement.Y = player.velocity.Y * player.deltaTime;

        }


        public override void UpdateAnimations()
        {
            player.UpdatePlayingAnimation(player.animation_ClimbingLadder);

        }




    }
}
