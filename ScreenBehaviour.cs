using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class ScreenBehaviour
    {
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public bool DrawMe { get; set; } // This is true for menus that we want drawn on the screen

        protected SpriteBatch spriteBatch;
        protected SpriteFont spriteFont;

        public ScreenBehaviour(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }










    }
}
