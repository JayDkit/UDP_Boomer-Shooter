using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Components.UI;
using GDLibrary.Core;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.App.Scripts.Player
{
    public class PlayerUI
    {
        private UISceneManager uiSceneManager;
        private int score = 0;
        private byte health = 0;
        private float time = 0;
        UIScene mainGameUIScene = new UIScene("Player UI");
        SpriteFont font = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/ui");
        SpriteFont playerFont = Application.Main.Content.Load<SpriteFont>("Assets/Fonts/PlayerUIFont");
        public PlayerUI(UISceneManager manager)
        {
            uiSceneManager = manager;

        }

        public void updateScore(int newScore) { score = newScore; }
        public void updateHealth(byte newHealth) { health = newHealth; }
        UITextObject previous = null;
        public void updateTime(float newTime) 
        {
            
            time = newTime;
            Vector2 tiDimensions = playerFont.MeasureString(time.ToString());
            Vector2 timeOrigin = new Vector2(tiDimensions.X / 2, tiDimensions.Y / 2);
            var timeextObject = new UITextObject("Time", UIObjectType.Text,
                new Transform2D(new Vector2(450, Application.Main.GraphicsDevice.Viewport.Height - 100), Vector2.One, 0),
                0,
                Color.White,
                SpriteEffects.None,
                timeOrigin,
                playerFont,
                time.ToString()
                );
            mainGameUIScene.Remove(previous);
            mainGameUIScene.Add(timeextObject);
            previous = timeextObject;

        }

        public void InitializeUI(Player player)
        {
            updateScore(player.Score);
            updateHealth(player.Health);
            //updateTime();
           
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

            //Text: health
            Vector2 hDimensions = playerFont.MeasureString(health.ToString());
            Vector2 healthOrigin = new Vector2(hDimensions.X / 2, hDimensions.Y / 2);
            var healthTextObj = new UITextObject("Score", UIObjectType.Text,
                new Transform2D(new Vector2(Application.Main.GraphicsDevice.Viewport.Width / 2, Application.Main.GraphicsDevice.Viewport.Height - 100), Vector2.One, 0),
                0,
                Color.Red,
                SpriteEffects.None,
                healthOrigin,
                playerFont,
                health.ToString()
                ); ;

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
            var timerextObject = new UITextObject("Timer", UIObjectType.Text,
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
            mainGameUIScene.Add(healthTextObj);
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
