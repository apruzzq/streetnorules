using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    class EnemySpawner
    {
        const int DRAW_WIDTH = 141;
        const int DRAW_HEIGHT = 211;
        static float frameRate = (1f / 12f);

        Vector2 position;
        Vector2 origin;
        Texture2D texture;
        Rectangle drawArea;
        int frameX;
        int frameY;
        float frameTime;
        Actor enemy;

        public EnemySpawner(Vector2 position, Texture2D texture, Actor enemy)
        {
            this.enemy = enemy;
            this.position = position;
            this.texture = texture;
            this.frameX = 0;
            this.frameY = 0;
            frameTime = 0f;
            this.origin = new Vector2((float)DRAW_WIDTH/2,DRAW_HEIGHT);
            this.drawArea = new Rectangle(frameX * DRAW_WIDTH, frameY * DRAW_HEIGHT, DRAW_WIDTH, DRAW_HEIGHT);
        }

        public void Update(GameTime GT)
        {
            frameTime += (float)GT.ElapsedGameTime.TotalSeconds;
            if (frameTime >= frameRate)
            {
                frameTime = 0f;
                frameX++;
                
                if (frameY == 0)
                {
                    if (frameX > 5)
                    {
                        frameX = 0;
                        frameY++;

                        SpawnEnemy();
                    }
                }
                else
                {
                    if (frameX > 5)
                    {
                        frameX = 0;
                        frameY--;

                        GameManager.Levels[GameManager.CurrentLevel].EnemySpawners.Remove(this);
                    }
                }

                this.drawArea = new Rectangle(frameX * DRAW_WIDTH, frameY * DRAW_HEIGHT, DRAW_WIDTH, DRAW_HEIGHT);
            }

        }
        private void SpawnEnemy()
        {
            Level level = GameManager.Levels[GameManager.CurrentLevel];
            this.enemy.Position = this.position;
            level.Actors.Add(this.enemy);
            this.enemy = null;
        }

        public void Draw(SpriteBatch SB)
        {
            //Noir derrière la porte ouverte
            SB.Draw(Game1.SprSinglePixel, Camera.GetScreenPosition(this.position),
                this.drawArea, Color.Black, 0f, this.origin, 1f, SpriteEffects.None, 0.66f);

            //Draw the ennemy in doorway
            if(this.enemy != null)
            enemy.DrawInDoorway(SB, 0.65f);

            //Door animation
            SB.Draw(this.texture, Camera.GetScreenPosition(this.position),
                this.drawArea, Color.White, 0f, this.origin, 1f, SpriteEffects.None, 0.64f);
        }
    }
}
