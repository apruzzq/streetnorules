﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Les_Loubards.Classes;

namespace Les_Loubards
{

    // private void HitSomeone(Actor whoToHit)  <<<<<<  ICI POUR LES POINTS DE DAMAGE DU HERO


    class Player1 : Player
    {
        #region draw area constants
        const int DRAW_HEIGHT_IDLE = 201;
        const int DRAW_HEIGHT_WALK = 212;
        const int DRAW_HEIGHT_JUMP = 306;
        const int DRAW_HEIGHT_JUMPKICK = 120;
        const int DRAW_HEIGHT_ATTACK1 = 206;
        const int DRAW_HEIGHT_ATTACK2 = 209;
        const int DRAW_HEIGHT_KICK = 207;
        const int DRAW_HEIGHT_KNIFE_THROWN = 223;
        const int DRAW_HEIGHT_KICK_OBJECT = 195;
        const int DRAW_HEIGHT_PICKUP_ITEM = 232;
        const int DRAW_HEIGHT_ARMSFOLDED = 0;
        const int DRAW_HEIGHT_TAKEHIT = 211;
        const int DRAW_HEIGHT_KNOCKEDOWN = 205;
        const int DRAW_HEIGHT_GETUP = 185;

        const int DRAW_WIDHT_IDLE = 90;
        const int DRAW_WIDHT_WALK = 110;
        const int DRAW_WIDHT_JUMP = 90;
        const int DRAW_WIDHT_JUMPKICK = 145;
        const int DRAW_WIDHT_ATTACK1 = 147;
        const int DRAW_WIDHT_ATTACK2 = 150;
        const int DRAW_WIDHT_ATTACK3 = 173;
        const int DRAW_WIDTH_KNIFE_THROWN = 189;
        const int DRAW_WIDTH_KICK_OBJECT = 125;
        const int DRAW_WIDTH_PICKUP_ITEM = 113;
        const int DRAW_WIDTH_ARMSFOLDED = 108;
        const int DRAW_WIDTH_TAKEHIT = 110;
        const int DRAW_WIDTH_KNOCKEDOWN = 208;
        const int DRAW_WIDTH_GETUP = 205;

        const int FRAME_Y_IDLE = 0;
        const int FRAME_Y_WALK = 201;
        const int FRAME_Y_JUMP = 413;
        const int FRAME_Y_JUMPKICK = 1028;
        const int FRAME_Y_ATTACK1 = 719;
        const int FRAME_Y_ATTACK2 = 925;
        const int FRAME_Y_ATTACK3 = 1148;
        const int FRAME_Y_KNIFE_THROWN = 1782;
        const int FRAME_Y_KICK_OBJECT = 1587;
        const int FRAME_Y_PICKUP_ITEM = 1355;
        const int FRAME_Y_ARMSFOLDED = 0;
        const int FRAME_Y_TAKEHIT = 818;
        const int FRAME_Y_KNOCKEDOWN = 0;
        const int FRAME_Y_GETUP = 410;



        #endregion

        #region other values
        const float MOVEMENT_THRESHOLD = 0.3f;
        static float acceptAttack2Time = Actor.FrameRate * 6f; // 6 frames
        static float acceptAttack3Time = Actor.FrameRate * 6f; // 6 frames
        #endregion


        PlayerIndex pIndex;
        public bool CarryingKnife; // public pour y acceder dans le HUDManager

        // Méthode pour la classe 
        public Player1(Vector2 position, Level inLevel, PlayerIndex pIndex)
            : base(position, inLevel, pIndex)
        {
            this.pIndex = pIndex;
            this.CarryingKnife = false;

            ResetIdleGraphic();
            this.Health = STARTING_HEALTH;

        }



