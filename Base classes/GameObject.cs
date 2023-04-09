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

        // Some GameObjects we only need to draw every frame - we do not need to update every frame. E.g. background tiles, spikes etc.
        public bool DrawOnly = false;


        // // The following references may be passed in when created a new GameObject instance:

        // Some GameObjects will need to detect collisions - e.g. player, spikes, doors etc.
        public ColliderManager colliderManager;

        // Some GameObjects will need to detect keyboard input - e.g. player, doors etc.
        public InputManager inputManager;

        // Some GameObjects will need to access information about the screen we are on - e.g. doors, bombs (to look for notes) etc.
        public GameScreen gameScreen;

        // Some GameObjects will need a reference to the player - e.g. doors, spikes etc.
        public Player player;

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
