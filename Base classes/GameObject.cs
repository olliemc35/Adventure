using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class GameObject
    {
        // The Enabled bool determines whether a gameObject is updated or not
        // This will be managed by ScreenManager - only gameObjects on the current screen will have Enabled set to true - otherwise false
        public bool Enabled = true;

        // Some GameObjects we only need to draw every frame - we do not need to update every frame. E.g. background tiles, spikes etc.
        public bool DrawOnly = false;


        // // The following references may be passed in when created a new GameObject instance:

        // Some GameObjects will need to access the AssetManager to "load-in" - e.g. player, doors etc
        public AssetManager assetManager;

        // Some GameObjects will need to detect collisions - e.g. player, spikes, doors etc.
        public ColliderManager colliderManager;

        // Some GameObjects will need to detect keyboard input - e.g. player, doors etc.
        public InputManager inputManager;

        // Some GameObjects will need to access the SoundManager - e.g. notes and musical platforms etc.
        public SoundManager soundManager;

        // Some GameObjects will need to access the ScreenManager so we can get information about the screen we are on - e.g. doors, bombs (to look for notes), player etc.
        public ScreenManager screenManager;

        // Some GameObjects will need a reference to the player - e.g. doors, spikes etc.
        public Player player;

        // Some GameObjects will need a reference to a list of other GameObjects - e.g. notes may need reference to gates, moving platforms need reference to any GameObjects on the platform (so they move at the same time) etc.
        public List<GameObject> attachedGameObjects;

        // Some GameObjects e.g. moving platforms can either be controlled by the player (via a Note) or act according to themselves. This bool determines which case is true.
        public bool playerControlled = false;

        // Some GameObjects need to be loaded into the game before others - e.g. NoteAndGatePuzzle needs to be loaded AFTER notes have
        public bool LoadLast = false;
        // Some GameObjects need to be loaded into the game first - e.g. MovingPlatforms need to be updated FIRST so that any attached gameObjects move before they themselves update
        // (e.g. if attached gameObject is CLIMABLE we need it to move first otherwise the CLIMB update will use the co-ordinates before the move)
        public bool LoadFirst = false;


        public GameObject()
        {
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gametime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void MoveManually(Vector2 moveVector)
        {

        }

        public virtual void AddAttachedGameObject(GameObject gameObject)
        {
            attachedGameObjects.Add(gameObject);
        }


        // These two functions are used in the ActionScreenBuilder to determine behaviour on AdjustHorizontally / AdjustVertically layers
        public virtual void AdjustHorizontally(ref List<int> ints)
        {

        }
        public virtual void AdjustVertically(ref List<int> ints)
        {

        }

        // Certain GameObjects will be attached to a Note and will execute some code whenever the player interacts with the note
        public virtual void HandleNoteTrigger()
        {

        }



    }
}
