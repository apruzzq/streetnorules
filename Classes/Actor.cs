using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{

    enum Direction
    {
        Left,
        Right,
        Neither
    }
    class Actor
    {

        #region other values
        const byte SHADOW_OPACITY = 30;
        public static float FrameRate = 1f / 16f; // vitesse défilement image

        public const int HIT_Y_RANGE = 15;
        public const float DOWN_TIME = 0.8f; // How long a character must stay on the ground when he has been knocked down
        public const float DEATH_FLASH_TIME = 0.2f;
        #endregion

        public Direction FacingDir;
        public Level InLevel;
        public Vector2 Position;
        public float LayerDepth;
        Vector2 originShadow;

        //Détection des collisions
        public bool IsAttacking;
        public bool IsAttackable;
        public int HitArea;
        public bool IsBoss;

        //Santé et mort
        public float Health;
        public int DeathFlashes;
        public bool IsVisible;

        public Actor(Vector2 position, Level inLevel)
        {
            this.Position = position;
            this.InLevel = inLevel;

            originShadow = new Vector2(Game1.SprCharacterShadow.Width / 2,
                                       Game1.SprCharacterShadow.Height);
            GetLayerDepth();
            this.IsVisible = true;
        }

        public virtual void Update(GameTime gT)
        {
        }

        public virtual void Draw(SpriteBatch SB)
        {
            SB.Draw(Game1.SprCharacterShadow, Camera.GetScreenPosition(this.Position),
                null, new  Color(Color.White, SHADOW_OPACITY), 0f,
                originShadow, 1f, SpriteEffects.None, this.LayerDepth);

        }

        public virtual void DrawInDoorway(SpriteBatch SB, float layerDepth)
        { }

        public void GetLayerDepth()
        {
            //Obtenir la position des acteurs comme une aire de jeu total%
            int min = InLevel.PlayBounds.Top;
            int max = InLevel.PlayBounds.Bottom;
            int range = min - max;
            float percent = ((float)this.Position.Y  - (float)max) / (float)range;

            //Convert % to a value of LayerDepth Range
            // Player LayerDepth section 0.4 (front) to 0.6(back) -  range of 0.2f
            this.LayerDepth = percent * 0.2f + 0.4f;
        }
        public void RemoveActorFromLevel()
        {
            InLevel.Actors.Remove(this);
        }

        public bool CheckForDeath()
        {
            if (this.Health <= 0)
            {
                //Yep, Je suis mort
                return true;
            }

            //Je ne suis pas encore mort
            return false;
        }


        protected Player ChoosePlayerToAttack(Player1 p1, Player2 p2)
        {
            if (Math.Sqrt(Math.Pow(p1.Position.X - this.Position.X, 2) + Math.Pow(p1.Position.Y - this.Position.Y, 2))
                > Math.Sqrt(Math.Pow(p2.Position.X - this.Position.X, 2) + Math.Pow(p2.Position.Y - this.Position.Y, 2)))
                return p2;
            else
                return p1;
        }

        #region Collision Detection
        public virtual void UpdateHitArea()
        { }

        public virtual void GetHit(Direction cameFrom, int damage)
        { }

        public virtual void GetKnockedDown(Direction cameFrom, int damage)
        { }
        #endregion
    }
}
