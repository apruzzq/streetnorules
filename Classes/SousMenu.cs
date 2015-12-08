using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Les_Loubards
{
     class SousMenu
    {
        private struct MenuItem //C'est un élément du menu, composé d'un texte et d'un lien.
        {
            public string itemText;
            public SousMenu itemLink;

            public MenuItem(string itemText, SousMenu itemLink)  //type MenuItem de deux composantes itemText itemLink
            {
                this.itemText = itemText;
                this.itemLink = itemLink;
                
            }
  
        }

        private TimeSpan changeSpan;  //vitesse de défilement du texte quand on clique sur un lien
        private WindowState windowState;  //défini les différentes fenêtres
        private List<MenuItem> itemList;  //liste d'item
        private int selectedItem;

        private double changeProgress;

        private SpriteFont spriteFont;
        private string menuTitle;        
        private Texture2D backgroundImage;

        public SousMenu(SpriteFont spriteFont, string menuTitle, Texture2D backgroundImage)
        {
            itemList = new List<MenuItem>();                        
            changeSpan = TimeSpan.FromMilliseconds(800);  //vitesse de cangement de fenetre
            selectedItem = 0;
            changeProgress = 0;
            windowState = WindowState.Inactive;  //toutes les fenêtres possible du menu sont fermées, sauf celle de démarrage

            this.spriteFont = spriteFont;
            this.menuTitle = menuTitle;          
            this.backgroundImage = backgroundImage;
        }

        public void AddMenuItem(string itemText, SousMenu itemLink)
        {
            MenuItem newItem = new MenuItem(itemText, itemLink);
            itemList.Add(newItem);//on ajoute un item à la liste dynamique
            
            
        }

        public void WakeUp()
        {
            windowState = WindowState.Starting;
        }

        public void Update(double timePassedSinceLastFrame) 
        {
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

        public SousMenu ProcessInput(KeyboardState lastKeybState, KeyboardState currentKeybState)//fonction qui se déplace dans la liste d'item et sélectionne le bon menu
        {
            if (lastKeybState.IsKeyUp(Keys.Down) && currentKeybState.IsKeyDown(Keys.Down))
                selectedItem++;//servira d'indice à la liste d'item. qaund on se déplace avec haut ou bas, on change la valeur de l'indice, donc on selectionne un autre item de la liste.

            

            if (lastKeybState.IsKeyUp(Keys.Up) && currentKeybState.IsKeyDown(Keys.Up))
                selectedItem--;

            

            if (selectedItem < 0)
                selectedItem = 0;

            if (selectedItem >= itemList.Count)//permet de bloquer le défilement d'item au dernier item.
                selectedItem = itemList.Count-1;

            if ((lastKeybState.IsKeyUp(Keys.Enter) && currentKeybState.IsKeyDown(Keys.Enter))) //
            {
                windowState = WindowState.Ending;
                return itemList[selectedItem].itemLink; //si on appui sur entré, la fonction retourne l'item correspondant en mettant windowstate en mode ending.
            }
            else if (lastKeybState.IsKeyDown(Keys.Escape))
                return null;
            else
                return this;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (windowState == WindowState.Inactive)
                return;

            float smoothedProgress = MathHelper.SmoothStep(0,1,(float)changeProgress);

            int verPosition = 300; //hauteur du menu
            float horPosition = 300;//largeur
            float horPosition2 = 300;//largeur2
            float alphaValue;
            float bgLayerDepth;
            Color bgColor;

            switch (windowState)
            {
                case WindowState.Starting://animation ouverture nouveau menu
                    horPosition -= 300 * (1.0f - (float)smoothedProgress);  //fais défiler progressivement le menu de la guache vers le centre
                    horPosition2 += 300 * (1.0f - (float)smoothedProgress);
                    alphaValue = smoothedProgress;//transparrance allant progressivement de 0 (invisible) à 1 (max)
                    bgLayerDepth = 0.5f;
                    bgColor = new Color(new Vector4(1, 1, 1, alphaValue));
                    break;
                case WindowState.Ending://animation fermeture menu
                    horPosition += 200 * (float)smoothedProgress; //centre vers droite
                    horPosition2 -= 200 * (float)smoothedProgress; //centre vers droite
                    alphaValue = 1.0f - smoothedProgress; //1 à 0
                    bgLayerDepth = 1;
                    bgColor = Color.White;
                    break;
                default:
                    alphaValue = 1;
                    bgLayerDepth = 1;
                    bgColor = Color.White;
                    break;
            }
            
            Color titleColor = new Color(new Vector4(1, 1, 1, alphaValue));
            spriteBatch.Draw(backgroundImage, new Vector2(), null, bgColor, 0, Vector2.Zero, 1, SpriteEffects.None, bgLayerDepth);
            spriteBatch.DrawString(spriteFont, menuTitle, new Vector2(horPosition, 20), titleColor,0,Vector2.Zero, 1.5f, SpriteEffects.None, 0); // défilement du titre
            
            for (int itemID = 0; itemID < itemList.Count; itemID++) //défilement des items
            {                    
                Vector2 itemPostition = new Vector2(horPosition, verPosition);
                Vector2 itemPostition2 = new Vector2(horPosition2, verPosition);
                Color itemColor = Color.White;
                
                if (itemID == selectedItem)
                    itemColor = new Color(new Vector4(1,0,0,alphaValue));
                else
                    itemColor = new Color(new Vector4(1,1,1,alphaValue));

                if (itemID % 2 == 0)//un item sur 2
                    spriteBatch.DrawString(spriteFont, itemList[itemID].itemText, itemPostition, itemColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                else
                    spriteBatch.DrawString(spriteFont, itemList[itemID].itemText, itemPostition2, itemColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                verPosition += 30;
            }            
        }
    }
}
