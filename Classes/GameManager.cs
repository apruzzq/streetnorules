using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    enum GameState
    {
        MainMenu,
        HowToPlay,
        ChooseNumberPlayer,
        ChooseLevel,
        Option,
        Playing,
        MenuIngame,
    }

    static class GameManager
    {
        public static GameState GameState = GameState.MainMenu;
        public static List<Level> Levels = new List<Level>();
        public static int CurrentLevel = 0;
        public static int LevelChoisie;
        public static int NumberPlayers;
        public static bool Pause = false;
        public static string Langage = "Anglais";

        public static void Update(GameTime gT)
        {
            if (InputHelper.WasKeyPressed(Keys.P))
                Pause = !Pause;

            switch (GameState)
            {
                case GameState.MainMenu:

                    MenuManager_principal.Update();

                    break;

                case GameState.HowToPlay:
                    // Await Input to go back to the Main Menu
                    if (InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A)
                         || InputHelper.WasKeyPressed(Keys.Space))
                    {
                        GameState = GameState.Option;
                    }
                    break;

                case GameState.Option:
                    MenuManager_Option.Update();
                    break;


                // choix de la difficulté se trouve dans le MenuManager2
                case GameState.ChooseNumberPlayer:
                    MenuManager_choixPlayer.Update();
                    break;

                case GameState.ChooseLevel:
                    MenuManager_ChoixNiveau.Update();
                    break;

                case GameState.Playing:
                    // Update the level
                    Levels[CurrentLevel].Update(gT);

                    #region Activer le menuIngame
                    if (InputHelper.WasKeyPressed(Keys.Escape)
                        || InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.Start))
                    {
                        GameManager.GameState = GameState.MenuIngame;
                        GameManager.Pause = true;
                        MenuManager_InGame.menuItems = new List<MenuItem>();
                        MenuManager_InGame.CreateMenuItems();
                        MenuManager_InGame.WakeUp();
                    }
                    #endregion

                    break;

                case GameState.MenuIngame:
                    MenuManager_InGame.Update();
                    break;
            }
        }

        public static void Draw(SpriteBatch SB, SpriteBatch SBHUD, GameTime gameTime)
        {
            switch (GameState)
            {
                case GameState.MainMenu:
                    MenuManager_principal.Draw(SBHUD, gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;

                case GameState.HowToPlay:
                    SBHUD.Draw(Game1.SprHTPScreen, Vector2.Zero, Color.White);
                    break;

                case GameState.Option:
                    MenuManager_Option.Draw(SBHUD, gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;

                case GameState.ChooseNumberPlayer:
                    MenuManager_choixPlayer.Draw(SBHUD, gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;

                case GameState.ChooseLevel:
                    MenuManager_ChoixNiveau.Draw(SBHUD, gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;

                case GameState.Playing:
                    Levels[CurrentLevel].Draw(SB, SBHUD);
                    break;

                case GameState.MenuIngame:
                    MenuManager_InGame.Draw(SBHUD, gameTime.ElapsedGameTime.TotalMilliseconds);
                    break;
            }
        }

        private static EnemyClose RandomEnemyClose(Vector2 vect, Level inlevel)
        {
            if (Game1.Random.NextDouble() >= 0.5f)
                return new EnemyCloseMehdi(vect, inlevel);
            else
                return new EnemyCloseQuentin(vect, inlevel);
        }
        private static EnemyClose RandomEnemyClose(Direction dir, Level inlevel)
        {
            if (Game1.Random.NextDouble() >= 0.5f)
                return new EnemyCloseMehdi(dir, inlevel);
            else
                return new EnemyCloseQuentin(dir, inlevel);
        }

        private static EnemyClose2 RandomEnemyClose2(Vector2 vect, Level inlevel)
        {
            if (Game1.Random.NextDouble() >= 0.5f)
                return new EnemyCloseMehdi2(vect, inlevel);
            else
                return new EnemyCloseQuentin2(vect, inlevel);
        }
        private static EnemyClose2 RandomEnemyClose2(Direction dir, Level inlevel)
        {
            if (Game1.Random.NextDouble() >= 0.5f)
                return new EnemyCloseMehdi2(dir, inlevel);
            else
                return new EnemyCloseQuentin2(dir, inlevel);
        }

        public static void CreateLevels()
        {

            Levels.Clear();
            CurrentLevel = LevelChoisie - 1;
            Level level;
            EnemyClose enemy;
            EnemyClose2 enemy2;

            #region levels

            #region Level 1  L'Invitation
            level = new Level();

            //Level 1 Backgrounds
            level.AddBackGroundItem(Game1.SprStage1BGBack, Vector2.Zero, 0.2f, 0.95f);
            level.AddBackGroundItem(Game1.SprStage1BGMain, new Vector2(0, 0), 1f, 0.90f);
            level.AddBackGroundItem(Game1.SprPanneau, new Vector2(50, 150), 1f, 0.85f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(750, 260), 1f, 0.1f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(1400, 310), 1f, 0.1f);


            //Define level 1 starting PlayBounds
            level.PlayBounds = new Rectangle(0, 360, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT - 360);


            //Level 1 Actors
            level.Actors.Add(new Player1(new Vector2(-150, 500),
                            level,
                             PlayerIndex.One));
            level.Player1 = level.Actors[level.Actors.Count - 1] as Player1;
            level.Player1.SetIntroTargetPosition(new Vector2(270, 500));

            //Player 2
            if (NumberPlayers == 2)
            {
                level.Actors.Add(new Player2(new Vector2(-150, 700),
                                level,
                                 PlayerIndex.Two));
                level.Player2 = level.Actors[level.Actors.Count - 1] as Player2;
                level.Player2.SetIntroTargetPosition(new Vector2(270, 550));
            }

            TrashCan t1 = new TrashCan(level, new Vector2(500, 390), new PickUpKnife(level, new Vector2(500, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1100, 390), new PickUpKnife(level, new Vector2(1100, 300)));
            level.GameItems.Add(t1);

            //Créer 2 ennemis avec les bars croisés.
            enemy = RandomEnemyClose(new Vector2(656, 458), level);
            enemy.SetToArmsFolded(Direction.Left);
            level.Actors.Add(enemy);

            enemy = RandomEnemyClose(new Vector2(702, 527), level);
            enemy.SetToArmsFolded(Direction.Left);
            level.Actors.Add(enemy);

            CreateLevel1CutScenes(level);

            Levels.Add(level);

            // Accès à la barre de vie du player
            HUDManager. SetLevel(level);
            Camera.Position = new Vector2(Game1.SCREEN_WIDHT / 2, Game1.SCREEN_HEIGHT / 2);
            #endregion

            #region Level 2  L'alcool
            level = new Level(); // creation d'un nouveau niveau ( le niveau 2)

            //Level 2 backgrounds
            level.AddBackGroundItem(Game1.SprStage2BGBack, new Vector2(0, 0)/*vector2.zero*/, 0.4f, 0.95f);
            level.AddBackGroundItem(Game1.SprStage2BGMain, new Vector2(0, 0/*Game1.SprStage2BGBack.Height - 3*/), 1f, 0.90f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(620, 260), 1f, 0.1f);

            //Define level 2 starting PlayBounds
            level.PlayBounds = new Rectangle(0, 360, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT - 360);

            if (LevelChoisie == 2)
            {
                //Level 2 Actors
                level.Actors.Add(new Player1(new Vector2(-150, 500),
                                level,
                                 PlayerIndex.One));
                level.Player1 = level.Actors[level.Actors.Count - 1] as Player1;
                level.Player1.SetIntroTargetPosition(new Vector2(150, 500));

                //Player 2
                if (NumberPlayers == 2)
                {
                    level.Actors.Add(new Player2(new Vector2(-150, 700),
                                    level,
                                     PlayerIndex.Two));
                    level.Player2 = level.Actors[level.Actors.Count - 1] as Player2;
                    level.Player2.SetIntroTargetPosition(new Vector2(270, 550));
                }


                HUDManager.SetLevel(level);
                Camera.Position = new Vector2(Game1.SCREEN_WIDHT / 2, Game1.SCREEN_HEIGHT / 2);
            }

            enemy2 = RandomEnemyClose2(new Vector2(700, 355), level);
            enemy2.SetToArmsFolded(Direction.Left);
            level.Actors.Add(enemy2);

            //Add a TrashCan with HealthPack
            TrashCan trashCan = new TrashCan(level, new Vector2(400, 425),
                new PickUpKnife(level, Vector2.Zero));
            level.GameItems.Add(trashCan);

            //TrashCan trashCan2 = new TrashCan(level, new Vector2(350, 250),
            //    new PickUpKnife(level, Vector2.Zero));
            //level.GameItems.Add(trashCan2);

            //TrashCan trashCan3 = new TrashCan(level, new Vector2(350, 300),
            //    new PickUpKnife(level, Vector2.Zero));
            //level.GameItems.Add(trashCan3);

            PickUpHealthPack biere;
            biere = new PickUpHealthPack(level, new Vector2(500, 445), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(615, 455), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(580, 445), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(500, 425), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(510, 425), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(525, 420), 10);
            level.GameItems.Add(biere);
            biere = new PickUpHealthPack(level, new Vector2(502, 435), 10);
            level.GameItems.Add(biere);


            CreateLevel2CutScenes(level);

            Levels.Add(level);


            #endregion

            #region Level 3  La vengeance
            level = new Level();


            //Level 1 Backgrounds
            level.AddBackGroundItem(Game1.SprStage3RER, new Vector2(0, 0), 1f, 0.90f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(750, 260), 1f, 0.1f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(1400, 310), 1f, 0.1f);


            //Define level 1 starting PlayBounds
            level.PlayBounds = new Rectangle(0, 360, 1000, Game1.SCREEN_HEIGHT - 360);

            if (LevelChoisie == 3)
            {

                level.Actors.Add(new Player1(new Vector2(-150, 500),
                                level,
                                 PlayerIndex.One));
                level.Player1 = level.Actors[level.Actors.Count - 1] as Player1;
                level.Player1.SetIntroTargetPosition(new Vector2(-10, 500));
                //Player 2
                if (NumberPlayers == 2)
                {
                    level.Actors.Add(new Player2(new Vector2(-150, 700),
                                    level,
                                     PlayerIndex.Two));
                    level.Player2 = level.Actors[level.Actors.Count - 1] as Player2;
                    level.Player2.SetIntroTargetPosition(new Vector2(270, 550));
                }
                HUDManager.SetLevel(level);
                Camera.Position = new Vector2(Game1.SCREEN_WIDHT / 2, Game1.SCREEN_HEIGHT / 2);
            }


            //level.CurrentTrigger = new TriggerNoEnemies();

            CreateLevel3CutScenes(level);
            Levels.Add(level);

            #endregion

            #region Level 5  Un vs Plein de monde
            level = new Level();

           



            //Level 5 Backgrounds
            level.AddBackGroundItem(Game1.SprStage3RER, new Vector2(0, 0/*Game1.SprStage1BGBack.Height - 3*/), 1f, 0.90f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(750, 260), 1f, 0.1f);
            level.AddBackGroundItem(Game1.SprStage1FGEntrance, new Vector2(1400, 310), 1f, 0.1f);


            //Define level 5 starting PlayBounds
            level.PlayBounds = new Rectangle(0, 360, 1800, Game1.SCREEN_HEIGHT - 360);

            if (LevelChoisie == 4)
            {
                //Level 5 Actors
                level.Actors.Add(new Player1(new Vector2(-150, 500),
                                level,
                                 PlayerIndex.One));
                level.Player1 = level.Actors[level.Actors.Count - 1] as Player1;
                level.Player1.SetIntroTargetPosition(new Vector2(-10, 500));
                //Player 2
                if (NumberPlayers == 2)
                {
                    level.Actors.Add(new Player2(new Vector2(-150, 700),
                                    level,
                                     PlayerIndex.Two));
                    level.Player2 = level.Actors[level.Actors.Count - 1] as Player2;
                    level.Player2.SetIntroTargetPosition(new Vector2(270, 550));
                }

                HUDManager.SetLevel(level);
                Camera.Position = new Vector2(Game1.SCREEN_WIDHT / 2, Game1.SCREEN_HEIGHT / 2);
            }


            t1 = new TrashCan(level, new Vector2(600, 390), new PickUpKnife(level, new Vector2(600, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(800, 390), new PickUpKnife(level, new Vector2(800, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1000, 390), new PickUpKnife(level, new Vector2(1000, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1200, 390), new PickUpKnife(level, new Vector2(1200, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1400, 390), new PickUpKnife(level, new Vector2(1400, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1600, 390), new PickUpKnife(level, new Vector2(1600, 300)));
            level.GameItems.Add(t1);

            t1 = new TrashCan(level, new Vector2(1800, 390), new PickUpKnife(level, new Vector2(1800, 300)));
            level.GameItems.Add(t1);






            CreateLevel5CutScenes(level);
            Levels.Add(level);
            #endregion

            #endregion
        }

        static private void CreateLevel5CutScenes(Level level)
        {
            //CutScene Information
            CutScene scene;
            string line;
            VoiceSoundEnum voiceCueName;
            Texture2D portrait;
            float timer;
            //SceneEvent information
            SceneEvent sceneEvent;
            Trigger trigger;

            #region vagues

            #region Vague de 1 à 10
            #region Vague 1
            scene = new CutScene();
            //line1
            line = "Dernier niveau de Street No Rules";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line2
            line = "Vous devez survivre le plus longtemps possible et atteindre le plus grand nombre de vagues...";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 3.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line
            line = "                           Vague 1";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);


            #region Vague 2
            scene = new CutScene();
            //line1
            line = "                           Vague 2";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 3
            scene = new CutScene();
            //line1
            line = "                           Vague 3";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 4
            scene = new CutScene();
            //line1
            line = "                           Vague 4";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 5
            scene = new CutScene();
            //line1
            line = "                           Vague 5";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 6
            scene = new CutScene();
            //line1
            line = "                           Vague 6";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 7
            scene = new CutScene();
            //line1
            line = "                           Vague 7";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 8
            scene = new CutScene();
            //line1
            line = "                           Vague 8";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 9
            scene = new CutScene();
            //line1
            line = "                           Vague 9";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 10
            scene = new CutScene();
            //line1
            line = "                           Vague 10";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Vague de 11 à 20
            #region Vague 11
            scene = new CutScene();
            //line1
            line = "                           Vague 11";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 12
            scene = new CutScene();
            //line1
            line = "                           Vague 12";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 13
            scene = new CutScene();
            //line1
            line = "                           Vague 13";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 14
            scene = new CutScene();
            //line1
            line = "                           Vague 14";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 15
            scene = new CutScene();
            //line1
            line = "                           Vague 15";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 16
            scene = new CutScene();
            //line1
            line = "                           Vague 16";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 17
            scene = new CutScene();
            //line1
            line = "                           Vague 17";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 18
            scene = new CutScene();
            //line1
            line = "                           Vague 18";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 19
            scene = new CutScene();
            //line1
            line = "                           Vague 19";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 20
            scene = new CutScene();
            //line1
            line = "                           Vague 20";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Vague de 21 à 30
            #region Vague 21
            scene = new CutScene();
            //line1
            line = "                           Vague 21";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 22
            scene = new CutScene();
            //line1
            line = "                           Vague 22";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 23
            scene = new CutScene();
            //line1
            line = "                           Vague 23";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 24
            scene = new CutScene();
            //line1
            line = "                           Vague 24";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 25
            scene = new CutScene();
            //line1
            line = "                           Vague 25";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 26
            scene = new CutScene();
            //line1
            line = "                           Vague 26";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 27
            scene = new CutScene();
            //line1
            line = "                           Vague 27";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 28
            scene = new CutScene();
            //line1
            line = "                           Vague 28";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 29
            scene = new CutScene();
            //line1
            line = "                           Vague 29";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 30
            scene = new CutScene();
            //line1
            line = "                           Vague 30 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Vague de 31 à 40
            #region Vague 31
            scene = new CutScene();
            //line1
            line = "                           Vague 31 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 32
            scene = new CutScene();
            //line1
            line = "                           Vague 32 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 33
            scene = new CutScene();
            //line1
            line = "                           Vague 33 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 34
            scene = new CutScene();
            //line1
            line = "                           Vague 34 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 35
            scene = new CutScene();
            //line1
            line = "                           Vague 35 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 36
            scene = new CutScene();
            //line1
            line = "                           Vague 36 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 37
            scene = new CutScene();
            //line1
            line = "                           Vague 37 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 38
            scene = new CutScene();
            //line1
            line = "                           Vague 38 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 39
            scene = new CutScene();
            //line1
            line = "                           Vague 39 ";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Vague 40
            scene = new CutScene();
            //line1
            line = "                           Vague 40";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemies();
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose2(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #endregion

            #region Fin
            scene = new CutScene();
            //line1
            line = "                        !! FINI !!";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprLevel5;
            timer = 2.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);

            sceneEvent = new SceneEvent();
            trigger = new TriggerNextLevel();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion

        }



        static private void CreateLevel3CutScenes(Level level)
        {
            //CutScene Information
            CutScene scene;
            string line;
            VoiceSoundEnum voiceCueName;
            Texture2D portrait;
            float timer;
            //SceneEvent information
            SceneEvent sceneEvent;
            Trigger trigger;


            #region Annonce de la première vague
            scene = new CutScene();
            //line1
            line = "*Riiing* *Riiing*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line2
            if (GameManager.Langage == "Francais")
                line = "Allo ?";
            if (GameManager.Langage == "Anglais")
                line = "Hi ?";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line3
            if (GameManager.Langage == "Francais")
            line = "Je te vois ! J'envoi des potes sur toi pour te demonter !! *clic*";
            if (GameManager.Langage == "Anglais")
                line = "I see you ! I send some friends over you..!";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemy;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Annonce de la deuxième vague
            scene = new CutScene();
            //line1
            line = "*Riiing* *Riiing*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line2
            if (GameManager.Langage == "Francais")
            line = "Allo ?!!";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //line3
            if (GameManager.Langage == "Francais")
            line = "Tkt pas je t'en envoie d'autres, tu vas bientot etre K.O. *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);


            #region Annonce de la troisième vague
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line2
            if (GameManager.Langage == "Francais")
            line = "Quoi?!!!";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line3
            if (GameManager.Langage == "Francais")
            line = "Grrr c'est pas fini !! *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Annonce de la quatrième vague
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line2
            if (GameManager.Langage == "Francais")
            line = "...";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line3
            if (GameManager.Langage == "Francais")
            line = "Dans la vie, y a ceux qui tapent et ceux qui se font taper...";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            //line4
            if (GameManager.Langage == "Francais")
            line = "Toi ... tu tfais taper ! *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);


            #region Annonce de la cinquième vague
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing* Ziva tes mort ! *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Annonce de la sixième vague
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing* Alors on veut jouer son petit LOUBARD hein ?!";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            if (GameManager.Langage == "Francais")
            line = "J'ramene tout ca a la morgue et jrentre a la maison ! *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayerAngry;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);

            #region Annonce de la septième vague
            scene = new CutScene();
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing* Jtamene un petit cadeau Muwhahaha! *clic*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyCloseQuentin2(Direction.Left, level));
            level.SceneEvents.Add(sceneEvent);

            #region Annonce de la huitième vague
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "*Riiing* *Riiing* J'ai des muscles a des endroits dont tu n'as jamais entendu parler.";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            if (GameManager.Langage == "Francais")
            line = " Dommage que tu n'en aies pas dans les bras.";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayerAngry;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

           if (GameManager.Langage == "Francais")
            line = " Ok maintenant je viens et jte pete la tronche ! *clic*";
           voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemy;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(0, 0, 1000, 1000));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddEnemy(new EnemyCloseMehdi2(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            level.SceneEvents.Add(sceneEvent);


            #region Annonce de la fin
            scene = new CutScene();
            //line1
            if (GameManager.Langage == "Francais")
            line = "Aaaaargh! OK ok ok ! J'arrete ! Je m'en vais";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSenemy;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            if (GameManager.Langage == "Francais")
            line = "Je prefere mieux ca....";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayerAngry;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);
            level.CutScenes.Add(scene);
            #endregion

            sceneEvent = new SceneEvent();
            trigger = new TriggerNextLevel();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);

        }

        static private void CreateLevel2CutScenes(Level level)
        {
            //CutScene Information
            CutScene scene;
            string line;
            VoiceSoundEnum voiceCueName;
            Texture2D portrait;
            float timer;

            //SceneEvent information
            SceneEvent sceneEvent;
            Trigger trigger;

            #region Level Introduction
            #region CutScene
            scene = new CutScene();


            //Line1

            if (GameManager.Langage == "Francais")
                line = "Y parait que y'a de l'alcool ici!!";
            else if (GameManager.Langage == "Anglais")
                line = "it seems that there is alcohol here!";
            else
            {
                line = "";
            }
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayer;
            timer = 2.7f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line2
            if (GameManager.Langage == "Francais")
                line = "Ouai mais pas pour toi mon p'tit gars !";
            else if (GameManager.Langage == "Anglais")
                line = "Yeah but not for you runt";
            voiceCueName = VoiceSoundEnum.YA;
            portrait = Game1.SprCSenemyLaugh;
            timer = 2.7f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line3
            if (GameManager.Langage == "Francais")
                line = "Comment tu m'appelles toi ?";
           else if (GameManager.Langage == "Anglais")
                line = "Repeat if you dare?";
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayerAngry;
            timer = 1.7f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line4
            if (GameManager.Langage == "Francais")
                line = "MON PETIT GARS";
            else if (GameManager.Langage == "Anglais")
                line = "RUNT";
            voiceCueName = VoiceSoundEnum.YA;
            portrait = Game1.SprCSenemyLaugh;
            timer = 1.7f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line5
            if (GameManager.Langage == "Francais")
                line = "Toi, tes potes et MOI! Finis la rigolade... On va se battre!";
            else if (GameManager.Langage == "Anglais")
                line = "Stop talking, let's fight !";
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayerAngry;
            timer = 2.7f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            level.CutScenes.Add(scene);
            #endregion

            #region Post Intro SceneEvent
            sceneEvent = new SceneEventActivateEnemies();
            trigger = new TriggerNoEnemiesNoCS();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            sceneEvent.AddPlayBounds(400); // End of cutscene, let Playbounds go to left
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));
            #endregion
            #endregion

            #region Move Outside
            sceneEvent = new SceneEvent();
            trigger = new TriggerHitBoxNoCS(new Rectangle(600, 422, 800, 200));
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Outside
            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemiesNoCS();
            sceneEvent.AddTrigger(trigger);
            // spawn 1 boss
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Right, level));

            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Beaten up final enemies
            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnemiesNoCS();
            sceneEvent.AddPlayBounds(100);
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Final CutScene
            scene = new CutScene();
            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(1250, 400, 200, 200));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddPlayBounds(2469);
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region They got away
            #endregion

            #region CutScene
            //Line1
            if (GameManager.Langage == "Francais")
                line = "Je ne sais pas ce qu'il me voulait ce dernier mec...";
            if (GameManager.Langage == "Anglais")
                line = "I don't know what this last guy wanted, anyway...";

            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayer;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line2
            if (GameManager.Langage == "Francais")
                line = "De toute facon je m'en fiche car ...";

            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayer;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line3
            if (GameManager.Langage == "Francais")
                line = "JE VAIS FAIRE LA FETE!";

            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayerAngry;
            timer = 2.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);


            level.CutScenes.Add(scene);
            #endregion


            #region Post2 CutScene event
            sceneEvent = new SceneEvent();
            trigger = new TriggerNextLevel();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion

        }

        static private void CreateLevel1CutScenes(Level level)
        {
            //CutScene Information
            CutScene scene;
            string line;
            VoiceSoundEnum voiceCueName;
            Texture2D portrait;
            float timer;

            //SceneEvent information
            SceneEvent sceneEvent;
            Trigger trigger;

            #region Level Introduction
            #region Cutscene
            scene = new CutScene();


            //Line1
            line = "*Riiing* *Riiing*";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 1.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line2
            if (GameManager.Langage == "Francais")
                line = "Allo?...Je suis sorti de chez moi, j'arrive dans 10 min!...";
            else if (GameManager.Langage == "Anglais")
                line = "Hi?...I just left my home, I arrive in 10 minutes!...";

            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 3.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            level.CutScenes.Add(scene);
            #endregion

            #region Post Intro SceneEvent
            sceneEvent = new SceneEvent();
            trigger = new TriggerHitBox(new Rectangle(500, 220, 300, 400)); // zone de triggerbox
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion
            #endregion

            #region First CutScene - Premiere scene
            #region CutScene
            scene = new CutScene();

            //Line 1
            if (GameManager.Langage == "Francais")
                line = "Wesh t'es qui ?! Mets pas les pieds ici toi !";
            else if (GameManager.Langage == "Anglais")
                line = "Hey ! Who are you ? , don't put your foot here you !";
            voiceCueName = VoiceSoundEnum.YA;
            portrait = Game1.SprCSenemy;
            timer = 2.2f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line 2
            if (GameManager.Langage == "Francais")
                line = "Je met les pieds ou je veux, et c'est souvent dans la TETE !";
            else if (GameManager.Langage == "Anglais")
                line = "I put my feet where I want ... And often in the FACE!";
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayer;
            timer = 2.8f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line 3
            if (GameManager.Langage == "Francais")
                line = "Assez parler ! Battons-nous, amenez vous les gars !";
            else if (GameManager.Langage == "Anglais")
                line = "Enough talk! Let us fight! Bring you guys!";
            voiceCueName = VoiceSoundEnum.YA;
            portrait = Game1.SprCSenemy;
            timer = 2.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line 4
            if (GameManager.Langage == "Francais")
                line = "Amenes toi !!";
            else if (GameManager.Langage == "Anglais")
                line = "Let's fight!";
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayerAngry;
            timer = 1.0f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            level.CutScenes.Add(scene);
            #endregion

            #region Post CutScene event
            sceneEvent = new SceneEventActivateEnemies();
            trigger = new TriggerNoEnemiesNoCS();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion
            #endregion

            #region just beat up the two thugs
            sceneEvent = new SceneEvent();
            trigger = new TriggerHitBoxNoCS(new Rectangle(960, 400, 200, 200));
            sceneEvent.AddTrigger(trigger);
            sceneEvent.AddPlayBounds(500);  // ICI ON RAJOUTE UNE ZONE DE JEU ( pas besoin de faire la touche Q, d'ailleur il faudra enlever cette touche)
            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region just came through the doorway
            sceneEvent = new SceneEvent();
            trigger = new TriggerNoEnnemiesHitBox(new Rectangle(1500, 400, 200, 200));
            sceneEvent.AddTrigger(trigger);
            //sceneEvent.AddEnemySpawners(
            //new EnemySpawner(new Vector2(969, 422), Game1.SprDoorEntrySpawner,
            //new EnemyRanged(new Vector2(969, 422), level)));

            //new EnemySpawner(new Vector2(969, 422), Game1.SprDoorEntrySpawner,
            //new EnemyRanged(new Vector2(969, 422), level)));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(RandomEnemyClose(Direction.Left, level));
            sceneEvent.AddEnemy(new EnemyRanged(Direction.Left, level));

            sceneEvent.AddPlayBounds(469);

            level.SceneEvents.Add(sceneEvent);
            #endregion

            #region Bottom of the elevator
            #region CutScene
            scene = new CutScene();

            //Line 1
            if (GameManager.Langage == "Francais")
                line = "Quoi ? Un bruit de bouteille par la-bas.";
            else if (GameManager.Langage == "Anglais")
                line = "What ? I hear the sound of a bottle.";
            voiceCueName = VoiceSoundEnum.HE;
            portrait = Game1.SprCSplayer;
            timer = 2.4f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            //Line 2
            if (GameManager.Langage == "Francais")
                line = "Je crois que je vais avoir une bouteille pour ce soir !";
            else if (GameManager.Langage == "Anglais")
                line = "I think I will have a bottle this night !";
            voiceCueName = VoiceSoundEnum.OH;
            portrait = Game1.SprCSplayer;
            timer = 3.5f;
            scene.AddLine(line, voiceCueName, portrait, timer);

            level.CutScenes.Add(scene);
            #endregion

            //Insert a trigger to take game to the next level (passer au niveau suivant grace a un declencheur )
            #region PostCutScene Event
            sceneEvent = new SceneEvent();
            //Trigger to take us to the next level
            trigger = new TriggerNextLevel();
            sceneEvent.AddTrigger(trigger);
            level.SceneEvents.Add(sceneEvent);
            #endregion
            #endregion
        }

        public static void GoToNextLevel()
        {
            CurrentLevel++; // on passe au niveau suivant
            HUDManager.SetLevel(Levels[CurrentLevel]);

            // Copy the needed details across to the next level
            // set the player1 to the SAME Players as the last level
            Levels[CurrentLevel].Player1 = Levels[CurrentLevel - 1].Player1; // On prend LE MEME hero que celui du niveau précédent ( on conserve la vie et ses couteau :p )

            //Make sure Player1 is in the new levels' Actors list
            Levels[CurrentLevel].Actors.Add(Levels[CurrentLevel].Player1);
            //Set Player1's Inlevel
            Levels[CurrentLevel].Player1.InLevel = Levels[CurrentLevel];


            //Reset Player1's draw Details

            Levels[CurrentLevel].Player1.ResetIdleGraphic();
            Levels[CurrentLevel].Player1.Position = new Vector2(-200, 500);
            Levels[CurrentLevel].Player1.SetIntroTargetPosition(new Vector2(300, 500));
            Levels[CurrentLevel].Player1.state = PlayerState.LevelIntro;


            if (GameManager.NumberPlayers == 2)
            {
                Levels[CurrentLevel].Player2 = Levels[CurrentLevel - 1].Player2;
                Levels[CurrentLevel].Actors.Add(Levels[CurrentLevel].Player2);
                //Set Player1's Inlevel
                Levels[CurrentLevel].Player2.InLevel = Levels[CurrentLevel];

                Levels[CurrentLevel].Player2.ResetIdleGraphic();
                Levels[CurrentLevel].Player2.Position = new Vector2(-200, 650);
                Levels[CurrentLevel].Player2.SetIntroTargetPosition(new Vector2(300, 560));
                Levels[CurrentLevel].Player2.state = PlayerState.LevelIntro;
            }


            Levels[CurrentLevel].LevelState = LevelState.FadeIn;

            Camera.Position = new Vector2(Game1.SCREEN_WIDHT / 2, Game1.SCREEN_HEIGHT / 2);
        }
    }
}
