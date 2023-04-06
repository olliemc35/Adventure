using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class HitboxRectangle
    {
        public Rectangle rectangle;

        public Texture2D texture;

        public bool isActive;

        // Use these bools to check whether I have collided with a particular sprite during the collision detectiong step
        public bool CollidedOnTop = false;
        public bool CollidedOnBottom = false;
        public bool CollidedOnRight = false;
        public bool CollidedOnLeft = false;

        public bool GlobalCollidedOnTop = false;
        public bool GlobalCollidedOnBottom = false;
        public bool GlobalCollidedOnRight = false;
        public bool GlobalCollidedOnLeft = false;

        public bool Highlight = false;

        public bool ResetColliderBools = false;
        // Hitboxes will always be positioned by hand w.r.t the original sprite. This means that we will need to offset from 
        // the spritePosition by a certain amount to guarantee everything is placed where it should be.

        public bool ghostTurnedOn = false;

        public bool invincible = false;

        public int offsetX = 0;
        public int offsetY = 0;



        // Create a hitbox which consists of a single rectangle. 
        public HitboxRectangle(int x, int y, int width, int height)
        {
            rectangle = new Rectangle(x, y, width, height);
        }

        // Create a hitbox which consists of a list of rectangles

    }
}
