using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class State
    {
        public bool Active = false;
        public bool enterStateFlag;

        public enum Exits
        {
            noExit,
            exitToNormalState,
            exitToSlidingWallStateFacingRight,
            exitToSlidingWallStateFacingLeft
        };

        public Exits exits = Exits.noExit;

        public Player player;

        public State(Player player)
        {
            this.player = player;
        }

        public virtual void Deactivate()
        {
            Active = false;
            exits = Exits.noExit;
        }

        public virtual void Activate()
        {
            Active = true;
            enterStateFlag = true;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void UpdateExits()
        {

        }

        public virtual void UpdateVelocityAndDisplacement()
        {

        }

        public virtual void UpdateAnimations()
        {

        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

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
