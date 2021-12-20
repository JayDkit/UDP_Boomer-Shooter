using GDLibrary;
using GDLibrary.Collections;
using GDLibrary.Components.UI;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDApp.App.Scripts.UI
{
    public class UIControls
    {
        private UIMenuManager uiMenuManager;

        public UIControls(UIMenuManager uiMenuManager)
        {
            this.uiMenuManager = uiMenuManager;
        }

        public void InitializeUI(Dictionary<string, Texture2D> textureDictionary, ContentDictionary<SpriteFont> fontDictionary)
        {
            UIObject controlsObject = null;
            var controlsUIScene = new UIScene(AppData.MENU_CONTROLS_NAME);

            var titleFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/Title");
            var title = "Controls";
            Vector2 dimensions = titleFont.MeasureString(title);
            Vector2 titleOrigin = new Vector2(dimensions.X / 2, dimensions.Y / 2);
            var titleTextObj = new UITextObject("Controls", UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, 180), Vector2.One, 0),
                0,
                Color.Red,
                SpriteEffects.None,
                titleOrigin,
                titleFont,
                title
                );
            controlsUIScene.Add(titleTextObj);


            //Text
            var playerFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/PlayerUIFont");

            var text = "WASD - MOVE\nArrow Keys & Mouse - LOOK\nLeft Mouse Click - SHOOT\nSpace Key - JUMP\nEscape - MENU";
            Vector2 tDimensions = playerFont.MeasureString(text);
            Vector2 timerOrigin = new Vector2(tDimensions.X / 2, tDimensions.Y / 2);
            var textObject = new UITextObject("Text", UIObjectType.Text,
                new Transform2D(new Vector2(AppData.GAME_RESOLUTION_WIDTH/2, AppData.GAME_RESOLUTION_HEIGHT / 2), Vector2.One, 0),
                0,
                Color.White,
                SpriteEffects.None,
                timerOrigin,
                playerFont,
                text
                );

            controlsUIScene.Add(textObject);

            var btnTexture = textureDictionary["genericbtn"];
            var origin = new Vector2(btnTexture.Width / 2.0f, btnTexture.Height / 2.0f);
            var exitBtn = new UIButtonObject(AppData.MENU_BACK_BTN_NAME, UIObjectType.Button,
                new Transform2D(new Vector2(AppData.GAME_RESOLUTION_WIDTH/2, AppData.GAME_RESOLUTION_HEIGHT - 200), Vector2.One, 0),
                0.1f,
                Color.Orange,
                origin,
                btnTexture,
                "Go Back",
                fontDictionary["menu"],
                Color.Black);

            //demo button color change
            exitBtn.AddComponent(new UIColorMouseOverBehaviour(Color.Orange, Color.White));

            controlsUIScene.Add(exitBtn);


            uiMenuManager.Add(controlsUIScene);
        }

    }
}
