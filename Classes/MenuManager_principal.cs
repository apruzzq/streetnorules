using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;


namespace Les_Loubards
{
    enum WindowState
    {
        Starting,
        Active,
        Ending,
        Inactive
    }

    struct MenuItem
    {
        string text;
        Vector2 position;


        public MenuItem(string text, Vector2 position)
        {
            this.text = text;
            this.position = position;
        }

        public void Draw(SpriteBatch SBHUD, Color drawColor, float xPosition)
        {
            // SBHUD.DrawString(Game1.FontSmall,  text, position, drawColor);
            SBHUD.DrawString(Game1.FontSmall, text, new Vector2(xPosition, this.position.Y), drawColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
    static class MenuManager_principal
    {


        public static List<MenuItem> menuItems = new List<MenuItem>();
        static int currentMenuItem = 0;
        public static WindowState windowState = WindowState.Inactive;
        static double changeProgress = 0;
        static float verPosition = 300;
        static float horPosition = 300;
        static float horPosition2 = 315;
        static TimeSpan changeSpan = TimeSpan.FromMilliseconds(800);



        public static void Update()
        {

            //Menu Navigation

            if (InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.DPadUp)
                || (InputHelper.NGS[(int)PlayerIndex.One].ThumbSticks.Left.Y < 0.3 &&
                    InputHelper.OGS[(int)PlayerIndex.One].ThumbSticks.Left.Y > 0.3)
                || InputHelper.WasKeyPressed(Keys.Up))
            {
                currentMenuItem--;
                if (currentMenuItem < 0)
                    currentMenuItem = menuItems.Count - 1;
            }


            if (InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.DPadDown)
                || (InputHelper.NGS[(int)PlayerIndex.One].ThumbSticks.Left.Y < -0.3 &&
                    InputHelper.OGS[(int)PlayerIndex.One].ThumbSticks.Left.Y > -0.3)
                || InputHelper.WasKeyPressed(Keys.Down))
            {
                currentMenuItem++;
                if (currentMenuItem >= menuItems.Count)
                    currentMenuItem = 0;
            }

            //Menu Item Actions
            if (InputHelper.WasButtonPressed(PlayerIndex.One, Buttons.A)
                || InputHelper.WasKeyPressed(Keys.Space))
            {
                switch (currentMenuItem)
                {
                    case 0: //Choose Number players
                        MenuManager_choixPlayer.menuItems = new List<MenuItem>();
                        MenuManager_choixPlayer.CreateMenuItems();
                        GameManager.GameState = GameState.ChooseNumberPlayer;
                        MenuManager_choixPlayer.windowState = WindowState.Starting;
                        break;

                    case 1: // Options
                        MenuManager_Option.menuItems = new List<MenuItem>();
                        MenuManager_Option.CreateMenuItems();
                        GameManager.GameState = GameState.Option;
                        MenuManager_Option.windowState = WindowState.Starting;
                        break;

                    case 2: // Exit Game
                        Game1.ExitGame();
                        break;
                }
            }
        }

        public static void Draw(SpriteBatch SBHUD, double timePassedSinceLastFrame)
        {

            if (windowState == WindowState.Inactive)
                return;

            float smoothedProgress = MathHelper.SmoothStep(0, 1, (float)changeProgress);
            float alphaValue;
            Color bgColor;




            //Draw the Background >> Always
            SBHUD.Draw(Game1.SprTitleScreenPrincipalMenu, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), Color.White);


            verPosition = 300;
            horPosition = 300;
            horPosition2 = 315;

            switch (windowState)
            {
                case WindowState.Starting://animation ouverture nouveau menu
                    horPosition -= 300 * (1.0f - (float)smoothedProgress);  //fais défiler progressivement le menu de la gauche vers le centre
                    horPosition2 += 300 * (1.0f - (float)smoothedProgress);
                    alphaValue = smoothedProgress;//transparrance allant progressivement de 0 (invisible) à 1 (max)
                    bgColor = new Color(new Vector4(1, 1, 1, alphaValue));
                    break;
                case WindowState.Ending://animation fermeture menu
                    horPosition += 200 * (float)smoothedProgress; //centre vers droite
                    horPosition2 -= 200 * (float)smoothedProgress; //centre vers droite
                    alphaValue = 1.0f - smoothedProgress; //1 à 0
                    bgColor = Color.White;
                    break;
                default:
                    alphaValue = 1;
                    bgColor = Color.White;
                    break;


            }
            //Give a slightly darkened overplay onver the background
            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), new Color(Color.Black, alphaValue / 2));

            //défilement du titre
            SBHUD.DrawString(Game1.FontSmall, "Street No Rules", new Vector2(horPosition2, 20), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            for (int itemID = 0; itemID < menuItems.Count; itemID++) //défilement des items
            {

                Color itemColor = Color.White;

                if (itemID == currentMenuItem)
                    itemColor = new Color(new Vector4(1, 0, 0, alphaValue));
                else
                    itemColor = new Color(new Vector4(1, 1, 1, alphaValue));

                if (itemID % 2 == 0)//un item sur 2
                {
                    menuItems[itemID].Draw(SBHUD, itemColor, horPosition);
                }
                else
                {

                    menuItems[itemID].Draw(SBHUD, itemColor, horPosition2);
                }

            }


            if ((windowState == WindowState.Starting) || (windowState == WindowState.Ending))  //compteur qui se charge jusqu'à 1.0f
                changeProgress += (timePassedSinceLastFrame / changeSpan.TotalMilliseconds);//vitesse de défilement

            if (changeProgress >= 1.0f) // Permet d'afficher la page entrain de charger directe, et juste elle
            {
                changeProgress = 0.0f; //compteur réinitialisé pour pouvoir rechanger de page
                if (windowState == WindowState.Starting)
                    windowState = WindowState.Active;
                else if (windowState == WindowState.Ending)
                    windowState = WindowState.Inactive;
            }
        }





        public static void CreateMenuItems()
        {
            if (GameManager.Langage == "Francais")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("Nouvelle partie", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Options", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Quitter", new Vector2(horPosition, verPosition + 80)));
            }
            else if (GameManager.Langage == "Anglais")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("New game", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Options", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Quit", new Vector2(horPosition, verPosition + 80)));
            }
            else if (GameManager.Langage == "Espagnol")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("Nuevo juego", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("opciones", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Salir", new Vector2(horPosition, verPosition + 80)));
            }

            WakeUp();
        }

        public static void WakeUp()
        {
            windowState = WindowState.Starting;
        }
    }
}



