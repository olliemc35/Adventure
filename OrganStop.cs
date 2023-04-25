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
        public AnimatedGameObject organBottom = new AnimatedGameObject();
        public MovingPlatform_AB platform;
        public HitboxRectangle tubeHitbox = new HitboxRectangle();
        int distanceFromBase;

        public OrganStop(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player, int distanceFromBase) 
        {
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.player = player;
            this.distanceFromBase = distanceFromBase;
            platform = new MovingPlatform_AB(initialPosition, endPoint, filename, speed, assetManager, colliderManager, player);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            platform.LoadContent();

            int Height = Math.Abs((int)platform.positions[1].Y - (int)platform.positions[0].Y) / 8 + distanceFromBase / 8;


            for (int i = 1; i <= Height; i++)
            {
                AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + 8 + 8 * i), "OrganStopMiddle", assetManager);
                animatedGameObject.LoadContent();
                organTiles.Add(animatedGameObject);
                platform.attachedGameObjects.Add(animatedGameObject);
            }

            organBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + distanceFromBase), "OrganStopBottom", assetManager);
            organBottom.LoadContent();


            // The idleHitbox corresponds to the platform (the top).
            // We also need a hitbox for the tube - here we create the tubeHitbox
            tubeHitbox = new HitboxRectangle((int)organTiles[0].position.X, (int)organTiles[0].position.Y, organTiles[0].idleHitbox.rectangle.Width, 8 * (Height - 1));
            tubeHitbox.texture = assetManager.hitboxTexture;
            tubeHitbox.isActive = true;

            //platform.idleHitbox.rectangle.Height = 8 * Height;

        }
        public override void Update(GameTime gameTime)
        {
            platform.Update(gameTime);

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                animatedGameObject.Update(gameTime);
            }

            tubeHitbox.rectangle.X = (int)organTiles[0].position.X;
            tubeHitbox.rectangle.Y = (int)organTiles[0].position.Y;


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            platform.Draw(spriteBatch);

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                if (animatedGameObject.position.Y <= platform.positions[0].Y + distanceFromBase)
                {
                    animatedGameObject.Draw(spriteBatch);

                }
            }

            organBottom.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }


        public override void AddAttachedGameObject(GameObject gameObject)
        {
            platform.AddAttachedGameObject(gameObject);
        }

        public override void HandleNoteTrigger()
        {
            platform.movePlatform = true;
        }
    }
}
