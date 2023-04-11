using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class DeadState : State
    {
        public DeadState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {
         
        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                player.UpdatePlayingAnimation(player.animation_Dead, 1);
                enterStateFlag = false;
            }

            if (player.animation_playing == player.animation_Idle)
            {
                exits = Exits.exitToNormalState;
            }
 
        }

       

    }
}
