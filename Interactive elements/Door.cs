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


        public Door(Vector2 initialPosition, string filename, int x, int y, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player) : base(initialPosition, filename, assetManager)
        {
            ScreenNumberToMoveTo = x;
            DoorNumberToMoveTo = y;
            CollisionObject = true;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox) && inputManager.OnKeyUp(Keys.Up))
            {
                screenManager.activeScreen.ChangeScreenFlag = true;
                screenManager.PreviousScreenNumber = screenManager.activeScreen.screenNumber;
                screenManager.ScreenNumber = ScreenNumberToMoveTo;
                screenManager.DoorNumberToMoveTo = DoorNumberToMoveTo;
            }



        }


    }
}
