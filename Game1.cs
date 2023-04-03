using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Adventure
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public ScreenManager screenManager;

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
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 180;
            //graphics.SynchronizeWithVerticalRetrace = true; // This is vsync
            //graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            References.content = Content;
            References.game = this;

            //IsFixedTimeStep = false;

        }

        protected override void Initialize()
        {
            References.graphicsDevice = GraphicsDevice;
            //ScreenHeight = graphics.PreferredBackBufferHeight;
            //ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = 180;
            ScreenWidth = 320;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerMillisecond * (1000 / (double)60))); // Lock at 60 FPS


            base.Initialize();
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenManager = new ScreenManager(spriteBatch);
            screenManager.LoadScreens(Content);

        }

        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // Only update every 60FPS
            if (Math.Abs(gameTime.ElapsedGameTime.TotalMilliseconds - TargetElapsedTime.TotalMilliseconds) < 1)
            {
                screenManager.Update(gameTime);
                base.Update(gameTime);
            }
            else
            {
                // Otherwise stop everything for 1 frame (so everything can catch up).
                System.Threading.Thread.Sleep(1);
            }

        }

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