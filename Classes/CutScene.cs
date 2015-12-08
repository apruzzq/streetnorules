using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Les_Loubards
{
    class CutScene
    {

        float currentTime;

        List<string> lines;
        List<VoiceSoundEnum> voiceCutNames;
        List<Texture2D> portraits;
        List<float> timers;

        int currentLine;

        public CutScene()
        {
            lines = new List<string>();
            voiceCutNames = new List<VoiceSoundEnum>();
            portraits = new List<Texture2D>();
            timers = new List<float>();
            currentLine = 0;
        }

        public void AddLine(string line, VoiceSoundEnum voiceCutName, Texture2D portrait, float timer)
        {
            this.lines.Add(WrapText(line));
            this.voiceCutNames.Add(voiceCutName);
            this.portraits.Add(portrait);
            this.timers.Add(timer);
        }

        private string WrapText(string text) // cest la fonction qui va nous faire sauter une ligne lorsquon depasse la boite de dialogue ( lorsquon a des textes a ralonge)
        {
            int maxLineWidth = 400; // (400pixels)
            string[] words = text.Split(' '); // separer le texte entre chaque mot par un espace
            StringBuilder sb = new StringBuilder(); // quelque chose qui peut-il point tous de retour ensemble
            float lineWidth = 0f; //définir la largeur actuelle de 0 que nous n'avons pas utilisé
            float spaceWidth = Game1.FontSmall.MeasureString(" ").X; // how much space does a ' ' <== space take up

            foreach (string word in words)
            {
                Vector2 size = Game1.FontSmall.MeasureString(word);

                // Make sure adding this word will not go over the lineWidth limit
                if (lineWidth + size.X < maxLineWidth)
                {
                    //Add on the word to the final string
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    //Adding this word WILL take us over the maxLineWidth limit
                    //So we move onto the next line
                    sb.Append(Environment.NewLine + word + " ");
                    lineWidth = size.X + spaceWidth; // Update lineWidth value
                }
            }

            //Send back final string
            return sb.ToString();
        }

        public void PlayFirstLine()
        {
            currentTime = timers[0];
            SoundManager.PlayVoiceSound(voiceCutNames[0]);
            MusicManager.ChangeToVolume(0.2f);
        }

        public void PlayScene(GameTime gT)
        {
            currentTime -= (float)gT.ElapsedGameTime.TotalSeconds;

            //This line has been showing itself for long enough
            if (currentTime <= 0)
                GoToNextLine();

            if(InputHelper.WasKeyPressed(Keys.Space)
                ||InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A))
            {
                SoundManager.StopVoiceSound();
                GoToNextLine();
            }
        }

        private void GoToNextLine()
        {

            //Move on to the next one
            currentLine++;

            //Has the current CutScene finished ?
            if (currentLine > timers.Count - 1)
            {
                //yes, this cutscene is nox over
                GameManager.Levels[GameManager.CurrentLevel].ActivateSceneEvent();
                //Play next cutScene next time
                GameManager.Levels[GameManager.CurrentLevel].CurrentCutScene++;
                return;
            }

            currentTime = this.timers[currentLine];
            SoundManager.PlayVoiceSound(voiceCutNames[currentLine]);
        }

        public void Draw(SpriteBatch SB)
        {
            // Black background un peu transparant
            SB.Draw(Game1.SprSinglePixel, new Rectangle(140, 40, 400 + Game1.SprCSplayer.Width + 3, Game1.SprCSplayer.Height), new Color(Color.Black, 0.6f));

            // Draw portrait

            SB.Draw(this.portraits[currentLine], new Vector2(140, 40), Color.White);

            //Draw line text
            SB.DrawString(Game1.FontSmall, this.lines[currentLine], new Vector2(140 + Game1.SprCSplayer.Width + 3, 40), Color.White);

        }

    }
}
