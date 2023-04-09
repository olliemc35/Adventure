using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class OrbVessel : AnimatedGameObject
    {
        public int orbWindowLeft;
        public int orbWindowRight;
        public int orbEndLeft;
        public int orbEndRight;

        public List<AnimatedGameObject> listOfParts = new List<AnimatedGameObject>();



        public OrbVessel(int orbWindowLeft, int orbWindowRight, int orbEndLeft, int orbEndRight, AssetManager assetManager)
        {
            this.assetManager = assetManager;
            this.orbEndLeft = orbEndLeft;
            this.orbEndRight = orbEndRight;
            this.orbWindowLeft = orbWindowLeft;
            this.orbWindowRight = orbWindowRight;

            for (int i = 0; i < 40; i++)
            {
                if (i == 0)
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbVesselEndLeft", assetManager));
                }
                else if (i == 39)
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbVesselEndRight", assetManager));
                }
                else if (i == 4)
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbWindowLeft", assetManager));
                }
                else if (i >= 5 && i <= 8)
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbWindowMiddle", assetManager));
                }
                else if (i == 9)
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbWindowRight", assetManager));
                }
                else
                {
                    listOfParts.Add(new AnimatedGameObject(new Vector2(8 * i, 0), "OrbMiddle", assetManager));
                }
            }
        }

        public override void LoadContent()
        {
            for (int i = 0; i < 40; i++)
            {
                listOfParts[i].LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < 40; i++)
            {
                listOfParts[i].Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 40; i++)
            {
                listOfParts[i].Draw(spriteBatch);
            }
        }







    }
}
