using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    class Knife : GameItem
    {
        const int BASE_SPEED = 15;
        const int BASE_DAMAGE = 15;

        public static Vector2 Offset = new Vector2(178, 191);//new Vector2(45, 144);

        private int speed;
        private Vector2 origin;
        private float zHeight;
        private Actor whoThrewMe;

        public Knife(Vector2 startPos, Direction thrownDirection, Level InLevel, Actor whoThrewMe)
        {
            this.whoThrewMe = whoThrewMe;
            this.zHeight = startPos.Y;
            this.InLevel = InLevel;
            this.origin = new Vector2(Game1.SprThrowingKnife.Width / 2, Game1.SprThrowingKnife.Height / 2);
            GetLayerDepth(zHeight);

            if (thrownDirection == Direction.Left)
            {
                this.speed = BASE_SPEED * -1;
                this.Position = new Vector2(
                    startPos.X - Offset.X, startPos.Y - Offset.Y);
            }
            else
            {
                this.speed = BASE_SPEED;
                this.Position = new Vector2(
                    startPos.X + Offset.X, startPos.Y - Offset.Y);
            }
        }

        public override void Update()
        {
            this.Position.X += speed;

            //Check for Actor Collision
            for (int i = 0; i < InLevel.Actors.Count; i++)
            {
                if (this.whoThrewMe != InLevel.Actors[i]) // la on verifie quon a pas la meme personne qui va se lancer lui meme son propre couto ( ce serai bete xD )
                {
                    if (this.CheckEnemyCollision(InLevel.Actors[i]))
                    {
                        //Which way are we travelling ?
                        if (this.speed < 0) //Going left
                        {
                            InLevel.Actors[i].GetKnockedDown(Direction.Left, BASE_DAMAGE);
                            InLevel.GameItems.Remove(this);
                            return;
                        }
                        else // going right
                        {
                            InLevel.Actors[i].GetKnockedDown(Direction.Right, BASE_DAMAGE);
                            InLevel.GameItems.Remove(this);
                            return;
                        }
                    }
                }
            }

            //Have we gone out of playbounds
            if (this.Position.X < InLevel.PlayBounds.Left
                || this.Position.X > InLevel.PlayBounds.Right)
                InLevel.GameItems.Remove(this);
        }

        public override void Draw(SpriteBatch SB)
        {
            if (this.speed < 0)// I am going left
                SB.Draw(Game1.SprThrowingKnife, Camera.GetScreenPosition(this.Position),
                   null, Color.White, 0f, this.origin, 1f, SpriteEffects.None, this.LayerDepth);
            else // I am going right
                SB.Draw(Game1.SprThrowingKnife, Camera.GetScreenPosition(this.Position),
                   null, Color.White, 0f, this.origin, 1f, SpriteEffects.FlipHorizontally, this.LayerDepth);
        }


    }
}
