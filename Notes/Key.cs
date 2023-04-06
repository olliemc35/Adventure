using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Key : MovingGameObject
    {
        public AnimatedSprite animation_Interacted;
        public Key(Vector2 position, string filename):base(position, filename) { }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            animation_Interacted = spriteSheet.CreateAnimatedSprite("Interacted");
        }


    }
}
