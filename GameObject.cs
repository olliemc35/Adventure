using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class GameObject
    {
        // The Enabled bool determines whether a gameObject is updated or not
        // This will be managed by ScreenManager - only gameObjects on the current screen will have Enabled set to true - otherwise false
        public bool Enabled = true;

        public GameObject()
        {
        }

        public virtual void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
        }

        public virtual void Update(GameTime gametime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

    }
}
