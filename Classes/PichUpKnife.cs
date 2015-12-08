using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Les_Loubards
{
    class PickUpKnife : GameItem
    {
        private Vector2 origin;

        public PickUpKnife(Level inLevel, Vector2 startPos)
        {
            this.InLevel = inLevel;
            this.Position = startPos;
            this.origin = new Vector2(Game1.SprThrowingKnife.Width / 2,
                Game1.SprThrowingKnife.Height / 2);
            this.GetLayerDepth(this.Position.Y);
        }

        //Does not need to override update(gT)
        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Game1.SprThrowingKnife, Camera.GetScreenPosition(this.Position),
                null, Color.White, 0f, origin, 1f, SpriteEffects.None, this.LayerDepth);
        }

        public override void GetPickedUp(Player1 p)
        {
            p.GiveKnife();
            base.GetPickedUp(p);
        }

        public override void GetPickedUp(Player2 p)
        {
            p.GiveKnife();
            base.GetPickedUp(p);
        }

        public override bool CheckCollision(Actor actor)
        {
            //1) Are we withing y range ?
            if (actor.Position.Y > this.Position.Y - Actor.HIT_Y_RANGE
                    && actor.Position.Y < this.Position.Y + Actor.HIT_Y_RANGE)
            {
                //2) are we touching?
                float dist = Math.Abs(actor.Position.X - this.Position.X); //distance from centre to centre
                float minDist = this.HitAera + actor.HitArea; //minimum distance for a collision
                if (dist < minDist)
                    return true;
            }

            //No collision : 
            return false;
        }
    }
}
