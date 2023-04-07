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

        public HookPoint(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            radius = 8 * 10;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            animation_InRange = spriteSheet.CreateAnimatedSprite("InRange");
        }

        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(References.player.position, position) <= radius)
            {
                InRange = true;
                UpdatePlayingAnimation(animation_InRange);
            }
            else
            {
                InRange = false;
                UpdatePlayingAnimation(animation_Idle);
            }

            if (!References.player.playerStateManager.swingingState.Active && InRange && References.player.flagHookButtonPressed)
            {
                References.player.playerStateManager.DeactivatePlayerStates();
                References.player.playerStateManager.swingingState.Activate();
                References.player.playerStateManager.swingingState.hookPoint = this;
            }

            base.Update(gameTime);
        }
    }
}
