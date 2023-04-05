using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Content.Processors;

namespace Adventure
{
    public class Sprite : GameObject
    {
        public AsepriteFile asepriteFile;
        public TextureAtlas textureAtlas;

        public Texture2D spriteTexture;
        public Vector2 spritePosition;
        public Vector2 previousSpritePosition;
        public string spriteFilename;


        public bool climable = false;

        // Every sprite will have a list of hitboxes (nearly all sprites will only have a single one)
        public List<HitboxRectangle> spriteHitboxes = new List<HitboxRectangle>();

        // Every sprite will have a "base" hitbox which we call idleHitbox
        public HitboxRectangle idleHitbox;
        public Texture2D spriteHitboxTexture;


        // We set CollisionSprite to true if we want the player / other objects to collide with this sprite
        public bool CollisionSprite = false;


        // The drawHitboxes bool is for DEBUGGING purposes
        // At some point we may want to draw the hitboxes to the screen to see exactly what is going on 
        public bool drawHitboxes = false;

        // The highlight bool is for DEBUGGING purposes
        // E.g. in collision detection code we can test for if (sprite.Highlight) to isolate that particular sprite in the code
        public bool Highlight = false;


        public Sprite()
        {
        }

        public Sprite(Vector2 position)
        {
            spritePosition = position;
            previousSpritePosition = position;
        }


        public Sprite(Vector2 position, string filename)
        {
            spritePosition = position;
            spriteFilename = filename;
            previousSpritePosition = position;
        }

        public Sprite(string filename)
        {
            spriteFilename = filename;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            //References.counter += 1;
            //Debug.WriteLine(References.counter);
            // Load in the relevant aseprite file
            asepriteFile = contentManager.Load<AsepriteFile>(spriteFilename);
            //Sprite sprite = SpriteProcessor.Process(graphicsDevice, asepriteFile, aseFrameIndex: 0);
            textureAtlas = TextureAtlasProcessor.Process(graphicsDevice, asepriteFile);
            //spriteTexture = SpriteProcessor.Process(graphicsDevice, asepriteFile, aseFrameIndex: 0).TextureRegion.Texture;
            spriteTexture = textureAtlas.GetRegion(0).Texture;


            // Create the idleHitbox
            idleHitbox = new HitboxRectangle((int)spritePosition.X, (int)spritePosition.Y, spriteTexture.Width, spriteTexture.Height);
            spriteHitboxes.Add(idleHitbox);

            spriteHitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            spriteHitboxTexture.SetData(new Color[] { Color.White });

        }

        public override void Update(GameTime gametime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes && idleHitbox.isActive)
            {
                spriteBatch.Draw(spriteHitboxTexture, idleHitbox.rectangle, Color.Red);
            }
            if (Enabled)
            {
                spriteBatch.Draw(spriteTexture, spritePosition, Color.White);
            }
        }



        public void TurnOffAllHitboxes()
        {
            foreach (HitboxRectangle hitbox in spriteHitboxes)
            {
                hitbox.isActive = false;
            }
        }

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
