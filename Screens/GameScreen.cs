using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Adventure
{
    public class GameScreen
    {
        public List<ScreenBehaviour> screenBehaviours = new List<ScreenBehaviour>();
        public bool Visible { get; set; } // Whether the screen can be drawn on the monitor
        public bool Enabled { get; set; } // Whether the user can interact with the screen

        protected SpriteBatch spriteBatch;


        public List<GameObject> screenGameObjectsToRemove = new List<GameObject>();

        public List<Door> screenDoors = new List<Door>();
        public List<ActionScreenTransitionWall> actionScreenTransitionWalls = new List<ActionScreenTransitionWall>();
        public List<Note> screenNotes = new List<Note>();
        public NoteShip screenNoteShip;



        public List<GameObject> screenGameObjects = new List<GameObject>();
        public List<GameObject> screenGameObjectsToLoadIn = new List<GameObject>();

        public List<GameObject> gameObjectsDrawOnly = new List<GameObject>();

        public List<HitboxRectangle> hitboxesToCheckCollisionsWith = new List<HitboxRectangle>();
        public List<HitboxRectangle> hitboxesForAimLine = new List<HitboxRectangle>();

        public List<HitboxRectangle> terrainHitboxes = new List<HitboxRectangle>();
        public List<HitboxRectangle> hazardHitboxes = new List<HitboxRectangle>();


        public Texture2D Background { get; set; }
        public Rectangle backgroundPosition = new Rectangle(0, 0, References.game.Window.ClientBounds.Width, References.game.Window.ClientBounds.Height);

        public Camera camera;

        public bool cameraBehaviourType1 = false; // "stationary:" If screen is to remain stationary on screen
        public bool cameraBehaviourType2 = false; // "long horizontal corridor:" If screen permits horizontal scrolling
        public bool cameraBehaviourType3 = false;
        public bool cameraBehaviourType4 = false;
        public bool cameraBehaviourType5 = false;

        public int ScreenHeight = 0;
        public int ScreenWidth = 0;

        // We may create a level with 20 x 12 tiles (say). This is the actualScreenSize.
        // We may choose to render it at 10 x 6 tiles (say). This is the renderScreenSize.
        // We do this so that we do not lose image quality and can view big screens zoomed in (zooming in via the camera loses image quality).
        // This distinction is important for the Camera.
        // Note that we measure both in terms of tiles. So e.g. a 160 x 90 resolution corresponds to 10 x 6 tiles (of size 16x16). (Dividing gives something like 10 x 5.6... and we round up to the nearest tile.)

        public int actualScreenWidth;
        public int actualScreenHeight;

        public int renderScreenWidth;
        public int renderScreenHeight;


        public bool ChangeScreenFlag = false;
        public bool ChangeScreenFlag_Wall = false;


        public Vector2 respawnPoint;


        public int screenNumber = 0;


        public int renderTargetIndex = 0;


        public List<ScreenBehaviour> ScreenBehaviours
        {
            get { return screenBehaviours; }
        }

        public GameScreen(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            Visible = false;
            Enabled = false;
            DisableScreenGameObjects();
            //camera = new Camera();
            //camera.Transform = Matrix.Identity;


        }

        public GameScreen(SpriteBatch spriteBatch, Texture2D background)
        {
            this.spriteBatch = spriteBatch;
            Background = background;
            Visible = false;
            Enabled = false;
            DisableScreenGameObjects();
        }



        public void EnableScreenGameObjects()
        {
            foreach (GameObject gameObject in screenGameObjects)
            {
                gameObject.Enabled = true;

            }
        }
        public void DisableScreenGameObjects()
        {
            foreach (GameObject gameObject in screenGameObjects)
            {
                gameObject.Enabled = false;
            }
        }

        public virtual void LoadContent()
        {
            foreach (GameObject gameObject in screenGameObjects)
            {
                gameObject.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {

            HandleInput();

            foreach (GameObject gameObject in screenGameObjects)
            {
                if (gameObject.Enabled)
                {
                    gameObject.Update(gameTime);
                }
            }



            if (screenGameObjectsToRemove != null)
            {
                foreach (GameObject gameObject in screenGameObjectsToRemove)
                {
                    screenGameObjects.Remove(gameObject);
                }

                screenGameObjectsToRemove.Clear();

            }

          


        



            if (Enabled)
            {
                foreach (ScreenBehaviour screenBehaviour in screenBehaviours)
                {
                    if (screenBehaviour.Enabled == true)
                    {
                        screenBehaviour.Update(gameTime);
                    }
                }
            }

        }

        // The spriteBatch.Begin() and spriteBatch.End() commands, which need to sandwich the code below, are called in the ScreenManager Draw method.
        public virtual void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(Background, backgroundPosition, Color.White);

            if (Visible)
            {
                foreach (ScreenBehaviour screenBehaviour in screenBehaviours)
                {
                    if (screenBehaviour.DrawMe && screenBehaviour.Visible)
                    {
                        screenBehaviour.Draw(gameTime);
                    }
                }

                foreach (GameObject gameObject in gameObjectsDrawOnly)
                {
                    gameObject.Draw(spriteBatch);
                }

                foreach (GameObject gameObject in screenGameObjects)
                {
                    gameObject.Draw(spriteBatch);
                }

                

            }
        }


        public virtual void HandleInput()
        {
        }

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (ScreenBehaviour screenBehaviour in screenBehaviours)
            {
                screenBehaviour.Enabled = true;
                if (screenBehaviour.DrawMe)
                {
                    screenBehaviour.Visible = true;
                }
            }
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (ScreenBehaviour screenBehaviour in screenBehaviours)
            {
                screenBehaviour.Enabled = false;
                if (screenBehaviour.DrawMe)
                {
                    screenBehaviour.Visible = false;
                }
            }
        }

        // Disable a screen so cannot interact with it, but it will still be present on screen.
        public virtual void Disable()
        {
            Visible = true;
            Enabled = false;

            foreach (GameObject gameObject in screenGameObjects)
            {
                gameObject.Enabled = false;
            }

            foreach (ScreenBehaviour screenBehaviour in screenBehaviours)
            {
                screenBehaviour.Enabled = false;

                if (screenBehaviour.DrawMe)
                {
                    screenBehaviour.Visible = true;
                }
            }
        }

    }
}
