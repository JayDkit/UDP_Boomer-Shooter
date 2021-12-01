//#define DEMO

using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Components.UI;
using GDLibrary.Core;
using GDLibrary.Graphics;
using GDLibrary.Inputs;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using GDLibrary.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GDApp.Content.Scripts.Player;
using GDApp.Content.Scripts.Turrets;
using GDApp.Content.Scripts.Turrets.Bullets;
using GDApp.Scripts.Debug;
using GDApp.Content.Scripts.Level;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GDLibrary.Collections;

namespace GDApp
{
    public class Main : Game
    {
        #region Fields

        PlayerUI playerUI;
        private SpriteFont font;
        FramerateCounter fps = new FramerateCounter();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Stores and updates all scenes (which means all game objects i.e. players, cameras, pickups, behaviours, controllers)
        /// </summary>
        private SceneManager sceneManager;

        /// <summary>
        /// Draws all game objects with an attached and enabled renderer
        /// </summary>
        private RenderManager renderManager;

        /// <summary>
        /// Updates and Draws all ui objects
        /// </summary>
        private UISceneManager uiSceneManager;

        private SoundManager soundManager;
        private EventDispatcher eventDispatcher;
        /// <summary>
        /// Renders all ui objects
        /// </summary>
        //private PhysicsManager physicsManager;

        /// <summary>
        /// Quick lookup for all textures used within the game
        /// </summary>
        private Dictionary<string, Texture2D> textureDictionary;

        /// <summary>
        /// Quick lookup for all fonts used within the game
        /// </summary>
        private ContentDictionary<SpriteFont> fontDictionary;

        //temp
        private Scene activeScene;
        private GameObject camera;
        private PlayerGun gun;
        private StandardTurret turret;
        private StandardBullet bullet;

        private GameObject archetypalCube;
        private UITextObject nameTextObj;
        #endregion Fields

        #region Constructors

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #endregion Constructors

        public delegate void MyDelegate(string s, bool b);

        public List<MyDelegate> delList = new List<MyDelegate>();

        public void DoSomething(string msg, bool enableIt)
        {
        }
        /// <summary>
        /// Initialize engine, dictionaries, assets, level contents
        /// </summary>
        protected override void Initialize()
        {
            //     function < void(string, bool) > fPtr = DoSomething;

            var myDel = new MyDelegate(DoSomething);
            myDel("sdfsdfdf", true);
            delList.Add(DoSomething);        
                
            //move here so that UISceneManager can use!
            _spriteBatch = new SpriteBatch(GraphicsDevice); //19.11.21

            //data, input, scene manager
            InitializeEngine("Boomer Shooter", 1920, 1080);

            //load structures that store assets (e.g. textures, sounds) or archetypes (e.g. Quad game object)
            InitializeDictionaries();

            //load assets into the relevant dictionary
            LoadAssets();

            //level with scenes and game objects
            InitializeLevel();

            //add menu and ui
            
            InitializeUI();

            //TODO - remove hardcoded mouse values - update Screen class to centre the mouse with hardcoded value - remove later
            Input.Mouse.Position = Screen.Instance.ScreenCentre;

            //turn on/off debug info
            InitializeDebugUI(true);

            base.Initialize();
        }

        #region Initialization - Dictionaries & Assets

        /// <summary>
        /// Stores all re-used assets and archetypal game objects
        /// </summary>
        private void InitializeDictionaries()
        {
            textureDictionary = new Dictionary<string, Texture2D>();
            fontDictionary = new ContentDictionary<SpriteFont>();
        }

        /// <summary>
        /// Load resources from file
        /// </summary>
        private void LoadAssets()
        {
            LoadTextures();
            LoadFonts();
            LoadSounds();
        }

        /// <summary>
        /// Load fonts to dictionary
        /// </summary>
        private void LoadFonts()
        {
            fontDictionary.Add("Assets/Fonts/ui");
            fontDictionary.Add("Assets/Fonts/menu");
            fontDictionary.Add("Assets/Fonts/debug");
        }

        /// <summary>
        /// Load sound data used by sound manager
        /// </summary>
        private void LoadSounds()
        {
            //for example...
            //soundManager.Add(new GDLibrary.Managers.Cue("smokealarm",
            //    Content.Load<SoundEffect>("Assets/Sounds/Effects/smokealarm1"),
            //    SoundCategoryType.Alarm, new Vector3(1, 0, 0), false));

            //object[] parameters = { "smokealarm"};

            //EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
            //    EventActionType.OnPlay, parameters));
        }

        /// <summary>
        /// Load texture data from file and add to the dictionary
        /// </summary>
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

