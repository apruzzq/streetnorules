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
    public static class Menu
    {
        static Game game;

        static List<SousMenu> menuList; //liste de SousMenu (qui est la classe SousMenu)
        static SousMenu activeMenu;
        static SousMenu niveauFacile;//quelques sous-menus contenants les items (boutons nouveau jeu, option)
        static SousMenu niveauMoyen;
        static SousMenu niveauDifficile;

        static KeyboardState lastKeybState;//raccourci
        static public bool menusRunning = true;//vérifie que le menu est en route
        static public bool decision = false;//dit si le joueur a décidé de lancé le jeu
        static SpriteBatch spriteBatch;

        public static void Initialize()
        {

            lastKeybState = Keyboard.GetState();
            menuList = new List<SousMenu>();//instanciation de la liste de menu contenants des listes d'items

        }

        public static void CreateMenuItems()
        {

            SpriteFont menuFont = game.Content.Load<SpriteFont>("menu\\FontMenu");//police du menu
            Texture2D backgroundImage = game.Content.Load<Texture2D>("menu\\bg");//première image
            Texture2D bg = game.Content.Load<Texture2D>("menu\\bg2");//deuxième

            niveauFacile = new SousMenu(null, null, null);
            niveauMoyen = new SousMenu(null, null, null);//les start games sont en réalité des sous-menus vides
            niveauDifficile = new SousMenu(null, null, null);

            //un menu window est constitué d'une police pour ses items, d'un titre apparaissant en haut et d'une image de font.
            SousMenu menuMain = new SousMenu(menuFont, "STREET NO RULES", backgroundImage);
            SousMenu menuNewGame = new SousMenu(menuFont, "Pour moi, ils sont deja tous morts!", bg);
            SousMenu menuOptions = new SousMenu(menuFont, "Options Menu", backgroundImage);
            menuList.Add(menuMain);
            menuList.Add(menuNewGame);
            menuList.Add(menuOptions);//on ajoute ces trois menus à notre liste.

            menuMain.AddMenuItem("Nouvelle Descente", menuNewGame);//ajout d'un item à la liste d'item du menu lui même contenu dans une liste de menus........
            menuMain.AddMenuItem("Code de niveau", menuMain);
            menuMain.AddMenuItem("Options", menuOptions);
            menuMain.AddMenuItem("Finir la bagarre", null);

            menuNewGame.AddMenuItem("Avorton", niveauFacile);
            menuNewGame.AddMenuItem("Kaid", niveauMoyen);
            menuNewGame.AddMenuItem("Loubard", niveauDifficile);
            menuNewGame.AddMenuItem("Retour wesh!", menuMain);

            menuOptions.AddMenuItem("Changement des controles", menuMain);
            menuOptions.AddMenuItem("Changement du visuel", menuMain);
            menuOptions.AddMenuItem("Changement du son", menuMain);
            menuOptions.AddMenuItem("Retour yo!", menuMain);

            activeMenu = menuMain;
            menuMain.WakeUp();
        }

      /*  protected static void UnloadContent()
        {
        }*/

        public static void Update(GameTime gameTime)
        {

            KeyboardState keybState = Keyboard.GetState();

            if (menusRunning)
            {
                foreach (SousMenu currentMenu in menuList)
                    currentMenu.Update(gameTime.ElapsedGameTime.TotalMilliseconds); //défilmenent du menu
                MenuInput(keybState);
            }
            else
            {
            }

            lastKeybState = keybState;
        }

        public static void MenuInput(KeyboardState currentKeybState)
        {
            SousMenu newActive = activeMenu.ProcessInput(lastKeybState, currentKeybState);

            if (newActive == niveauFacile)
            {
                //niveau facile
                menusRunning = false;
                GameManager.GameState = GameState.Playing;
            }
            else if (newActive == niveauMoyen)
            {
                //niveau normal
                menusRunning = false;
                GameManager.GameState = GameState.Playing;
            }
            else if (newActive == niveauDifficile)
            {
                //niveau difficile
                menusRunning = false;
                GameManager.GameState = GameState.Playing;
            }
            else if (newActive == null)
                game.Exit();
            else if (newActive != activeMenu)
                newActive.WakeUp();

            activeMenu = newActive;
        }

        public static void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1, 0);

            if (menusRunning)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                foreach (SousMenu currentMenu in menuList)
                    currentMenu.Draw(spriteBatch);
                spriteBatch.End();
                game.Window.Title = "Menu en route ...";
            }
            else
            {
                game.Window.Title = "Jeu en route ...";
            }

            List<string> ppEffectsList = new List<string>();
            ppEffectsList.Add("HorBlur");
            ppEffectsList.Add("VerBlur");
            float blurRadius = (float)Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 1000.0f);
            if (blurRadius < 0)
                blurRadius = 0;
        }
    }
}
