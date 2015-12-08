using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Les_Loubards
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public const int SCREEN_WIDHT = 800;
        public const int SCREEN_HEIGHT = 600;
        public static Random Random;

        static bool exitGame;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch spriteBatchHUD;

        public static SpriteFont FontSmall;
        public static SpriteFont FontLarge;

        public static Texture2D SprSinglePixel;

        #region LevelTextures
        //In-Game
        public static Texture2D SprStage1BGMain;
        public static Texture2D SprStage1BGBack;
        public static Texture2D SprStage1FGEntrance;
        public static Texture2D SprStage2BGMain;
        public static Texture2D SprStage2BGBack;
        public static Texture2D SprStage3RER;
        public static Texture2D SprPause;

        //CutScene
        public static Texture2D SprCSenemy;
        public static Texture2D SprCSenemyLaugh;
        public static Texture2D SprCSplayer;
        public static Texture2D SprCSplayerAngry;
        public static Texture2D SprLevel5;
        #endregion

        #region Character Textures
        public static Texture2D SprCharacterAttacks;
        public static Texture2D SprCharacterReacts;
        public static Texture2D SprCharacterShadow;

        public static Texture2D SprCharacterAttacksMehdi;
        public static Texture2D SprCharacterReactsMehdi;
        public static Texture2D SprCharacterAttacksQuentin;
        public static Texture2D SprCharacterReactsQuentin;
        public static Texture2D SprCharacterReactsEnnemi;

        #endregion

        #region Game Item Textures
        public static Texture2D SprThrowingKnife;
        public static Texture2D SprHealthPack;
        public static Texture2D SprTrashCanNormal;
        public static Texture2D SprTrashCanHit;
        public static Texture2D SprPanneau;
        #endregion

        #region TitleScreen Textures
        public static Texture2D SprTitleScreenPrincipalMenu;
        public static Texture2D SprTitleStreetNoRules;
        public static Texture2D SprTitleChoiceDifficulty;
        public static Texture2D SprTitleChoiceLevel;
        public static Texture2D SprHTPScreen;
        #endregion

        #region VisageDoom Textures
        public static Texture2D A1, A2, A3, A4, A5, B1, B2, C1, C2, C3, C4, C5, Death; // Quentin
        public static Texture2D D1, D2, D3, D4, D5, E1, E2, F1, F2, DeathMehdi; // Mehdi
        #endregion

        #region Music
        public static Song MusicTitleScreen;
        public static Song MusicFighting;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            exitGame = false;
            IsMouseVisible = true;
            Window.Title = "Street No Rules !";
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatchHUD = new SpriteBatch(GraphicsDevice);
            Random = new Random();

            SprSinglePixel = Content.Load<Texture2D>(@"Textures\SinglePixel");
            SprPause = Content.Load<Texture2D>(@"Textures\Pause");
            FontSmall = Content.Load<SpriteFont>(@"Textures\FontSmall");
            FontLarge = Content.Load<SpriteFont>(@"Textures\FontLarge");

            //Level 1
            SprStage1BGBack = Content.Load<Texture2D>(@"Textures\Level\SKY");
            SprStage1BGMain = Content.Load<Texture2D>(@"Textures\Level\carte");
            SprStage1FGEntrance = Content.Load<Texture2D>(@"Textures\Level\lampadaire");

            //level 2
            SprStage2BGBack = Content.Load<Texture2D>(@"Textures\Level\SKY2");
            SprStage2BGMain = Content.Load<Texture2D>(@"Textures\Level\carte2");
            //level3
            SprStage3RER = Content.Load<Texture2D>(@"Textures\Level\rer");

            SprCharacterAttacks = Content.Load<Texture2D>(@"Textures\Characters\warrior");
            SprCharacterReacts = Content.Load<Texture2D>(@"Textures\Characters\warrior_reacts");
            SprCharacterShadow = Content.Load<Texture2D>(@"Textures\Characters\shadow");

            SprCharacterAttacksMehdi = Content.Load<Texture2D>(@"Textures\Characters\warrior_mehdi");
            SprCharacterReactsMehdi = Content.Load<Texture2D>(@"Textures\Characters\warrior_react_mehdi");
            SprCharacterAttacksQuentin = Content.Load<Texture2D>(@"Textures\Characters\warrior");
            SprCharacterReactsQuentin = Content.Load<Texture2D>(@"Textures\Characters\warrior_reacts");

            SprCSenemy = Content.Load<Texture2D>(@"Textures\CutScene\enemy");
            SprCSenemyLaugh = Content.Load<Texture2D>(@"Textures\CutScene\enemyLaugh");
            SprCSplayer = Content.Load<Texture2D>(@"Textures\CutScene\player");
            SprCSplayerAngry = Content.Load<Texture2D>(@"Textures\CutScene\playerAngry");
            SprLevel5 = Content.Load<Texture2D>(@"Textures\CutScene\StreetNoRulesLevel5");

            SprThrowingKnife = Content.Load<Texture2D>(@"Textures\throwingKnife");
            SprHealthPack = Content.Load<Texture2D>(@"Textures\healthPack");
            SprTrashCanHit = Content.Load<Texture2D>(@"Textures\trashcanHit");
            SprTrashCanNormal = Content.Load<Texture2D>(@"Textures\trashcanNormal");
            SprPanneau = Content.Load<Texture2D>(@"Textures\Panneau");

            SprTitleScreenPrincipalMenu = Content.Load<Texture2D>(@"Textures\TitleScreen");
            SprTitleStreetNoRules = Content.Load<Texture2D>(@"Textures\TitleScreen2");
            SprTitleChoiceDifficulty = Content.Load<Texture2D>(@"Textures\TitleScreen3");
            SprTitleChoiceLevel = Content.Load<Texture2D>(@"Textures\TitleScreen4");
            SprHTPScreen = Content.Load<Texture2D>(@"Textures\HowToPlay");

            A1 = Content.Load<Texture2D>(@"Textures\VisageDoom\A1");
            A2 = Content.Load<Texture2D>(@"Textures\VisageDoom\A2");
            A3 = Content.Load<Texture2D>(@"Textures\VisageDoom\A3");
            A4 = Content.Load<Texture2D>(@"Textures\VisageDoom\A4");
            A5 = Content.Load<Texture2D>(@"Textures\VisageDoom\A5");
            B1 = Content.Load<Texture2D>(@"Textures\VisageDoom\B1");
            B2 = Content.Load<Texture2D>(@"Textures\VisageDoom\B2");
            C1 = Content.Load<Texture2D>(@"Textures\VisageDoom\C1");
            C2 = Content.Load<Texture2D>(@"Textures\VisageDoom\C2");
            C3 = Content.Load<Texture2D>(@"Textures\VisageDoom\C3");
            C4 = Content.Load<Texture2D>(@"Textures\VisageDoom\C4");
            C5 = Content.Load<Texture2D>(@"Textures\VisageDoom\C5");
            Death = Content.Load<Texture2D>(@"Textures\VisageDoom\Death");

            D1 = Content.Load<Texture2D>(@"Textures\VisageDoom\D1");
            D2 = Content.Load<Texture2D>(@"Textures\VisageDoom\D2");
            D3 = Content.Load<Texture2D>(@"Textures\VisageDoom\D3");
            D4 = Content.Load<Texture2D>(@"Textures\VisageDoom\D4");
            D5 = Content.Load<Texture2D>(@"Textures\VisageDoom\D5");
            E1 = Content.Load<Texture2D>(@"Textures\VisageDoom\E1");
            E2 = Content.Load<Texture2D>(@"Textures\VisageDoom\E2");
            F1 = Content.Load<Texture2D>(@"Textures\VisageDoom\F1");
            F2 = Content.Load<Texture2D>(@"Textures\VisageDoom\F2");
            DeathMehdi = Content.Load<Texture2D>(@"Textures\VisageDoom\DeathMehdi");

            MusicFighting = Content.Load<Song>(@"Music\WARRIOR_THEME");
            MusicTitleScreen = Content.Load<Song>(@"Music\MENU-THEME");

            SoundManager.Initialize(Content);
            MenuManager_principal.CreateMenuItems();

            //Play TitleScreen Introduction VoiceOver  ICI CEST LE SON QUE L'ON METTRA AU MENU
            SoundManager.PlayVoiceSound(VoiceSoundEnum.TITLE_SCREEN);


            //Play TitleScreen music
            MusicManager.PlaySong(Game1.MusicTitleScreen);

            MenuManager_ChoixNiveau.CreateMenuItems();
            MenuManager_Option.langage_items = false;
            MenuManager_Option.CreateMenuItems();




        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || exitGame)
                this.Exit();

            InputHelper.UpdateStates();

            GameManager.Update(gameTime);
            SoundManager.Update();
            MusicManager.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatchHUD.Begin();
            GameManager.Draw(spriteBatch, spriteBatchHUD, gameTime);
            spriteBatch.End();
            spriteBatchHUD.End();

            base.Draw(gameTime);
        }

        public static void ExitGame()
        {
            exitGame = true;
        }


    }
}
