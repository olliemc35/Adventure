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
        public List<Spike> screenSpikes = new List<Spike>();
        public List<Note> screenNotes = new List<Note>();
        public List<Gate> screenGates = new List<Gate>();
        public List<NoteAndGatePuzzle> noteAndGatePuzzles = new List<NoteAndGatePuzzle>();
        public List<NoteAndGateAndOrbPuzzle> noteAndGateAndOrbPuzzles = new List<NoteAndGateAndOrbPuzzle>();
        public List<HangingRopeWithWeightAttached> screenHangingRopes = new List<HangingRopeWithWeightAttached>();
        public List<MovingPlatform> screenMovingPlatforms = new List<MovingPlatform>();
        public List<Beam> screenBeams = new List<Beam>();
        public List<BouncingOrb> screenBouncingOrbs = new List<BouncingOrb>();
        public List<HookPoint> screenHookPoints = new List<HookPoint>();
        public List<AnimatedGameObject> screenHazards = new List<AnimatedGameObject>();
        public List<AnimatedGameObject> screenClimables = new List<AnimatedGameObject>();
        public NoteShip screenNoteShip;



        public List<GameObject> screenGameObjects = new List<GameObject>();
        public List<GameObject> screenGameObjectsToLoadIn = new List<GameObject>();

        public List<GameObject> gameObjectsDrawOnly = new List<GameObject>();

        public List<HitboxRectangle> hitboxesToCheckCollisionsWith = new List<HitboxRectangle>();
        public List<HitboxRectangle> hitboxesForAimLine = new List<HitboxRectangle>();

        


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

        public bool ChangeScreenFlag = false;

        public Vector2 respawnPoint;


        public int screenNumber = 0;

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
            camera = new Camera();
            camera.Transform = Matrix.Identity;


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

        public virtual void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            foreach (GameObject gameObject in screenGameObjects)
            {
                gameObject.LoadContent(content, graphicsDevice);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
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

            camera.UpdateTransform(this, References.player);
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
