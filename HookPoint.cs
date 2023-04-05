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
    public class HookPoint : AnimationSprite
    {
        public float radius;
        public bool InRange = false;

        public AnimatedSprite animatedSprite_InRange;

        public HookPoint(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            radius = 8 * 10;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            animatedSprite_InRange = spriteSheet.CreateAnimatedSprite("InRange");
            animatedSpriteAndTag.Add("InRange", animatedSprite_InRange);

        }

        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(References.player.spritePosition, spritePosition) <= radius)
            {
                InRange = true;
                nameOfCurrentAnimationSprite = "InRange";

                //animatedSprite_Idle.Play("InRange");
                //currentFrame = frameAndTag["InRange"].From;
                //tagOfCurrentFrame = "InRange";
            }
            else
            {
                InRange = false;
                nameOfCurrentAnimationSprite = "Idle";
                //animatedSprite_Idle.Play("Idle");
                //currentFrame = frameAndTag["Idle"].From;
                //tagOfCurrentFrame = "Idle";
            }

            base.Update(gameTime);
        }
    }
}
