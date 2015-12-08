using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    class EnemyCloseMehdi :EnemyClose
    {
        
        public EnemyCloseMehdi(Vector2 position, Level inLevel)
            : base(position, inLevel)
        {
            JeChangeToutesLesVariables();
            ResetIdleGraphic();
            drawColor = Color.Red; //Set the draw Color
            this.Health = STARTING_HEALTH;
            this.IsBoss = false;
        }

        public EnemyCloseMehdi(Direction startingSide, Level inLevel)
            : base(Vector2.Zero, inLevel)
        {
            JeChangeToutesLesVariables();
            this.FacingDir = startingSide;

            ResetIdleGraphic();
            drawColor = Color.Red; //Set the draw Color
            this.Health = STARTING_HEALTH;
            this.IsBoss = true;
        }

        private void JeChangeToutesLesVariables()
        {

            DRAW_HEIGHT_IDLE = 225;
            DRAW_HEIGHT_WALK = 225;
            DRAW_HEIGHT_ATTACK1 = 225;
            DRAW_HEIGHT_ATTACK2 = 225;
            DRAW_HEIGHT_ATTACK3 = 225;
            DRAW_HEIGHT_KICK = 225;
            DRAW_HEIGHT_ARMSFOLDED = 225;
            DRAW_HEIGHT_TAKEHIT = 225;
            DRAW_HEIGHT_KNOCKEDOWN = 225;
            DRAW_HEIGHT_GETUP = 225;

            DRAW_WIDHT_IDLE = 105;
            DRAW_WIDHT_WALK = 100;
            DRAW_WIDHT_ATTACK1 = 112;
            DRAW_WIDHT_ATTACK2 = 161;
            DRAW_WIDHT_ATTACK3 = 151;
            DRAW_WIDTH_ARMSFOLDED = 105;
            DRAW_WIDTH_TAKEHIT = 108;
            DRAW_WIDTH_KNOCKEDOWN = 216;
            DRAW_WIDTH_GETUP = 180;

            FRAME_Y_IDLE = 0;
            FRAME_Y_WALK = 225;
            FRAME_Y_ATTACK1 = 720;
            FRAME_Y_ATTACK2 = 945;
            FRAME_Y_ATTACK3 = 945;
            FRAME_Y_ARMSFOLDED = 0;
            FRAME_Y_TAKEHIT = 450;
            FRAME_Y_KNOCKEDOWN = 0;
            FRAME_Y_GETUP = 225;

            FRAME_X_IDLE = 5;
            FRAME_X_WALK = 8;
            FRAME_X_ATTACK1 = 4;
            FRAME_X_ATTACK1_COLLISION = 3;
            FRAME_X_ATTACK2 = 4;
            FRAME_X_ATTACK2_COLLISION = 3;
            FRAME_X_ATTACK3 = 5;
            FRAME_X_ATTACK3_COLLISION = 4;
            FRAME_X_ARMSFOLDED = 1;
            FRAME_X_TAKEHIT = 4;
            FRAME_X_KNOCKEDOWN = 3;
            FRAME_X_GETUP = 5;
            FRAME_X_JUMP = 5;

            TEXTURE_ATTACK = Game1.SprCharacterAttacksMehdi;
            TEXTURE_REACTS = Game1.SprCharacterReactsMehdi;

        }

        protected override void AnimateKnockDown(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 1.5f)
            {

                currentFrameTime = 0f;
                frameX++;

                // permet d'être ejecté par un coup de pied, opposé au sens du personnage
                if (FacingDir == Direction.Left)
                {
                    Position.X = Position.X + 5;
                }
                else if (FacingDir == Direction.Right)
                {
                    Position.X = Position.X - 5;
                }


                if (frameX > FRAME_X_KNOCKEDOWN)//7
                {
                    frameX = 0;
                    stateTime = 0f;
                }

                if (CheckForDeath())
                {
                    state = EnemyCloseState.Dying;
                    SoundManager.PlaySound(SoundEnum.ACTOR_DEATH);
                    stateTime = 1f;
                    frameX = 4;
                    return;
                }


                // on a finit, on change alors d'état
                state = EnemyCloseState.Down;
                SetDrawArea();

            }


        }



        protected override void AnimateGettingUp(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 1.5f)
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos

                if (frameX > FRAME_X_GETUP)//si on arrive à la fin de la première ligne
                {
                    ResetIdleGraphic();
                    return;
                }

                SetDrawArea();
            }
        }

    }
}
