using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Les_Loubards.Classes;

namespace Les_Loubards
{

    // private void HitSomeone(Actor whoToHit)  <<<<<<  ICI POUR LES POINTS DE DAMAGE DU HERO


    enum PlayerState
    {
        //Take Hit Cycle (actions consistant a recevoir des degats, tomber, mourrir ... )
        TakeHit,
        KnockedDown,
        Down,
        GettingUp,
        Dying,
        
        //Mouvements
        Idle,
        Walking,
        Jumping,
        JumpKick,
        Attack1,
        Attack2,
        Attack3,
        ThrownKnife,
        KickObject,
        PickUpItem,

        LevelIntro
    }

    class Player : Actor
    {

        #region other values
        const float MOVEMENT_THRESHOLD = 0.3f;
        static float acceptAttack2Time = Actor.FrameRate * 6f; // 6 frames
        static float acceptAttack3Time = Actor.FrameRate * 6f; // 6 frames
        public const float STARTING_HEALTH = 120f; // nombres de points de vie pour le joueur
        public int NombreDeVie = 3;
        public bool IsAttacked;
        public bool HitLeft;
        #endregion


        PlayerIndex pIndex;



        //TextureDetails
        protected Texture2D texture;
        protected Vector2 originCharacter;
        protected Rectangle DrawArea;
        protected float currentFrameTime;
        protected int drawWidth;
        protected int drawHeight;
        protected int frameX;
        protected int frameY;

        //Mouvement
        public PlayerState state;
        protected Vector2 jumpingPos; // Can't use normal Position as this would also change the position of the Actor's shadow
        protected float landingHeight; // Need to know where the landing of the jump is
        protected Vector2 speed;

        //Attacks
        protected float stateTime;
        protected bool makeCombo; 
        protected GameItem aboutToUse; //Need a reference so action on a gameitem can be delayed until the correct trame of aniamtion


        //Cut Scene
        protected Vector2 targetPosition;

        // Méthode pour la classe 
        public Player(Vector2 position, Level inLevel, PlayerIndex pIndex)
            : base(position, inLevel)
        {
            this.pIndex = pIndex;

            this.Health = STARTING_HEALTH;
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

        /// <summary>
        /// Ne laisse pas le warrior sortir de la partie jouable
        /// </summary>
        protected void ConstrainToScreen()
        {
            #region Limitation du terrain
            if (Position.X < Camera.Position.X - Game1.SCREEN_WIDHT / 2 + drawWidth / 2 - 20)
                Position.X = Camera.Position.X - Game1.SCREEN_WIDHT / 2 + drawWidth / 2 - 20;
            if (Position.X > Camera.Position.X + Game1.SCREEN_WIDHT / 2 - drawWidth / 2 + 20)
                Position.X = Camera.Position.X + Game1.SCREEN_WIDHT / 2 - drawWidth / 2 + 20;
            if (Position.Y < InLevel.PlayBounds.Top)
                Position.Y = InLevel.PlayBounds.Top;
            if (Position.Y > InLevel.PlayBounds.Bottom)
                Position.Y = InLevel.PlayBounds.Bottom;
            #endregion
        }


        public void GiveHealth(int howMuch)
        {
            this.Health += howMuch;
            if (this.Health > STARTING_HEALTH)
                this.Health = STARTING_HEALTH;
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


        #endregion


    }

}
