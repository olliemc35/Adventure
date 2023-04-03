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

        public ClimbingLadderState(Player player) : base(player)
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
            player.spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

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
            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height <= ladder.positionOfTopLeftCorner.Y && player.spriteDirectionY != 1)
            {
                player.spriteVelocity.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }

            // I've hit the bottom and I'm not going up
            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height > ladder.positionOfTopLeftCorner.Y && player.SpriteCollidedOnBottom && player.spriteDirectionY != -1)
            {
                player.spriteVelocity.Y = 0;
                exits = Exits.exitToNormalState;
                return;
            }
        }


        public override void UpdateVelocityAndDisplacement()
        {
            player.spriteVelocity.X = 0;
            player.spriteDisplacement.X = 0;
            player.spriteVelocity.Y = climbingSpeed * player.spriteDirectionY;
            player.spriteDisplacement.Y = player.spriteVelocity.Y * player.deltaTime;

        }


        public override void UpdateAnimations()
        {
            player.animatedSprite.Play("ClimbingLadder");
            player.currentFrame = player.frameAndTag["ClimbingLadder"].From;
            player.tagOfCurrentFrame = "ClimbingLadder";
            player.TurnOffAllHitboxes();
            player.idleHitbox.isActive = true;
        }




    }
}
