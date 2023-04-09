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
    public class Symbol : AnimatedGameObject
    {
        public bool TurnedOn = false;
        public AnimatedSprite animation_Interacted;

        public Symbol(Vector2 initialPosition, string filename, AssetManager assetManager) : base(initialPosition, filename, assetManager)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();
            animation_Interacted = spriteSheet.CreateAnimatedSprite("Interacted");

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
                UpdatePlayingAnimation(animation_Interacted);
            }
            else
            {
                UpdatePlayingAnimation(animation_Idle);
            }



        }
    }
}
