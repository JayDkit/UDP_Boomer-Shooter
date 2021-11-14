using GDLibrary.Utilities;
using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Editor;
using GDLibrary.Graphics;
using GDLibrary.Inputs;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDApp
{
    public class Main : Game
    {
#if DEBUG
        private GameObject cObject = null;
#endif

        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Stores all scenes (which means all game objects i.e. players, cameras, pickups, behaviours, controllers)
        /// </summary>
        private SceneManager sceneManager;

        /// <summary>
        /// Renders all game objects with an attached and enabled renderer
        /// </summary>
        private RenderManager renderManager;

        /// <summary>
        /// Quick lookup for all textures used within the game
        /// </summary>
        private Dictionary<string, Texture2D> textureDictionary;

        private Scene activeScene;

        #endregion Fields

        #region Constructors

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #endregion Constructors

        #region Initialization - Scene manager, Application data, Screen, Input, Scenes, Game Objects

        protected override void Initialize()
        {
            //data, input, scene manager
            InitializeEngine("My Game Title Goes Here", 1024, 768);

            InitializeDictionaries();

            LoadAssets();

            //level with scenes and game objects
            InitializeLevel();

            //centre the mouse with hardcoded value - remove later
            Input.Mouse.Position = new Vector2(512, 384);

#if DEBUG
            RunDemos();
            InitializeEditorHelpers();
#endif

            base.Initialize();
        }

#if DEBUG

        private void InitializeEditorHelpers()
        {
            //a game object to record camera positions to an XML file for use in a curve later
            var curveRecorder = new GameObject("curve recorder", GameObjectType.Editor);
            curveRecorder.AddComponent(new CurveRecorderController());
            activeScene.Add(curveRecorder);
        }

        private void RunDemos()
        {
            #region Curve Demo

            //var curve1D = new GDLibrary.Parameters.Curve1D(CurveLoopType.Cycle);
            //curve1D.Add(0, 0);
            //curve1D.Add(10, 1000);
            //curve1D.Add(20, 2000);
            //curve1D.Add(40, 4000);
            //curve1D.Add(60, 6000);
            //var value = curve1D.Evaluate(500, 2);

            #endregion Curve Demo

            #region Serialization Single Object Demo

            var demoSaveLoad = new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f));
            SerializationUtility.Save("DemoSingle.xml", demoSaveLoad);
            var readSingle = SerializationUtility.Load("DemoSingle.xml",
                typeof(DemoSaveLoad)) as DemoSaveLoad;

            #endregion Serialization Single Object Demo

            #region Serialization List Objects Demo

            List<DemoSaveLoad> listDemos = new List<DemoSaveLoad>();
            listDemos.Add(new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(10, 20, 30), new Vector3(4, 9, -18), new Vector3(15f, 1f, 202.5f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(100, 200, 300), new Vector3(145, 290, -80), new Vector3(6.5f, 1.1f, 8.05f)));

            SerializationUtility.Save("ListDemo.xml", listDemos);
            var readList = SerializationUtility.Load("ListDemo.xml",
                typeof(List<DemoSaveLoad>)) as List<DemoSaveLoad>;

            #endregion Serialization List Objects Demo
        }

