using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Les_Loubards
{
    static class HUDManager
    {
        public static float Opacity;
        private static float compteursefaitfrapper = 0;
        private static float compteursefaitfrapper2 = 0;
        private static int compteur = 0;
        private static int compteur2 = 0;
        private static GameTime gt = new GameTime();

        static Level currentLevel;

        //Indication de taille de la barre
        static int HealthBarWidth = 200;
        static int HealthBarHeight = 20;

        //Méthode de la classe
        public static void SetLevel(Level level)
        {
            currentLevel = level;
        }

        public static void DrawHUD(SpriteBatch SBHUD)
        {
            //Dessin de la partie en noir sur la l'écran
            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, 100), Color.Black);

            #region VisageDoom
            #region Quentin
            compteur += 2;
            if (currentLevel.Player1.Health >= 75)
            {
                if ((currentLevel.Player1.IsAttacked == true) || (compteursefaitfrapper >= 1))
                {
                    if (currentLevel.Player1.IsAttacked == true)
                        compteursefaitfrapper += gt.ElapsedGameTime.Seconds;
                    if ((compteursefaitfrapper <= 20) && (currentLevel.Player1.HitLeft == false))
                        SBHUD.Draw(Game1.A4, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteursefaitfrapper <= 30)
                        SBHUD.Draw(Game1.A5, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteur <= 300)
                        compteur = 0;
                    compteursefaitfrapper += gt.ElapsedGameTime.Seconds; ;
                    if (compteursefaitfrapper > 20)
                    {
                        compteursefaitfrapper = 0;
                        currentLevel.Player1.HitLeft = !currentLevel.Player1.HitLeft;
                    }
                    currentLevel.Player1.IsAttacked = false;
                }

                else if (compteur <= 100)
                    SBHUD.Draw(Game1.A1, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 300)
                    SBHUD.Draw(Game1.A2, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 500)
                    SBHUD.Draw(Game1.A1, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 700)
                    SBHUD.Draw(Game1.A3, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 900)
                    compteur = 0;
            }
            else if (currentLevel.Player1.Health >= 50)
            {
                if ((currentLevel.Player1.IsAttacked == true) || (compteursefaitfrapper >= 1))
                {
                    if (currentLevel.Player1.IsAttacked == true)
                        compteursefaitfrapper += gt.ElapsedGameTime.Seconds;
                    if ((compteursefaitfrapper <= 20) && (currentLevel.Player1.HitLeft == false))
                        SBHUD.Draw(Game1.A4, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteursefaitfrapper <= 30)
                        SBHUD.Draw(Game1.A5, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteur <= 300)
                        compteur = 0;
                    compteursefaitfrapper += gt.ElapsedGameTime.Seconds; ;
                    if (compteursefaitfrapper > 20)
                    {
                        compteursefaitfrapper = 0;
                        currentLevel.Player1.HitLeft = !currentLevel.Player1.HitLeft;
                    }
                    currentLevel.Player1.IsAttacked = false;
                }

                else if (compteur <= 100)
                    SBHUD.Draw(Game1.B1, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 300)
                    SBHUD.Draw(Game1.B2, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 500)
                    compteur = 0;
                else
                    compteur = 0;
            }
            else if (currentLevel.Player1.Health > 0)
            {
                if ((currentLevel.Player1.IsAttacked == true) || (compteursefaitfrapper >= 1))
                {
                    if (currentLevel.Player1.IsAttacked == true)
                        compteursefaitfrapper += gt.ElapsedGameTime.Seconds;
                    if ((compteursefaitfrapper <= 20) && (currentLevel.Player1.HitLeft == false))
                        SBHUD.Draw(Game1.C4, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteursefaitfrapper <= 30)
                        SBHUD.Draw(Game1.C5, new Rectangle(280, 0, 85, 85), Color.White);
                    else if (compteur <= 300)
                        compteur = 0;
                    compteursefaitfrapper += gt.ElapsedGameTime.Seconds; ;
                    if (compteursefaitfrapper > 20)
                    {
                        compteursefaitfrapper = 0;
                        currentLevel.Player1.HitLeft = !currentLevel.Player1.HitLeft;
                    }
                    currentLevel.Player1.IsAttacked = false;
                }

                else if (compteur <= 100)
                    SBHUD.Draw(Game1.C1, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 300)
                    SBHUD.Draw(Game1.C2, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 500)
                    SBHUD.Draw(Game1.C1, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 700)
                    SBHUD.Draw(Game1.C3, new Rectangle(280, 0, 85, 85), Color.White);
                else if (compteur <= 900)
                    compteur = 0;
            }
            else //if (currentLevel.Player1.Health == 0)
                SBHUD.Draw(Game1.Death, new Rectangle(280, 0, 85, 85), Color.White);
            #endregion

            #region Mehdi ( mode 2 joueur )
            if (GameManager.NumberPlayers == 2)
            {
                compteur2 += 2;
                if (currentLevel.Player2.Health >= 75)
                {
                    if ((currentLevel.Player2.IsAttacked == true) || (compteursefaitfrapper2 >= 1))
                    {
                        if (currentLevel.Player2.IsAttacked == true)
                            compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds;
                        if ((compteursefaitfrapper2 <= 20) && (currentLevel.Player2.HitLeft == false))
                            SBHUD.Draw(Game1.D4, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteursefaitfrapper2 <= 30)
                            SBHUD.Draw(Game1.D5, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteur2 <= 300)
                            compteur2 = 0;
                        compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds; ;
                        if (compteursefaitfrapper2 > 20)
                        {
                            compteursefaitfrapper2 = 0;
                            currentLevel.Player2.HitLeft = !currentLevel.Player2.HitLeft;
                        }
                        currentLevel.Player2.IsAttacked = false;
                    }

                    else if (compteur2 <= 100)
                        SBHUD.Draw(Game1.D1, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 300)
                        SBHUD.Draw(Game1.D2, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 500)
                        SBHUD.Draw(Game1.D1, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 700)
                        SBHUD.Draw(Game1.D3, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 900)
                        compteur2 = 0;
                }
                else if (currentLevel.Player2.Health >= 50)
                {
                    if ((currentLevel.Player2.IsAttacked == true) || (compteursefaitfrapper2 >= 1))
                    {
                        if (currentLevel.Player2.IsAttacked == true)
                            compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds;
                        if ((compteursefaitfrapper2 <= 20) && (currentLevel.Player2.HitLeft == false))
                            SBHUD.Draw(Game1.D4, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteursefaitfrapper2 <= 30)
                            SBHUD.Draw(Game1.D5, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteur2 <= 300)
                            compteur2 = 0;
                        compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds; ;
                        if (compteursefaitfrapper2 > 20)
                        {
                            compteursefaitfrapper2 = 0;
                            currentLevel.Player2.HitLeft = !currentLevel.Player2.HitLeft;
                        }
                        currentLevel.Player2.IsAttacked = false;
                    }

                    else if (compteur2 <= 100)
                        SBHUD.Draw(Game1.E1, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 300)
                        SBHUD.Draw(Game1.E2, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 500)
                        compteur2 = 0;
                    else
                        compteur2 = 0;
                }
                else if (currentLevel.Player2.Health > 0)
                {
                    if ((currentLevel.Player2.IsAttacked == true) || (compteursefaitfrapper2 >= 1))
                    {
                        if (currentLevel.Player2.IsAttacked == true)
                            compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds;
                        if ((compteursefaitfrapper2 <= 20) && (currentLevel.Player2.HitLeft == false))
                            SBHUD.Draw(Game1.D4, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteursefaitfrapper2 <= 30)
                            SBHUD.Draw(Game1.D5, new Rectangle(420, 0, 85, 85), Color.White);
                        else if (compteur2 <= 300)
                            compteur2 = 0;
                        compteursefaitfrapper2 += gt.ElapsedGameTime.Seconds; ;
                        if (compteursefaitfrapper2 > 20)
                        {
                            compteursefaitfrapper2 = 0;
                            currentLevel.Player2.HitLeft = !currentLevel.Player2.HitLeft;
                        }
                        currentLevel.Player2.IsAttacked = false;
                    }

                    else if (compteur2 <= 100)
                        SBHUD.Draw(Game1.F1, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 300)
                        SBHUD.Draw(Game1.F2, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 500)
                        SBHUD.Draw(Game1.F1, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur2 <= 700)
                        SBHUD.Draw(Game1.F2, new Rectangle(420, 0, 85, 85), Color.White);
                    else if (compteur <= 900)
                        compteur2 = 0;
                }
                else //if (currentLevel.Player1.Health == 0)
                    SBHUD.Draw(Game1.DeathMehdi, new Rectangle(420, 0, 85, 85), Color.White);
            }
            #endregion
            #endregion

            #region 1 joueur
            if (GameManager.NumberPlayers == 1)
            {
                //Dessin du contour de la barre en rouge, les +2 sont pour créer le contour en question
                SBHUD.Draw(Game1.SprSinglePixel, new Vector2(60, 15),
                    new Rectangle(0, 0, HealthBarWidth + 2, HealthBarHeight + 2), Color.Red);

                //Dessin de la barre jaune
                // percent est le nombre de pts de vie restant sur le total
                float percent = currentLevel.Player1.Health / Player.STARTING_HEALTH;
                //Dessin dépendant de percent
                int drawWidht = (int)(percent * HealthBarWidth);

                //Dessin de la barre jaune est seulement d'une pixel en moins par rapport au contour rouge
                SBHUD.Draw(Game1.SprSinglePixel, new Vector2(61, 16), new Rectangle(0, 0, drawWidht, HealthBarHeight), Color.Yellow);

                //text indicant Player1
                SBHUD.DrawString(Game1.FontSmall, "Skartt", new Vector2(60, -2), Color.White,
                    0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

                //Si le joueur porte un couteau, l'indiquer dans la barre de vie
                if (currentLevel.Player1.CarryingKnife)
                    SBHUD.Draw(Game1.SprThrowingKnife, new Vector2(100, 40), Color.White);

                //Dessin du nombre de vies restantes
                SBHUD.DrawString(Game1.FontSmall, currentLevel.Player1.NombreDeVie.ToString(), new Vector2(270, 25), Color.White);
            }

            #endregion

            #region Mode 2 joueurs
            if (GameManager.NumberPlayers == 2)
            {
                //1er joueur
                if (currentLevel.Player1.Health > 0)
                {
                    //Dessin du contour de la barre en rouge, les +2 sont pour créer le contour en question
                    SBHUD.Draw(Game1.SprSinglePixel, new Vector2(60, 15),
                        new Rectangle(0, 0, HealthBarWidth + 2, HealthBarHeight + 2), Color.Red);

                    //Dessin de la barre jaune
                    // percent est le nombre de pts de vie restant sur le total
                    float percent = currentLevel.Player1.Health / Player.STARTING_HEALTH;
                    //Dessin dépendant de percent
                    int drawWidht = (int)(percent * HealthBarWidth);

                    //Dessin de la barre jaune est seulement d'une pixel en moins par rapport au contour rouge
                    SBHUD.Draw(Game1.SprSinglePixel, new Vector2(61, 16), new Rectangle(0, 0, drawWidht, HealthBarHeight), Color.Yellow);

                    //text indicant Player1
                    SBHUD.DrawString(Game1.FontSmall, "Skartt", new Vector2(60, -2), Color.White,
                        0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

                    //Si le joueur porte un couteau, l'indiquer dans la barre de vie
                    if (currentLevel.Player1.CarryingKnife)
                        SBHUD.Draw(Game1.SprThrowingKnife, new Vector2(100, 40), Color.White);

                    //Dessin du nombre de vies restantes
                    SBHUD.DrawString(Game1.FontSmall, currentLevel.Player1.NombreDeVie.ToString(), new Vector2(270, 25), Color.White);
                }

                //2eme joueur
                if (currentLevel.Player2.Health > 0)
                {
                    //Dessin du contour de la barre en rouge, les +2 sont pour créer le contour en question
                    SBHUD.Draw(Game1.SprSinglePixel, new Vector2(Game1.SCREEN_WIDHT - HealthBarWidth + 2 - 60, 15),
                        new Rectangle(0, 0, HealthBarWidth + 2, HealthBarHeight + 2), Color.Red);

                    //Dessin de la barre jaune
                    // percent est le nombre de pts de vie restant sur le total
                    float percent2 = currentLevel.Player2.Health / Player.STARTING_HEALTH;
                    //Dessin dépendant de percent
                    int drawWidht2 = (int)(percent2 * HealthBarWidth);

                    //Dessin de la barre jaune est seulement d'une pixel en moins par rapport au contour rouge
                    SBHUD.Draw(Game1.SprSinglePixel, new Vector2(Game1.SCREEN_WIDHT - HealthBarWidth - 61, 16), new Rectangle(0, 0, drawWidht2, HealthBarHeight), Color.Yellow);

                    //text indicant Player2
                    SBHUD.DrawString(Game1.FontSmall, "Bob", new Vector2(Game1.SCREEN_WIDHT - HealthBarWidth - 60, -2), Color.White,
                        0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

                    //Si le joueur porte un couteau, l'indiquer dans la barre de vie
                    if (currentLevel.Player2.CarryingKnife)
                        SBHUD.Draw(Game1.SprThrowingKnife, new Vector2(Game1.SCREEN_WIDHT - 200, 30), Color.White);

                    //Dessin du nombre de vies restantes
                    SBHUD.DrawString(Game1.FontSmall, currentLevel.Player2.NombreDeVie.ToString(), new Vector2(Game1.SCREEN_WIDHT - 30, 25), Color.White);
                }
            }
            #endregion
        }

        #region DrawContinueScreen

        public static void DrawContinueScreen(SpriteBatch SBHUD, float timeRemaining)
        {
            if (Opacity <= 0.4f)
                Opacity += 0.005f;

            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT),
                new Color(Color.Red, Opacity));

            SBHUD.DrawString(Game1.FontLarge, "Continue", new Vector2(170, 200), Color.Gold);

            //nombre actuel
            int time = (int)timeRemaining;
            SBHUD.DrawString(Game1.FontLarge, time.ToString(), new Vector2(360, 300), Color.Gold);
        }

        public static void DrawContinueScreen_p1(SpriteBatch SBHUD, float timeRemaining)
        {
            //nombre actuel
            int time = (int)timeRemaining;
            SBHUD.DrawString(Game1.FontSmall, time.ToString(), new Vector2(130, 25), Color.White);
        }

        public static void DrawContinueScreen_p2(SpriteBatch SBHUD, float timeRemaining)
        {
            //nombre actuel
            int time = (int)timeRemaining;
            SBHUD.DrawString(Game1.FontSmall, time.ToString(), new Vector2(Game1.SCREEN_WIDHT - HealthBarWidth, 25), Color.White);
        }

        #endregion 

        public static void DrawGameOverScreen(SpriteBatch SBHUD)
        {
            if (Opacity <= 0.85f)
                Opacity += 0.005f;

            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), new Color(Color.Red, Opacity));

            SBHUD.DrawString(Game1.FontLarge, "GAME OVER", new Vector2(150, 80), Color.White);

            if (GameManager.NumberPlayers == 1)
                SBHUD.DrawString(Game1.FontLarge, "T'es mort.", new Vector2(275, 250), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            else
                SBHUD.DrawString(Game1.FontLarge, "Vous etes morts.", new Vector2(275, 250), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

            SBHUD.DrawString(Game1.FontLarge, "Appuie sur espace pour revenir au ", new Vector2(30, 400), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            SBHUD.DrawString(Game1.FontLarge, "menu principal", new Vector2(230, 480), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        }

        public static void DrawScreenFade(SpriteBatch SBHUD)
        {
            //Draw the level's FADE if level is fading
            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), new Color(Color.Black, Opacity));
        }


        public static void DrawGameCompleted(SpriteBatch SBHUD)
        {
            if (Opacity <= 1f)
                Opacity += 0.005f;

            SBHUD.Draw(Game1.SprSinglePixel, new Rectangle(0, 0, Game1.SCREEN_WIDHT, Game1.SCREEN_HEIGHT), new Color(Color.Black, Opacity));
            SBHUD.DrawString(Game1.FontLarge, "FIN", new Vector2(350, 150), new Color(Color.White, Opacity), 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            SBHUD.DrawString(Game1.FontLarge, "Tu as rejoint la soiree!", new Vector2(350, 250), new Color(Color.White, Opacity), 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            SBHUD.DrawString(Game1.FontLarge, "Appuie sur espace pour revenir au ", new Vector2(20, 400), new Color(Color.White, Opacity), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            SBHUD.DrawString(Game1.FontLarge, "menu principal", new Vector2(230, 480), new Color(Color.White, Opacity), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

        }
    }
}
