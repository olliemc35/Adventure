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
    public class Teleport : AnimatedGameObject
    {
        public float radius;
        public float speedBoost;
        public bool InRange;

        public AnimatedSprite animation_InRange;

        public Teleport(Vector2 initialPosition, string filename, AssetManager assetManager, Player player) : base(initialPosition, filename, assetManager, null, null, null, player)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            animation_InRange = spriteSheet.CreateAnimatedSprite("InRange");
        }



    }
}
