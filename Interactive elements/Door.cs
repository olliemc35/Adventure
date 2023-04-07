using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Adventure
{
    public class Door : AnimatedGameObject
    {
        public int ScreenNumberToMoveTo;
        public int DoorNumberToMoveTo;
     

        public Door(Vector2 initialPosition, string filename, int x, int y) : base(initialPosition, filename)
        {
            ScreenNumberToMoveTo = x;
            DoorNumberToMoveTo = y;
            CollisionObject = true;
        }
        public Door(Vector2 initialPosition, string filename, int x, int y, ColliderManager colliderManager, InputManager inputManager) : base(initialPosition, filename)
        {
            ScreenNumberToMoveTo = x;
            DoorNumberToMoveTo = y;
            CollisionObject = true;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox) && inputManager.OnKeyUp(Keys.Up))
            {
                References.activeScreen.ChangeScreen = true;
                References.PreviousScreenNumber = References.activeScreen.screenNumber;
                References.ScreenNumber = ScreenNumberToMoveTo;
                References.DoorNumberToMoveTo = DoorNumberToMoveTo;
            }



        }


    }
}