        public override void Update(GameTime gT)
        {
            switch (state)
            {
                #region all
                #region Idle and Walking
                case PlayerState.Idle:
                case PlayerState.Walking:



                    #region Attack - Input
                    //if (InputHelper.WasKeyPressed(Keys.A))
                    
                    if (InputHelper.WasButtonPressed(pIndex, Buttons.X)
                        || InputHelper.WasKeyPressed(Keys.A) && GameManager.NumberPlayers == 1)
                    {
                        #region Any GameItems to use ?
                        //are we close enough to a gameitem to use ?
                        for (int i = 0; i < InLevel.GameItems.Count; i++)
                        {
                            GameItem gameItem;

                            gameItem = InLevel.GameItems[i] as TrashCan;
                            if (gameItem != null)
                            {
                                if (gameItem.CheckCollision(this))
                                {
                                    //Pick it up !!
                                    //setup the animation
                                    this.aboutToUse = gameItem;
                                    this.state = PlayerState.KickObject;
                                    texture = Game1.SprCharacterReacts;
                                    currentFrameTime = 0f;
                                    frameX = 0;
                                    frameY = FRAME_Y_KICK_OBJECT;
                                    drawWidth = DRAW_WIDTH_KICK_OBJECT;
                                    drawHeight = DRAW_HEIGHT_KICK_OBJECT;
                                    originCharacter = new Vector2(drawWidth / 2, drawHeight);
                                    SetDrawArea();
                                    return;
                                }
                            }
                            gameItem = InLevel.GameItems[i] as PickUpKnife;
                            if (gameItem != null)
                            {
                                if (gameItem.CheckCollision(this))
                                {
                                    PickUpItem(gameItem);
                                    return;
                                }
                            }

                            gameItem = InLevel.GameItems[i] as PickUpHealthPack;
                            if (gameItem != null)
                            {
                                if (gameItem.CheckCollision(this))
                                {
                                    PickUpItem(gameItem);
                                    return;
                                }
                            }
                        }
                        #endregion

                        //setup draw area
                        drawWidth = DRAW_WIDHT_ATTACK1;
                        drawHeight = DRAW_HEIGHT_ATTACK1;//138
                        frameX = 0;
                        frameY = FRAME_Y_ATTACK1;//1437
                        SetDrawArea();
                        // correction des sprites, origine mouvement + 20 pixels lorsqu'il bouge à gauche, - 20 a droite
                        if (FacingDir == Direction.Left)
                            originCharacter = new Vector2(drawWidth / 2 + 21, drawHeight);
                        else if (FacingDir == Direction.Right)
                            originCharacter = new Vector2(drawWidth / 2 - 21, drawHeight);

                        stateTime = 0f;
                        state = PlayerState.Attack1;
                        makeCombo = false;

                        //Playing Attack Sound
                        SoundManager.PlaySound(SoundEnum.ATTACK);
                        return;
                    }
                    #endregion

                    #region Jump - Input
                    //if (InputHelper.WasKeyPressed(Keys.LeftShift))
                    if (InputHelper.WasButtonPressed(pIndex, Buttons.A)
                        || InputHelper.WasKeyPressed(Keys.LeftShift) && GameManager.NumberPlayers == 1)
                    {
                        landingHeight = Position.Y;
                        jumpingPos = Position;
                        speed.Y = -5; // Jump speed

                        state = PlayerState.Jumping;
                        texture = Game1.SprCharacterAttacks;
                        frameX = 0;
                        frameY = FRAME_Y_JUMP;
                        drawWidth = DRAW_WIDHT_JUMP;
                        drawHeight = DRAW_HEIGHT_JUMP;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                        SetDrawArea();
                        return;
                    }
                    #endregion

                    #region Throw Knife - Input
                    if (InputHelper.WasKeyPressed(Keys.LeftControl) && GameManager.NumberPlayers == 1 
                        ||InputHelper.WasButtonPressed(this.pIndex, Buttons.B))
                    {
                        if (CarryingKnife)
                        {
                            //setup animation
                            state = PlayerState.ThrownKnife;
                            frameX = 0;
                            frameY = FRAME_Y_KNIFE_THROWN;

                            texture = Game1.SprCharacterReacts;
                            drawWidth = DRAW_WIDTH_KNIFE_THROWN;
                            drawHeight = DRAW_HEIGHT_KNIFE_THROWN;
                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            SetDrawArea();

                            SoundManager.PlaySound(SoundEnum.KNIFE_GET_OUT);
                            return;

                        }
                    }
                    #endregion


                    Move(gT);
                    break;
                #endregion

                #region Jumping
                case PlayerState.Jumping:
                    #region Attack - Input
                    if (InputHelper.WasKeyPressed(Keys.A) && GameManager.NumberPlayers == 1
                        || InputHelper.WasButtonPressed(pIndex, Buttons.X))
                    {
                        state = PlayerState.JumpKick;
                        texture = Game1.SprCharacterReacts;
                        frameX = 0;
                        frameY = FRAME_Y_JUMPKICK;
                        drawWidth = DRAW_WIDHT_JUMPKICK;
                        drawHeight = DRAW_HEIGHT_JUMPKICK;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);

                        //Playing Attack Sound
                        SoundManager.PlaySound(SoundEnum.ATTACK);
                    }

                    #endregion

                    jumpingPos += speed;
                    Position.X = jumpingPos.X;
                    speed.Y += 0.3f; // Slow Verticle speed down because of gravity


                    if (jumpingPos.Y > landingHeight + 20f) // Move we landed 
                    {
                        ResetIdleGraphic();
                        speed.Y = 0;
                        break;
                    }


                    ConstrainToScreen();


                    AnimateJumping(gT);
                    break;
                #endregion

                #region jumpkick
                case PlayerState.JumpKick:
                    jumpingPos += speed;
                    Position.X = jumpingPos.X;
                    speed.Y += 0.3f; // Slow Verticle speed down because of gravity
                    if (jumpingPos.Y >= landingHeight) // Move we landed 
                    {
                        ResetIdleGraphic();
                        speed.Y = 0;
                        break;
                    }

                    ConstrainToScreen();
                    AnimateJumpKick(gT);
                    break;
                #endregion

                #region Knife Throw
                case PlayerState.ThrownKnife:
                    AnimateThrowKnife(gT);
                    break;
                #endregion

                #region Kick TrashCan
                case PlayerState.KickObject:
                    AnimateKickObject(gT);
                    break;
                #endregion

                #region PickUp Items
                case PlayerState.PickUpItem:
                    AnimatePickUpItem(gT);
                    break;

                #endregion

                #region Attack 1 et caracteristiques Attack 2
                case PlayerState.Attack1:
                    stateTime += (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime < acceptAttack2Time) //   Still time to make a combo :)
                    {
                        if (InputHelper.WasKeyPressed(Keys.A) && GameManager.NumberPlayers == 1
                            || InputHelper.WasButtonPressed(pIndex, Buttons.X))
                        {
                            makeCombo = true;
                        }
                    }
                    else //stateTime is bigger then accptAttak2Time
                    {
                        if (makeCombo)
                        {
                            //Initialisation de la draw area
                            //La majorité des caractéristiques se trouvent dans AnimateAttack2
                            frameX = 0;

                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            SetDrawArea();

                            //Set attack-input times
                            stateTime = 0f;
                            state = PlayerState.Attack2;
                            makeCombo = false;

                            //Playing Attack Sound
                            SoundManager.PlaySound(SoundEnum.ATTACK);
                            return;
                        }
                    }
                    AnimateAttack1(gT);
                    break;
                #endregion

                #region Attack 2 et caracteristiques Attack 3
                case PlayerState.Attack2:
                    stateTime += (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime < acceptAttack2Time) //   Still time to make a combo :)
                    {
                        if (InputHelper.WasKeyPressed(Keys.A) && GameManager.NumberPlayers == 1
                            || InputHelper.WasButtonPressed(pIndex, Buttons.X))
                        {
                            makeCombo = true;
                        }
                    }
                    else //stateTime is bigger then accptAttak2Time
                    {
                        if (makeCombo)
                        {
                            //Setup the draw area   ( experimental )
                            texture = Game1.SprCharacterReacts;
                            drawWidth = DRAW_WIDHT_ATTACK3;
                            drawHeight = DRAW_HEIGHT_KICK;
                            frameX = 0;
                            frameY = FRAME_Y_ATTACK3;
                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            SetDrawArea();

                            //Set attack-input times
                            stateTime = 0f;
                            state = PlayerState.Attack3;
                            makeCombo = false;

                            //Playing Attack Sound
                            SoundManager.PlaySound(SoundEnum.ATTACK);
                            return;
                        }
                    }
                    AnimateAttack2(gT);
                    break;
                #endregion

                #region Attack 3
                case PlayerState.Attack3:
                    AnimateAttack3(gT);
                    break;
                #endregion

                #region Take Hit and Die circle

                case PlayerState.TakeHit:
                    AnimateTakeHit(gT);
                    this.IsAttacked = true;
                    break;


                case PlayerState.KnockedDown:
                    AnimateKnockDown(gT);
                    break;


                case PlayerState.Down:
                    stateTime += (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime >= Actor.DOWN_TIME)
                    {
                        //Setup the GettingUp animation
                        state = PlayerState.GettingUp;
                        currentFrameTime = 0f;
                        frameX = 0;
                        frameY = FRAME_Y_GETUP;
                        drawWidth = DRAW_WIDTH_GETUP;
                        drawHeight = DRAW_HEIGHT_GETUP;
                        SetDrawArea();
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    }
                    break;


                case PlayerState.GettingUp:
                    AnimateGettingUp(gT);
                    break;

                case PlayerState.Dying:
                    //Flash body a few times
                    stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;

                    if (stateTime <= 0)
                    {
                        if (GameManager.NumberPlayers == 1)
                        {

                            stateTime = DEATH_FLASH_TIME;

                            IsVisible = !IsVisible;
                            DeathFlashes++;

                            if (DeathFlashes >= 8)
                            {
                                if (NombreDeVie <= 0)
                                {
                                    InLevel.LevelState = LevelState.GameOver;
                                }
                                else
                                {
                                    //Actor is dead enough
                                    InLevel.LevelState = LevelState.Continue;
                                    InLevel.TimerContinue -= (float)gT.ElapsedGameTime.TotalSeconds;
                                    HUDManager.Opacity = 0f;
                                }
                            }
                        }
                        else if (GameManager.NumberPlayers == 2)
                        {
                            stateTime = DEATH_FLASH_TIME;

                            if (InLevel.TimerContinue_p1 >= 0)
                            {
                                IsVisible = !IsVisible;
                                DeathFlashes++;
                            }
                            else
                            {
                                IsVisible = false;
                                InLevel.Player1.RemoveActorFromLevel();
                            }

                            if (DeathFlashes >= 3)
                            {
                                if (NombreDeVie > 0)
                                {
                                    //Actor is dead enough
                                    InLevel.TimerContinue_p1 -= (float)gT.ElapsedGameTime.TotalSeconds;
                                }
                                else
                                {
                                    IsVisible = false;
                                    InLevel.Player1.RemoveActorFromLevel();
                                }
                            }
                        }
                    }
                    break;
                #endregion


                #region Level intro
                case PlayerState.LevelIntro:
                    if (this.Position != targetPosition)
                    {
                        //Move to the targetPositiop,
                        LineUpXTargetPos();
                        LineUpYTargetPos();

                        AnimateWalking(gT);
                    }
                    else
                    {
                        ResetIdleGraphic();
                        InLevel.LevelState = LevelState.CutScene;
                        InLevel.CutScenes[InLevel.CurrentCutScene].PlayFirstLine();

                    }
                    break;
                #endregion

                #endregion
            }
        }


