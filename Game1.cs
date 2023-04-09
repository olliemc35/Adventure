using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // We wish to render pixels at 320 x 180 like in Celeste. This mirrors old retro games
            // With this, each tile will be 8 x 8
            //graphics.PreferredBackBufferWidth = 320;
            //graphics.PreferredBackBufferHeight = 180;
            ////graphics.SynchronizeWithVerticalRetrace = true; // This is vsync
            ////graphics.PreferMultiSampling = true;
            //graphics.ApplyChanges();

            assetManager = new AssetManager();
            soundManager = new SoundManager();
            colliderManager = new ColliderManager();
            inputManager = new InputManager();


            References.game = this;

            //IsFixedTimeStep = false;

        }

        protected override void Initialize()
        {

            //graphics.HardwareModeSwitch = false;
            ////
            //graphics.PreferredBackBufferWidth = 320;
            //graphics.PreferredBackBufferHeight = 180;
            ////graphics.HardwareModeSwitch = true;
            //graphics.ApplyChanges();

            ScreenHeight = 180;
            ScreenWidth = 320;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerMillisecond * (1000 / (double)60))); // Lock at 60 FPS


            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            base.Initialize();
            //renderTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            assetManager.LoadContent(Content, GraphicsDevice);
            screenManager = new ScreenManager(spriteBatch, assetManager, colliderManager, inputManager);
            soundManager.LoadContent(Content);
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

            // We draw all of our sprites on to the renderTarget,
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            screenManager.Draw(gameTime);
            // We then set it to null so that we can draw back on to the screen
            graphics.GraphicsDevice.SetRenderTarget(null);

            // Draw back to the screen as a Texture2D
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }



    }
}