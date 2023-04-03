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
    public class Symbol : AnimationSprite
    {
        public bool TurnedOn = false;
        public Symbol(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
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
                animatedSprite.Play("Interacted");
                currentFrame = frameAndTag["Interacted"].From;
                tagOfCurrentFrame = "Interacted";
            }
            else
            {
                animatedSprite.Play("Idle");
                currentFrame = frameAndTag["Idle"].From;
                tagOfCurrentFrame = "Idle";
            }



        }
    }
}
