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
    public class Teleport : AnimationSprite
    {
        public float radius;
        public float speedBoost;
        public bool InRange;

        public Teleport(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {

        }





    }
}
