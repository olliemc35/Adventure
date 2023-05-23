using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Adventure
{
    public class ScreenManager
    {
        SpriteBatch spriteBatch;
        SpriteFont menuFont;

        public List<GameScreen> screens = new List<GameScreen>();

        public GameScreen activeScreen;
        public GameScreen previousActiveScreen;

        public ColliderManager colliderManager;
        public InputManager inputManager;
        public AssetManager assetManager;
        public SoundManager soundManager;

        public Player player;


        public int ScreenNumber = 0;
        public int PreviousScreenNumber = 0;
        public int DoorNumberToMoveTo = 0;
        public int WallNumberToMoveTo = 0;



        public ScreenManager(SpriteBatch spriteBatch, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, SoundManager soundManager)
        {
            this.spriteBatch = spriteBatch;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.assetManager = assetManager;
            this.soundManager = soundManager;

            player = new Player(new Vector2(0, 0), "hoodedoldmanv2", assetManager, colliderManager, inputManager, this);

        }


        public void LoadScreens(ContentManager content, GraphicsDevice graphicsDevice)
        {
            menuFont = content.Load<SpriteFont>("menufont");


            CreateScreens(content, graphicsDevice);

            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
                screen.Hide();
            }

            activeScreen = screens[12];
            activeScreen.Show();

            player.LoadContent();
            player.position = activeScreen.respawnPoint;

        }

        public void Update(GameTime gameTime)
        {

            activeScreen.Update(gameTime);

            // We set a flag so that the screen changes AFTER everything on screen has updated
            // (We could alternatively change ChangeScreen slightly so the Door calls it directly, but this then changes the activeScreen mid-update which actually worked but just seemed wierd...)
            if (activeScreen.ChangeScreenFlag)
            {
                activeScreen.ChangeScreenFlag = false;
                ChangeScreen();
            }
            if (activeScreen.ChangeScreenFlag_Wall)
            {
                activeScreen.ChangeScreenFlag_Wall = false;
                ChangeScreen_Wall();
            }
            //soundManager.Update(gameTime);


        }

        public void ChangeScreen()
        {
            activeScreen = screens[ScreenNumber - 1];
            DisableShowScreenTransition(screens[PreviousScreenNumber - 1], screens[ScreenNumber - 1]);
            player.position.X = activeScreen.screenDoors[DoorNumberToMoveTo - 1].position.X;
            player.position.Y = activeScreen.screenDoors[DoorNumberToMoveTo - 1].position.Y;
            player.idleHitbox.rectangle.X = (int)player.position.X + player.idleHitbox.offsetX;
            player.idleHitbox.rectangle.Y = (int)player.position.Y + player.idleHitbox.offsetY;
            player.hurtHitbox.rectangle.X = (int)player.position.X + player.hurtHitbox.offsetX;
            player.hurtHitbox.rectangle.Y = (int)player.position.Y + player.hurtHitbox.offsetY;
        }

        public void ChangeScreen_Wall()
        {
            activeScreen = screens[ScreenNumber - 1];
            DisableShowScreenTransition(screens[PreviousScreenNumber - 1], screens[ScreenNumber - 1]);
            player.position = activeScreen.actionScreenTransitionWalls[WallNumberToMoveTo - 1].spawnPoint;
            player.idleHitbox.rectangle.X = (int)player.position.X + player.idleHitbox.offsetX;
            player.idleHitbox.rectangle.Y = (int)player.position.Y + player.idleHitbox.offsetY;
            player.hurtHitbox.rectangle.X = (int)player.position.X + player.hurtHitbox.offsetX;
            player.hurtHitbox.rectangle.Y = (int)player.position.Y + player.hurtHitbox.offsetY;
        }



        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, List<RenderTarget2D> renderTargets)
        {
            // Set the right screen size
            graphics.GraphicsDevice.SetRenderTarget(renderTargets[activeScreen.renderTargetIndex]);

            spriteBatch.Begin(transformMatrix: activeScreen.camera.Transform);
            activeScreen.Draw(gameTime);
            spriteBatch.End();

            // We then set it to null so that we can draw back on to the screen
            graphics.GraphicsDevice.SetRenderTarget(null);

            // Draw back to the screen as a Texture2D
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(renderTargets[activeScreen.renderTargetIndex], new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.End();



        }



        public void HideShowScreenTransition(GameScreen gameScreen1, GameScreen gameScreen2)
        {
            activeScreen.Hide();
            previousActiveScreen = gameScreen1;
            activeScreen = gameScreen2;
            gameScreen1.DisableScreenGameObjects();
            activeScreen.EnableScreenGameObjects();
            activeScreen.Show();
        }

        public void DisableShowScreenTransition(GameScreen gameScreen1, GameScreen gameScreen2)
        {
            gameScreen1.Enabled = false;
            gameScreen1.DisableScreenGameObjects();
            previousActiveScreen = activeScreen;
            activeScreen = gameScreen2;
            activeScreen.EnableScreenGameObjects();
            activeScreen.Show();
        }


        public void CreateScreens(ContentManager content, GraphicsDevice graphicsDevice)
        {

            for (int i = 1; i <= 18; i++)
            {

                ActionScreenBuilder level = new ActionScreenBuilder("NEWLevel_" + i.ToString(), assetManager, colliderManager, inputManager, this, soundManager, player);
                level.LoadContent(content, graphicsDevice);

                screens.Add(
                    new ActionScreen(spriteBatch, menuFont, player, inputManager, level.gameObjectsAsList, level.backgroundObjects, level.respawnPoint, i, level.renderTargetIndex, level.cameraTypeIndex)
                    );

            }


            //ActionScreenBuilder level = new ActionScreenBuilder("Level_TEST", assetManager, colliderManager, inputManager, this, soundManager, player, 10, 6);
            //level.LoadContent(content, graphicsDevice);

            //screens.Add(
            //    new ActionScreen(spriteBatch, menuFont, player, inputManager, level.gameObjectsAsList, level.backgroundObjects)
            //    {
            //        respawnPoint = new Vector2(8 * 1, 8 * 1),
            //        screenNumber = 0,
            //        cameraBehaviourType1 = true
            //    });


        }



    }
}
