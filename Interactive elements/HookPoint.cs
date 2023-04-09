using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class HookPoint : AnimatedGameObject
    {
        public float radius;
        public bool InRange = false;

        public AnimatedSprite animation_InRange;

        public HookPoint(Vector2 initialPosition, string filename, AssetManager assetManager, Player player) : base(initialPosition, filename, assetManager)
        {
            this.player = player;
            radius = 8 * 10;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            animation_InRange = spriteSheet.CreateAnimatedSprite("InRange");
        }

        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(player.position, position) <= radius)
            {
                InRange = true;
                UpdatePlayingAnimation(animation_InRange);
            }
            else
            {
                InRange = false;
                UpdatePlayingAnimation(animation_Idle);
            }

            if (!player.playerStateManager.swingingState.Active && InRange && player.flagHookButtonPressed)
            {
                player.playerStateManager.DeactivatePlayerStates();
                player.playerStateManager.swingingState.Activate();
                player.playerStateManager.swingingState.hookPoint = this;
            }

            base.Update(gameTime);
        }
    }
}
