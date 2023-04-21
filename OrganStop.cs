using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using System.Diagnostics;
using System.Net;

namespace Adventure
{
    public class OrganStop : GameObject
    {
        public List<AnimatedGameObject> organTiles = new List<AnimatedGameObject>();
        public MovingPlatform_AB platform;
        public OrganStop(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player)
        {
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.player = player;
            platform = new MovingPlatform_AB(initialPosition, endPoint, filename, speed, assetManager, colliderManager, player);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            platform.LoadContent();

            int Height = Math.Abs(((int)platform.positions[1].Y - (int)platform.positions[0].Y)) / 8;


            for (int i  = 0; i <= Height; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8 * j, platform.positions[0].Y + 8 * i), "OrganStopMiddle", assetManager);
                    animatedGameObject.LoadContent();
                    organTiles.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }
            }

            platform.idleHitbox.rectangle.Height = 8 * Height;

        }

        public override void Update(GameTime gameTime)
        {
            platform.Update(gameTime);

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                animatedGameObject.Update(gameTime);
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            platform.Draw(spriteBatch);

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                if (animatedGameObject.position.Y <= platform.positions[0].Y)
                {
                    animatedGameObject.Draw(spriteBatch);

                }
            }
            base.Draw(spriteBatch);
        }



    }
}