        public override void Draw(SpriteBatch SB)
        {
            if (IsVisible)
            {
                //Draw Character
                // Are we jumping?

                if (state == PlayerState.Jumping || state == PlayerState.JumpKick)
                {
                    //Facing Left or Right?
                    if (FacingDir == Direction.Right)
                        SB.Draw(texture, Camera.GetScreenPosition(jumpingPos), DrawArea, Color.White,
                            0f, originCharacter, 1f, SpriteEffects.None, LayerDepth);
                    else // We must be facing to the left!
                        SB.Draw(texture, Camera.GetScreenPosition(jumpingPos), DrawArea, Color.White,
                            0f, originCharacter, 1f, SpriteEffects.FlipHorizontally, LayerDepth);

                }
                else // Wz
                {
                    //Facing Left or Right?
                    if (FacingDir == Direction.Right)
                        SB.Draw(texture, Camera.GetScreenPosition(Position), DrawArea, Color.White,
                            0f, originCharacter, 1f, SpriteEffects.None, LayerDepth);
                    else // We must be facing to the left!
                        SB.Draw(texture, Camera.GetScreenPosition(Position), DrawArea, Color.White,
                            0f, originCharacter, 1f, SpriteEffects.FlipHorizontally, LayerDepth);
                }
            }

            //Draw Shadow
            base.Draw(SB);


        }

