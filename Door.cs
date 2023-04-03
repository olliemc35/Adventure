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
    public class Door : Sprite
    {
        public int ScreenNumberToMoveTo = 0;
        public int DoorNumberToMoveTo = 0;
        public Door(Vector2 initialPosition) : base(initialPosition)
        {
            CollisionSprite = true;
        }

        public Door(Vector2 initialPosition, int x) : base(initialPosition)
        {
            ScreenNumberToMoveTo = x;
            CollisionSprite = true;
        }

        public Door(Vector2 initialPosition, string filename, int x, int y) : base(initialPosition, filename)
        {
            ScreenNumberToMoveTo = x;
            DoorNumberToMoveTo = y;
            CollisionSprite = true;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