#endif

        private void InitializeDictionaries()
        {
            textureDictionary = new Dictionary<string, Texture2D>();
        }

        private void LoadAssets()
        {
            LoadTextures();
        }

        private void LoadTextures()
        {
            //debug
            textureDictionary.Add("checkerboard", Content.Load<Texture2D>("Assets/Demo/Textures/checkerboard"));

            //skybox
            textureDictionary.Add("skybox_front", Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
            textureDictionary.Add("skybox_left", Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
            textureDictionary.Add("skybox_right", Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
            textureDictionary.Add("skybox_back", Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
            textureDictionary.Add("skybox_sky", Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));
        }

        private void InitializeLevel()
        {
            activeScene = new Scene("level 1");

            InitializeSkybox(activeScene, 500);
            InitializeCameras(activeScene);
            InitializeCubes(activeScene);
            InitializeModels(activeScene);

            sceneManager.Add(activeScene);
            sceneManager.LoadScene("level 1");
        }

        private void InitializeSkybox(Scene level, float worldScale = 500)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["checkerboard"];
            material.Shader = new BasicShader();

            var archetypalQuad = new GameObject("quad", GameObjectType.Skybox);
            var renderer = new MeshRenderer();
            renderer.Material = material;
            archetypalQuad.AddComponent(renderer);
            renderer.Mesh = new QuadMesh();

            #endregion Archetype

            //back
            GameObject back = archetypalQuad.Clone() as GameObject;
            back.Name = "skybox_back";
            material.Texture = textureDictionary["skybox_back"];
            back.Transform.Translate(0, 0, -worldScale / 2.0f);
            back.Transform.Scale(worldScale, worldScale, null);
            level.Add(back);

            //left
            GameObject left = archetypalQuad.Clone() as GameObject;
            left.Name = "skybox_left";
            material.Texture = textureDictionary["skybox_left"];
            left.Transform.Translate(-worldScale / 2.0f, 0, 0);
            left.Transform.Scale(worldScale, worldScale, null);
            left.Transform.Rotate(0, 90, 0);
            level.Add(left);

            //right
            GameObject right = archetypalQuad.Clone() as GameObject;
            right.Name = "skybox_right";
            material.Texture = textureDictionary["skybox_right"];
            right.Transform.Translate(worldScale / 2.0f, 0, 0);
            right.Transform.Scale(worldScale, worldScale, null);
            right.Transform.Rotate(0, -90, 0);
            level.Add(right);

            //front
            GameObject front = archetypalQuad.Clone() as GameObject;
            front.Name = "skybox_front";
            material.Texture = textureDictionary["skybox_front"];
            front.Transform.Translate(0, 0, worldScale / 2.0f);
            front.Transform.Scale(worldScale, worldScale, null);
            front.Transform.Rotate(0, -180, 0);
            level.Add(front);

            //top
            GameObject top = archetypalQuad.Clone() as GameObject;
            top.Name = "skybox_sky";
            material.Texture = textureDictionary["skybox_sky"];
            top.Transform.Translate(0, worldScale / 2.0f, 0);
            top.Transform.Scale(worldScale, worldScale, null);
            top.Transform.Rotate(90, 0, 0);
            level.Add(top);
        }

        private void InitializeCameras(Scene level)
        {
            #region First Person Camera

            //add camera game object
            var camera = new GameObject("main camera", GameObjectType.Camera);
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            camera.AddComponent(new FirstPersonController(0.05f, 0.025f, 0.00009f));
            camera.Transform.SetTranslation(0, 0, 15);
            level.Add(camera);

            #endregion First Person Camera

            #region Curve Camera

            //add curve for camera translation
            var translationCurve = new Curve3D(CurveLoopType.Cycle);
            translationCurve.Add(new Vector3(0, 0, 10), 0);
            translationCurve.Add(new Vector3(0, 5, 15), 1000);
            translationCurve.Add(new Vector3(0, 0, 20), 2000);
            translationCurve.Add(new Vector3(0, -5, 25), 3000);
            translationCurve.Add(new Vector3(0, 0, 30), 4000);
            translationCurve.Add(new Vector3(0, 0, 10), 6000);

            //add camera game object
            var curveCamera = new GameObject("curve camera", GameObjectType.Camera);
            curveCamera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            curveCamera.AddComponent(new CurveBehaviour(translationCurve));
            curveCamera.AddComponent(new FOVOnScrollController(MathHelper.ToRadians(2)));
            level.Add(curveCamera);

            #endregion Curve Camera

            //set theMain camera, if we dont call this then the first camera added will be the Main
            level.SetMainCamera("main camera");

            //allows us to scale time on all game objects that based movement on Time
            // Time.Instance.TimeScale = 0.1f;
        }

        private void InitializeModels(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("model material");
            material.Texture = Content.Load<Texture2D>("Assets/Demo/Textures/checkerboard");
            material.Shader = new BasicShader();

            var archetypalSphere = new GameObject("sphere", GameObjectType.Consumable);
            var renderer = new ModelRenderer();
            renderer.Material = material;
            archetypalSphere.AddComponent(renderer);
            renderer.Model = Content.Load<Model>("Assets/Models/sphere");

            //downsize the model a little because the sphere is quite large
            archetypalSphere.Transform.SetScale(0.125f, 0.125f, 0.125f);

            #endregion Archetype

            var count = 0;
            for (var i = -8; i <= 8; i += 2)
            {
                var clone = archetypalSphere.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                clone.Transform.SetTranslation(-5, i, 0);
                level.Add(clone);
            }
        }

        private void InitializeCubes(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = Content.Load<Texture2D>("Assets/Demo/Textures/mona lisa");
            material.Shader = new BasicShader();

            var archetypalCube = new GameObject("cube", GameObjectType.Architecture);
            var renderer = new MeshRenderer();
            renderer.Material = material;
            archetypalCube.AddComponent(renderer);
            renderer.Mesh = new CubeMesh();

            #endregion Archetype

            var count = 0;
            for (var i = 1; i <= 8; i += 2)
            {
                var clone = archetypalCube.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                clone.Transform.SetTranslation(i, 0, 0);
                clone.Transform.SetScale(1, i, 1);
                level.Add(clone);
            }
        }

        /// <summary>
        /// Set application data, input, title and scene manager
        /// </summary>
        private void InitializeEngine(string gameTitle, int width, int height)
        {
            //set game title
            Window.Title = gameTitle;

            //instanciate scene manager to store all scenes
            sceneManager = new SceneManager();

            //initialize global application data
            Application.Main = this;
            Application.Content = Content;
            Application.GraphicsDevice = _graphics.GraphicsDevice; //TODO - is this necessary?
            Application.GraphicsDeviceManager = _graphics;
            Application.SceneManager = sceneManager;

            //instanciate render manager to render all drawn game objects
            renderManager = new RenderManager();

            //instanciate screen (singleton) and set resolution etc
            Screen.GetInstance().Set(width, height, true, false);

            //instanciate input components and store reference in Input for global access
            Input.Keys = new KeyboardComponent(this);
            Input.Mouse = new MouseComponent(this);
            Input.Gamepad = new GamepadComponent(this);

            //add all input components to component list so that they will be updated
            //Q. what would happen is we commented out these lines?
            Components.Add(Input.Keys);
            Components.Add(Input.Mouse);
            Components.Add(Input.Gamepad);
            Components.Add(Time.GetInstance(this));
        }

        #endregion Initialization - Scene manager, Application data, Screen, Input, Scenes, Game Objects

        #region Load & Unload Assets

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion Load & Unload Assets

        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
            //allow the system to update first
            base.Update(gameTime);

            //update every updateable game object
            //Q. what would happen is we commented out this line?
            sceneManager.Update();

#if DEBUG
            DemoFind();
#endif
        }

#if DEBUG

        private void DemoFind()
        {
            //lets look for an object - note - we can ONLY look for object AFTER SceneManager::Update has been called
            if (cObject == null)
                cObject = sceneManager.Find(gameObject => gameObject.Name.Equals("Clone - cube - 2"));

            //the ? is short for (if cObject != null) then...

            cObject?.Transform.Rotate(0,
                Time.Instance.UnscaledDeltaTimeMs * 3 / 60.0f, 0);
        }

#endif

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //render every renderable game object
            renderManager.Render(sceneManager.ActiveScene);

            base.Draw(gameTime);
        }

        #endregion Update & Draw
    }
}