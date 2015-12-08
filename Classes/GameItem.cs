using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Les_Loubards
{
    class GameItem
    {
        //Player area range = 128 pixels
        //Actor.HIT_Y_Range = 30 pixels;
        //get 30 as a % of player area = 23.4375%
        //Convert this to a % of 0.2f <-- The player areas possible LayerDepth values
        //0.234373 * 0.2 = 0.46875f;
        const float DEPTH_CHECK = 0.046875f;



        public Level InLevel;
        public Vector2 Position;
        public float LayerDepth;
        public int HitAera;

        public virtual void Update()
        { }

        public virtual void Draw(SpriteBatch SB)
        { }

        public virtual void TakeHit(Direction cameFrom)
        { }

        public virtual void GetPickedUp(Player1 p)
        {
            SoundManager.PlaySound(SoundEnum.PICK_UP_ITEM);
            this.InLevel.GameItems.Remove(this);
        }

        public virtual void GetPickedUp(Player2 p)
        {
            SoundManager.PlaySound(SoundEnum.PICK_UP_ITEM);
            this.InLevel.GameItems.Remove(this);
        }

        /// <summary>
        /// Used for Player to 'use' this gameItem, such as pickuping up gameItems
        /// or kicking TrashCans
        /// </summary>
        public virtual bool CheckCollision(Actor actor)
        {
            return false;

        }

        /// <summary>
        /// Used inside of TrashCan (Hit State) and Knife classes to check 
        /// for attacking collisions
        /// </summary>
        public bool CheckEnemyCollision(Actor actor)
        {
            //1) Is actor Attackable ?
            if (actor.IsAttackable)
            {
                //2) Are we within Y-Range for a collision ?
                if (actor.LayerDepth > this.LayerDepth - GameItem.DEPTH_CHECK
                    && actor.LayerDepth < this.LayerDepth + GameItem.DEPTH_CHECK)
                {
                    //3) Are we colliding ?
                    float dist = Math.Abs(actor.Position.X - this.Position.X);
                    float minDist = this.HitAera + actor.HitArea;
                    if (dist < minDist)
                    {
                        //We are colliding :)
                        return true;
                    }
                }
            }

            //No collision
            return false;
        }


        public void SetPosition(Vector2 position)
        {
            this.Position = position;
            GetLayerDepth(this.Position.Y);
        }

        public void GetLayerDepth(float yPos)
        {
            //Get actors position as a % total play area
            int min = InLevel.PlayBounds.Top;
            int max = InLevel.PlayBounds.Bottom;
            int range = min - max;
            float percent = (yPos - (float)max) / (float)range;

            //Convert % to a value of LayerDepth Range
            // Player LayerDepth section 0.4 (front) to 0.6(back) -  range of 0.2f
            this.LayerDepth = percent * 0.2f + 0.4f+0.0001f;
        }
    }
}