        #region Utilities

        public void GiveKnife()
        {
            this.CarryingKnife = true;
        }

        private void Move(GameTime gT)
        {
            #region X-Mouvements
            //mouvement gauche
            if (InputHelper.NGS[(int)pIndex].ThumbSticks.Left.X < -MOVEMENT_THRESHOLD
                || InputHelper.IsKeyHeld(Keys.Left) && GameManager.NumberPlayers == 1)
            {
                speed.X = -3;
                FacingDir = Direction.Left;
                state = PlayerState.Walking;
                drawWidth = DRAW_WIDHT_WALK;
                drawHeight = DRAW_HEIGHT_WALK;

            }

            //mouvement droit
            else if (InputHelper.NGS[(int)pIndex].ThumbSticks.Left.X > MOVEMENT_THRESHOLD
                || InputHelper.IsKeyHeld(Keys.Right) && GameManager.NumberPlayers == 1)
            {
                speed.X = 3;
                FacingDir = Direction.Right;
                state = PlayerState.Walking;

                drawWidth = DRAW_WIDHT_WALK;
                drawHeight = DRAW_HEIGHT_WALK;
            }

            else
            {
                speed.X = 0;
            }

            #endregion

            #region Y-Mouvements
            //if mouvement bas
            if (InputHelper.NGS[(int)pIndex].ThumbSticks.Left.Y < -MOVEMENT_THRESHOLD
                || InputHelper.IsKeyHeld(Keys.Down) && GameManager.NumberPlayers == 1)
            {
                speed.Y = 2;
                state = PlayerState.Walking;
                drawWidth = DRAW_WIDHT_WALK;
                drawHeight = DRAW_HEIGHT_WALK;
            }

            //mouvement haut
            else if (InputHelper.NGS[(int)pIndex].ThumbSticks.Left.Y > MOVEMENT_THRESHOLD
                || InputHelper.IsKeyHeld(Keys.Up) && GameManager.NumberPlayers == 1)
            {
                speed.Y = -2;
                state = PlayerState.Walking;
                drawWidth = DRAW_WIDHT_WALK;
                drawHeight = 212;
            }

            else
            {
                speed.Y = 0;
            }
            #endregion



            if (speed == Vector2.Zero)
            {
                //permet de lancer l'animation au repos
                frameY = FRAME_Y_IDLE;
                state = PlayerState.Idle;
                drawHeight = DRAW_HEIGHT_IDLE;
                drawWidth = DRAW_WIDHT_IDLE;
                originCharacter = new Vector2(drawWidth / 2, drawHeight);
                AnimateIdle(gT);
            }
            else // ce qui veut dire que le player est en mouvement
            {

                GetLayerDepth();
                ConstrainToScreen();
                AnimateWalking(gT);
                Position += speed;

            }
        }

