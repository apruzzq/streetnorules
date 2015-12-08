using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{

    enum EnemyRangedState
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
        MoveAway,
        ThrowKnife
    }

    class EnemyRanged : Actor
    {
        #region draw area constants
         const int DRAW_HEIGHT_NORMAL = 225;

         const int DRAW_WIDHT_IDLE = 105;
         const int DRAW_WIDHT_WALK = 100;
         const int DRAW_WIDHT_ATTACK3 = 151;
         const int DRAW_WIDTH_KNIFE_THROWN = 145;
         const int DRAW_WIDTH_ARMSFOLDED = 105;
         const int DRAW_WIDTH_TAKEHIT = 108;
         const int DRAW_WIDTH_KNOCKEDOWN = 216;
         const int DRAW_WIDTH_GETUP = 180;

         const int FRAME_Y_IDLE = 0;
         const int FRAME_Y_WALK = 225;
         const int FRAME_Y_ATTACK3 = 945;
         const int FRAME_Y_KNIFE_THROWN = 1620;
         const int FRAME_Y_ARMSFOLDED = 0;
         const int FRAME_Y_TAKEHIT = 450;
         const int FRAME_Y_KNOCKEDOWN = 0;
         const int FRAME_Y_GETUP = 225;

        #endregion

        #region constants
        const int HEALTH_BAR_WIDTH = 60;
        const int HEALTH_BAR_HEIGHT = 15;
        const float STARTING_HEALTH = 100;
        const int THROW_KNIFE_SAFE_DISTANCE = 300;

        #endregion

        #region variables

        EnemyRangedState state;

        //TextureDetails
        Texture2D texture;
        Vector2 originCharacter;
        Rectangle DrawArea;
        float currentFrameTime;
        int drawWidth;
        int drawHeight;
        int frameX;
        int frameY;
        Color drawColor;
        Player playerToAttack;

        //Movement
        float stateTime;
        Vector2 retreatTarget;

        #endregion

        public EnemyRanged(Vector2 position, Level inLevel)
            : base(position, inLevel)
        {
            ResetIdleGraphic();
            drawColor = new Color(0.2f, 0.2f, 1f); //Set the draw Color
            this.Health = STARTING_HEALTH;
            this.IsBoss = false;
        }

        /// <summary>
        /// Create an ennemy to spawn on a particular side of the screen
        /// </summary>
        public EnemyRanged(Direction startingSide, Level inLevel)
            : base(Vector2.Zero, inLevel)
        {
            this.FacingDir = startingSide;

            ResetIdleGraphic();
            drawColor = new Color(0.2f, 0.2f, 1f); //Set the draw Color
            this.Health = STARTING_HEALTH;
        }


        public override void Update(GameTime gT)
        {
            if (GameManager.NumberPlayers == 2)
                playerToAttack = ChoosePlayerToAttack(InLevel.Player1, InLevel.Player2);
            else
                playerToAttack = InLevel.Player1;

            switch (state)
            {
                #region all

                #region Retreat state
                case EnemyRangedState.Retreat: // l'enemi est dans un état de fuite, de retraite ( = retreat) 
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
                            this.Position.X += 2;
                            if (Position.X > retreatTarget.X)
                                Position.X = retreatTarget.X; // il n'ira pas trop loin à droite
                        }
                        else if (Position.X > retreatTarget.X)// L'enemi est a GAUCHE de son point de retraite
                        {
                            this.Position.X -= 2;
                            if (Position.X < retreatTarget.X)
                                Position.X = retreatTarget.X; // il n'ira pas trop loin là aussi
                        }

                        //Move to Y location 
                        if (Position.Y < retreatTarget.Y) // L'enemi est en haut de son point de retraite
                        {
                            this.Position.Y += 1.5f;
                            if (Position.Y > retreatTarget.Y)
                                Position.Y = retreatTarget.Y; // il n'ira pas trop loin en bas^^
                        }
                        else if (Position.Y > retreatTarget.X)// L'enemi est en bas de son point de retraite
                        {
                            this.Position.Y -= 1.5f;
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
                            drawHeight = DRAW_HEIGHT_NORMAL;
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

                #region MoveTo - MoveTO the Player, for a Hand-to-hand attack
                case EnemyRangedState.MoveTo:
                    //sommes-nous alignés avec le joueur
                    bool linedUpX = LinedUpXWithPlayerRanged();
                    bool linedUpY = LinedUpYWithPlayerClose();

                    if (linedUpX && linedUpY)
                    {
                        //Configurer l'etat de pré-attaque
                        frameX = 0;
                        frameY = FRAME_Y_IDLE;

                        state = EnemyRangedState.PreAttack;
                        drawWidth = DRAW_WIDHT_IDLE;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                        SetDrawArea();

                        // Combien de temps devons-nous rester dans cet etat ?
                        stateTime = 0.5f * (float)Game1.Random.NextDouble();

                        break;
                    }

                    AnimateWalking(gT);
                    break;
                #endregion

                #region MoveAway - Put distance between RangedEnemy and Player, then throw a knife.
                case EnemyRangedState.MoveAway:
                    //sommes-nous alignés avec le joueur
                    bool linedUpXRanged = LinedUpXWithPlayerRanged();
                    bool linedUpYClose = LinedUpYWithPlayerClose();


                    if (linedUpXRanged && linedUpYClose)
                    {
                        if (playerToAttack.IsAttackable)
                        {
                            //Configurer le lancer de couteau
                            frameX = 0;
                            frameY = FRAME_Y_KNIFE_THROWN;

                            state = EnemyRangedState.ThrowKnife;
                            texture = Game1.SprCharacterReactsMehdi;
                            SoundManager.PlaySound(SoundEnum.KNIFE_GET_OUT);
                            drawWidth = DRAW_WIDTH_KNIFE_THROWN;
                            drawHeight = DRAW_HEIGHT_NORMAL;
                            originCharacter = new Vector2(drawWidth / 2, drawHeight);
                            SetDrawArea();
                            break;
                        }
                    }

                    AnimateWalking(gT);
                    break;
                #endregion

                #region ThrowKnife
                case EnemyRangedState.ThrowKnife:
                    AnimateThrowKnife(gT);
                    break;
                #endregion

                #region PreAttack
                case EnemyRangedState.PreAttack:
                    //Suis-je toujours aligné avec le joueur ?
                    if (LinedUpXWithPlayerRanged() && LinedUpYWithPlayerClose())
                    {
                        // Est-ce que la préattaque a été assez longue ?
                        stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;
                        if (stateTime < 0)
                        {
                            //Le joueur est-il attaquable ??
                            if (!playerToAttack.IsAttackable)
                            {
                                GetRetreatTarget();
                                break;
                            }

                            if (NoOtherEnemiesAttack())
                            {
                                //Caractéristiques de la seule attaque au corps à corps
                                texture = Game1.SprCharacterAttacksMehdi;

                                drawWidth = DRAW_WIDHT_ATTACK3;
                                drawHeight = DRAW_HEIGHT_NORMAL;
                                frameX = 0;
                                frameY = FRAME_Y_ATTACK3;
                                originCharacter = new Vector2(drawWidth / 2, drawHeight);
                                SetDrawArea();

                                state = EnemyRangedState.Attack;
                                SoundManager.PlaySound(SoundEnum.ATTACK);

                                this.IsAttacking = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        //On n'est pas lignés avec le joueur :(
                        state = EnemyRangedState.MoveTo;
                        frameX = 0;
                        drawWidth = DRAW_WIDHT_WALK;
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                        return;
                    }

                    AnimateIdle(gT);
                    break;
                #endregion

                #region Attacks
                case EnemyRangedState.Attack:
                    AnimateKnockDownAttack(gT);
                    break;
                #endregion

                #region Take Hit and Die circle

                case EnemyRangedState.TakeHit:
                    AnimateTakeHit(gT);
                    break;


                case EnemyRangedState.KnockedDown:
                    AnimateKnockDown(gT);
                    break;


                case EnemyRangedState.Down:
                    stateTime += (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime >= Actor.DOWN_TIME)
                    {
                        ///Setup the GettingUp animation
                        state = EnemyRangedState.GettingUp;
                        currentFrameTime = 0f;
                        frameX = 0;
                        frameY = FRAME_Y_GETUP;
                        drawWidth = DRAW_WIDTH_GETUP;
                        drawHeight = DRAW_HEIGHT_NORMAL;
                        SetDrawArea();
                        originCharacter = new Vector2(drawWidth / 2, drawHeight);
                    }
                    break;


                case EnemyRangedState.GettingUp:
                    AnimateGettingUp(gT);
                    break;

                case EnemyRangedState.Dying:
                    //Flash body a few times
                    stateTime -= (float)gT.ElapsedGameTime.TotalSeconds;
                    if (stateTime <= 0)
                    {
                        stateTime = DEATH_FLASH_TIME;
                        IsVisible = !IsVisible;
                        DeathFlashes++;

                        if (DeathFlashes >= 8)
                        {
                            //Will I drop knife ?
                            if (Game1.Random.NextDouble() >= 0.5f) // le pourcentage de chance d'avoir le couteau ( 0f = 100%, 0.7f = 30% de chance ...)
                            {
                                this.InLevel.GameItems.Add(
                                    new PickUpKnife(this.InLevel, this.Position));
                            }

                            //Actor is dead enough
                            RemoveActorFromLevel(); //ON FAIT DISPARAITRE LE CADAVRE!!
                        }
                    }
                    break;
                #endregion

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
            SB.Draw(Game1.SprSinglePixel, new Vector2(drawPos.X - HEALTH_BAR_WIDTH / 2, drawPos.Y - drawHeight - HEALTH_BAR_HEIGHT),
                new Rectangle(0, 0, HEALTH_BAR_WIDTH + 2, HEALTH_BAR_HEIGHT + 2), new Color(Color.Red, 0.4f), 0f,
                Vector2.Zero, 1f, SpriteEffects.None, this.LayerDepth + 0.0001f);

            // How long do we draw the Enemy's Health Bar?
            float percent = this.Health / STARTING_HEALTH;
            int drawWidth = (int)(percent * HEALTH_BAR_WIDTH);

            //Yellow Health Bar
            SB.Draw(Game1.SprSinglePixel, new Vector2(drawPos.X - HEALTH_BAR_WIDTH / 2 + 1, drawPos.Y - drawHeight - HEALTH_BAR_HEIGHT + 1),
                new Rectangle(0, 0, drawWidth, HEALTH_BAR_HEIGHT), new Color(Color.Yellow, 0.4f), 0f,
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
        private void ResetIdleGraphic()
        {
            this.IsAttacking = false;

            texture = Game1.SprCharacterAttacksMehdi;

            IsAttackable = true;

            currentFrameTime = 0f;
            frameX = 0;
            frameY = FRAME_Y_IDLE;
            drawWidth = DRAW_WIDHT_IDLE;
            drawHeight = DRAW_HEIGHT_NORMAL;
            SetDrawArea();
            originCharacter = new Vector2(drawWidth / 2, drawHeight);

            this.HitArea = drawWidth / 2;

            if (this.playerToAttack != null)
                DecideWhatToDo();
        }

        private void DecideWhatToDo()
        {

            if (Game1.Random.NextDouble() < 0.5d)
            {
                //Decide to retreat
                GetRetreatTarget();

                //Set time to be in Retreat State
                stateTime = (float)(Game1.Random.NextDouble() + 1.5);
            }
            else
            {

                //Decide to attack
                this.state = EnemyRangedState.MoveAway;
                texture = Game1.SprCharacterAttacksMehdi;
                drawWidth = DRAW_WIDHT_WALK;
                frameX = 0;

            }
        }

        private void GetRetreatTarget()
        {
            state = EnemyRangedState.Retreat;

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

        private bool LinedUpXWithPlayerRanged()
        {
            //1 Le joueur est-il sur la droite ou sur la gauche de moi
            if (this.Position.X < playerToAttack.Position.X) //GAUCHE?
            {
                //2 Suis-je loin ?
                if (this.Position.X < playerToAttack.Position.X - THROW_KNIFE_SAFE_DISTANCE)
                {
                    return true;
                }
                else
                {
                    // Bouger à gauche un peu
                    this.Position.X -= 2.5f;
                    FacingDir = Direction.Right;

                    // Est ce que le mouvement est en dehors de l'écran
                    if (this.Position.X <= Camera.Position.X - Game1.SCREEN_WIDHT / 2)
                        this.state = EnemyRangedState.MoveTo;

                    return false;
                }
            }
            //1 Le joueur est-il sur la droite ou sur la gauche de moi
            else if (this.Position.X >= playerToAttack.Position.X) //DROITE?
            {
                //2 Suis-je loin ?
                if (this.Position.X > playerToAttack.Position.X + THROW_KNIFE_SAFE_DISTANCE)
                {
                    return true;
                }
                else
                {
                    // Bouger à droite un peu
                    this.Position.X += 2.5f;
                    FacingDir = Direction.Left;

                    // Est ce que le mouvement est en dehors de l'écran
                    if (this.Position.X >= Camera.Position.X + Game1.SCREEN_WIDHT / 2)
                        this.state = EnemyRangedState.MoveTo;

                    return false;
                }
            }

            return false;
        }

        private bool LinedUpYWithPlayerClose()
        {
            //Le joueur est-il au dessus ou en dessous
            //8 px à été choisis arbitrairement sachant que la maximum pour atteindre le joueur en Y est de 15
            if (playerToAttack.Position.Y <= this.Position.Y - 8) // Le player est au dessus
            {
                //Bouger l'ennemi vers le haut
                this.Position.Y -= 1.5f;
                return false;
            }
            else if (playerToAttack.Position.Y >= this.Position.Y + 8) // Le player est en dessous
            {
                //L'ennemi bouge vers le bas
                this.Position.Y += 1.5f;
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool LinedUpXWithPlayerClose()
        {
            //Le joueur est-il au dessus ou en dessous
            //8 px à été choisis arbitrairement sachant que la maximum pour atteindre le joueur en X est de 15
            if (playerToAttack.Position.X <= this.Position.X - 8) // Le player est a gauche
            {
                //Bouger l'ennemi vers la gauche
                this.Position.X -= 1.5f;
                return false;
            }
            else if (playerToAttack.Position.X >= this.Position.X + 8) // Le player est à droite
            {
                //L'ennemi bouge vers la droite
                this.Position.X += 1.5f;
                return false;
            }
            else
            {
                return true;
            }

        }

        private void ThrowKnife()
        {
            //Lancer un couteau
            this.InLevel.GameItems.Add(new Knife(
                this.Position, this.FacingDir, this.InLevel, this));

            //Le son du couteau
            SoundManager.PlaySound(SoundEnum.KNIFE_THROW);
        }

        private bool NoOtherEnemiesAttack()
        {
            // Boucle à travers tous les acteurs
            // Voir si personne d'autre attaque
            for (int i = 0; i < InLevel.Actors.Count; i++)
            {
                if (InLevel.Actors[i].IsAttacking)
                {
                    return false; //Quelqu'un d'autre attaque
                }
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
            state = EnemyRangedState.TakeHit;
            texture = Game1.SprCharacterReactsMehdi;
            frameX = 0;
            frameY = FRAME_Y_TAKEHIT;
            drawWidth = DRAW_WIDTH_TAKEHIT;
            drawHeight = DRAW_HEIGHT_NORMAL;
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
            state = EnemyRangedState.KnockedDown;
            texture = Game1.SprCharacterReactsMehdi;
            frameX = 0;
            frameY = FRAME_Y_KNOCKEDOWN;
            drawWidth = DRAW_WIDTH_KNOCKEDOWN;
            drawHeight = DRAW_HEIGHT_NORMAL;
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
                actor = InLevel.Actors[i] as EnemyClose;
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
            whoToHit.GetKnockedDown(FacingDir, 5);
        }
        #endregion

        #region Animations

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
                if (frameX > 5)//9 normalement
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
                if (frameX > 4)
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


                if (frameX > 3)//7
                {
                    frameX = 0;
                    stateTime = 0f;
                }

                if (CheckForDeath())
                {
                    state = EnemyRangedState.Dying;
                    SoundManager.PlaySound(SoundEnum.ACTOR_DEATH);
                    stateTime = 1f;
                    frameX = 4;
                    return;
                }


                // on a finit, on change alors d'état
                state = EnemyRangedState.Down;
                SetDrawArea();

            }


        }



        private void AnimateGettingUp(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 1.5f)
            {
                currentFrameTime = 0f;
                frameX++;
                //nombres d'images pour le repos

                if (frameX > 5)//si on arrive à la fin de la première ligne
                {
                    ResetIdleGraphic();
                    return;
                }

                SetDrawArea();
            }
        }

        private void AnimateWalking(GameTime gT)
        {

            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds * 1.5f; // (* 1.5f) permet d'accelerer la vitesse de défilement
            if (currentFrameTime >= Actor.FrameRate)
            {
                // coordonnée exacte axe Y
                frameY = FRAME_Y_WALK;
                drawHeight = DRAW_HEIGHT_NORMAL;

                currentFrameTime = 0;
                frameX++;

                if (frameX > 8)//10
                {
                    frameX = 0;
                }
                SetDrawArea();

            }

        }

        private void AnimateKnockDownAttack(GameTime gT)
        {
            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 0.8f) // 20% plus rapide
            {

                //Caractéristiques se trouvent dans préattack


                currentFrameTime = 0f;
                frameX++;

                //nombres d'images pour le repos
                if (frameX > 5)
                {
                    ResetIdleGraphic();
                    return;
                }

                //Collision Detection
                if (frameX == 4)
                    CheckPlayerCollision();

                SetDrawArea();
            }
        }


        private void AnimateThrowKnife(GameTime gT)
        {

            currentFrameTime += (float)gT.ElapsedGameTime.TotalSeconds;
            if (currentFrameTime >= Actor.FrameRate * 2.0f)
            {
                currentFrameTime = 0f;
                

                if (frameX > 4)
                {
                    //frameX = 0;
                    //frameY++;
                    //if (frameY > 9)
                    //{
                    texture = Game1.SprCharacterAttacksMehdi;
                    GetRetreatTarget(); // On le force à battre en retraite
                    return;
                    //}
                }
                //Do we throw the knife ?
                if (frameX == 3)
                {
                    ThrowKnife();
                }
                SetDrawArea();
                frameX++;
            }
        }

        #endregion
    }
}
