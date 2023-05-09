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

        // If this hitbox corresponds to a terrain element then we will detect collisions against it only if isActive is set to TRUE
        // In certain situations we may want to change the value of isActive - e.g. to implement one-way platforms we want isActive to be FALSE whenever the player is underneath the platform and TRUE otherwise.
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

        public HitboxRectangle()
        {
            
        }

        // Create a hitbox which consists of a single rectangle. 
        public HitboxRectangle(int x, int y, int width, int height)
        {
            rectangle = new Rectangle(x, y, width, height);
        }


    }
}
