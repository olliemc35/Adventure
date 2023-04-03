using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;


namespace Adventure
{
    public class Pivot : Sprite
    {
        public bool TopRight = false;
        public bool TopLeft = false;
        public bool BottomRight = false;
        public bool BottomLeft = false;

        public bool isPivotActive = false;

        public bool aboutToBeRemoved = false;


        public Pivot() : base()
        {

        }


        public Pivot(Vector2 position) : base(position)
        {

        }


        public void ResetOrientationBools()
        {
            TopRight = false;
            TopLeft = false;
            BottomRight = false;
            BottomLeft = false;

        }
    }
}
