#define DEMO

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
using GDApp.Content.Scripts.Turrets;
using GDApp.Content.Scripts.Turrets.Bullets;
using GDApp.Scripts.Debug;
using GDApp.Content.Scripts.Level;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GDLibrary.Collections;
using GDApp.App.Scripts.UI;
using GDApp.App.Scripts.Player;
using System;

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

        /// <summary>
        /// Updates and Draws all menu objects
        /// </summary>
        private MyMenuManager uiMenuManager;

        private SoundManager soundManager;
        private EventDispatcher eventDispatcher;
        /// <summary>
        /// Renders all ui objects
        /// </summary>
        private PhysicsManager physicsManager;

        /// <summary>
        /// Quick lookup for all textures used within the game
        /// </summary>
        private Dictionary<string, Texture2D> textureDictionary;

        /// <summary>
        /// Quick lookup for all fonts used within the game
        /// </summary>
        private ContentDictionary<SpriteFont> fontDictionary;

        //temp
        private Scene menu;
        private Scene level1;
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

            //Start with menu scene
            menu = new Scene("menu scene");
            level1 = new Scene("level 1");
            sceneManager.Add(menu);
            sceneManager.Add(level1);

            //InitializeMenu();

            //level with scenes and game objects
            InitializeLevel();



            //TODO - remove hardcoded mouse values - update Screen class to centre the mouse with hardcoded value - remove later
            Input.Mouse.Position = Screen.Instance.ScreenCentre;

            //turn on/off debug info
            InitializeDebugUI(true, true);

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

            //ui
            textureDictionary.Add("ui_progress_32_8", Content.Load<Texture2D>("Assets/Textures/UI/Controls/ui_progress_32_8"));
            textureDictionary.Add("progress_white", Content.Load<Texture2D>("Assets/Textures/UI/Controls/progress_white"));

            //menu
            textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/mainmenu"));
            textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/audiomenu"));
            textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/controlsmenu"));
            textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/UI/Backgrounds/exitmenuwithtrans"));
            textureDictionary.Add("genericbtn", Content.Load<Texture2D>("Assets/Textures/UI/Controls/genericbtn"));
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
            
        }
        /// <summary>
        /// Adds component to draw debug info to the screen
        /// </summary>
        private void InitializeDebugUI(bool showDebug, bool showCollisionSkins = true)
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
            if (showCollisionSkins)
                Components.Add(new GDLibrary.Utilities.GDDebug.PhysicsDebugDrawer(this, Color.Red));


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
            physicsManager = new PhysicsManager(this);

            //instanciate scene manager to store all scenes
            sceneManager = new SceneManager(this);

            //create the ui scene manager to update and draw all ui scenes
            uiSceneManager = new UISceneManager(this, _spriteBatch);

            //create the ui menu manager to update and draw all menu scenes
            //uiMenuManager = new MyMenuManager(this, _spriteBatch, this);

            //add support for playing sounds
            soundManager = new SoundManager(this);

            //picking support using physics engine
            //this predicate lets us say ignore all the other collidable objects except interactables and consumables
            Predicate<GameObject> collisionPredicate =
                (collidableObject) =>
                {
                    if (collidableObject != null)
                        return collidableObject.GameObjectType
                        == GameObjectType.Interactable
                        || collidableObject.GameObjectType == GameObjectType.Consumable;

                    return false;
                };

            //initialize global application data
            Application.Main = this;
            Application.Content = Content;
            Application.GraphicsDevice = _graphics.GraphicsDevice;
            Application.GraphicsDeviceManager = _graphics;
            Application.PhysicsManager = physicsManager;
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

            //add physics manager to enable CD/CR and physics
            Components.Add(physicsManager);

            //add scene manager to update game objects
            Components.Add(sceneManager);

            //add render manager to draw objects
            Components.Add(renderManager);

            //add ui scene manager to update and drawn ui objects
            Components.Add(uiSceneManager);

            //add ui menu manager to update and drawn menu objects
            //Components.Add(uiMenuManager);

            //add sound
            Components.Add(soundManager);
        }

        private void InitializeMenu()
        {
            var camera = new GameObject("menu camera", GameObjectType.Camera);
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            camera.Transform.SetTranslation(0, 2f, -10);
            menu.Add(camera);

            InitializeSkybox(menu, 1000);

            UIMenu uiMenu = new UIMenu();
            uiMenu.InitializeGameMenu(uiMenuManager, _graphics, textureDictionary, fontDictionary);

            sceneManager.LoadScene("menu scene");
        }


        /// <summary>
        /// Create a scene, add content, add to the scene manager, and load default scene
        /// </summary>
        public void InitializeLevel()
        {
            Player player = new Player();

            InitializeCameras(level1);
            InitializeSkybox(level1, 1000);


            //Test load data from Level1.xml
            LoadLevelXML loadLevel1 = new LoadLevelXML(level1);
            loadLevel1.setGroundFloor();
            loadLevel1.LoadLevelFromXML();
            InitializeProps(level1);
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

            //add menu and ui
            //InitializeUI();
            playerUI = new PlayerUI(uiSceneManager);
            playerUI.InitializeUI(player);

            gun = new PlayerGun();
            gun.InitializeModel(level1);
            level1.Add(gun);

            sceneManager.LoadScene(level1);
        }

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
            /*
            #region First Person Camera

            //add camera game object
            var camera = new GameObject("main camera", GameObjectType.Camera);

            //add components
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));
            camera.AddComponent(new FPSController(0.05f, 0.025f, 0.00009f));

            //set initial position
            camera.Transform.SetTranslation(0, 1.43f, 0);

            //add to level
            level.Add(camera);

            #endregion First Person Camera
            */
            //add camera game object
            var camera = new GameObject(AppData.CAMERA_FIRSTPERSON_COLLIDABLE_NAME, GameObjectType.Camera);

            //set initial position - important to set before the collider as collider capsule feeds off this position
            camera.Transform.SetTranslation(0, 2.6f, 0);

            //add components
            camera.AddComponent(new Camera(_graphics.GraphicsDevice.Viewport));

            //adding a collidable surface that enables acceleration, jumping
            //var collider = new CharacterCollider(2, 2, true, false);

            var collider = new MyHeroCollider(2, 2, true, false);
            camera.AddComponent(collider);
            collider.AddPrimitive(new Capsule(camera.Transform.LocalTranslation,
                Matrix.CreateRotationX(MathHelper.PiOver2), 0.3f, 1.6f),
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collider.Enable(false, 2);

            //add controller to actually move the collidable camera
            camera.AddComponent(new FPSController(0.5f, 0.3f, 0.006f, 12));
            level.Add(camera);

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
            //level.SetMainCamera("main camera");
            level.SetMainCamera(AppData.CAMERA_FIRSTPERSON_COLLIDABLE_NAME);
            //allows us to scale time on all game objects that based movement on Time
            // Time.Instance.TimeScale = 0.1f;
        }

        #endregion Initialization - Engine, Cameras, Content

        private void InitializeProps(Scene level)
        {
            #region Barrels
            var texture = Content.Load<Texture2D>("Assets/Textures/Props/Barrel_Texture");
            var shader = new BasicShader(Application.Content, false, true);
            var barrelMaterial = new BasicMaterial("barrel", shader, texture);

            var barrel = new GameObject("barrel", GameObjectType.Prop);
            var barrelModel = Content.Load<Model>("Assets/Models/Props/Barrel");
            var barrelRenderer = new ModelRenderer(barrelModel, barrelMaterial);

            barrel.AddComponent(barrelRenderer);
            barrel.Transform.SetScale(0.45f, 0.45f, 0.45f);
            barrel.Transform.SetTranslation(-40, 0, -40);
            level.Add(barrel);

            var count = 0;
            for (var i = 0; i <= 4; i++)
            {
                var clone = barrel.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";

                if (i == 0)
                {
                    clone.Transform.SetScale(0.45f, 0.45f, 0.45f);
                    clone.Transform.SetTranslation(-39, 0, -40);
                }
                else if (i == 1)
                {
                    clone.Transform.SetScale(0.45f, 0.45f, 0.45f);
                    clone.Transform.SetTranslation(-39.5f, 0, -41);
                }

                else if (i == 2)
                {
                    clone.Transform.SetScale(0.45f, 0.45f, 0.45f);
                    clone.Transform.SetTranslation(-12, 0, -78);
                }

                else if (i == 3)
                {
                    clone.Transform.SetScale(0.45f, 0.45f, 0.45f);
                    clone.Transform.SetTranslation(-23, 0, -55);
                }

                else if (i == 4)
                {
                    clone.Transform.SetScale(0.45f, 0.45f, 0.45f);
                    clone.Transform.SetTranslation(10, 5.5f, -78);
                }

                level.Add(clone);
            }
            #endregion

            #region Bins
            texture = Content.Load<Texture2D>("Assets/Textures/Props/BinTexture");
            shader = new BasicShader(Application.Content, false, true);
            var binMaterial = new BasicMaterial("bin", shader, texture);

            var bin = new GameObject("bin", GameObjectType.Prop);
            var binModel = Content.Load<Model>("Assets/Models/Props/Bin");
            var binRenderer = new ModelRenderer(binModel, binMaterial);

            bin.Transform.SetScale(0.3f, 0.3f, 0.3f);
            bin.Transform.SetTranslation(-20, -0.15f, -77);
            bin.AddComponent(binRenderer);
            level.Add(bin);

            count = 0;
            for (var i = 0; i <= 4; i++)
            {
                var binClone = bin.Clone() as GameObject;
                binClone.Name = $"{binClone.Name} - {count++}";

                if (i == 0)
                {
                    binClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                    binClone.Transform.SetTranslation(-5, -0.15f, -40);
                }
                else if (i == 1)
                {
                    binClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                    binClone.Transform.SetTranslation(-3, -0.18f, -6);
                }
                else if (i == 2)
                {
                    binClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                    binClone.Transform.SetTranslation(-3, -0.18f, -20);
                }
                else if (i == 3)
                {
                    binClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                    binClone.Transform.SetTranslation(-2, -0.18f, -49);
                }
                else if (i == 4)
                {
                    binClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                    binClone.Transform.SetTranslation(-2, -0.18f, -67);
                }

                level.Add(binClone);
                #endregion

                #region Boxes
                texture = Content.Load<Texture2D>("Assets/Textures/Props/BoxTexture");
                shader = new BasicShader(Application.Content, false, true);
                var boxMaterial = new BasicMaterial("box", shader, texture);

                var box = new GameObject("box", GameObjectType.Prop);
                var boxModel = Content.Load<Model>("Assets/Models/Props/Box");
                var boxRenderer = new ModelRenderer(boxModel, boxMaterial);

                box.Transform.SetScale(0.3f, 0.3f, 0.3f);
                box.Transform.SetTranslation(-49, -0.2f, -48);
                box.AddComponent(boxRenderer);
                level.Add(box);

                count = 0;
                for (var j = 0; j <= 7; j++)
                {
                    var boxClone = box.Clone() as GameObject;
                    boxClone.Name = $"{boxClone.Name} - {count++}";

                    if (j == 0)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetRotation(0, 10, 0);
                        boxClone.Transform.SetTranslation(-49, 0.6f, -48.5f);
                    }
                    else if (j == 1)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-49, -0.2f, -49);
                    }
                    else if (j == 2)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-40, -0.2f, -86);
                    }
                    else if (j == 3)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-26, -0.2f, -80);
                    }
                    else if (j == 4)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(12, -0.2f, -41);
                    }
                    else if (j == 5)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-21, -0.2f, -68);
                    }
                    else if (j == 6)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-30, 5.5f, -70);
                    }
                    else if (j == 7)
                    {
                        boxClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        boxClone.Transform.SetTranslation(-27, 5.5f, -66);
                    }
                    level.Add(boxClone);
                }
                #endregion

                #region Palettes
                texture = Content.Load<Texture2D>("Assets/Textures/Props/PaletteTexture");
                shader = new BasicShader(Application.Content, false, true);
                var paletteMaterial = new BasicMaterial("palette", shader, texture);

                var palette = new GameObject("palette", GameObjectType.Prop);
                var paletteModel = Content.Load<Model>("Assets/Models/Props/Palette");
                var paletteRenderer = new ModelRenderer(paletteModel, paletteMaterial);

                palette.Transform.SetScale(0.4f, 0.4f, 0.4f);
                palette.Transform.SetRotation(40, 90, 0);
                palette.Transform.SetTranslation(-49, 0.5f, -44);
                palette.AddComponent(paletteRenderer);
                level.Add(palette);

                count = 0;
                for (var j = 0; j <= 1; j++)
                {
                    var paletteClone = palette.Clone() as GameObject;
                    paletteClone.Name = $"{paletteClone.Name} - {count++}";

                    #region Hallways
                    if (j == 0)
                    {
                        paletteClone.Transform.SetScale(0.4f, 0.4f, 0.4f);
                        paletteClone.Transform.SetTranslation(-24, 0, -62);
                    }
                    if (j == 1)
                    {
                        paletteClone.Transform.SetScale(0.4f, 0.4f, 0.4f);
                        paletteClone.Transform.SetTranslation(-15, 5.75f, -78);
                    }
                    #endregion
                    level.Add(paletteClone);
                }
                #endregion

                #region Bolts
                texture = Content.Load<Texture2D>("Assets/Textures/Props/SteelTexture");
                shader = new BasicShader(Application.Content, false, true);
                var boltMaterial = new BasicMaterial("bolt", shader, texture);

                var bolt = new GameObject("bolt", GameObjectType.Prop);
                var boltModel = Content.Load<Model>("Assets/Models/Props/Bolt");
                var boltRenderer = new ModelRenderer(boltModel, boltMaterial);

                bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                bolt.Transform.SetRotation(90, 0, 0);
                bolt.Transform.SetTranslation(-44, 0.05f, -44);
                bolt.AddComponent(boltRenderer);
                level.Add(bolt);

                count = 0;
                for (var j = 0; j <= 5; j++)
                {
                    var boltClone = bolt.Clone() as GameObject;
                    boltClone.Name = $"{boltClone.Name} - {count++}";

                    #region Hallways
                    if (j == 0)
                    {
                        bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                        bolt.Transform.SetRotation(90, 0, 0);
                        boltClone.Transform.SetTranslation(-25, 0.05f, -62);
                    }
                    else if (j == 1)
                    {
                        bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                        bolt.Transform.SetRotation(90, 40, 0);
                        boltClone.Transform.SetTranslation(0, 0.05f, -84);
                    }
                    else if (j == 2)
                    {
                        bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                        bolt.Transform.SetRotation(-90, 70, 0);
                        boltClone.Transform.SetTranslation(34, 0.05f, -12);
                    }
                    else if (j == 3)
                    {
                        bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                        bolt.Transform.SetRotation(90, 0, 0);
                        boltClone.Transform.SetTranslation(10, 0.05f, -42);
                    }
                    else if (j == 4)
                    {
                        bolt.Transform.SetScale(0.05f, 0.05f, 0.05f);
                        bolt.Transform.SetRotation(90, 20, 0);
                        boltClone.Transform.SetTranslation(-10, 0.05f, -50);
                    }
                    level.Add(boltClone);
                    #endregion
                }
                #endregion


                #region Gears
                texture = Content.Load<Texture2D>("Assets/Textures/Props/CopperTexture");
                shader = new BasicShader(Application.Content, false, true);
                var gearMaterial = new BasicMaterial("gear", shader, texture);

                var gear = new GameObject("gear", GameObjectType.Prop);
                var gearModel = Content.Load<Model>("Assets/Models/Props/Gear");
                var gearRenderer = new ModelRenderer(gearModel, gearMaterial);

                gear.Transform.SetScale(0.15f, 0.15f, 0.15f);
                gear.Transform.SetTranslation(-38, 0f, -50);
                gear.AddComponent(gearRenderer);
                level.Add(gear);

                count = 0;
                for (var k = 0; k <= 7; k++)
                {
                    var gearClone = gear.Clone() as GameObject;
                    gearClone.Name = $"{gearClone.Name} - {count++}";

                    #region Hallways
                    if (k == 0)
                    {
                        gearClone.Transform.SetScale(0.15f, 0.15f, 0.15f);
                        gearClone.Transform.SetTranslation(-38, 0f, -50);
                    }
                    else if (k == 1)
                    {
                        gearClone.Transform.SetScale(0.15f, 0.15f, 0.15f);
                        gearClone.Transform.SetRotation(50, 90, 0);
                        gearClone.Transform.SetTranslation(-49.5f, 0.2f, -68);
                    }
                    else if (k == 2)
                    {
                        gearClone.Transform.SetScale(0.15f, 0.15f, 0.15f);
                        gearClone.Transform.SetTranslation(7, 0, -50);
                    }
                    else if (k == 3)
                    {
                        gearClone.Transform.SetScale(0.15f, 0.15f, 0.15f);
                        gearClone.Transform.SetRotation(60, 0, 0);
                        gearClone.Transform.SetTranslation(-6, 0.2f, -47.7f);
                    }
                    else if (k == 4)
                    {
                        gearClone.Transform.SetScale(0.15f, 0.15f, 0.15f);
                        gearClone.Transform.SetTranslation(-21, 0, -52f);
                    }
                    else if (k == 5)
                    {
                        gearClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        gearClone.Transform.SetTranslation(8, 0, -60f);
                    }
                    else if (k == 6)
                    {
                        gearClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        gearClone.Transform.SetRotation(60, -90, 0);
                        gearClone.Transform.SetTranslation(17.55f, 0.27f, -71f);
                    }
                    else if (k == 7)
                    {
                        gearClone.Transform.SetScale(0.3f, 0.3f, 0.3f);
                        gearClone.Transform.SetRotation(60, 0, 0);
                        gearClone.Transform.SetTranslation(-10, 0.25f, -75.3f);
                    }
                    level.Add(gearClone);
                    #endregion
                }
                #endregion

                #region Newspapers
                texture = Content.Load<Texture2D>("Assets/Textures/Props/NewspaperTexture");
                shader = new BasicShader(Application.Content, false, true);
                var newsMaterial = new BasicMaterial("newspaper", shader, texture);

                var newspaper = new GameObject("newspaper", GameObjectType.Prop);
                var newsModel = Content.Load<Model>("Assets/Models/Props/Newspaper");
                var newsRenderer = new ModelRenderer(newsModel, newsMaterial);
                newspaper.Transform.SetRotation(-90, 40, 0);
                newspaper.Transform.SetScale(0.5f, 0.5f, 0.5f);
                newspaper.AddComponent(newsRenderer);
                level.Add(newspaper);

                count = 0;
                for (var k = 0; k <= 1; k++)
                {
                    var newsClone = newspaper.Clone() as GameObject;
                    newsClone.Name = $"{newsClone.Name} - {count++}";

                    //balcony
                    if (k == 0)
                    {
                        newsClone.Transform.SetScale(0.5f, 0.5f, 0.5f);
                        newsClone.Transform.SetRotation(-90, -40, 0);
                        newsClone.Transform.SetTranslation(-20, 5.2f, -77f);
                    }
                    //hallway
                    else if (k == 1)
                    {
                        newsClone.Transform.SetScale(0.5f, 0.5f, 0.5f);
                        newsClone.Transform.SetRotation(-90, -40, 0);
                        newsClone.Transform.SetTranslation(-15, -0.1f, -50);
                    }
                    level.Add(newsClone);
                }
                #endregion

                #region Nuts
                texture = Content.Load<Texture2D>("Assets/Textures/Props/SteelTexture");
                shader = new BasicShader(Application.Content, false, true);
                var nutMaterial = new BasicMaterial("nut", shader, texture);

                var nut = new GameObject("nut", GameObjectType.Prop);
                var nutModel = Content.Load<Model>("Assets/Models/Props/Nut");
                var nutRenderer = new ModelRenderer(nutModel, nutMaterial);
                nut.Transform.SetTranslation(-34, 0f, -55);
                nut.Transform.SetScale(0.1f, 0.1f, 0.1f);
                nut.AddComponent(nutRenderer);
                level.Add(nut);

                count = 0;
                for (var k = 0; k <= 4; k++)
                {
                    var nutClone = nut.Clone() as GameObject;
                    nutClone.Name = $"{nutClone.Name} - {count++}";

                    #region Hallways
                    if (k == 0)
                    {
                        nutClone.Transform.SetTranslation(-30, 0f, -74);
                        nutClone.Transform.SetScale(0.1f, 0.1f, 0.1f);
                    }
                    else if (k == 1)
                    {
                        nutClone.Transform.SetTranslation(-15, 0f, -60);
                        nutClone.Transform.SetScale(0.1f, 0.1f, 0.1f);
                    }
                    else if (k == 2)
                    {
                        nutClone.Transform.SetTranslation(15, 0f, -43);
                        nutClone.Transform.SetScale(0.1f, 0.1f, 0.1f);
                    }
                    else if (k == 3)
                    {
                        nutClone.Transform.SetTranslation(-6, 0f, -53);
                        nutClone.Transform.SetScale(0.1f, 0.1f, 0.1f);
                    }
                    else if (k == 4)
                    {
                        nutClone.Transform.SetTranslation(-6, 0f, -27);
                        nutClone.Transform.SetScale(0.1f, 0.1f, 0.1f);
                    }
                    #endregion
                    level.Add(nutClone);
                }

                #endregion
            }
        }



        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
            if (Input.Keys.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                InitializeMenu();
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPause));
            }
            base.Update(gameTime);
            //GameObject obj = sceneManager?.Find(gameObject => gameObject.Name.Equals("main camera"));
            //System.Diagnostics.Debug.WriteLine("Menu camera: " + obj?.Scene.Name);


#if DEMO
            
            //bullet.Update();
            //turret.Update();
            //gun.Update();
            //DemoFind();
            //fps.Update(gameTime);
#endif
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            base.Draw(gameTime);
        }

        #endregion Update & Draw


    }
}