using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{

    enum EnemyCloseState2
    {
        //Take Hit Cycle
        //Idle n'est pu necessaire
        TakeHit,
        KnockedDown,
        Down,
        GettingUp,
        Dying, //lorsqu'il meurt

        //AI
        Retreat,
        MoveTo,
        PreAttack,
        Attack,
        Wait
    }

    class EnemyClose2 : Actor
    {
        #region drawconstants
        protected int DRAW_HEIGHT_IDLE;
        protected int DRAW_HEIGHT_WALK;
        protected int DRAW_HEIGHT_ATTACK1;
        protected int DRAW_HEIGHT_ATTACK2;
        protected int DRAW_HEIGHT_ATTACK3;
        protected int DRAW_HEIGHT_KICK;
        protected int DRAW_HEIGHT_ARMSFOLDED;
        protected int DRAW_HEIGHT_TAKEHIT;
        protected int DRAW_HEIGHT_KNOCKEDOWN;
        protected int DRAW_HEIGHT_GETUP;

        protected int DRAW_WIDHT_IDLE;
        protected int DRAW_WIDHT_WALK;
        protected int DRAW_WIDHT_ATTACK1;
        protected int DRAW_WIDHT_ATTACK2;
        protected int DRAW_WIDHT_ATTACK3;
        protected int DRAW_WIDTH_ARMSFOLDED;
        protected int DRAW_WIDTH_TAKEHIT;
        protected int DRAW_WIDTH_KNOCKEDOWN;
        protected int DRAW_WIDTH_GETUP;

        protected int FRAME_Y_IDLE;
        protected int FRAME_Y_WALK;
        protected int FRAME_Y_ATTACK1;
        protected int FRAME_Y_ATTACK2;
        protected int FRAME_Y_ATTACK3;
        protected int FRAME_Y_ARMSFOLDED;
        protected int FRAME_Y_TAKEHIT;
        protected int FRAME_Y_KNOCKEDOWN;
        protected int FRAME_Y_GETUP;

        protected int FRAME_X_IDLE;
        protected int FRAME_X_WALK;
        protected int FRAME_X_ATTACK1;
        protected int FRAME_X_ATTACK1_COLLISION;
        protected int FRAME_X_ATTACK2;
        protected int FRAME_X_ATTACK2_COLLISION;
        protected int FRAME_X_ATTACK3;
        protected int FRAME_X_ATTACK3_COLLISION;
        protected int FRAME_X_ARMSFOLDED;
        protected int FRAME_X_TAKEHIT;
        protected int FRAME_X_KNOCKEDOWN;
        protected int FRAME_X_GETUP;
        protected int FRAME_X_JUMP;

        protected Texture2D TEXTURE_ATTACK;
        protected Texture2D TEXTURE_REACTS;
        #endregion

        #region constants
        protected const int HEALTH_BAR_WIDTH = 60;
        protected const int HEALTH_BAR_HEIGHT = 15;
        protected  const float STARTING_HEALTH = 100;

        #endregion

        #region variables

       protected EnemyCloseState2 state;

        //TextureDetails
        Texture2D texture = Game1.SprCharacterAttacks;
        Vector2 originCharacter;
        Rectangle DrawArea;
        protected float currentFrameTime;
      protected  int drawWidth;
       protected int drawHeight;
       protected int frameX;
        protected int frameY;
        protected Color drawColor;
        protected Player playerToAttack;

        //Movement
       protected float stateTime;
        Vector2 retreatTarget;
        int attackNumber;

        #endregion

        /// <summary>
        /// Create an ennemy at a specified position
        /// </summary>
        public EnemyClose2(Vector2 position, Level inLevel)
            : base(position, inLevel)
        {
            ResetIdleGraphic();
            drawColor = Color.White; //Set the draw Color
            this.Health = 300;//STARTING_HEALTH;
            this.IsBoss = true;
        }


        /// <summary>
        /// Create an ennemy to spawn on a particular side of the screen
        /// </summary>
        public EnemyClose2(Direction startingSide, Level inLevel)
            : base(Vector2.Zero, inLevel)
        {
            this.FacingDir = startingSide;

            ResetIdleGraphic();
            drawColor = Color.White; //Set the draw Color
            this.Health = 300;
            this.IsBoss = true;
        }

        public void SetToArmsFolded(Direction facingDir)
        {
            texture = TEXTURE_ATTACK;
            drawWidth = DRAW_WIDTH_ARMSFOLDED;
            drawHeight = DRAW_HEIGHT_ARMSFOLDED;
            frameX = 0;
            frameY = FRAME_Y_ARMSFOLDED;
            SetDrawArea();
            state = EnemyCloseState2.Wait;
            this.originCharacter = new Vector2(drawWidth / 2, drawHeight);
            this.FacingDir = facingDir;
        }

        public override void Update(GameTime gT)
        {
            if (GameManager.NumberPlayers == 2)
                playerToAttack = ChoosePlayerToAttack(InLevel.Player1, InLevel.Player2);
            else
                playerToAttack = InLevel.Player1;


            switch (state)
            {
                #region Retreat state
                case EnemyCloseState2.Retreat: // l'enemi est dans un état de fuite, de retraite ( = retreat) 
                    stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime <= 0)
                    {
                        //Run out of time
                        DecideWhatToDo();
                    }
                    else
                    {
                        //Retreat to retreaTarget ( ici on va deplacer l'enemi) 
                        //Move to X target 
                        if (Position.X < retreatTarget.X) // L'enemi est a DROITE de son point de retraite
                        {
                            this.Position.X += 3;
                            if (Position.X > retreatTarget.X)
                                Position.X = retreatTarget.X; // il n'ira pas trop loin à droite^^
                        }
                        else if (Position.X > retreatTarget.X)// L'enemi est a GAUCHE de son point de retraite
                        {
                            this.Position.X -= 3;
                            if (Position.X < retreatTarget.X)
                                Position.X = retreatTarget.X; // il n'ira pas trop loin là aussi
                        }

                        //Move to Y location 
                        if (Position.Y < retreatTarget.Y) // L'enemi est en haut de son point de retraite
                        {
                            this.Position.Y += 2f;
                            if (Position.Y > retreatTarget.Y)
                                Position.Y = retreatTarget.Y; // il n'ira pas trop loin en bas^^
                        }
                        else if (Position.Y > retreatTarget.X)// L'enemi est en bas de son point de retraite
                        {
                            this.Position.Y -= 2f;
                            if (Position.Y < retreatTarget.Y)
                                Position.Y = retreatTarget.Y; // il n'ira pas trop loin en haut de son point de retraite là aussi
                        }

                        //Make sure that this enemy is ALWAYS facing the player
                        if (this.Position.X < playerToAttack.Position.X) // Le joueur est a sa droite
                            this.FacingDir = Direction.Right;
                        else // le jouest a sa gauche
                            this.FacingDir = Direction.Left;

                        //Which animation to use?  
                        if (this.Position == retreatTarget)
                        {
                            //At Location. Idle  // ici l'énemi ne bouge pas, il faut donc prendre le dessin lorsqu'il est inactif, en gros lorsqu'il "dance" parce qu'il est pret au combat
                            frameY = FRAME_Y_IDLE;
                            drawWidth = DRAW_WIDHT_IDLE;
                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            AnimateIdle(gT);
                        }
                        else
                        {
                            //Not at location. Animate the Walk // ici l'enemi se deplace, il faut donc prendre le dessin lorsqu'il marche
                            drawWidth = DRAW_WIDHT_WALK;
                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            AnimateWalking(gT); // Nouvelle animation de l'énemi (car avant il ne se deplacait pas), elle a été copier-collé de celle du player
                        }
                    }
                    break;
                #endregion

                #region MoveTo
                case EnemyCloseState2.MoveTo:
                    //sommes-nous alignés avec le joueur
                    bool linedUpX = LinedUpXWithPlayer();
                    bool linedUpY = LinedUpYWithPlayer();

                    if (linedUpX && linedUpY)
                    {
                        //Setup PreAttack state
                        frameX = 0;
                        frameY = 0;

                        state = EnemyCloseState2.PreAttack;
                        drawWidth = DRAW_WIDHT_IDLE;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                        SetDrawArea();

                        // how long do we stay in the PreAttack state for ?
                        stateTime = 0;//.5f * (float)Game1.Random.NextDouble();

                        break;
                    }

                    AnimateWalking(gT);
                    break;
                #endregion

                #region PreAttack
                case EnemyCloseState2.PreAttack:
                    //Am I still lined up with the player ?
                    if (LinedUpXWithPlayer() && LinedUpYWithPlayer())
                    {
                        // Have we bee in PreAttack state long enough
                        stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;
                        if (stateTime < 0)
                        {
                            //Is plauyer attackable ?
                            if (!playerToAttack.IsAttackable)
                            {
                                GetRetreatTarget();
                                break;
                            }

                            if (NoOtherEnemiesAttack())
                            {
                                //It's time to ATTACK!!!!!
                                texture = TEXTURE_ATTACK;
                                frameX = 0;
                                drawWidth = DRAW_WIDHT_ATTACK1;
                                drawHeight = DRAW_HEIGHT_ATTACK1;
                                frameY = FRAME_Y_ATTACK1;

                                originCharacter = new Vector2(drawWidth / 2, drawHeight);
                                SetDrawArea();
                                state = EnemyCloseState2.Attack;
                                SoundManager.PlaySound(SoundEnum.ATTACK);

                                attackNumber = 1;
                                this.IsAttacking = true;
                            }
                        }
                    }
                    else
                    {
                        //we are not lined up with the player :(
                        state = EnemyCloseState2.MoveTo;
                        frameX = 0;
                        drawWidth = DRAW_WIDHT_WALK;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    }

                    AnimateIdle(gT);
                    break;
                #endregion

                #region Attacks
                case EnemyCloseState2.Attack:
                    switch (attackNumber)
                    {
                        case 1:
                            AnimateAttack1(gT);
                            break;

                        case 2:
                            AnimateAttack2(gT);
                            break;

                        case 3:
                            AnimateAttack3(gT);
                            break;
                    }
                    break;
                #endregion

                #region Take Hit and Die circle

                case EnemyCloseState2.TakeHit:
                    AnimateTakeHit(gT);
                    break;


                case EnemyCloseState2.KnockedDown:
                    AnimateKnockDown(gT);
                    break;


                case EnemyCloseState2.Down:
                    stateTime += (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime >= Actor.DOWN_TIME)
                    {
                        //Setup the GettingUp animation
                        state = EnemyCloseState2.GettingUp;
                        currentFrameTime = 0f;
                        frameX = 0;
                        frameY = FRAME_Y_GETUP;
                        drawWidth = DRAW_WIDTH_GETUP;
                        drawHeight = DRAW_HEIGHT_GETUP;
                        SetDrawArea();
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    }
                    break;


                case EnemyCloseState2.GettingUp:
                    AnimateGettingUp(gT);
                    break;

                case EnemyCloseState2.Dying:
                    //Flash body a few times
                    stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime <= 0)
                    {
                        stateTime = DEATH_FLASH_TIME;
                        IsVisible = !IsVisible;
                        DeathFlashes++;


                        if (DeathFlashes >= 8)
                        {
                            //Actor is dead enough
                            RemoveActorFromLevel(); //ON FAIT DISPARAITRE LE CADAVRE!!
                        }
                    }
                    break;
                #endregion

            }
        }

        public override void Draw(SpriteBatch SB)
        {
            Vector2 drawPos = Camera.GetScreenPosition(Position);
            GetLayerDepth();

            if (IsVisible)
            {
                //Facing Left or Right?
                if (FacingDir == Direction.Right)
                    SB.Draw(texture, drawPos, DrawArea, drawColor,
                        0f, originCharacter, 1f, SpriteEffects.None, LayerDepth);
                else // We must be facing to the left!
                    SB.Draw(texture, drawPos, DrawArea, drawColor,
                        0f, originCharacter, 1f, SpriteEffects.FlipHorizontally, LayerDepth);

            }

            #region HealthBar


            // Red Health Bar
            SB.Draw(Game1.SprSinglePixel, new Vector2(drawPos.X - HEALTH_BAR_WIDTH - HEALTH_BAR_WIDTH / 2, drawPos.Y - drawHeight - HEALTH_BAR_HEIGHT),
                new Rectangle(0, 0, HEALTH_BAR_WIDTH * 3 + 2, HEALTH_BAR_HEIGHT + 2), new Color(Color.DarkRed, 0.4f), 0f,
                Vector2.Zero, 1f, SpriteEffects.None, this.LayerDepth + 0.0001f);

            // How long do we draw the Enemy's Health Bar?
            float percent = this.Health / STARTING_HEALTH;
            int drawWidth = (int)(percent * HEALTH_BAR_WIDTH);

            //Yellow Health Bar
            SB.Draw(Game1.SprSinglePixel, new Vector2(drawPos.X - HEALTH_BAR_WIDTH - HEALTH_BAR_WIDTH / 2 + 1, drawPos.Y - drawHeight - HEALTH_BAR_HEIGHT + 1),
                new Rectangle(0, 0, drawWidth, HEALTH_BAR_HEIGHT), new Color(Color.DarkSeaGreen, 0.4f), 0f,
                Vector2.Zero, 1f, SpriteEffects.None, this.LayerDepth);

            #endregion

            //draw actor shadow
            base.Draw(SB);
        }

        public override void DrawInDoorway(SpriteBatch SB, float layerDepth)
        {
            Vector2 drawPos = Camera.GetScreenPosition(Position);

            if (FacingDir == Direction.Right)
            {
                SB.Draw(texture, drawPos, DrawArea, drawColor,
                    0f, originCharacter, 1f, SpriteEffects.None, layerDepth);
            }
            else
            {

                SB.Draw(texture, drawPos, DrawArea, drawColor,
                    0f, originCharacter, 1f, SpriteEffects.FlipHorizontally, layerDepth);
            }
        }


        #region AI Methods
        public void ResetIdleGraphic()
        {
            //this.IsAttacking = false;

            texture = TEXTURE_ATTACK;
            IsAttackable = true;

            currentFrameTime = 0f;
            frameX = 0;
            frameY = FRAME_Y_IDLE;
            drawWidth = DRAW_WIDHT_IDLE;
            drawHeight = DRAW_HEIGHT_IDLE;
            SetDrawArea();
            originCharacter = new Vector2(drawWidth / 2, drawHeight);

            this.HitArea = drawWidth / 2;

            if (playerToAttack != null)
                DecideWhatToDo();
        }

        private void DecideWhatToDo()
        {
            if (Game1.Random.NextDouble() < 0.5d)
            {
                //Decide to retreat
                GetRetreatTarget();

                //Set time to be in Retreat State
                stateTime = 0;//(float)(Game1.Random.NextDouble() + 1.5);
            }
            else
            {
                //Decide to attack
                this.state = EnemyCloseState2.MoveTo;
                drawWidth = DRAW_WIDHT_WALK;
                frameX = 0;

            }
        }

        private void GetRetreatTarget()
        {
            state = EnemyCloseState2.Retreat;

            //Retreat to wich side of the player ?
            if (Game1.Random.NextDouble() < 0.5d)
            {
                //Go to LEFT side of Player
                retreatTarget.X = Game1.Random.Next(
                                    (int)(playerToAttack.Position.X - 200),
                                    (int)(playerToAttack.Position.X - 100));

                // Is this postion OFF-SCREEN ? ( en dehors de l'écran a gauche)
                if (retreatTarget.X < Camera.Position.X - Game1.SCREEN_WIDHT / 2)
                {
                    //Go to RIGHT side of Player
                    retreatTarget.X = Game1.Random.Next(
                                        (int)(playerToAttack.Position.X + 100),
                                        (int)(playerToAttack.Position.X + 200));
                }
            }
            else
            {
                //Go to RIGHT side of Player
                retreatTarget.X = Game1.Random.Next(
                                    (int)(playerToAttack.Position.X + 100),
                                    (int)(playerToAttack.Position.X + 200));

                // Is this postion OFF-SCREEN ? ( en dehors de l'écran a droite)
                if (retreatTarget.X > Camera.Position.X + Game1.SCREEN_WIDHT / 2)
                {
                    //Go to LEFT side of Player
                    retreatTarget.X = Game1.Random.Next(
                                        (int)(playerToAttack.Position.X - 200),
                                        (int)(playerToAttack.Position.X - 100));
                }
            }

            //Get Y retreat Target
            retreatTarget.Y = Game1.Random.Next(InLevel.PlayBounds.Top, InLevel.PlayBounds.Bottom);
        }

        private bool LinedUpXWithPlayer()
        {
            //Le player est-il à gauche ou à droite
            //Position maximale = player1 - 80px car il peut être atteint à cette distance
            if (playerToAttack.Position.X <= this.Position.X - 80) // Le joueur est à gauche
            {
                //Bouger à gauche
                this.Position.X -= 4f;
                FacingDir = Direction.Left;
                //On retourne faux donc car on n'est pas aligné avec le joueur
                return false;
            }
            else if (playerToAttack.Position.X >= this.Position.X + 80) //Le joueur est à droite
            {

                //Bouger à droite
                //Alonso ton bug était là, car c'était pas un X mais un Y ^^ -> this.Position.Y += 2.5f;
                this.Position.X += 4f;
                FacingDir = Direction.Right;
                return false;
            }
            else
            {
                // On est donc aligné en X
                return true;
            }

        }

        private bool LinedUpYWithPlayer()
        {
            //Le joueur est-il au dessus ou en dessous
            //8 px à été choisis arbitrairement sachant que la maximum pour atteindre le joueur en Y est de 15
            if (playerToAttack.Position.Y <= this.Position.Y - 8) // Le player est au dessus
            {
                //Bouger l'ennemi vers le haut
                this.Position.Y -= 2f;
                return false;
            }
            else if (playerToAttack.Position.Y >= this.Position.Y + 8) // Le player est en dessous
            {
                //L'ennemi bouge vers le bas
                this.Position.Y += 2f;
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool NoOtherEnemiesAttack()
        {
            // Boucle à travers tous les acteurs
            // Voir si personne d'autre attaque
            for (int i = 0; i < InLevel.Actors.Count; i++)
            {
                /*  if (InLevel.Actors[i].IsAttacking)
                  {
                      return false; //Quelqu'un d'autre attaque
                  }*/
            }

            // Ici personne d'autre attaque
            return true;
        }
        #endregion

        #region collision Detection
        public override void UpdateHitArea()
        {
            HitArea = drawWidth / 2;
        }

        public override void GetHit(Direction cameFrom, int damage)
        {
            this.IsAttacking = false;

            this.Health -= damage;
            if (CheckForDeath())
            {
                GetKnockedDown(cameFrom, 0);
                return;
            }



            //Set state and texture;
            state = EnemyCloseState2.TakeHit;
            texture = TEXTURE_REACTS;
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
            this.IsAttacking = false;

            //Set state and texture
            IsAttackable = false;
            state = EnemyCloseState2.KnockedDown;
            texture = TEXTURE_REACTS;
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

        private void CheckPlayerCollision()
        {
            UpdateHitArea();

            for (int i = InLevel.Actors.Count - 1; i >= 0; i--)
            {
                Actor actor;

                //Make sure are not looking at ourself;
                actor = InLevel.Actors[i] as EnemyClose2;
                if (actor == this)
                    continue;

                //Are we looking at the player?
                actor = InLevel.Actors[i] as Player;
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
                            //3) Which way is the Enemy facing?
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

        private void HitSomeone(Actor whoToHit)
        {
            switch (attackNumber)
            {
                case 1: // Punch 1
                case 2: // Punch 2
                    //15 <= nombre de dommages
                    whoToHit.GetHit(FacingDir, 30);
                    break;

                case 3: //Attack 3, the spinkick
                    whoToHit.GetKnockedDown(FacingDir, 45);
                    break;
            }
        }
        #endregion

        #region Animations

        protected void SetDrawArea()
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
                if (frameX > FRAME_X_IDLE)//9 normalement
                    frameX = 0;

                SetDrawArea();
            }
        }

        private void AnimateTakeHit(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate) //10% plus lentement
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos
                if (frameX > FRAME_X_TAKEHIT)
                {
                    ResetIdleGraphic();
                    return;
                }
                SetDrawArea();
            }
        }

        protected virtual void AnimateKnockDown(GameTime gT)
        {


        }



        protected virtual void AnimateGettingUp(GameTime gT)
        {
            
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

                if (frameX > FRAME_X_WALK)//10
                {
                    frameX = 0;
                }
                SetDrawArea();

            }

        }

        private void AnimateAttack1(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {
                drawWidth = DRAW_WIDHT_ATTACK1;
                drawHeight = DRAW_HEIGHT_ATTACK1;
                frameY = FRAME_Y_ATTACK1;

                currentFrameTime = 0f;
                frameX++;

                //nombres d'images pour le repos
                if (frameX > FRAME_X_ATTACK1)
                {
                    //CONFIGURATION ATTAQUE 2
                    drawWidth = DRAW_WIDHT_ATTACK2;
                    drawHeight = DRAW_HEIGHT_ATTACK2;
                    frameY = FRAME_Y_ATTACK2;
                    frameX = 0;

                    originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    attackNumber++;// on passe de l'attque 1 a l'attaque 2 

                    SoundManager.PlaySound(SoundEnum.ATTACK);

                    return;
                }

                //Collision Detection
                if (frameX == FRAME_X_ATTACK1_COLLISION)
                    CheckPlayerCollision();

                SetDrawArea();
            }
        }

        private void AnimateAttack2(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {

                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos
                if (frameX > FRAME_X_ATTACK2)
                {
                    //CONFIGURATION ATTACK 3
                    texture = TEXTURE_REACTS;
                    drawWidth = DRAW_WIDHT_ATTACK3;
                    drawHeight = DRAW_HEIGHT_ATTACK3;
                    frameX = 0;
                    frameY = FRAME_Y_ATTACK3;
                    originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    attackNumber++;// on passe de l'attaque 2 à l'attaque 3

                    SoundManager.PlaySound(SoundEnum.ATTACK);

                    return;
                }

                //Collision Detection
                if (frameX == FRAME_X_ATTACK2_COLLISION)
                    CheckPlayerCollision();

                SetDrawArea();
            }
        }

        private void AnimateAttack3(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate)
            {
                currentFrameTime = 0f;

                frameX++;

                //nombres d'images pour le repos
                if (frameX > FRAME_X_ATTACK3)
                {
                    ResetIdleGraphic();
                    return;
                }

                //Collision Detection
                if (frameX == FRAME_X_ATTACK3_COLLISION)
                {
                    CheckPlayerCollision();
                }

                SetDrawArea();
            }
        }

        #endregion
    }
}
