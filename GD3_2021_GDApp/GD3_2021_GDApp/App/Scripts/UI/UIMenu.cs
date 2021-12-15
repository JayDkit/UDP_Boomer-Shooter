using GDLibrary;
using GDLibrary.Collections;
using GDLibrary.Components.UI;
using GDLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDApp.App.Scripts.UI
{
    public class UIMenu
    {

        public void InitializeGameMenu(MyMenuManager uiMenuManager, GraphicsDeviceManager _graphics, Dictionary<string, Texture2D> textureDictionary, ContentDictionary<SpriteFont> fontDictionary)
        {
            //a re-usable variable for each ui object
            UIObject menuObject = null;

            #region Main Menu

            /************************** Main Menu Scene **************************/
            //make the main menu scene
            var mainMenuUIScene = new UIScene(AppData.MENU_MAIN_NAME);

            /**************************** Background Image ****************************/

            //main background
            var texture = textureDictionary["mainmenu"];
            //get how much we need to scale background to fit screen, then downsizes a little so we can see game behind background
            var scale = _graphics.GetScaleForTexture(texture, new Vector2(0.8f, 0.8f));

            menuObject = new UITextureObject("main background",
                UIObjectType.Texture,
                new Transform2D(Screen.Instance.ScreenCentre, scale, 0), //sets position as center of screen
                0,
                new Color(255, 255, 255, 200),
                texture.GetOriginAtCenter(), //if we want to position image on screen center then we need to set origin as texture center
                texture);

            //add ui object to scene
            mainMenuUIScene.Add(menuObject);

            /**************************** Game Title ****************************/
            var titleFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/Title");
            var title = "BOOMER SHOOTER";
            Vector2 dimensions = titleFont.MeasureString(title);
            Vector2 titleOrigin = new Vector2(dimensions.X / 2, dimensions.Y / 2);
            var titleTextObj = new UITextObject("Title", UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, 180), Vector2.One, 0),
                0,
                Color.Red,
                SpriteEffects.None,
                titleOrigin,
                titleFont,
                title
                );
            mainMenuUIScene.Add(titleTextObj);
            /**************************** Play Button ****************************/

            var btnTexture = textureDictionary["genericbtn"];
            var sourceRectangle
                = new Microsoft.Xna.Framework.Rectangle(0, 0,
                btnTexture.Width, btnTexture.Height);
            var origin = new Vector2(btnTexture.Width / 2.0f, btnTexture.Height / 2.0f);

            var playBtn = new UIButtonObject(AppData.MENU_PLAY_BTN_NAME, UIObjectType.Button,
                new Transform2D(AppData.MENU_PLAY_BTN_POSITION,
                0.5f * Vector2.One, 0),
                0.1f,
                Color.White,
                SpriteEffects.None,
                origin,
                btnTexture,
                null,
                sourceRectangle,
                "Play",
                fontDictionary["menu"],
                Color.Black,
                Vector2.Zero);

            //demo button color change
            playBtn.AddComponent(new UIColorMouseOverBehaviour(Color.Orange, Color.White));

            mainMenuUIScene.Add(playBtn);

            /**************************** Controls Button ****************************/

            //same button texture so we can re-use texture, sourceRectangle and origin

            var controlsBtn = new UIButtonObject(AppData.MENU_CONTROLS_BTN_NAME, UIObjectType.Button,
                new Transform2D(AppData.MENU_CONTROLS_BTN_POSITION, 0.5f * Vector2.One, 0),
                0.1f,
                Color.White,
                origin,
                btnTexture,
                "Controls",
                fontDictionary["menu"],
                Color.Black);

            //demo button color change
            controlsBtn.AddComponent(new UIColorMouseOverBehaviour(Color.Orange, Color.White));

            mainMenuUIScene.Add(controlsBtn);

            /**************************** Exit Button ****************************/

            //same button texture so we can re-use texture, sourceRectangle and origin

            //use a simple/smaller version of the UIButtonObject constructor
            var exitBtn = new UIButtonObject(AppData.MENU_EXIT_BTN_NAME, UIObjectType.Button,
                new Transform2D(AppData.MENU_EXIT_BTN_POSITION, 0.5f * Vector2.One, 0),
                0.1f,
                Color.Orange,
                origin,
                btnTexture,
                "Exit",
                fontDictionary["menu"],
                Color.Black);

            //demo button color change
            exitBtn.AddComponent(new UIColorMouseOverBehaviour(Color.Orange, Color.White));

            mainMenuUIScene.Add(exitBtn);

            #endregion Main Menu

            //add scene to the menu manager
            uiMenuManager.Add(mainMenuUIScene);

            /************************** Controls Menu Scene **************************/

            /************************** Options Menu Scene **************************/

            /************************** Exit Menu Scene **************************/

            //finally we say...where do we start
            uiMenuManager.SetActiveScene(AppData.MENU_MAIN_NAME);
        }
    }
}
