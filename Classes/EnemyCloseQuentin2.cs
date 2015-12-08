using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    class EnemyCloseQuentin2 : EnemyClose2
    {

        public EnemyCloseQuentin2(Vector2 position, Level inLevel)
            : base(position, inLevel)
        {
            JeChangeToutesLesVariables();
            ResetIdleGraphic();
            drawColor = Color.White; //Set the draw Color
            this.Health = 300;
            this.IsBoss = true;
        }

        public EnemyCloseQuentin2(Direction startingSide, Level inLevel)
            : base(Vector2.Zero, inLevel)
        {
            JeChangeToutesLesVariables();
            this.FacingDir = startingSide;

            ResetIdleGraphic();
            drawColor = Color.White; //Set the draw Color
            this.Health = 300;
            this.IsBoss = true;
        }

        private void JeChangeToutesLesVariables()
        {
            DRAW_HEIGHT_IDLE = 201;
            DRAW_HEIGHT_WALK = 212;
            DRAW_HEIGHT_ATTACK1 = 206;
            DRAW_HEIGHT_ATTACK2 = 209;
            DRAW_HEIGHT_ATTACK3 = 207;
            DRAW_HEIGHT_KICK = 207;
            DRAW_HEIGHT_ARMSFOLDED = 201;
            DRAW_HEIGHT_TAKEHIT = 211;
            DRAW_HEIGHT_KNOCKEDOWN = 205;
            DRAW_HEIGHT_GETUP = 185;

            DRAW_WIDHT_IDLE = 90;
            DRAW_WIDHT_WALK = 110;
            DRAW_WIDHT_ATTACK1 = 147;
            DRAW_WIDHT_ATTACK2 = 150;
            DRAW_WIDHT_ATTACK3 = 173;
            DRAW_WIDTH_ARMSFOLDED = 90;
            DRAW_WIDTH_TAKEHIT = 110;
            DRAW_WIDTH_KNOCKEDOWN = 208;
            DRAW_WIDTH_GETUP = 205;

            FRAME_Y_IDLE = 0;
            FRAME_Y_WALK = 201;
            FRAME_Y_ATTACK1 = 719;
            FRAME_Y_ATTACK2 = 925;
            FRAME_Y_ATTACK3 = 1148;
            FRAME_Y_ARMSFOLDED = 0;
            FRAME_Y_TAKEHIT = 818;
            FRAME_Y_KNOCKEDOWN = 0;
            FRAME_Y_GETUP = 410;

            FRAME_X_IDLE = 8;
            FRAME_X_WALK = 8;
            FRAME_X_ATTACK1 = 5;
            FRAME_X_ATTACK1_COLLISION = 3;
            FRAME_X_ATTACK2 = 4;
            FRAME_X_ATTACK2_COLLISION = 3;
            FRAME_X_ATTACK3 = 5;
            FRAME_X_ATTACK3_COLLISION = 3;
            FRAME_X_ARMSFOLDED = 0;
            FRAME_X_TAKEHIT = 5;
            FRAME_X_KNOCKEDOWN = 17;
            FRAME_X_GETUP = 13;
            FRAME_X_JUMP = 10;

            TEXTURE_ATTACK = Game1.SprCharacterAttacksQuentin;
            TEXTURE_REACTS = Game1.SprCharacterReactsQuentin;



        }

        protected override void AnimateKnockDown(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 0.7f) // 30% plus rapide
            {


                // permet d'être ejecté par un coup de pied, opposé au sens du personnage
                if (FacingDir == Direction.Left)
                {
                    Position.X = Position.X + 5;
                }
                else if (FacingDir == Direction.Right)
                {
                    Position.X = Position.X - 5;
                }




                // Si première ligne de sprites alors true
                if (frameY == FRAME_Y_KNOCKEDOWN)
                {
                    // caractéristiques part1

                    drawWidth = DRAW_WIDTH_KNOCKEDOWN;

                    //si on arrive à la fin de la premiere ligne, alors on change les caracteristiques
                    if (frameX > 8)//7
                    {
                        //caractéristiques part2
                        frameX = 0;
                        drawHeight = DRAW_HEIGHT_KNOCKEDOWN;
                        drawWidth = DRAW_WIDTH_KNOCKEDOWN;
                        frameY = FRAME_Y_KNOCKEDOWN + DRAW_HEIGHT_KNOCKEDOWN;
                        stateTime = 0f;
                    }
                }

                // true si on est dans la 2ème ligne de sprite
                else
                {

                    // true si on arrive à la fin de la 2eme ligne de sprite
                    if (frameX >= 7/*5*/ && frameY == FRAME_Y_KNOCKEDOWN + DRAW_HEIGHT_KNOCKEDOWN)
                    {

                        if (CheckForDeath())
                        {
                            state = EnemyCloseState2.Dying;
                            SoundManager.PlaySound(SoundEnum.ACTOR_DEATH);
                            stateTime = 1f;
                            frameX = 5;
                            return;
                        }

                        // on a finit, on change alors d'état
                        state = EnemyCloseState2.Down;
                        return;

                    }
                }

                SetDrawArea();

                currentFrameTime = 0f;
                frameX++;

            }

        }

        protected override void AnimateGettingUp(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate*0.7f)
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos

                if (frameY == 410)
                {
                    if (frameX > 6)//si on arrive à la fin de la première ligne
                    {
                        frameX = 0;
                        frameY = 595;
                        drawHeight = DRAW_HEIGHT_GETUP;
                        drawWidth = DRAW_WIDTH_GETUP;
                        stateTime = 0f;

                    }
                }

                else
                {
                    if (frameX > 5)
                    {
                        ResetIdleGraphic();
                        return;
                    }
                }
                SetDrawArea();
            }
        }

    }
}
