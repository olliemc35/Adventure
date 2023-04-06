using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Camera
    {
        public Matrix Transform { get; set; }

        public void UpdateTransform(GameScreen screen, Player player)
        {

            if (screen.cameraBehaviourType1)
            {
                Transform = Matrix.Identity;
                return;
            }
            else if (screen.cameraBehaviourType2)
            {
                //var position = Matrix.CreateTranslation(-player.spritePosition.X, -player.spritePosition.Y, 0);
                //Debug.WriteLine("here");
                //var offset = Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);

                int x = DistanceToNearestInteger(player.position.X);

                var position1 = Matrix.CreateTranslation(-x - player.idleHitbox.rectangle.Width / 2, 0, 0);
                var position2 = Matrix.CreateTranslation(-screen.ScreenWidth + References.game.ScreenWidth / 2, 0, 0);
                var offset = Matrix.CreateTranslation(References.game.ScreenWidth / 2, 0, 0);


                if (x + player.idleHitbox.rectangle.Width / 2 >= References.game.ScreenWidth / 2 && x + player.idleHitbox.rectangle.Width / 2 <= screen.ScreenWidth - References.game.ScreenWidth / 2)
                {
                    Transform = position1 * offset;
                }
                else if (x + player.idleHitbox.rectangle.Width / 2 >= screen.ScreenWidth - References.game.ScreenWidth / 2)
                {
                    Transform = position2 * offset;
                }
                else
                {
                    Transform = Matrix.Identity;
                }

            }
            else if (screen.cameraBehaviourType3)
            {
                //var position = Matrix.CreateTranslation(-player.spritePosition.X, -player.spritePosition.Y, 0);

                //var offset = Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);
                var scale = Matrix.CreateScale(2);
                var offset = Matrix.CreateTranslation(0, -References.game.ScreenHeight, 0);
                Transform = scale * offset;

            }
            else if (screen.cameraBehaviourType4)
            {
                var position1 = Matrix.CreateTranslation(-player.position.X - player.idleHitbox.rectangle.Width / 2, -player.position.Y - player.idleHitbox.rectangle.Height / 2, 0);
                var offset = Matrix.CreateTranslation(References.game.ScreenWidth / 2 - 120, References.game.ScreenHeight / 2 - 30, 0);


                var scale = Matrix.CreateScale(2);

                Transform = position1 * offset * scale;

            }
            else if (screen.cameraBehaviourType5)
            {
                // Not right - want the camera to track the player until lands on noteShip
                if (References.player.position.X < screen.screenNoteShip.position.X)
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
