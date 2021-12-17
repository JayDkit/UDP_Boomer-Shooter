using GDLibrary;
using GDLibrary.Components.UI;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDApp.App.Scripts.UI
{
    class UIGameOver
    {
        private UISceneManager uiSceneManager;
        UIScene gameOverUIScene = new UIScene("GameOver UI");
        SpriteFont font = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/Title");
        SpriteFont playerFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/PlayerUIFont");

        public UIGameOver(UISceneManager manager)
        {
            uiSceneManager = manager;
        }


        public void InitializeUI()
        {
            var strGameOver = "GAME OVER";
            Vector2 dimensions = font.MeasureString(strGameOver);
            Vector2 txtOrigin = new Vector2(dimensions.X / 2, dimensions.Y / 2);
            var gameOverTextObj = new UITextObject(strGameOver, UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, 200), Vector2.One, 0),
                0,
                Color.Red,
                SpriteEffects.None,
                txtOrigin,
                font,
                strGameOver
                );
            gameOverUIScene.Add(gameOverTextObj);
            uiSceneManager.Add(gameOverUIScene);
            uiSceneManager.SetActiveScene("GameOver UI");
          
        }
    }
}
