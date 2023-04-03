using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace Adventure
{
    public class SoundManager
    {
        public bool playCMajArpeggio = false;
        public bool flagPlayCMajArpeggio = false;
        public bool flagTest = false;

        public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();

        public SoundManager()
        {

        }

        public void LoadContent(ContentManager contentManager)
        {
            SoundEffect sound = contentManager.Load<SoundEffect>("CMajArpeggio");
            soundEffectInstances.Add(sound.CreateInstance());
            soundEffectInstances[0].IsLooped = true;

            SoundEffect sound2 = contentManager.Load<SoundEffect>("TestForGame2");
            soundEffectInstances.Add(sound2.CreateInstance());
            soundEffectInstances[1].IsLooped = true;
        }


        public void Update(GameTime gameTime)
        {
            if (flagPlayCMajArpeggio)
            {
                soundEffectInstances[0].Play();
                flagPlayCMajArpeggio = false;
            }

            if (flagTest)
            {
                soundEffectInstances[1].Play();
                flagTest = false;
            }


        }
    }


}
