using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Gun : AnimatedGameObject
    {
        //public Player player;

        //public ColliderManager spriteCollider = new ColliderManager();

        //public MouseState mouseState;
        //public MouseState oldMouseState;
        //public Vector2 mousePosition = new Vector2();
        //public bool flagLeftMouseClick = false;

        //public Vector2 gunEndPoint = new Vector2();
        //public Vector2 gunStartPoint = new Vector2();

        //public List<Rectangle> aimLine = new List<Rectangle>();

        //public List<Bullet> bullets = new List<Bullet>();

        //public Gun() : base()
        //{

        //}
        //public Gun(Vector2 initialPosition) : base(initialPosition)
        //{

        //}

        //public Gun(Player player) : base()
        //{
        //    this.player = player;
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    if (player.gunEquipped)
        //    {
        //        mouseState = Mouse.GetState();
        //        mousePosition.X = ((float)2 / 5) * mouseState.X;
        //        mousePosition.Y = ((float)3 / 8) * mouseState.Y;
        //        flagLeftMouseClick = mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        //        oldMouseState = mouseState;

        //        FindLine();

        //        if (flagLeftMouseClick)
        //        {
        //            player.fire = true;
        //            Fire();
        //        }

        //        if (bullets.Count() > 0)
        //        {
        //            for (int i = 0; i < bullets.Count(); i++)
        //            {
        //                bullets[i].Update(gameTime);
        //            }

        //            for (int i = bullets.Count() - 1; i >= 0; i--)
        //            {
        //                if (bullets[i].remove)
        //                {
        //                    bullets.RemoveAt(i);
        //                }
        //            }
        //        }



        //        base.Update(gameTime);
        //    }
        //}

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (Rectangle rectangle in aimLine)
        //    {
        //        spriteBatch.Draw(player.idleHitbox.texture, rectangle, Color.YellowGreen);
        //    }

        //    if (bullets.Count() > 0)
        //    {
        //        for (int i = 0; i < bullets.Count(); i++)
        //        {
        //            bullets[i].Draw(spriteBatch);
        //        }
        //    }
        //}

        //public void FindLine()
        //{


        //    gunStartPoint = player.position;
        //    gunStartPoint.X += player.idleHitbox.rectangle.Width - 1;
        //    gunStartPoint.Y += 0.5f * player.idleHitbox.rectangle.Height + 1;

        //    // We now find the gunEndpoint
        //    // We want this to be the end of the screen in whichever direction the mouse is pointing 
        //    // We will then stop it short if there is a collision with any object 

        //    Vector2 displacement = mousePosition - gunStartPoint;
        //    displacement.Normalize();
        //    float maxSize = (float)Math.Sqrt(References.game.ScreenHeight * References.game.ScreenHeight + References.game.ScreenWidth * References.game.ScreenWidth);
        //    displacement *= maxSize;
        //    gunEndPoint.X = gunStartPoint.X + displacement.X;
        //    gunEndPoint.Y = gunStartPoint.Y + displacement.Y;

        //    // Clear it every frame
        //    aimLine.Clear();

        //    // Then add in the correct boxes using a method in spriteCollider
        //    spriteCollider.CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxesAndUpdateListOfRectangles(gunStartPoint, gunEndPoint, References.activeScreen.hitboxesForAimLine, aimLine);

        //}

        //public void Fire()
        //{
        //    //Bullet bullet = new Bullet(gunStartPoint, "Bullet", aimLine);
        //    Bullet bullet = new Bullet(gunStartPoint, "Bullet", gunEndPoint);
        //    bullet.LoadContent(References.content, References.graphicsDevice);
        //    bullets.Add(bullet);
        //}

    }
}
