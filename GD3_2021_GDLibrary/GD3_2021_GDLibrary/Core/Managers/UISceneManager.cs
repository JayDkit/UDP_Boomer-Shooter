using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary.Components.UI
{
    /// <summary>
    /// Stores a dictionary of ui scenes and updates and draws the currently active scene
    /// </summary>
    public class UISceneManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Dictionary<string, UIScene> scenes;
        private UIScene activeScene;
        private string activeSceneName;

        /// <summary>
        /// Instanciate a ui scene manager to store all ui scenes (e.g. in-game ui)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        public UISceneManager(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            scenes = new Dictionary<string, UIScene>();
            activeScene = null;
            activeSceneName = "";
        }

        public bool SetActiveScene(string sceneName)
        {
            UIScene scene = Find(sceneName);
            if (scene != null)
            {
                activeScene = scene;
                activeSceneName = sceneName;
                return true;
            }
            return false;
        }

        public void Add(UIScene scene)
        {
            if (!scenes.ContainsKey(scene.Name))
                scenes.Add(scene.Name, scene);
        }

        public bool Remove(string key)
        {
            return scenes.Remove(key);
        }

        public UIScene Find(string key)
        {
            UIScene uiScene;
            scenes.TryGetValue(key, out uiScene);
            return uiScene;
        }

        public void Clear()
        {
            if (scenes.Count != 0)
                scenes.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            activeScene?.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend, null, null);
            activeScene?.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}