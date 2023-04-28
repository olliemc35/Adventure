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
    public class Ivy : AnimatedGameObject
    {
        public List<AnimatedGameObject> ivyTiles = new List<AnimatedGameObject>();

        public Ivy(Vector2 position, string filename, AssetManager assetManager, ColliderManager colliderManager, Player player, int number) : base (position, filename, assetManager, colliderManager, null, null, player)
        {
            for (int i = 0; i < number; i++)
            {
                ivyTiles.Add(new AnimatedGameObject(new Vector2(position.X, position.Y + 8 * i), filename, assetManager, colliderManager, null, null, player));
            }

            Climable = true;
            LoadLast = true;
            //CollisionObject = true;

           
        }

        public override void LoadContent()
        {
            
            foreach (AnimatedGameObject ivyTile in ivyTiles) { ivyTile.LoadContent(); }

            // Create the idleHitbox
            idleHitbox = new HitboxRectangle((int)position.X, (int)position.Y, ivyTiles[0].idleHitbox.rectangle.Width, 8 * ivyTiles.Count);
            idleHitbox.texture = assetManager.hitboxTexture;
            //hitboxes.Add(idleHitbox);

        }

        public override void Update(GameTime gametime)
        {
            UpdateClimable();
            foreach (AnimatedGameObject ivyTile in ivyTiles) { ivyTile.Update(gametime); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedGameObject ivyTile in ivyTiles)
            {
                ivyTile.Draw(spriteBatch);
            }

            //spriteBatch.Draw(idleHitbox.texture, idleHitbox.rectangle, Color.Red);
            //spriteBatch.Draw(ivyTiles[0].idleHitbox.texture, ivyTiles[0].idleHitbox.rectangle, Color.Red);

        }


        public override void MoveManually(Vector2 moveVector)
        {
            base.MoveManually(moveVector);

            foreach (AnimatedGameObject ivyTile in ivyTiles)
            {
                ivyTile.MoveManually(moveVector);
            }

        }


        public override void AdjustHorizontally(ref List<int> ints)
        {
            position.X += ints[0];

            foreach (AnimatedGameObject ivyTile in ivyTiles)
            {
                ivyTile.position.X += ints[0];
            }

            ints.RemoveAt(0);
        }
        public override void AdjustVertically(ref List<int> ints)
        {
            position.Y += ints[0];

            foreach (AnimatedGameObject ivyTile in ivyTiles)
            {
                ivyTile.position.Y += ints[0];
            }

            ints.RemoveAt(0);
        }

    }
}
