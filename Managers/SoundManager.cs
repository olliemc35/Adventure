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
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class SoundManager
    {
        public bool playCMajArpeggio = false;
        public bool flagPlayCMajArpeggio = false;
        public bool flagTest = false;

        public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();

        public List<string> soundEffectStrings;

        public IDictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();


        public SoundManager()
        {
            soundEffectStrings = new List<string>()
            {
                "A",
                "C",
                "E",
                "G",
                "GLow",
                "F",
                "FHigh",
                "CMajArpeggio",
                "TestForGame2",
                "TestForGame",
                "HighCBell",
                "HighEflatBell",
                "HighGBell",
                "TimpaniC"

            };
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (string filename in soundEffectStrings)
            {
                SoundEffect sound = contentManager.Load<SoundEffect>(filename);
                soundEffects.Add(filename,sound);
            }

            //SoundEffect sound = contentManager.Load<SoundEffect>("CMajArpeggio");
            //soundEffectInstances.Add(sound.CreateInstance());
            //soundEffectInstances[0].IsLooped = true;

            //SoundEffect sound2 = contentManager.Load<SoundEffect>("TestForGame2");
            //soundEffectInstances.Add(sound2.CreateInstance());
            //soundEffectInstances[1].IsLooped = true;
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
