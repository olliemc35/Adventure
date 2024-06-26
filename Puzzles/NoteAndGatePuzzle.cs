﻿using Microsoft.Xna.Framework.Content;
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
    public class NoteAndGatePuzzle : GameObject
    {
        // This order is very important for the puzzle
        public List<int> noteOrder = new List<int>();
        public List<Note> notes = new List<Note>();
        public Gate gate;
        public int indexWhichMustBePlayedNext = 0;
        public AnimatedGameObject symbolPlate;
        public List<Symbol> symbols = new List<Symbol>();

        public NoteAndGatePuzzle(Vector2 symbolPlatePosition, string symbolPlateFilename, List<int> noteOrder, AssetManager assetManager)
        {
            this.noteOrder = noteOrder;
            this.assetManager = assetManager;
            symbolPlate = new AnimatedGameObject(symbolPlatePosition, symbolPlateFilename, assetManager);
            attachedGameObjects = new List<GameObject>();
        }

        public NoteAndGatePuzzle(Vector2 symbolPlatePosition, string symbolPlateFilename, List<Note> notes, Gate gate, AssetManager assetManager)
        {
            this.notes = notes;
            this.gate = gate;
            this.assetManager = assetManager;
            symbolPlate = new AnimatedGameObject(symbolPlatePosition, symbolPlateFilename, assetManager);

        }

        public override void LoadContent()
        {

            List<Note> tempNotes = new List<Note>();

            if (attachedGameObjects != null)
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {
                    // We will be fed the notes in the order given by the ActionScreenBuilder
                    // This is simply going from top-to-bottom, then left-to-right, in the order drawn on the Aseprite file
                    if (gameObject is Note note)
                    {
                        tempNotes.Add(note);
                    }
                    else if (gameObject is Gate gate)
                    {
                        this.gate = gate;
                    }
                }
            }

            for (int i = 0; i < noteOrder.Count; i++)
            {
                notes.Add(tempNotes[noteOrder[i]]);
            }

            symbolPlate.LoadContent();



            for (int i = 0; i < notes.Count; i++)
            {
                Symbol symbol = new Symbol(new Vector2(symbolPlate.position.X + 12 * (i + 1), symbolPlate.position.Y + 4), notes[i].symbolFilename, assetManager);
                symbol.LoadContent();
                symbols.Add(symbol);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // We must update the puzzle logic first (BEFORE NOTES) as otherwise we will never detect
            // when notes[i].flagPlayerInteractedWith is true/false . (The order of the update steps are absolutely crucial.) 

            if (!gate.open)
            {
                PuzzleLogic();
            }

            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].Update(gameTime);
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            symbolPlate.Draw(spriteBatch);

            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].Draw(spriteBatch);
            }

        }


        public void PuzzleLogic()
        {

            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].flagPlayerInteractedWith)
                {
                    if (i == indexWhichMustBePlayedNext)
                    {
                        symbols[i].TurnedOn = true;
                        indexWhichMustBePlayedNext += 1;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            for (int j = 0; j < notes.Count; j++)
                            {
                                symbols[j].TurnedOn = false;
                            }

                            symbols[0].TurnedOn = true;
                            indexWhichMustBePlayedNext = 1;
                        }
                        else
                        {
                            for (int j = 0; j < notes.Count; j++)
                            {
                                symbols[j].TurnedOn = false;
                            }
                            indexWhichMustBePlayedNext = 0;
                        }
                    }
                }
            }

            if (indexWhichMustBePlayedNext == notes.Count)
            {
                gate.open = true;
            }
        }







    }
}
