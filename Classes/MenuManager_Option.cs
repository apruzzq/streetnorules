using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;


namespace Les_Loubards
{

    static class MenuManager_Option
    {

        public static List<MenuItem> menuItems = new List<MenuItem>();
        static int currentMenuItem = 0;
        public static WindowState windowState = WindowState.Inactive;
        static double changeProgress = 0;
        static float verPosition = 300;
        static float horPosition = 300;
        static float horPosition2 = 315;
        static TimeSpan changeSpan = TimeSpan.FromMilliseconds(800);
        public static bool langage_items;



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
                if (langage_items == false)
                {
                    switch (currentMenuItem)
                    {

                        case 0: //How to play
                            GameManager.GameState = GameState.HowToPlay;
                            MenuManager_ChoixNiveau.windowState = WindowState.Starting;
                            break;

                        case 1: //Langue
                            langage_items = true;
                            menuItems = new List<MenuItem>();
                            CreateMenuItems();
                            GameManager.GameState = GameState.Option;
                            MenuManager_Option.windowState = WindowState.Starting;
                            break;

                        case 2: // Mode plein écran
                            Game1.graphics.IsFullScreen = !Game1.graphics.IsFullScreen;
                            Game1.graphics.ApplyChanges();
                            MenuManager_principal.menuItems = new List<MenuItem>();
                            MenuManager_principal.CreateMenuItems();
                            GameManager.GameState = GameState.MainMenu;
                            MenuManager_principal.windowState = WindowState.Starting;
                            break;

                        case 3: // Retour précédent
                            MenuManager_principal.menuItems = new List<MenuItem>();
                            MenuManager_principal.CreateMenuItems();
                            GameManager.GameState = GameState.MainMenu;
                            MenuManager_principal.windowState = WindowState.Starting;
                            break;
                    }
                }
                else if (langage_items)
                {
                    switch (currentMenuItem)
                    {
                        case 0: //Français
                            langage_items = false;
                            GameManager.Langage = "Francais";
                            menuItems = new List<MenuItem>();
                            CreateMenuItems();
                            GameManager.GameState = GameState.Option;
                            MenuManager_Option.windowState = WindowState.Starting;
                            break;

                        case 1: //Anglais
                            langage_items = false;
                            GameManager.Langage = "Anglais";
                            menuItems = new List<MenuItem>();
                            CreateMenuItems();
                            GameManager.GameState = GameState.Option;
                            MenuManager_Option.windowState = WindowState.Starting;
                            break;

                        case 2: //espagnol
                            langage_items = false;
                            GameManager.Langage = "Espagnol";
                            menuItems = new List<MenuItem>();
                            CreateMenuItems();
                            GameManager.GameState = GameState.Option;
                            MenuManager_Option.windowState = WindowState.Starting;
                            break;

                        case 4: // Retour précédent
                            langage_items = false;
                            menuItems = new List<MenuItem>();
                            CreateMenuItems();
                            GameManager.GameState = GameState.MainMenu;
                            MenuManager_principal.windowState = WindowState.Starting;
                            break;
                    }
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




            //Draw the Background >> Always,  et c'est ici qu'on choisit l'image a montrer
            SBHUD.Draw(Game1.SprTitleChoiceLevel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), Color.White);


            verPosition = 300;
            horPosition = 300;
            horPosition2 = 315;

            switch (windowState)
            {
                case WindowState.Starting://animation ouverture nouveau menu
                    horPosition -= 300 * (1.0f - (float)smoothedProgress);  //fais défiler progressivement le menu de la guache vers le centre
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
            //Give a slightly darkened overplay over the background
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
            if (langage_items && GameManager.Langage == "Francais")
            {
                menuItems.Add(new MenuItem("Francais", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Anglais", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Espagnol", new Vector2(horPosition, verPosition + 60)));
                menuItems.Add(new MenuItem("Retour", new Vector2(horPosition, verPosition + 110)));
            }
            else if (langage_items == false && GameManager.Langage == "Francais")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("Comment jouer", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Langage", new Vector2(horPosition, verPosition + 30)));

                if (Game1.graphics.IsFullScreen)
                    menuItems.Add(new MenuItem("Plein ecran (1)", new Vector2(horPosition, verPosition + 60)));
                else
                    menuItems.Add(new MenuItem("Plein ecran (0)", new Vector2(horPosition, verPosition + 60)));

                menuItems.Add(new MenuItem("Retour", new Vector2(horPosition, verPosition + 110)));
            }
            else if (langage_items && GameManager.Langage == "Anglais")
            {
                menuItems.Add(new MenuItem("French", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("English", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Spanish", new Vector2(horPosition, verPosition + 60)));
                menuItems.Add(new MenuItem("Back", new Vector2(horPosition, verPosition + 110)));
            } 
            else if (langage_items == false && GameManager.Langage == "Anglais")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("How To Play", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Language", new Vector2(horPosition, verPosition + 30)));

                if (Game1.graphics.IsFullScreen)
                    menuItems.Add(new MenuItem("FullScreen (1)", new Vector2(horPosition, verPosition + 60)));
                else
                    menuItems.Add(new MenuItem("FullScreen (0)", new Vector2(horPosition, verPosition + 60)));

                menuItems.Add(new MenuItem("Back", new Vector2(horPosition, verPosition + 110)));
            }
            else if (langage_items && GameManager.Langage == "Espagnol")
            {
                menuItems.Add(new MenuItem("Frances", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Ingles", new Vector2(horPosition2, verPosition + 30)));
                menuItems.Add(new MenuItem("Espanol", new Vector2(horPosition, verPosition + 60)));
                menuItems.Add(new MenuItem("Volver", new Vector2(horPosition, verPosition + 110)));
            }
            else if (langage_items == false && GameManager.Langage == "Espagnol")
            {
                //initialisation des boutons, la suite dans le draw
                menuItems.Add(new MenuItem("Como jugar", new Vector2(horPosition, verPosition)));
                menuItems.Add(new MenuItem("Idioma", new Vector2(horPosition, verPosition + 30)));

                if (Game1.graphics.IsFullScreen)
                    menuItems.Add(new MenuItem("Pantalla completa (1)", new Vector2(horPosition, verPosition + 60)));
                else
                    menuItems.Add(new MenuItem("Pantalla completa (0)", new Vector2(horPosition, verPosition + 60)));

                menuItems.Add(new MenuItem("Volver", new Vector2(horPosition, verPosition + 110)));
            }
        }

        public static void WakeUp()
        {
            windowState = WindowState.Starting;
        }
    }
}




