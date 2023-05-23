using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Gate : MovingPlatform
    {
        public bool open = false;
        public bool opened = false;

        public Gate(Vector2 initialPosition, Vector2 endPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { initialPosition, endPosition }, filename, 1, new List<int>() { 0, 1 }, assetManager, colliderManager, screenManager, player)
        {

        }


        public override void Update(GameTime gameTime)
        {

            if (open && !opened)
            {
                base.Update(gameTime);


                if (position == positions[1])
                {
                    opened = true;
                }

            }

            if (!open && opened)
            {
                base.Update(gameTime);

                if (position == positions[0])
                {
                    opened = false;
                }

            }




        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


        public override void HandleNoteTrigger()
        {
            if (open)
            {
                open = false;
            }
            else
            {
                open = true;
            }
        }

        public override void AdjustHorizontally(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X + ints[0], positions[0].Y);
            positions[1] = new Vector2(positions[1].X + ints[1], positions[1].Y);
            position.X += ints[0];


            ints.RemoveRange(0, 2);

        }
        public override void AdjustVertically(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X, positions[0].Y + ints[0]);
            positions[1] = new Vector2(positions[1].X, positions[1].Y + ints[1]);
            position.Y += ints[0];


            ints.RemoveRange(0, 2);

        }

    }
}
