using GDLibrary.Components;
using GDLibrary.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary.Managers
{
    public class RenderManager : DrawableGameComponent
    {
        #region Fields

        protected GraphicsDevice graphicsDevice;
        private IRenderScene sceneRenderer;

        #endregion Fields

        #region Properties

        protected IRenderScene SceneRenderer
        {
            get => sceneRenderer;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("You must set a valid renderer (e.g. ForwardRenderer) for the render manager!");

                //if we're setting a different renderer then set it!
                if (sceneRenderer != value)
                    sceneRenderer = value;
            }
        }

        #endregion Properties

        #region Constructors

        public RenderManager(Game game, IRenderScene sceneRenderer) : base(game)
        {
            //cache this de-reference to save us some CPU cycles since its called often in Render below
            graphicsDevice = Application.GraphicsDevice;

            //set the render used to render/draw the scene
            SceneRenderer = sceneRenderer;
        }

        #endregion Constructors

        #region Actions - Draw

        public override void Draw(GameTime gameTime)
        {
            sceneRenderer.Render(graphicsDevice, Camera.Main);
        }

        #endregion Actions - Draw
    }
}

/*
 namespace GDLibrary.Managers
{
    public class RenderManager : IDisposable
    {
        #region Fields

        protected GraphicsDevice graphicsDevice;

        //temps
        private Renderer renderer;

        private Material material;
        private Shader shader;

        #endregion Fields

        #region Constructors

        public RenderManager()
        {
            //cache this de-reference to save us some CPU cycles since its called often in Render below
            graphicsDevice = Application.GraphicsDevice;
        }

        #endregion Constructors

        #region Initialization

        public virtual void Initialize()
        {
        }

        #endregion Initialization

        #region Actions - Render

        public virtual void Render(Scene scene)
        {
            //set depth and blend state

            //render game objects
            var length = scene.Renderers.Count;

            for (var i = 0; i < length; i++)
            {
                renderer = scene.Renderers[i];
                material = renderer.Material;

                if (material == null)
                    throw new NullReferenceException("This game object has no material set for its renderer!");

                //access the shader (e.g. BasicEffect) for this rendered game object
                shader = material.Shader;

                //TODO - set Camera.Main in Main or SceneManager
                //Set View and Projection
                shader.PrePass(Camera.Main);

                //Set World matrix
                shader.Pass(renderer);

                renderer.Draw(graphicsDevice);
            }

            //render post processing

            //render ui
        }

        #endregion Actions - Render

        #region Actions - Housekeeping

        //TODO - add dispose code
        public virtual void Dispose()
        {
        }

        #endregion Actions - Housekeeping
    }
}
 */