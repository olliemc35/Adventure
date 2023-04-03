using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class DeadState : State
    {

        public bool Dead = false;
        public bool Respawn = false;

        public DeadState(Player player) : base(player)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                enterStateFlag = false;
                Dead = true;
            }

            //Debug.WriteLine(Respawn);

            UpdateAnimations();
            UpdateVelocityAndDisplacement();
        }

        public override void UpdateVelocityAndDisplacement()
        {
            if (Respawn)
            {
                player.spritePosition.X = References.activeScreen.respawnPoint.X;
                player.spritePosition.Y = References.activeScreen.respawnPoint.Y;
                player.idleHitbox.rectangle.X = (int)player.spritePosition.X + player.idleHitbox.offsetX;
                player.idleHitbox.rectangle.Y = (int)player.spritePosition.Y + player.idleHitbox.offsetY;
                player.animatedSprite.Position = player.spritePosition;
                player.spriteVelocity.X = 0;
                player.spriteVelocity.Y = 0;
            }
        }

        public override void UpdateAnimations()
        {
            if (Dead)
            {
                player.animatedSprite.Play("Dead");
                player.currentFrame = player.frameAndTag["Dead"].From;
                player.tagOfCurrentFrame = "Dead";
                player.TurnOffAllHitboxes();
                player.idleHitbox.isActive = true;
            }
            else if (Respawn)
            {
                player.animatedSprite.Play("Respawn");
                player.currentFrame = player.frameAndTag["Respawn"].From;
                player.tagOfCurrentFrame = "Respawn";
                player.TurnOffAllHitboxes();
                player.idleHitbox.isActive = true;
            }

            player.animatedSprite.OnAnimationLoop = () =>
            {
                if (player.tagOfCurrentFrame == "Dead")
                {
                    Dead = false;
                    Respawn = true;
                    player.animatedSprite.OnAnimationLoop = null;
                }

                if (player.tagOfCurrentFrame == "Respawn")
                {
                    Respawn = false;
                    player.animatedSprite.Play("Idle");
                    player.currentFrame = player.frameAndTag["Idle"].From;
                    player.tagOfCurrentFrame = "Idle";
                    player.TurnOffAllHitboxes();
                    player.idleHitbox.isActive = true;

                    exits = Exits.exitToNormalState;

                    player.animatedSprite.OnAnimationLoop = null;

                }
            };
        }


    }
}