        private void PickUpItem(GameItem gameItem)
        {
            //Pick it up !!
            //setup the animation
            this.aboutToUse = gameItem;
            this.state = PlayerState.PickUpItem;
            texture = Game1.SprCharacterReacts;
            currentFrameTime = 0f;
            frameX = 0;
            frameY = FRAME_Y_PICKUP_ITEM;//1100
            drawWidth = DRAW_WIDTH_PICKUP_ITEM;//132
            drawHeight = DRAW_HEIGHT_PICKUP_ITEM;//145
            originCharacter = new Vector2(drawWidth / 2, drawHeight);
            SetDrawArea();
        }

        private void ThrowKnife()
        {
            //Lancer un couteau
            this.InLevel.GameItems.Add(new Knife(
                this.Position, this.FacingDir, this.InLevel, this));

            this.CarryingKnife = false;//player is no longer carrying knife
            //Le son du couteau
            SoundManager.PlaySound(SoundEnum.KNIFE_THROW);
        }

        public void Continue()
        {
            this.Health = STARTING_HEALTH;

            //Setup the "Getting up" animation"
            this.state = PlayerState.GettingUp;
            texture = Game1.SprCharacterReacts;
            currentFrameTime = 0f;
            frameX = 0;
            frameY = FRAME_Y_GETUP;
            drawWidth = DRAW_WIDTH_GETUP;
            drawHeight = DRAW_HEIGHT_GETUP;
            SetDrawArea();
        }

        public void SetIntroTargetPosition(Vector2 targetPosition)
        {
            this.targetPosition = targetPosition;
            this.state = PlayerState.LevelIntro;
            this.InLevel.LevelState = LevelState.Intro;
            //Setup walking animation
            this.drawWidth = DRAW_WIDHT_WALK;
            this.drawHeight = DRAW_HEIGHT_WALK;
            this.frameX = 0;
            this.frameY = FRAME_Y_WALK;
            this.texture = Game1.SprCharacterAttacks;
            SetDrawArea();
        }

        private void LineUpXTargetPos()
        {
            //Le player est-il à gauche ou à droite
            //Position maximale = player1 - 80px car il peut être atteint à cette distance
            if (targetPosition.X < this.Position.X) // La cible est à gauche
            {
                //Bouger à gauche
                this.Position.X -= 3f;
                FacingDir = Direction.Left;
                //On retourne faux donc car on n'est pas aligné avec la cible
                if (targetPosition.X >= this.Position.X)
                {
                    this.Position.X = targetPosition.X;
                }
            }
            else if (targetPosition.X > this.Position.X) //La cible est à droite
            {

                //Bouger à droite
                this.Position.X += 2.5f;
                FacingDir = Direction.Right;
                if (targetPosition.X <= this.Position.X)
                {
                    this.Position.X = targetPosition.X;
                }
            }

        }

