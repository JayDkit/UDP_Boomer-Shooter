using GDLibrary;
using GDLibrary.Components.UI;
using GDLibrary.Core;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Player
{
    public class PlayerUI 
    {
        private UISceneManager uiSceneManager;
        private int score;
        private sbyte health;
        private float time;

        public PlayerUI(UISceneManager manager)
        {
            uiSceneManager = manager;
        }

        public void InitializeUI()
        {
            //create the scene
            var mainGameUIScene = new UIScene("Player UI");


            var font = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/ui");
            var playerFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/PlayerUIFont");


            //Texture : reticle
            var reticle = Application.Main.Content.Load<Texture2D>("Assets/Textures/UI/reticle");
            Vector2 reticleOrigin = new Vector2(reticle.Width / 2, reticle.Height / 2);
            var reticleTexture = new UITextureObject(
                "Reticle",
                UIObjectType.Texture,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, Application.Main.GraphicsDevice.Viewport.Height / 2), new Vector2(0.5f, 0.5f), 0),
                0,
                Color.Red,
                SpriteEffects.None,
                reticleOrigin,
                reticle,
                reticle,
                new Rectangle(0,0, reticle.Width, reticle.Height)
                );
            //Texture
            var blackTexture = Application.Main.Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/backgroundTexture");
            var backgroundTexture = new UITextureObject(
                "BlackTexture",
                UIObjectType.Texture,
                new Transform2D(new Vector2(0, Application.Main.GraphicsDevice.Viewport.Height - 210),
                new Vector2(10, 2), 0),
                0,
                blackTexture
                );

            //Text : score
            Vector2 dimensions = playerFont.MeasureString(score.ToString("D6"));
            Vector2 scoreOrigin = new Vector2(dimensions.X / 2, dimensions.Y / 2);
            var scoreTextObj = new UITextObject("Score", UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, Application.Main.GraphicsDevice.Viewport.Height - 180), Vector2.One, 0),
                0, 
                Color.White,
                SpriteEffects.None,
                scoreOrigin,
                playerFont,
                score.ToString("D6")
                );
            //Text : ammo
            var strAmmo = "Ammo";
            Vector2 aDimensions = playerFont.MeasureString(strAmmo);
            Vector2 ammoOrigin = new Vector2(aDimensions.X / 2, aDimensions.Y / 2);
            var ammoTextObject = new UITextObject("Ammo", UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width - 450, Application.Main.GraphicsDevice.Viewport.Height - 150), Vector2.One, 0),
                0,
                Color.White,
                SpriteEffects.None,
                ammoOrigin,
                playerFont,
                strAmmo
                );
            //Text : timer
            var strTimer = "Timer";
            Vector2 tDimensions = playerFont.MeasureString(strTimer);
            Vector2 timerOrigin = new Vector2(tDimensions.X / 2, tDimensions.Y / 2);
            var timerextObject = new UITextObject("Ammo", UIObjectType.Text,
                new Transform2D(new Vector2(450, Application.Main.GraphicsDevice.Viewport.Height - 150), Vector2.One, 0),
                0,
                Color.White,
                SpriteEffects.None,
                timerOrigin,
                playerFont,
                strTimer
                );

            //add the ui element to the scene
            mainGameUIScene.Add(backgroundTexture); //First add background texture. ALWAYS!
            mainGameUIScene.Add(reticleTexture);

            mainGameUIScene.Add(scoreTextObj);
            mainGameUIScene.Add(ammoTextObject);
            mainGameUIScene.Add(timerextObject);
            

            #region Add Scene To Manager & Set Active Scene

            //add the ui scene to the manager
            uiSceneManager.Add(mainGameUIScene);

            //set the active scene
            uiSceneManager.SetActiveScene("Player UI");

            #endregion Add Scene To Manager & Set Active Scene
        }
    }
}
