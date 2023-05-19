using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Adventure
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public ScreenManager screenManager;
        public SoundManager soundManager;
        public ColliderManager colliderManager;
        public AssetManager assetManager;
        public InputManager inputManager;
        public int ScreenHeight;
        public int ScreenWidth;

        public RenderTarget2D renderTarget; // We create a renderTarget - essentially a rectangle on to which we draw all of our sprites
        public List<RenderTarget2D> renderTargets;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            assetManager = new AssetManager();
            soundManager = new SoundManager();
            colliderManager = new ColliderManager();
            inputManager = new InputManager();


            References.game = this;

        }

        protected override void Initialize()
        {
            // Work in a native/virtual resolution of 640 x 360
            // Tile sizes will be 16 x 16 and so we fit 40 tiles horizontally and 23 (really 22.5) tiles vertically on the screen
            ScreenWidth = 640;
            ScreenHeight = 360;

            // Lock the game to run at 60 FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerMillisecond * (1000 / (double)60)));

            // Output to 720p
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.SynchronizeWithVerticalRetrace = true; // This is vsync
            graphics.PreferMultiSampling = true; // This is anti-aliasing
            graphics.ApplyChanges();


            renderTargets = new List<RenderTarget2D>()
            {
                new RenderTarget2D(GraphicsDevice, 160, 90, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24),
                new RenderTarget2D(GraphicsDevice, 320, 180, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24),
                new RenderTarget2D(GraphicsDevice, 640, 360, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24),
                new RenderTarget2D(GraphicsDevice, 1280, 720, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24)
            };

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            assetManager.LoadContent(Content, GraphicsDevice);
            soundManager.LoadContent(Content);
            screenManager = new ScreenManager(spriteBatch, assetManager, colliderManager, inputManager, soundManager);
            screenManager.LoadScreens(Content, GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // Only update every 60FPS
            if (Math.Abs(gameTime.ElapsedGameTime.TotalMilliseconds - TargetElapsedTime.TotalMilliseconds) < 1)
            {
                inputManager.Update();
                screenManager.Update(gameTime);
                base.Update(gameTime);
            }
            else
            {
                // Otherwise stop everything for 1 frame (so everything can catch up).
                System.Threading.Thread.Sleep(1);
            }


        }



        // SEE: https://community.monogame.net/t/transforming-objects-for-a-rendertarget2d/13959


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            screenManager.Draw(gameTime, graphics, renderTargets);

            base.Draw(gameTime);
        }



    }
}