        private void LineUpYTargetPos()
        {

            if (targetPosition.Y < this.Position.Y) // La cible est au dessus
            {
                //Bouger l'ennemi vers le haut
                this.Position.Y -= 2f;
                if (targetPosition.Y >= this.Position.Y)
                {
                    this.Position.Y = targetPosition.Y;
                }

            }
            else if (targetPosition.Y > this.Position.Y) // La cible est en dessous
            {
                //L'ennemi bouge vers le bas
                this.Position.Y += 2f;
                if (targetPosition.Y <= this.Position.Y)
                {
                    this.Position.Y = targetPosition.Y;
                }
            }

        }
        #endregion

        #region Collision Detection
        public override void UpdateHitArea()
        {
            HitArea = drawWidth / 2;
        }

        private void CheckEnemyCollision()
        {
            UpdateHitArea();

            for (int i = InLevel.Actors.Count - 1; i >= 0; i--)
            {
                Actor actor;

                //Make sure are not looking at ourself;
                actor = InLevel.Actors[i] as Player;
                if (actor == this)
                    continue;

                //Are we looking at a Ranged enemy?
                actor = InLevel.Actors[i] as EnemyRanged;
                if (actor != null)
                {
                    //Update the current actors Hit Area
                    actor.UpdateHitArea();

                    //1)    Is actor attackable ?
                    if (actor.IsAttackable)
                    {
                        //2) Are we within Y-Range?
                        if (actor.Position.Y > this.Position.Y - HIT_Y_RANGE
                            && actor.Position.Y < this.Position.Y + HIT_Y_RANGE)
                        {
                            //3) Which way is the player facing?
                            if (this.FacingDir == Direction.Left)
                            {
                                //4) Is actor in front of us ? ** LEFT **
                                if (actor.Position.X < this.Position.X)
                                {
                                    //5) Player LEFT edge <MORE LEFT> then actors RIGHT?
                                    if (this.Position.X - HitArea < actor.Position.X + actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                            //3) Which way is the player facing?
                            else //  Player is facing RIGHT
                            {
                                //4) Is actor in front of us ? ** RIGHT **
                                if (actor.Position.X > this.Position.X)
                                {
                                    //5) Player RIGHT edge <MORE RIGHT> then actors LEFT?
                                    if (this.Position.X + HitArea > actor.Position.X - actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                        }
                    }
                }

                //Are we looking at an enemy?
                actor = InLevel.Actors[i] as EnemyClose;
                if (actor != null)
                {
                    //Update the current actors Hit Area
                    actor.UpdateHitArea();

                    //1)    Is actor attackable ?
                    if (actor.IsAttackable)
                    {
                        //2) Are we within Y-Range?
                        if (actor.Position.Y > this.Position.Y - HIT_Y_RANGE
                            && actor.Position.Y < this.Position.Y + HIT_Y_RANGE)
                        {
                            //3) Which way is the player facing?
                            if (this.FacingDir == Direction.Left)
                            {
                                //4) Is actor in front of us ? ** LEFT **
                                if (actor.Position.X < this.Position.X)
                                {
                                    //5) Player LEFT edge <MORE LEFT> then actors RIGHT?
                                    if (this.Position.X - HitArea < actor.Position.X + actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                            //3) Which way is the player facing?
                            else //  Player is facing RIGHT
                            {
                                //4) Is actor in front of us ? ** RIGHT **
                                if (actor.Position.X > this.Position.X)
                                {
                                    //5) Player RIGHT edge <MORE RIGHT> then actors LEFT?
                                    if (this.Position.X + HitArea > actor.Position.X - actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                        }
                    }
                }



                //Are we looking at an enemy?
                actor = InLevel.Actors[i] as EnemyClose2;
                if (actor != null)
                {
                    //Update the current actors Hit Area
                    actor.UpdateHitArea();

                    //1)    Is actor attackable ?
                    if (actor.IsAttackable)
                    {
                        //2) Are we within Y-Range?
                        if (actor.Position.Y > this.Position.Y - HIT_Y_RANGE
                            && actor.Position.Y < this.Position.Y + HIT_Y_RANGE)
                        {
                            //3) Which way is the player facing?
                            if (this.FacingDir == Direction.Left)
                            {
                                //4) Is actor in front of us ? ** LEFT **
                                if (actor.Position.X < this.Position.X)
                                {
                                    //5) Player LEFT edge <MORE LEFT> then actors RIGHT?
                                    if (this.Position.X - HitArea < actor.Position.X + actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                            //3) Which way is the player facing?
                            else //  Player is facing RIGHT
                            {
                                //4) Is actor in front of us ? ** RIGHT **
                                if (actor.Position.X > this.Position.X)
                                {
                                    //5) Player RIGHT edge <MORE RIGHT> then actors LEFT?
                                    if (this.Position.X + HitArea > actor.Position.X - actor.HitArea)
                                    {
                                        //Great, HIT THEM !
                                        HitSomeone(actor);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HitSomeone(Actor whoToHit)  // ICI POUR LES POINTS DE DAMAGE
        {
            switch (state)
            {
                case PlayerState.Attack1:
                case PlayerState.Attack2:
                    //15 <= nombre de dommages
                    whoToHit.GetHit(FacingDir, 15);
                    // whoToHit.GetHit(FacingDir, 100);
                    break;

                case PlayerState.Attack3:
                    if (whoToHit.IsBoss)
                        whoToHit.GetHit(FacingDir, 25);
                    else
                        whoToHit.GetKnockedDown(FacingDir, 25);
                    break;

                case PlayerState.JumpKick:
                    whoToHit.GetKnockedDown(FacingDir, 10);
                    break;
            }
        }

        public override void GetHit(Direction cameFrom, int damage)
        {
            this.Health -= damage;
            if (CheckForDeath())
            {
                GetKnockedDown(cameFrom, 0);
                return;
            }



            //Set state and texture;
            state = PlayerState.TakeHit;
            texture = Game1.SprCharacterReacts;
            frameX = 0;
            frameY = FRAME_Y_TAKEHIT;
            drawWidth = DRAW_WIDTH_TAKEHIT;
            drawHeight = DRAW_HEIGHT_TAKEHIT;
            SetDrawArea();

            //Play TakeHit sound
            SoundManager.PlaySound(SoundEnum.TAKE_HIT);
            


            //Face the man who BEAT YOU!!
            if (cameFrom == Direction.Right)
                FacingDir = Direction.Left;
            else
                FacingDir = Direction.Right;

        }

        public override void GetKnockedDown(Direction cameFrom, int damage)
        {
            //Set state and texture
            IsAttackable = false;
            state = PlayerState.KnockedDown;
            texture = Game1.SprCharacterReacts;
            frameX = 0;
            frameY = FRAME_Y_KNOCKEDOWN;
            drawWidth = DRAW_WIDTH_KNOCKEDOWN;
            drawHeight = DRAW_HEIGHT_KNOCKEDOWN;
            SetDrawArea();

            //Play TakeHit sound
            SoundManager.PlaySound(SoundEnum.TAKE_HIT);

            //Face the man who BEAT YOU!!
            if (cameFrom == Direction.Right)
                FacingDir = Direction.Left;
            else
                FacingDir = Direction.Right;

            this.Health -= damage;
        }

        #endregion

        #region Animations

        /// <summary>
        /// Relance l'état de repos en attendant l'intéraction avec le joueur
        /// </summary>
        public void ResetIdleGraphic()
        {
            this.IsAttackable = true;

            texture = Game1.SprCharacterAttacks;
            state = PlayerState.Idle;

            currentFrameTime = 0f;
            frameX = 0;
            frameY = FRAME_Y_IDLE;
            drawWidth = DRAW_WIDHT_IDLE;
            drawHeight = DRAW_HEIGHT_IDLE;
            SetDrawArea();
            originCharacter = new Vector2(drawWidth / 2, drawHeight);
            this.HitArea = drawWidth / 2;
        }

        private void SetDrawArea()
        {
            // changement de méthode pour frameY par rapport à la vidéo, en effet, ce n'est
            // pas assez précis, frameY est donc ici la coordonnée exacte du début du sprite
            DrawArea = new Rectangle(frameX * drawWidth,
                frameY, drawWidth, drawHeight);
        }

        private void AnimateIdle(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos
                if (frameX > 8)//9 normalement
                    frameX = 0;

                SetDrawArea();
            }
        }

        private void AnimateWalking(GameTime gT)
        {
            if (frameY == FRAME_Y_WALK)//570
                originCharacter = new Vector2(drawWidth / 2, drawHeight);
            else
                originCharacter = new Vector2(drawWidth / 2 - 21, drawHeight);

            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds * 1.5f; // (* 1.5f) permet d'accelerer la vitesse de défilement
            if (currentFrameTime >= Actor.FrameRate)
            {
                // coordonnée exacte axe Y
                frameY = FRAME_Y_WALK;

                currentFrameTime = 0;
                frameX++;

                if (frameX > 8)//10
                {
                    frameX = 0;
                }
                SetDrawArea();

            }

        }

        private void AnimateJumping(GameTime gT)
        {
            drawHeight = 306;//250

            originCharacter = new Vector2(drawWidth / 2, drawHeight);
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds /* 2.3f*/;
            if (currentFrameTime >= Actor.FrameRate)
            {

                currentFrameTime = 0f;
                frameX++;

                if (frameX > 9)//16
                {

                    //a revoir, permet de savoir que faire lorsque le dernier
                    // sprite est atteint, si on est encore en l'air que faire
                    frameX = 0;
                    return;
                }


                DrawArea = new Rectangle(frameX * drawWidth,
                   413/*750*/, drawWidth, drawHeight);
            }
        }

        private void AnimateJumpKick(GameTime gT)
        {

            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds / 4;
            if (currentFrameTime >= Actor.FrameRate)
            {
                currentFrameTime = 0f;
                frameX++;
                if (frameX > 2)//4
                {
                    state = PlayerState.Jumping;
                    drawHeight = DRAW_HEIGHT_JUMPKICK;//250
                    drawWidth = DRAW_WIDHT_JUMPKICK;
                    frameX = 7;//6
                    frameY = FRAME_Y_JUMPKICK;//750
                }

                //Collision Detection
                if (frameX == 2)
                    CheckEnemyCollision();

                SetDrawArea();
            }
        }

        private void AnimateAttack1(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds / 1.3f;
            if (currentFrameTime >= Actor.FrameRate)
            {
                drawWidth = DRAW_WIDHT_ATTACK1;
                drawHeight = DRAW_HEIGHT_ATTACK1;//138
                frameY = FRAME_Y_ATTACK1;//1437

                currentFrameTime = 0f;
                frameX++;

                //nombres d'images pour le repos
                if (frameX > 5)//7
                {
                    ResetIdleGraphic();
                    return;
                }

                //Collision Detection
                if (frameX == 4)
                    CheckEnemyCollision();

                SetDrawArea();
            }
        }

        private void AnimateAttack2(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds / 2f;
            if (currentFrameTime >= Actor.FrameRate)
            {
                drawWidth = DRAW_WIDHT_ATTACK2;
                drawHeight = DRAW_HEIGHT_ATTACK2;
                frameY = FRAME_Y_ATTACK2;//1578

                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos
                if (frameX > 4)//11
                {
                    ResetIdleGraphic();
                    return;
                }

                //Collision Detection
                if (frameX == 3)//5
                    CheckEnemyCollision();

                SetDrawArea();
            }
        }

        private void AnimateAttack3(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate) // 20% plus rapide
            {



                currentFrameTime = 0f;
                frameX++;

                //nombres d'images avant d'atteindre le repos
                if (frameX > 5)//12
                {
                    ResetIdleGraphic();
                    return;
                }

                //Collision Detection
                if (frameX == 3)//7
                    CheckEnemyCollision();

                SetDrawArea();
            }
        }

        private void AnimateThrowKnife(GameTime gT)
        {

            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {
                currentFrameTime = 0f;
                frameX++;


                if (frameX > 3)
                {
                    ResetIdleGraphic();
                    return;
                }

                //Do we throw the knife ?
                if (frameX == 3)
                {
                    ThrowKnife();
                }
                SetDrawArea();

            }
        }

        private void AnimateKickObject(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {

                currentFrameTime = 0f;
                frameX++;

                //finished animation?
                if (frameX > 9)
                {
                    ResetIdleGraphic();
                    return;
                }

                //correct frame to kick object
                if (frameX == 5)
                {
                    this.aboutToUse.TakeHit(this.FacingDir);
                    this.aboutToUse = null;
                }

                SetDrawArea();
            }
        }

        private void AnimatePickUpItem(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {

                currentFrameTime = 0f;
                frameX++;

                //finished animation?
                if (frameX > 10)//4
                {
                    ResetIdleGraphic();
                    return;
                }

                //correct frame to pick object
                if (frameX == 5)
                {
                    this.aboutToUse.GetPickedUp(this);
                    this.aboutToUse = null;
                }

                SetDrawArea();
            }
        }

        private void AnimateTakeHit(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos
                if (frameX > 5)//3
                {
                    ResetIdleGraphic();
                    return;
                }
                SetDrawArea();
            }
        }

        private void AnimateKnockDown(GameTime gT)
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
                            // on retire ici le nombre de vie
                            this.NombreDeVie--;
                            state = PlayerState.Dying;
                            SoundManager.PlaySound(SoundEnum.ACTOR_DEATH);
                            stateTime = 1f;
                            frameX = 5;
                            return;
                        }

                        // on a finit, on change alors d'état
                        state = PlayerState.Down;
                        return;

                    }
                }

                SetDrawArea();

                currentFrameTime = 0f;
                frameX++;

            }

        }

        private void AnimateGettingUp(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
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

        #endregion


    }

}

