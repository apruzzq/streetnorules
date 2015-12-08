using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Les_Loubards
{
    class Trigger
    {
        public Trigger()
        { }

        public virtual void Update()
        { }

        public void ActivateTriggerNoCS()
        {
            GameManager.Levels[GameManager.CurrentLevel].ActivateSceneEvent();
        }

        public void ActivateTriggerWithCS()
        {
            Level level = GameManager.Levels[GameManager.CurrentLevel];

            level.LevelState = LevelState.CutScene;
            level.Player1.ResetIdleGraphic();

            if (GameManager.NumberPlayers == 2)
            {
                level.Player2.ResetIdleGraphic();
            }

            level.CutScenes[level.CurrentCutScene].PlayFirstLine();
            MusicManager.ChangeToVolume(0.4f);
        }
    }

    /// <summary>
    /// Triggers when Player is the ONLY actor left in a level's Actors list
    /// Triggers a CutScene to Play
    /// </summary>
    class TriggerNoEnemies : Trigger
    {
        public TriggerNoEnemies()
            : base()
        { }

        public override void Update()
        {
            // Make sure Player is the ONLY Actor Left in the Actors List ( pas denemy)
            if (GameManager.Levels[GameManager.CurrentLevel].Actors.Count == 1) // Y a t'il un seul actor dans le jeu ?
            {
                // There is only ONE player
                // If All other enemies are beaten up, then there will be only ONE actor
                // Is it the Player?
                if (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null) // Est ce que cet actor est un player ?
                {
                    this.ActivateTriggerWithCS();
                }
            }

            else if (GameManager.Levels[GameManager.CurrentLevel].Actors.Count == 2)
            {
                if ((GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null
                    || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player1 != null)
                   && (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player2 != null
                    || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player2 != null))
                {
                    this.ActivateTriggerWithCS();
                }
            }
        }

    }

    /// <summary>
    /// Déclanchement lorsque le joueur est le seul acteur restant dans la liste d'acteurs
    /// ******** DOES NOT triggers a CutScene !! ********
    /// </summary>
    class TriggerNoEnemiesNoCS : Trigger
    {
        public TriggerNoEnemiesNoCS()
            : base()
        { }

        public override void Update()
        {
            // Make sure Player is the ONLY Actor Left in the Actors List ( pas denemy)
            if (GameManager.NumberPlayers == 1)
            {
                if (GameManager.Levels[GameManager.CurrentLevel].Actors.Count == 1) // Y a t'il un seul actor dans le jeu ?
                {
                    // There is only ONE player
                    // If All other enemies are beaten up, then there will be only ONE actor
                    // Is it the Player?
                    if (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player != null) // Est ce que cet actor est un player ?
                    {
                        // We know we are looking at the player, La on sait qu'on a faire a un player et pas un enemyclose, ou un enemyranged 
                        this.ActivateTriggerNoCS();
                    }
                }
            }

            else if (GameManager.NumberPlayers == 2)
            {
                if (GameManager.Levels[GameManager.CurrentLevel].Actors.Count == 1) // Y a t'il un seul actor dans le jeu ?
                {
                    // There is only ONE player
                    // If All other enemies are beaten up, then there will be only ONE actor
                    // Is it the Player?
                    if (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player != null) // Est ce que cet actor est un player ?
                    {
                        // We know we are looking at the player, La on sait qu'on a faire a un player et pas un enemyclose, ou un enemyranged 
                        this.ActivateTriggerNoCS();
                    }
                }


                if (GameManager.Levels[GameManager.CurrentLevel].Actors.Count == 2)
                {
                    if ((GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null
                        || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player1 != null)
                       && (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player2 != null
                        || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player2 != null))
                    {
                        this.ActivateTriggerNoCS();
                    }
                }
            }


        }
    }


    /// <summary>
    /// Déclanchement lorsque le joueur touche la hitbox.
    /// Déclanche une cinématique avant de jouer
    /// </summary>
    class TriggerHitBox : Trigger
    {
        Rectangle hitBox;

        public TriggerHitBox(Rectangle hitBox)
            : base()
        {
            this.hitBox = hitBox;
        }

        public override void Update()
        {
            Level level = GameManager.Levels[GameManager.CurrentLevel];

            //Convert Player hitArea to a rectangle ( pour collisionner avec le triggerbox )
            Rectangle playerHitBox1 = new Rectangle(
                (int)level.Player1.Position.X - level.Player1.HitArea,
                (int)level.Player1.Position.Y - 5,
                level.Player1.HitArea * 2,
                10);

            if (GameManager.NumberPlayers == 1)
            {
                if (playerHitBox1.Intersects(this.hitBox))
                {
                    this.ActivateTriggerWithCS();
                }
            }
            else if (GameManager.NumberPlayers == 2)
            {
                Rectangle playerHitBox2 = new Rectangle(
                (int)level.Player2.Position.X - level.Player2.HitArea,
                (int)level.Player2.Position.Y - 5,
                level.Player2.HitArea * 2,
                10);

                if (playerHitBox1.Intersects(this.hitBox) || playerHitBox2.Intersects(this.hitBox))
                {
                    this.ActivateTriggerWithCS();
                }
            }
        }


    }

    /// <summary>
    /// Trigger when a Player is touching it's hitbox.
    /// ******** DOES NOT triggers a CutScene !! ********
    /// </summary>
    class TriggerHitBoxNoCS : Trigger
    {
        Rectangle hitBox;

        public TriggerHitBoxNoCS(Rectangle hitBox)
            : base()
        {
            this.hitBox = hitBox;
        }

        public override void Update()
        {
            Level level = GameManager.Levels[GameManager.CurrentLevel];

            //Convert Player hitArea to a rectangle ( pour collisionner avec la triggerbox )
            Rectangle playerHitBox1 = new Rectangle(
                (int)level.Player1.Position.X - level.Player1.HitArea,
                (int)level.Player1.Position.Y - 5,
                level.Player1.HitArea * 2,
                10);

            if (GameManager.NumberPlayers == 1)
            {
                if (playerHitBox1.Intersects(this.hitBox))
                {
                    this.ActivateTriggerNoCS();
                }
            }
            else if (GameManager.NumberPlayers == 2)
            {
                Rectangle playerHitBox2 = new Rectangle(
                (int)level.Player2.Position.X - level.Player2.HitArea,
                (int)level.Player2.Position.Y - 5,
                level.Player2.HitArea * 2,
                10);

                if (playerHitBox1.Intersects(this.hitBox) || playerHitBox2.Intersects(this.hitBox))
                {
                    this.ActivateTriggerNoCS();
                }
            }
        }

    }

    /// <summary>
    /// Triggers when the player is the only actor left in the level
    /// AND player is toucing e hitbox
    /// Trigger a utScene to play
    /// </summary>
    class TriggerNoEnnemiesHitBox : Trigger
    {
        Rectangle hitBox;

        public TriggerNoEnnemiesHitBox(Rectangle hitBox)
            : base()
        {
            this.hitBox = hitBox;
        }

        public override void Update()
        {
            Level level = GameManager.Levels[GameManager.CurrentLevel];

            //Convert Player hitArea to a rectangle ( pour collisionner avec le triggerbox )
            Rectangle playerHitBox1 = new Rectangle(
                (int)level.Player1.Position.X - level.Player1.HitArea,
                (int)level.Player1.Position.Y - 5,
                level.Player1.HitArea * 2,
                10);

            if (GameManager.NumberPlayers == 1)
            {
                if (playerHitBox1.Intersects(this.hitBox))
                {
                    // Make sure Player is the ONLY Actor Left in the Actors List ( pas denemy)
                    if (level.Actors.Count == 1) // Y a t'il un seul actor dans le jeu ?
                    {
                        // There is only ONE player
                        // If All other enemies are beaten up, then there will be only ONE actor
                        // Is it the Player?
                        if (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null) // Est ce que cet actor est un player ?
                        {
                            // We know we are looking at the player, La on sait qu'on a faire a un player et pas un enemyclose, ou un enemyranged 
                            this.ActivateTriggerWithCS();
                        }
                    }
                }
            }
            else if (GameManager.NumberPlayers == 2)
            {
                Rectangle playerHitBox2 = new Rectangle(
                (int)level.Player2.Position.X - level.Player2.HitArea,
                (int)level.Player2.Position.Y - 5,
                level.Player2.HitArea * 2,
                10);


                if (playerHitBox1.Intersects(this.hitBox) || playerHitBox2.Intersects(this.hitBox))
                {
                    // Make sure Player is the ONLY Actor Left in the Actors List ( pas denemy)
                    if (level.Actors.Count == 1) // Y a t'il un seul actor dans le jeu ?
                    {
                        // There is only ONE player
                        // If All other enemies are beaten up, then there will be only ONE actor
                        // Is it the Player?
                        if (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null
                            || GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player2 != null) // Est ce que cet actor est un player ?
                        {
                            // We know we are looking at the player, La on sait qu'on a faire a un player et pas un enemyclose, ou un enemyranged 
                            this.ActivateTriggerWithCS();
                        }
                    }


                    if (level.Actors.Count == 2)
                    {
                        if ((GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player1 != null
                        || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player1 != null)
                       && (GameManager.Levels[GameManager.CurrentLevel].Actors[0] as Player2 != null
                        || GameManager.Levels[GameManager.CurrentLevel].Actors[1] as Player2 != null))  // Est ce que cet actor est un player ?
                        {
                            // We know we are looking at the player, La on sait qu'on a faire a un player et pas un enemyclose, ou un enemyranged 
                            this.ActivateTriggerWithCS();
                        }
                    }
                }

            }
        }
    }

    class TriggerNextLevel : Trigger
    {
        public TriggerNextLevel()
            : base()
        { }

        public override void Update()
        {
            //Check if there is actually a next level to go to
            if (GameManager.CurrentLevel + 1 < GameManager.Levels.Count)
            {
                //Yes, FadeOut as normal
                GameManager.Levels[GameManager.CurrentLevel].LevelState = LevelState.FadeOut; // Le level courant va etre en etat de "disparition" (fadeout)
            }
            else
            {
                //No, finished final level . GoTo GameCompleted Screen
                GameManager.Levels[GameManager.CurrentLevel].LevelState = LevelState.Completed;
            }
        }
    }
}
