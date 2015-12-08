using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    enum TrashCanState
    {
        Normal,
        Hit
    }
    class TrashCan : GameItem
    {
        const int BASE_DAMAGE = 10;
        static Vector2 baseHitSpeed = new Vector2(6, -6);//vitesse quand on donne un coup de pied
        static float gravity = 0.3f;

        TrashCanState state;
        Texture2D texture;
        Vector2 origin;
        Vector2 speed;

        GameItem dropItem;

        /// <summary>
        /// créé un couteau quand on tape dans la poubelle
        /// </summary>
        /// <param name="inLevel"></param>
        /// <param name="startPos"></param>
        /// <param name="dropItem"></param>

        public TrashCan(Level inLevel, Vector2 startPos, GameItem dropItem)
        {
            this.InLevel = inLevel;
            this.state = TrashCanState.Normal;
            this.texture = Game1.SprTrashCanNormal;
            this.Position = startPos;
            this.speed = Vector2.Zero;
            this.origin = new Vector2(texture.Width / 2, texture.Height); //Lower Centre
            this.HitAera = Game1.SprTrashCanNormal.Width / 2;

            this.GetLayerDepth(this.Position.Y);

            this.dropItem = dropItem;
        }

        /// <summary>
        /// poubelle sans couteau
        /// </summary>
        /// <param name="inLevel"></param>
        /// <param name="startPos"></param>

        public TrashCan(Level inLevel, Vector2 startPos)
        {
            this.InLevel = inLevel;
            this.state = TrashCanState.Normal;
            this.texture = Game1.SprTrashCanNormal;
            this.Position = startPos;
            this.speed = Vector2.Zero;
            this.origin = new Vector2(texture.Width / 2, texture.Height);
            this.HitAera = Game1.SprTrashCanNormal.Width / 2;

            this.GetLayerDepth(this.Position.Y);

        }

        public override void Update()
        {
            if (state == TrashCanState.Hit)
            {
                this.speed.Y += gravity; //pull down for gracity
                this.Position += speed;

                //Check for Actor Collision
                for (int i = 0; i < InLevel.Actors.Count; i++)
                {
                    if (InLevel.Actors[i] as Player == null)
                    {
                        if (this.CheckEnemyCollision(InLevel.Actors[i]))
                        {
                            //Which way are we travelling ?
                            if (this.speed.X < 0) //Going left
                            {
                                InLevel.Actors[i].GetKnockedDown(Direction.Left, BASE_DAMAGE);
                            }
                            else // going right
                            {
                                InLevel.Actors[i].GetKnockedDown(Direction.Right, BASE_DAMAGE);
                            }
                        }
                    }
                }

                if (speed.Y >= 6)//landed back on the ground
                {
                    RemoveTrashCan();
                }
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            //Check for facing direction based on speed
            if (this.speed.X > 0)//headinf left so the hit must have come from et raight
            {
                SB.Draw(this.texture, Camera.GetScreenPosition(this.Position),
                    null, Color.White, 0f, this.origin, 1f, SpriteEffects.FlipHorizontally, this.LayerDepth);
            }
            else // head to the right, so the hit come from the left
            {
                SB.Draw(this.texture, Camera.GetScreenPosition(this.Position),
                    null, Color.White, 0f, this.origin, 1f, SpriteEffects.None, this.LayerDepth);
            }

        }

        private void RemoveTrashCan()
        {
            if (this.dropItem != null)
            {
                //drop the item we are carriying
                InLevel.GameItems.Add(this.dropItem);
                this.dropItem.SetPosition(this.Position);
                this.dropItem = null;
            }

            //Remove TrashCan from level
            InLevel.GameItems.Remove(this);
            GameManager.Levels[GameManager.CurrentLevel].GameItems.Remove(this);
        }

        public override void TakeHit(Direction cameFrom)
        {
            //set speed and texture
            this.state = TrashCanState.Hit;
            this.texture = Game1.SprTrashCanHit;

            //set speed based off hitdirection
            if (cameFrom == Direction.Left)
                this.speed = new Vector2(-baseHitSpeed.X, baseHitSpeed.Y);
            else
                this.speed = baseHitSpeed;

            SoundManager.PlaySound(SoundEnum.TRASH_CAN_HIT);
        }



        public override bool CheckCollision(Actor actor)
        {

            //1)    Is actor attackable ?
            if (actor.IsAttackable)
            {
                //2) Are we within Y-Range?
                if (actor.Position.Y > this.Position.Y - Actor.HIT_Y_RANGE
                    && actor.Position.Y < this.Position.Y + Actor.HIT_Y_RANGE)
                {
                    //3) Which way is the player facing?
                    if (actor.FacingDir == Direction.Left)
                    {
                        //4) Am I infront of actor ?
                        if (this.Position.X < actor.Position.X)
                        {
                            //5) Is actors left edge more left then my right edge ?
                            if (actor.Position.X - actor.HitArea < this.Position.X + this.HitAera)
                            {
                                //There is a collision
                                return true;

                            }
                        }
                    }
                    //3) Which way is the player facing?
                    else //  Actor is facing RIGHT
                    {
                        //4) Is I in front of Actor ? ** RIGHT **
                        if (this.Position.X > actor.Position.X)
                        {
                            //5) Is the actors right edge >MORE RIGHT> then my Left edge ?
                            if (actor.Position.X + actor.HitArea > this.Position.X - this.HitAera)
                            {
                                //There is a collision
                                return true;
                            }
                        }
                    }
                }
            }


            //no collision
            return false;
        }
    }
}
