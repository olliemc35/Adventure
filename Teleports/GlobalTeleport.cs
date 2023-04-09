using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class GlobalTeleport : Teleport
    {
        public GlobalTeleport(Vector2 initialPosition, string filename, AssetManager assetManager, Player player) : base(initialPosition, filename, assetManager, player)
        {
            InRange = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            UpdatePlayingAnimation(animation_InRange);
        }


        public override void Update(GameTime gameTime)
        {


            if (!player.playerStateManager.teleportState.Active)
            {
                if (player.flagTeleportButtonPressed)
                {
                    player.playerStateManager.DeactivatePlayerStates();
                    player.playerStateManager.teleportState.Activate();
                    player.playerStateManager.teleportState.portal = this;
                    player.playerStateManager.teleportState.isTeleportGlobal = true;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



    }
}
