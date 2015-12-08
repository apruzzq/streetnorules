using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Les_Loubards
{
    enum LevelState
    {
        Intro,
        CutScene,
        Playing,
        Continue,
        GameOver,
        Completed,
        QuitGame,



        FadeOut,// le level disparait
        FadeIn  // le level apparait
    }

    class Level
    {
        struct BackgroundItem
        {
            Texture2D texture;
            Vector2 position;
            float speed;
            float layerDepth;

            public BackgroundItem(Texture2D texture, Vector2 position,
                float speed, float layerDepth)
            {
                this.texture = texture;
                this.position = position;
                this.speed = speed;
                this.layerDepth = layerDepth;
            }

            public void Draw(SpriteBatch SB)
            {
                SB.Draw(this.texture, Camera.GetScreenPosition(this.position) /**/, null, Color.White, 0f,
                    Vector2.Zero, 1f, SpriteEffects.None, this.layerDepth);
            }
        }


        List<BackgroundItem> backgrounds;
        public List<Actor> Actors;
        public List<EnemySpawner> EnemySpawners;
        public List<GameItem> GameItems;
        public Rectangle PlayBounds;
        public Player1 Player1;
        public Player2 Player2;
        public LevelState LevelState;
        public float TimerContinue;
        public float TimerContinue_p1;
        public float TimerContinue_p2;
        private bool PauseAdded;
        private BackgroundItem PauseItem;

        //cutscene
        public List<CutScene> CutScenes;
        public int CurrentCutScene;
        public Trigger CurrentTrigger;
        public List<SceneEvent> SceneEvents;
        public int CurrentSceneEvent;

        public Level()
        {
            this.LevelState = LevelState.Playing;

            backgrounds = new List<BackgroundItem>();
            Actors = new List<Actor>();
            EnemySpawners = new List<EnemySpawner>();
            GameItems = new List<GameItem>();
            CutScenes = new List<CutScene>();
            CurrentCutScene = 0;
            SceneEvents = new List<SceneEvent>();
            CurrentSceneEvent = 0;
            TimerContinue = 11f;
            TimerContinue_p1 = 11f;
            TimerContinue_p2 = 11f;
            PauseAdded = false;
        }

        public void Update(GameTime gT)
        {
            switch (this.LevelState)
            {
                #region Intro
                case LevelState.Intro:
                    Player1.Update(gT);
                    if (GameManager.NumberPlayers == 2)
                        Player2.Update(gT);
                    break;
                #endregion

                #region CutScene
                case LevelState.CutScene:
                    CutScenes[CurrentCutScene].PlayScene(gT);
                    break;
                #endregion

                #region Playing
                case LevelState.Playing:

                    if (GameManager.Pause == false)
                    {
                        // supprime le sprite PAUSE, si il est présent
                        if (PauseAdded == true)
                        {
                            backgrounds.Remove(PauseItem);
                            PauseAdded = false;
                        }


                        //Update Actors
                        for (int i = 0; i < Actors.Count; i++)
                            Actors[i].Update(gT);

                        //Udate GameItems
                        for (int i = 0; i < GameItems.Count; i++)
                            GameItems[i].Update();

                        //Update ennemySpawners
                        for (int i = 0; i < EnemySpawners.Count; i++)
                            EnemySpawners[i].Update(gT);

                        //Update Camera Position
                        UpdateCameraPosition();

                        //Update Trigger
                        this.CurrentTrigger.Update();

                        


                        #region  permet de lancer le compteur en mode deux joueur
                        if (GameManager.NumberPlayers == 2)
                        {
                            if (Player1.Health <= 0)
                            {
                                if (Player1.NombreDeVie > 0)
                                {
                                    if (TimerContinue_p1 > 0)
                                    {
                                        TimerContinue_p1 -= (float)gT.ElapsedGameTime.TotalSeconds;
                                    }

                                    if (InputHelper.WasKeyPressed(Keys.Space) && TimerContinue_p1 > 0)
                                    {
                                        this.Player1.IsVisible = true;
                                        this.Player1.Continue();
                                        TimerContinue_p1 = 11f;
                                    }
                                }
                                else
                                    TimerContinue_p1 = 0;
                            }

                            if (Player2.Health <= 0)
                            {
                                if (Player2.NombreDeVie > 0)
                                {

                                    if (TimerContinue_p2 > 0)
                                    {
                                        TimerContinue_p2 -= (float)gT.ElapsedGameTime.TotalSeconds;
                                    }

                                    if (InputHelper.WasKeyPressed(Keys.Space) && TimerContinue_p2 > 0)
                                    {
                                        this.Player2.IsVisible = true;
                                        this.Player2.Continue();
                                        TimerContinue_p2 = 11f;
                                    }
                                }
                                else
                                    TimerContinue_p2 = 0;
                            }

                            if (Player1.Health <= 0 && Player2.Health <= 0 && TimerContinue_p1 <= 0 && TimerContinue_p2 <= 0)
                            {
                                //fin du temp game over
                                this.LevelState = LevelState.GameOver;
                            }

                        }

                    }
                    else if (GameManager.Pause && PauseAdded == false)
                    {
                        PauseItem = new BackgroundItem(Game1.SprPause, new Vector2(Camera.Position.X - Game1.SprPause.Width / 2, Game1.SCREEN_HEIGHT / 2), 0f, 0f);
                        backgrounds.Add(PauseItem);
                        PauseAdded = true;
                    }
                        #endregion

                    break;
                #endregion

                #region Continue
                case LevelState.Continue:
                    TimerContinue -= (float)gT.ElapsedGameTime.TotalSeconds;

                    //TimerContinue fini ?
                    if (TimerContinue <= 0)
                    {
                        //fin du temps game over
                        this.LevelState = LevelState.GameOver;
                    }

                    //make sure the person behind doesn't take over my hard progress !!!
                    /*if (InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A)
                        || InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.B)
                        || InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.X)
                        || InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.Y))*/
                    if (InputHelper.WasKeyPressed(Keys.Enter))
                    {
                        this.TimerContinue--;
                    }

                    //Has Player pressed start to continue ?
                    // if(InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.Start))
                    if (InputHelper.WasKeyPressed(Keys.Space))
                    {
                        this.Player1.IsVisible = true;
                        this.Player1.Continue();
                        TimerContinue = 11f;

                        this.LevelState = LevelState.Playing;
                    }
                    break;
                #endregion

                #region GameOver
                case LevelState.GameOver:
                    /* if(InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.Start) ||
                        InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A ||
                        InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.B) ||
                         InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.X))*/

                    if (InputHelper.WasKeyPressed(Keys.Space))
                    {
                        GameManager.GameState = GameState.MainMenu;
                        MenuManager_principal.WakeUp();
                    }
                    break;
                #endregion

                #region Completed
                case LevelState.Completed:
                    if (InputHelper.WasKeyPressed(Keys.Space)
                        || (InputHelper.WasKeyPressed(Keys.Enter)))
                    /*if(InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A)||
                        InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.Start))*/
                    {
                        GameManager.GameState = GameState.MainMenu;
                        MenuManager_principal.WakeUp();
                        MusicManager.PlaySong(Game1.MusicTitleScreen);
                        SoundManager.PlayVoiceSound(VoiceSoundEnum.TITLE_SCREEN);
                        MusicManager.ChangeToVolume(1f);
                    }
                    break;
                #endregion

                #region FadeIn
                case LevelState.FadeIn:
                    HUDManager.Opacity -= 0.008f;
                    if (HUDManager.Opacity <= 0)
                    {
                        HUDManager.Opacity = 0;
                        LevelState = LevelState.Intro; // Le level est "completement chargé" et donc on passe a l'intro du niveau suivant
                    }
                    break;
                #endregion

                #region FadeOut
                case LevelState.FadeOut:
                    HUDManager.Opacity += 0.008f;
                    if (HUDManager.Opacity >= 1)
                    {
                        HUDManager.Opacity = 1;
                        GameManager.GoToNextLevel();
                    }
                    break;
                #endregion

                #region QuitGame
                case LevelState.QuitGame:
                    GameManager.GameState = GameState.MainMenu;
                    MenuManager_principal.WakeUp();
                    MusicManager.PlaySong(Game1.MusicTitleScreen);
                    SoundManager.PlayVoiceSound(VoiceSoundEnum.TITLE_SCREEN);
                    MusicManager.ChangeToVolume(1f);
                    break;
                #endregion
            }
        }

        public void Draw(SpriteBatch SB, SpriteBatch SBHUD)
        {
            #region Always draw me
            //Dessin des backgrounds
            for (int i = 0; i < backgrounds.Count; i++)
                backgrounds[i].Draw(SB);

            //Dessin Actors
            for (int i = 0; i < Actors.Count; i++)
                Actors[i].Draw(SB);

            //Update ennemySpawners
            for (int i = 0; i < EnemySpawners.Count; i++)
                EnemySpawners[i].Draw(SB);

            //Dessin GameItems
            for (int i = 0; i < GameItems.Count; i++)
                GameItems[i].Draw(SB);

            //Dessin de la barre de vie player
            if (this.LevelState == LevelState.Playing)
            {

                HUDManager.DrawHUD(SBHUD);



                if (GameManager.NumberPlayers == 2)
                {
                    if (Player1.Health <= 0 && Player1.NombreDeVie > 0)
                    {
                        if (TimerContinue_p1 > 0 && TimerContinue_p1 < 10)
                            HUDManager.DrawContinueScreen_p1(SBHUD, TimerContinue_p1);

                    }

                    if (Player2.Health <= 0 && Player2.NombreDeVie > 0)
                    {
                        if (TimerContinue_p2 > 0 && TimerContinue_p2 < 10)
                            HUDManager.DrawContinueScreen_p2(SBHUD, TimerContinue_p2);
                    }
                }
            }

            #endregion

            if (this.LevelState == LevelState.CutScene)
                CutScenes[CurrentCutScene].Draw(SBHUD);

            if (this.LevelState == LevelState.Continue)
                HUDManager.DrawContinueScreen(SBHUD, this.TimerContinue);

            if (this.LevelState == LevelState.GameOver)
                HUDManager.DrawGameOverScreen(SBHUD);

            if (this.LevelState == LevelState.FadeIn
                || this.LevelState == LevelState.FadeOut)
                HUDManager.DrawScreenFade(SBHUD);

            if (this.LevelState == LevelState.Completed)
                HUDManager.DrawGameCompleted(SBHUD);
        }

        private void UpdateCameraPosition()
        {
            if (Player1.Position.X > Camera.Position.X)
            {
                Camera.Position.X += 3;
                if (Camera.Position.X + Game1.SCREEN_WIDHT / 2 > this.PlayBounds.Right)
                    Camera.Position.X = this.PlayBounds.Right - Game1.SCREEN_WIDHT / 2;
            }
        }

        public void AddBackGroundItem(Texture2D texture, Vector2 position,
            float speed, float layerDepth)
        {
            backgrounds.Add(new BackgroundItem(texture, position, speed, layerDepth));
        }
        public void AddToPlayBounds(int HowMuch)
        {
            this.PlayBounds.Width += HowMuch;
        }

        public void ActivateSceneEvent()
        {

            SceneEvents[CurrentSceneEvent].Activate(this);
            // Nest Scene event to happen next time
            CurrentSceneEvent++;

            this.LevelState = LevelState.Playing;
            MusicManager.ChangeToVolume(1.0f);

        }

        public static void GetStarttSidePosition(Actor enemy, Level level)
        {
            if (enemy.FacingDir == Direction.Left)
                enemy.Position = new Vector2(Camera.Position.X - Game1.SCREEN_WIDHT / 2 - 80,
                    Game1.Random.Next(level.PlayBounds.Height) + level.PlayBounds.Top);

            else if (enemy.FacingDir == Direction.Right)
                enemy.Position = new Vector2(Camera.Position.X + Game1.SCREEN_WIDHT / 2 + 80,
                    Game1.Random.Next(level.PlayBounds.Height) + level.PlayBounds.Top);

            else //Not left nor right, leaves only neither
            {
                if (Game1.Random.NextDouble() >= 0.5)
                {
                    //Picked Left
                    enemy.Position = new Vector2(Camera.Position.X - Game1.SCREEN_WIDHT / 2 - 80,
                        Game1.Random.Next(level.PlayBounds.Height) + level.PlayBounds.Top);
                }
                else
                {
                    //Picked right
                    enemy.Position = new Vector2(Camera.Position.X + Game1.SCREEN_WIDHT / 2 + 80,
                        Game1.Random.Next(level.PlayBounds.Height) + level.PlayBounds.Top);
                }
            }

        }

    }
}
