using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Adventure
{
    public class GateAndOrbEmitterPuzzle : GameObject
    {
        public MovingOrbReceptor orbReceptor;
        public Gate gate;

        public GateAndOrbEmitterPuzzle()
        {
            attachedGameObjects = new List<GameObject>();
        }

        public override void LoadContent()
        {
            foreach (GameObject gameObject in attachedGameObjects)
            {
                if (gameObject is MovingOrbReceptor orbReceptor)
                {
                    this.orbReceptor = orbReceptor;
                }
                else if (gameObject is Gate gate)
                {
                    this.gate = gate;
                }
            }
        }

        public override void Update(GameTime gametime)
        {
            if (orbReceptor.animation_playing == orbReceptor.animation_Hit)
            {
                gate.open = true;
            }
            else
            {
                gate.open = false;
            }
        }


    }
}
