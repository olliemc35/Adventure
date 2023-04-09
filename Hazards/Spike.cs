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
using System.Xml;

namespace Adventure
{
    public class Spike : Hazard
    {

        public Spike(Vector2 initialPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(initialPosition, filename, assetManager, colliderManager, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            idleHitbox.isActive = true;

            
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
        }
    }
}
