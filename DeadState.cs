using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                player.UpdatePlayingAnimation(player.animation_Dead, 1);
                enterStateFlag = false;
                Dead = true;
            }

            if (player.animation_playing == player.animation_Idle)
            {
                exits = Exits.exitToNormalState;
            }
            //Debug.WriteLine(Respawn);

            //UpdateAnimations();
            //UpdateVelocityAndDisplacement();



        }

        public override void UpdateVelocityAndDisplacement()
        {
            if (Respawn)
            {
                player.position.X = References.activeScreen.respawnPoint.X;
                player.position.Y = References.activeScreen.respawnPoint.Y;
                player.idleHitbox.rectangle.X = (int)player.position.X + player.idleHitbox.offsetX;
                player.idleHitbox.rectangle.Y = (int)player.position.Y + player.idleHitbox.offsetY;
                //player.animatedSprite_Idle.Position = player.spritePosition;
                player.velocity.X = 0;
                player.velocity.Y = 0;
            }
        }

        public override void UpdateAnimations()
        {
            if (Dead)
            {
                player.UpdatePlayingAnimation(player.animation_Dead, 1);
            }
            else if (Respawn)
            {
                player.UpdatePlayingAnimation(player.animation_Respawn, 1);

            }

            //player.animation_Dead.OnAnimationLoop = (animatedSprite_Dead) =>
            //{
            //    Dead = false;
            //    Respawn = true;
            //    player.animation_Dead.OnAnimationLoop = null;
            //};


            //player.animation_Respawn.OnAnimationLoop = (animatedSprite_Respawn) =>
            //{
            //    Respawn = false;
            //    player.nameOfCurrentAnimationSprite = "Idle";

            //    //player.animatedSprite_Idle.Play("Idle");
            //    //player.currentFrame = player.frameAndTag["Idle"].From;
            //    //player.tagOfCurrentFrame = "Idle";
            //    //player.TurnOffAllHitboxes();
            //    //player.idleHitbox.isActive = true;

            //    exits = Exits.exitToNormalState;

            //    player.animation_Respawn.OnAnimationLoop = null;
            //};


            //player.animatedSprite_Idle.OnAnimationLoop = () =>
            //{
            //    if (player.tagOfCurrentFrame == "Dead")
            //    {
            //        Dead = false;
            //        Respawn = true;
            //        player.animatedSprite_Idle.OnAnimationLoop = null;
            //    }

            //    if (player.tagOfCurrentFrame == "Respawn")
            //    {
            //        Respawn = false;
            //        player.animatedSprite_Idle.Play("Idle");
            //        player.currentFrame = player.frameAndTag["Idle"].From;
            //        player.tagOfCurrentFrame = "Idle";
            //        player.TurnOffAllHitboxes();
            //        player.idleHitbox.isActive = true;

            //        exits = Exits.exitToNormalState;

            //        player.animatedSprite_Idle.OnAnimationLoop = null;

            //    }
            //};
        }


    }
}