        protected override void LoadContent()
        {
            //  _spriteBatch = new SpriteBatch(GraphicsDevice); //Move to Initialize for UISceneManager
            font = Content.Load<SpriteFont>("Assets/Fonts/Arial");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion Initialization - Dictionaries & Assets

        #region Initialization - UI & Menu

        /// <summary>
        /// Adds menu and UI elements
        /// </summary>
        private void InitializeUI()  //19.11.21
        {
            //TODO
            //InitializeGameMenu();
            playerUI = new PlayerUI(uiSceneManager);
            playerUI.InitializeUI();
            //InitializeGameUI();
        }

        /// <summary>
        /// Adds ui elements seen in-game (e.g. health, timer)
        /// </summary>
        private void InitializeGameUI()
        {
            //create the scene
            var mainGameUIScene = new UIScene(AppData.UI_SCENE_MAIN_NAME);

            #region Add Health Bar

            //add a health bar in the centre of the game window
            var texture = textureDictionary["progress_white"];
            var position = new Vector2(_graphics.PreferredBackBufferWidth / 2, 50);
            var origin = new Vector2(texture.Width / 2, texture.Height / 2);

            //create the UI element
            var healthTextureObj = new UITextureObject("health",
                UIObjectType.Texture,
                new Transform2D(position, new Vector2(2, 0.5f), 0),
                0,
                Color.White,
                origin,
                texture);

            //add a demo time based behaviour - because we can!
            healthTextureObj.AddComponent(new UITimeColorFlipBehaviour(Color.White, Color.Red, 1000));

            //add a progress controller
            healthTextureObj.AddComponent(new UIProgressBarController(5, 10));

            //add the ui element to the scene
            mainGameUIScene.Add(healthTextureObj);

            #endregion Add Health Bar

            #region Add Text

            var font = fontDictionary["ui"];
            var str = "player name";

            //create the UI element
            nameTextObj = new UITextObject(str, UIObjectType.Text,
                new Transform2D(new Vector2(50, 50),
                Vector2.One, 0),
                0, font, "Brutus Maximus");

            //  nameTextObj.Origin = font.MeasureString(str) / 2;
            //  nameTextObj.AddComponent(new UIExpandFadeBehaviour());

            //add the ui element to the scene
            mainGameUIScene.Add(nameTextObj);

            #endregion Add Text

            #region Add Scene To Manager & Set Active Scene

            //add the ui scene to the manager
            uiSceneManager.Add(mainGameUIScene);

            //set the active scene
            uiSceneManager.SetActiveScene(AppData.UI_SCENE_MAIN_NAME);

            #endregion Add Scene To Manager & Set Active Scene
        }

        /// <summary>
        /// Adds component to draw debug info to the screen
        /// </summary>
        private void InitializeDebugUI(bool showDebug)
        {
            
            if (showDebug)
            {
                Components.Add(new GDLibrary.Utilities.GDDebug.PerfUtility(
                    this,
                    _spriteBatch,
                    Content.Load<SpriteFont>("Assets/GDDebug/Fonts/ui_debug"),
                    new Vector2(40, 40),
                    Color.White));
            }
        }

        #endregion Initialization - UI & Menu

        #region Initialization - Engine, Cameras, Content

        /// <summary>
        /// Set application data, input, title and scene manager
        /// </summary>
        private void InitializeEngine(string gameTitle, int width, int height)
        {
            //set game title
            Window.Title = gameTitle;

            //the most important element! add event dispatcher for system events
            eventDispatcher = new EventDispatcher(this);
            //add physics manager to enable CD/CR and physics
            //physicsManager = new PhysicsManager(this);

            //instanciate scene manager to store all scenes
            sceneManager = new SceneManager(this);

            //create the ui scene manager to update and draw all ui scenes
            uiSceneManager = new UISceneManager(this, _spriteBatch);

            //add support for playing sounds
            soundManager = new SoundManager(this);

            //initialize global application data
            Application.Main = this;
            Application.Content = Content;
            Application.GraphicsDevice = _graphics.GraphicsDevice;
            Application.GraphicsDeviceManager = _graphics;
            Application.SceneManager = sceneManager;
			//Application.PhysicsManager = physicsManager;

            //instanciate render manager to render all drawn game objects using preferred renderer (e.g. forward, backward)
            renderManager = new RenderManager(this, new ForwardRenderer(), false);

            //instanciate screen (singleton) and set resolution etc
            Screen.GetInstance().Set(width, height, true, true); //change mosue cursor to false

            //instanciate input components and store reference in Input for global access
            Input.Keys = new KeyboardComponent(this);
            Input.Mouse = new MouseComponent(this);
            Input.Gamepad = new GamepadComponent(this);

            //************* add all input components to component list so that they will be updated and/or drawn ***********/
			//add event dispatcher
            Components.Add(eventDispatcher);       
                 
            //add time support
            Components.Add(Time.GetInstance(this));

            //add input support
            Components.Add(Input.Keys);
            Components.Add(Input.Mouse);
            Components.Add(Input.Gamepad);

            //add scene manager to update game objects
            Components.Add(sceneManager);

            //add render manager to draw objects
            Components.Add(renderManager);

            //add ui scene manager to update and drawn ui objects
            Components.Add(uiSceneManager);

            //add physics manager to enable CD/CR and physics
            //Components.Add(physicsManager);
            
			//add sound
            Components.Add(soundManager);
        }

        /// <summary>
        /// Create a scene, add content, add to the scene manager, and load default scene
        /// </summary>
        private void InitializeLevel()
        {
            activeScene = new Scene("level 1");
			InitializeCameras(activeScene);
            InitializeSkybox(activeScene, 1000);

            //Test load data from Level1.xml
            LoadLevelXML loadLevel1 = new LoadLevelXML(activeScene);
            loadLevel1.setGroundFloor();
            loadLevel1.LoadLevelFromXML();

            //StandardBullet bulletPrefab = new StandardBullet();
            //bulletPrefab.InitializeModel(activeScene);
            //activeScene.Add(bulletPrefab);
            //turret = new StandardTurret();
            //turret.InitializeModel(activeScene);
            //turret.bulletPrefab = bulletPrefab;
            //activeScene.Add(turret);
            //StandardBullet tempbullet = new StandardBullet();
            //tempbullet.InitializeModel(activeScene);
            //activeScene.Add(tempbullet);
            gun = new PlayerGun();
            gun.InitializeModel(activeScene);
            activeScene.Add(gun);

            sceneManager.Add(activeScene);
            sceneManager.LoadScene("level 1");
        }

        /// <summary>
        /// Demo of the new physics manager and collidable objects
        /// </summary>
        private void InitializeCollidables()
        {
        }

        /// <summary>


        /// <summary>
        /// Set up the skybox using a QuadMesh
        /// </summary>
        /// <param name="level">Scene Stores all game objects for current...</param>
        /// <param name="worldScale">float Value used to scale skybox normally 250 - 1000</param>
        private void InitializeSkybox(Scene level, float worldScale = 500)
        {
            #region Reusable - You can copy and re-use this code elsewhere, if required

            //re-use the code on the gfx card
            var shader = new BasicShader(Application.Content, true, true);
            //re-use the vertices and indices of the primitive
            var mesh = new QuadMesh();
            //create an archetype that we can clone from
            var archetypalQuad = new GameObject("quad", GameObjectType.Skybox, true);

            #endregion Reusable - You can copy and re-use this code elsewhere, if required

            GameObject clone = null;
            //back
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_back";
            clone.Transform.Translate(0, 0, -worldScale / 2.0f);
            clone.Transform.Scale(worldScale, worldScale, 1);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_back_material", shader, Color.White, 1, textureDictionary["skybox_back"])));
            level.Add(clone);

            //left
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_left";
            clone.Transform.Translate(-worldScale / 2.0f, 0, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, 90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_left_material", shader, Color.White, 1, textureDictionary["skybox_left"])));
            level.Add(clone);

            //right
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_right";
            clone.Transform.Translate(worldScale / 2.0f, 0, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, -90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_right_material", shader, Color.White, 1, textureDictionary["skybox_right"])));
            level.Add(clone);

            //front
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_front";
            clone.Transform.Translate(0, 0, worldScale / 2.0f);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(0, -180, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_front_material", shader, Color.White, 1, textureDictionary["skybox_front"])));
            level.Add(clone);

            //top
            clone = archetypalQuad.Clone() as GameObject;
            clone.Name = "skybox_sky";
            clone.Transform.Translate(0, worldScale / 2.0f, 0);
            clone.Transform.Scale(worldScale, worldScale, null);
            clone.Transform.Rotate(90, -90, 0);
            clone.AddComponent(new MeshRenderer(mesh, new BasicMaterial("skybox_sky_material", shader, Color.White, 1, textureDictionary["skybox_sky"])));
            level.Add(clone);
        }

        /// <summary>
        /// Initialize the camera(s) in our scene
        /// </summary>
        /// <param name="level"></param>
        private void InitializeCameras(Scene level)
        {
            #region First Person Camera

            //add camera game object
            var camera = new GameObject("main camera", GameObjectType.Camera);

            //set viewport
            //var viewportLeft = new Viewport(0, 0,
            //    _graphics.PreferredBackBufferWidth / 2,
            //    _graphics.PreferredBackBufferHeight);

            //add components
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            camera.AddComponent(new FPSController(0.05f, 0.025f, 0.00009f));

            //set initial position
            camera.Transform.SetTranslation(0, 1.43f, 0);

            //add to level
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

            //set viewport
            //var viewportRight = new Viewport(_graphics.PreferredBackBufferWidth / 2, 0,
            //    _graphics.PreferredBackBufferWidth / 2,
            //    _graphics.PreferredBackBufferHeight);

            //add components
            curveCamera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            curveCamera.AddComponent(new CurveBehaviour(translationCurve));
            curveCamera.AddComponent(new FOVOnScrollController(MathHelper.ToRadians(2)));

            //add to level
            level.Add(curveCamera);

            #endregion Curve Camera

            //set theMain camera, if we dont call this then the first camera added will be the Main
            level.SetMainCamera("main camera");

            //allows us to scale time on all game objects that based movement on Time
            // Time.Instance.TimeScale = 0.1f;
        }

        #endregion Initialization - Engine, Cameras, Content



        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
        	/*
        	if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                //DEMO - raise event
                //EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                //    EventActionType.OnPause));

                object[] parameters = { nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnRemoveObject, parameters));

                ////renderManager.StatusType = StatusType.Off;
            }
            else if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.U))
            {
                //DEMO - raise event

                object[] parameters = { "main game ui", nameTextObj };

                EventDispatcher.Raise(new EventData(EventCategoryType.UiObject,
                    EventActionType.OnAddObject, parameters));

                //renderManager.StatusType = StatusType.Drawn;
                //EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                //  EventActionType.OnPlay));
            }
            */
            base.Update(gameTime);
#if DEMO
            activeScene.Update();
            //bullet.Update();
            //turret.Update();
            //gun.Update();
            //DemoFind();
            fps.Update(gameTime);
#endif
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.HotPink);
            base.Draw(gameTime);

            #if DEMO
            _spriteBatch.Begin();
            fps.DrawFps(_spriteBatch, font, new Vector2(10f, 10f), Color.MonoGameOrange);
            _spriteBatch.End();
            #endif
            
        }

        #endregion Update & Draw

#if DEMO

        private void InitializeEditorHelpers()
        {
            //a game object to record camera positions to an XML file for use in a curve later
            var curveRecorder = new GameObject("curve recorder", GameObjectType.Editor);
            curveRecorder.AddComponent(new GDLibrary.Editor.CurveRecorderController());
            activeScene.Add(curveRecorder);
        }

        private void RunDemos()
        {
            // CurveDemo();
            // SaveLoadDemo();

            EventSenderDemo();
        }

        private void EventSenderDemo()
        {
        }

        private void CurveDemo()
        {
            //var curve1D = new GDLibrary.Parameters.Curve1D(CurveLoopType.Cycle);
            //curve1D.Add(0, 0);
            //curve1D.Add(10, 1000);
            //curve1D.Add(20, 2000);
            //curve1D.Add(40, 4000);
            //curve1D.Add(60, 6000);
            //var value = curve1D.Evaluate(500, 2);
        }

        private void SaveLoadDemo()
        {
        #region Serialization Single Object Demo

            var demoSaveLoad = new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f));
            GDLibrary.Utilities.SerializationUtility.Save("DemoSingle.xml", demoSaveLoad);
            var readSingle = GDLibrary.Utilities.SerializationUtility.Load("DemoSingle.xml",
                typeof(DemoSaveLoad)) as DemoSaveLoad;
            /*//Test save data
            List<LoadLevelXML> listDemos = new List<LoadLevelXML>();
            listDemos.Add(new LoadLevelXML("Wall", new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f)));
            listDemos.Add(new LoadLevelXML("Wall", new Vector3(10, 20, 30), new Vector3(4, 9, -18), new Vector3(15f, 1f, 202.5f)));
            listDemos.Add(new LoadLevelXML("Wall", new Vector3(100, 200, 300), new Vector3(145, 290, -80), new Vector3(6.5f, 1.1f, 8.05f)));

            GDLibrary.Utilities.SerializationUtility.Save("Level1.xml", listDemos);
            var readList = GDLibrary.Utilities.SerializationUtility.Load("Level1.xml",
                typeof(List<LoadLevelXML>)) as List<LoadLevelXML>;
            */
        #endregion Serialization Single Object Demo

        #region Serialization List Objects Demo

            List<DemoSaveLoad> listDemos = new List<DemoSaveLoad>();
            listDemos.Add(new DemoSaveLoad(new Vector3(1, 2, 3), new Vector3(45, 90, -180), new Vector3(1.5f, 0.1f, 20.25f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(10, 20, 30), new Vector3(4, 9, -18), new Vector3(15f, 1f, 202.5f)));
            listDemos.Add(new DemoSaveLoad(new Vector3(100, 200, 300), new Vector3(145, 290, -80), new Vector3(6.5f, 1.1f, 8.05f)));

            GDLibrary.Utilities.SerializationUtility.Save("ListDemo.xml", listDemos);
            var readList = GDLibrary.Utilities.SerializationUtility.Load("ListDemo.xml",
                typeof(List<DemoSaveLoad>)) as List<DemoSaveLoad>;

        #endregion Serialization List Objects Demo
        }

#endif
    }
}