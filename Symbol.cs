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
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class Symbol : AnimationSprite
    {
        public bool TurnedOn = false;
        public AnimatedSprite animatedSprite_Interacted;

        public Symbol(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
        }
        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            animatedSprite_Interacted = spriteSheet.CreateAnimatedSprite("Interacted");
            animatedSpriteAndTag.Add("Interacted", animatedSprite_Interacted);

        }

        public override void Update(GameTime gameTime)
        {
            ManageAnimations();
            base.Update(gameTime);
        }

        public override void ManageAnimations()
        {

            if (TurnedOn)
            {
                nameOfCurrentAnimationSprite = "Interacted";

                //animatedSprite_Idle.Play("Interacted");
                //currentFrame = frameAndTag["Interacted"].From;
                //tagOfCurrentFrame = "Interacted";
            }
            else
            {
                nameOfCurrentAnimationSprite = "Idle";

                //animatedSprite_Idle.Play("Idle");
                //currentFrame = frameAndTag["Idle"].From;
                //tagOfCurrentFrame = "Idle";
            }



        }
    }
}
