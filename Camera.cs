using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Camera
    {
        public Matrix Transform { get; set; }

        // This integer will determine the kind of behaviour of the camera
        public int behaviourType;

        // 0: fixed to the screen
        // 1: centered on the player 


        public Camera(int i)
        {
            behaviourType = i;
        }

        public void UpdateTransform(GameScreen screen, Player player)
        {

            if (behaviourType == 0)
            {
                Transform = Matrix.Identity;
                return;
            }
            else if (behaviourType == 1)
            {
                // Here we would like the camera to be centered on the player and move (smoothly) when the player passes the midpoint of the screen
                // We are working with two different sizes - the actual size of the screen and the resolution we are rendering it at 
                // E.g. we may have a screen of size 20 x 12 (tiles) but rendering it at a resolution 10 x 6 (tiles).
                // We do this so that we do not lose out on image quality. If instead we made the camera scale (i.e. zoom in x2) then we would lose image quality.

                // The way we calculate this is to focus the camera on the center of the player, and then calculate the desired X and Y offsets.
                // These are easy to see and depend on where the player is on screen.
                // We can do the X and Y translations completely separately from one another.
                // Finally note that Matrix.CreateTranslation(x,y,0) means move -x in the X direction (i.e. to the left) and -y in the Y direction (which in the case means to the bottom if y>0).
                // (This is unfortunately extremely confusing and must be to do with the way XNA/MonoGame draws to the screen.)

                // Set our camera centers
                int cameraCenterX = DistanceToNearestInteger(player.position.X) + player.idleHitbox.rectangle.Width / 2;
                int cameraCenterY = DistanceToNearestInteger(player.position.Y) + player.idleHitbox.rectangle.Height / 2;
 
                Transform = Matrix.Identity;

                // DO X AND Y SEPARATELY 

                if (cameraCenterX <= screen.renderScreenWidth / 2)
                {
                    // do nothing
                }
                else if (cameraCenterX > screen.renderScreenWidth / 2 && cameraCenterX <= screen.actualScreenWidth - screen.renderScreenWidth / 2)
                {
                    Transform *= Matrix.CreateTranslation(-cameraCenterX + screen.renderScreenWidth / 2, 0, 0);
                }
                else if (cameraCenterX > screen.actualScreenWidth - screen.renderScreenWidth / 2)
                {
                    Transform *= Matrix.CreateTranslation(-screen.actualScreenWidth + screen.renderScreenWidth, 0, 0);
                }

                if (cameraCenterY >= screen.actualScreenHeight / 2 + screen.renderScreenHeight / 2)
                {
                    Transform *= Matrix.CreateTranslation(0, -screen.actualScreenHeight / 2, 0);
                }
                else if (cameraCenterY >= screen.renderScreenHeight / 2 && cameraCenterY <= screen.actualScreenHeight / 2 + screen.renderScreenHeight / 2)
                {
                    Transform *= Matrix.CreateTranslation(0, -cameraCenterY + screen.renderScreenHeight / 2, 0);
                }
                else if (cameraCenterY < screen.renderScreenHeight / 2)
                {
                    // do nothing
                }


            }
            else if (behaviourType == 2)
            {
                ////var position = Matrix.CreateTranslation(-player.spritePosition.X, -player.spritePosition.Y, 0);

                ////var offset = Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);
                //var scale = Matrix.CreateScale(2);
                //Transform = scale;
                ////var offset = Matrix.CreateTranslation(0, -References.game.ScreenHeight, 0);
                ////Transform = scale * offset;
                ///
                // Set our camera centers
                int cameraCenterX = DistanceToNearestInteger(player.position.X) + player.idleHitbox.rectangle.Width / 2;

                Transform = Matrix.Identity;

                // DO X AND Y SEPARATELY 

                if (cameraCenterX <= screen.renderScreenWidth / 2)
                {
                    // do nothing
                }
                else if (cameraCenterX > screen.renderScreenWidth / 2 && cameraCenterX <= screen.actualScreenWidth - screen.renderScreenWidth / 2)
                {
                    Transform *= Matrix.CreateTranslation(-cameraCenterX + screen.renderScreenWidth / 2, 0, 0);
                }
                else if (cameraCenterX > screen.actualScreenWidth - screen.renderScreenWidth / 2)
                {
                    Transform *= Matrix.CreateTranslation(-screen.actualScreenWidth + screen.renderScreenWidth, 0, 0);
                }

                

            }
            else if (behaviourType == 3)
            {
                var position1 = Matrix.CreateTranslation(-player.position.X - player.idleHitbox.rectangle.Width / 2, -player.position.Y - player.idleHitbox.rectangle.Height / 2, 0);
                var offset = Matrix.CreateTranslation(References.game.ScreenWidth / 2 - 120, References.game.ScreenHeight / 2 - 30, 0);


                var scale = Matrix.CreateScale(2);

                Transform = position1 * offset * scale;

            }
            else if (behaviourType == 4)
            {
                // Not right - want the camera to track the player until lands on noteShip
                if (player.position.X < screen.screenNoteShip.position.X)
                {
                    int x1 = DistanceToNearestInteger(player.position.X);

                    var position11 = Matrix.CreateTranslation(-x1 - player.idleHitbox.rectangle.Width / 2, 0, 0);
                    var position21 = Matrix.CreateTranslation(-screen.ScreenWidth + References.game.ScreenWidth / 2, 0, 0);
                    var offset1 = Matrix.CreateTranslation(References.game.ScreenWidth / 2, 0, 0);


                    if (x1 + player.idleHitbox.rectangle.Width / 2 >= References.game.ScreenWidth / 2 && x1 + player.idleHitbox.rectangle.Width / 2 <= screen.ScreenWidth - References.game.ScreenWidth / 2)
                    {
                        Transform = position11 * offset1;
                    }
                    else if (x1 + player.idleHitbox.rectangle.Width / 2 >= screen.ScreenWidth - References.game.ScreenWidth / 2)
                    {
                        Transform = position21 * offset1;
                    }
                    else
                    {
                        Transform = Matrix.Identity;
                    }
                    return;
                }

                int x = DistanceToNearestInteger(screen.screenNoteShip.position.X + screen.screenNoteShip.idleHitbox.rectangle.Width);

                var position1 = Matrix.CreateTranslation(-x - screen.screenNoteShip.idleHitbox.rectangle.Width / 2, 0, 0);
                var position2 = Matrix.CreateTranslation(-screen.ScreenWidth + References.game.ScreenWidth / 2, 0, 0);
                var offset = Matrix.CreateTranslation(References.game.ScreenWidth / 2, 0, 0);


                if (x + screen.screenNoteShip.idleHitbox.rectangle.Width / 2 >= References.game.ScreenWidth / 2 && x + screen.screenNoteShip.idleHitbox.rectangle.Width / 2 <= screen.ScreenWidth - References.game.ScreenWidth / 2)
                {
                    Transform = position1 * offset;
                }
                else if (x + screen.screenNoteShip.idleHitbox.rectangle.Width / 2 >= screen.ScreenWidth - References.game.ScreenWidth / 2)
                {
                    Transform = position2 * offset;
                }
                else
                {
                    Transform = Matrix.Identity;
                }
            }
        }




        // In XNA matrices are written "row-first," i.e. they are the conjugate to what we usually think of as matrices. So be wary of trying to write down math...
        // Matrix product * corresponds to composition of transformations.
        // Matrix.CreateTranslation(X,Y,Z) : X is the value we translate by on x-axis, sim. for Y and Z.

        // So we understand as follows: take e.g. the first case of cameraBehaviourType2, Transform = position2 * offset corresponds to first 
        // translating by -player.spritePosition.X - player.idleHitbox.rectangle.Width/2 follows by a translation by References.game.ScreenWidth / 2.
        // Thus this is just one big translation. (Of course, translations are commutative.) A piece of paper and a few examples shows why this indeed works...

        // The fancy SpriteBatch call, incorporating this transformation, is basically telling XNA to apply this transformation to the screen we see on the monitor.





        public int DistanceToNearestInteger(float x)
        {
            if (Math.Ceiling(x) - x <= 0.5)
            {
                return (int)Math.Ceiling(x);
            }
            else
            {
                return (int)Math.Floor(x);
            }

        }

    }
}
