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
    public class Spike : Sprite
    {
        public Spike() : base()
        {

        }

        public Spike(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public Spike(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            idleHitbox.isActive = true;
        }
    }
}
