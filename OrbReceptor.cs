using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class OrbReceptor : AnimatedGameObject
    {
        public AnimatedSprite animation_Hit;

        public OrbReceptor(Vector2 position, string filename) : base(position, filename)
        {

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            animation_Hit = spriteSheet.CreateAnimatedSprite("Hit");

            animation_Hit.OnAnimationEnd = (hello) =>
            {
                UpdatePlayingAnimation(animation_Idle);
            };

        }

    }
}
