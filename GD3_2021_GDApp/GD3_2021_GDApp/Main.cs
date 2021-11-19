#define DEMO
using GDApp.Scripts.Debug;
using GDLibrary.Utilities;
using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Editor;
using GDLibrary.Graphics;
using GDLibrary.Inputs;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using GDLibrary.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GDApp.Content.Scripts.Player;

namespace GDApp
{
    public class Main : Game
    {
        #region Fields

        PlayerUI playerUI = new PlayerUI();
        private SpriteFont font;
        FramerateCounter fps = new FramerateCounter();

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
        private GameObject camera;
        private PlayerGun gun;

        #endregion Fields

        #region Constructors

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #endregion Constructors

        #region Initialization - Scene manager, Application data, Screen, Input, Scenes, Game Objects

        /// <summary>
        /// Initialize engine, dictionaries, assets, level contents
        /// </summary>
        protected override void Initialize()
        {
            //data, input, scene manager
            InitializeEngine("My Game Title Goes Here", 1024, 768);

            //load structures that store assets (e.g. textures, sounds) or archetypes (e.g. Quad game object)
            InitializeDictionaries();

            //load assets into the relevant dictionary
            LoadAssets();

            //level with scenes and game objects
            InitializeLevel();

            //TODO - remove hardcoded mouse values - update Screen class
            //centre the mouse with hardcoded value - remove later
            Input.Mouse.Position = new Vector2(512, 384);

            base.Initialize();
        }

        /// <summary>
        /// Stores all re-used assets and archetypal game objects
        /// </summary>

        private void InitializeDictionaries()
        {
            textureDictionary = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Load resources from file
        /// </summary>
        private void LoadAssets()
        {
            LoadTextures();
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

            //walls
            textureDictionary.Add("brick", Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/brick"));
            textureDictionary.Add("floor", Content.Load<Texture2D>("Assets/Textures/Architecture/Floors/TarmacTexture"));

            //Turrets + Pickups (Non-Props)
            textureDictionary.Add("speed_pickup", Content.Load<Texture2D>("Assets/Textures/Pickups/ElectricityTexture"));
            textureDictionary.Add("health_pickup", Content.Load<Texture2D>("Assets/Textures/Pickups/HealthKitTexture"));
            textureDictionary.Add("rapidFire_pickup", Content.Load<Texture2D>("Assets/Textures/Pickups/BrassTexture"));
            textureDictionary.Add("turret", Content.Load<Texture2D>("Assets/Demo/Textures/grey"));
        }

        /// <summary>
        /// Create a scene, add content, add to the scene manager, and load default scene
        /// </summary>
        private void InitializeLevel()
        {
            activeScene = new Scene("level 1");

            //InitializeSkybox(activeScene, 500);
            InitializeCameras(activeScene);
            // InitializeCubes(activeScene);
            InitializeFloors(activeScene);
            InitializeWalls(activeScene);
            InitializePickups(activeScene);
            InitializeTurrets(activeScene);
            //InitializeModels(activeScene);
            gun = new PlayerGun(this);
            gun.InitializeModel(activeScene);





            sceneManager.Add(activeScene);
            sceneManager.LoadScene("level 1");
        }


        /// <summary>
        /// Set up the skybox using a QuadMesh
        /// </summary>
        /// <param name="level">Scene Stores all game objects for current...</param>
        /// <param name="worldScale">float Value used to scale skybox normally 250 - 1000</param>
        private void InitializeSkybox(Scene level, float worldScale = 1000)
        {
            #region Non-copy implementation BAD
            //front
            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["skybox_front"];
            material.Shader = new BasicShader();
            var archetypalQuad = new GameObject("quad", GameObjectType.Skybox);
            var renderer = new MeshRenderer();
            renderer.Material = material;
            archetypalQuad.AddComponent(renderer);
            renderer.Mesh = new QuadMesh();
            archetypalQuad.Transform.Translate(0, 0, -worldScale / 2.0f);
            archetypalQuad.Transform.Scale(worldScale, worldScale, null);
            level.Add(archetypalQuad);
            //right
            var material2 = new BasicMaterial("simple diffuse");
            material2.Texture = textureDictionary["skybox_left"];
            material2.Shader = new BasicShader();
            var archetypalQuad2 = new GameObject("quad", GameObjectType.Skybox);
            var renderer2 = new MeshRenderer();
            renderer2.Material = material2;
            archetypalQuad2.AddComponent(renderer2);
            renderer2.Mesh = new QuadMesh();
            archetypalQuad2.Transform.Translate(worldScale / 2.0f, 0, 0);
            archetypalQuad2.Transform.Scale(worldScale, worldScale, null);
            archetypalQuad2.Transform.Rotate(0, -90, 0);
            level.Add(archetypalQuad2);
            //left
            var material3 = new BasicMaterial("simple diffuse");
            material3.Texture = textureDictionary["skybox_right"];
            material3.Shader = new BasicShader();
            var archetypalQuad3 = new GameObject("quad", GameObjectType.Skybox);
            var renderer3 = new MeshRenderer();
            renderer3.Material = material3;
            archetypalQuad3.AddComponent(renderer3);
            renderer3.Mesh = new QuadMesh();
            archetypalQuad3.Transform.Translate(-worldScale / 2.0f, 0, 0);
            archetypalQuad3.Transform.Scale(worldScale, worldScale, null);
            archetypalQuad3.Transform.Rotate(0, 90, 0);
            level.Add(archetypalQuad3);
            //back
            var material4 = new BasicMaterial("simple diffuse");
            material4.Texture = textureDictionary["skybox_back"];
            material4.Shader = new BasicShader();
            var archetypalQuad4 = new GameObject("quad", GameObjectType.Skybox);
            var renderer4 = new MeshRenderer();
            renderer4.Material = material4;
            archetypalQuad4.AddComponent(renderer4);
            renderer4.Mesh = new QuadMesh();
            archetypalQuad4.Transform.Translate(0, 0, worldScale / 2.0f);
            archetypalQuad4.Transform.Scale(worldScale, worldScale, null);
            archetypalQuad4.Transform.Rotate(0, -180, 0);
            level.Add(archetypalQuad4);
            //top
            var material5 = new BasicMaterial("simple diffuse");
            material5.Texture = textureDictionary["skybox_sky"];
            material5.Shader = new BasicShader();
            var archetypalQuad5 = new GameObject("quad", GameObjectType.Skybox);
            var renderer5 = new MeshRenderer();
            renderer5.Material = material5;
            archetypalQuad5.AddComponent(renderer5);
            renderer5.Mesh = new QuadMesh();
            archetypalQuad5.Transform.Translate(0, worldScale / 2.0f, 0);
            archetypalQuad5.Transform.Scale(worldScale, worldScale, null);
            archetypalQuad5.Transform.Rotate(90, 90, 0);
            level.Add(archetypalQuad5);
            #endregion Non-copy implementation

            #region Nial buggy version
            /*
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
            level.Add(top); */
            #endregion Nial buggy version
        }

        /// <summary>
        /// Initialize the camera(s) in our scene
        /// </summary>
        /// <param name="level"></param>
        private void InitializeCameras(Scene level)
        {
            #region First Person Camera

            //add camera game object
            camera = new GameObject("main camera", GameObjectType.Camera);
            //  _graphics.PreferredBackBufferWidth
            int width = 1024, height = 768;
            var viewport = new Viewport(0, 0, width, height);
            camera.AddComponent(new Camera(viewport));
            camera.AddComponent(new FPSController(0.05f, 0.025f, 0.00009f)); camera.Transform.SetTranslation(0, 0, 15);
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

        /// <summary>
        /// Add demo game objects based on FBX vertex data
        /// </summary>
        /// <param name="level"></param>
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

        private void InitializePickups(Scene level)
        {
            #region Archetype

            //Speed Pickup (represented by lightning bolt) - Commented out lines are to create Rapid-Fire pickup
            //(represented by a bullet), both are included so they can be swapped between for demonstration purposes
            //for CA2.
            var speedMaterial = new BasicMaterial("model material");
            speedMaterial.Texture = Content.Load<Texture2D>("Assets/Textures/Pickups/ElectricityTexture");
            //speedMaterial.Texture = Content.Load<Texture2D>("Assets/Textures/Pickups/BrassTexture");
            speedMaterial.Shader = new BasicShader();

            var speedPickup = new GameObject("speed_pickup", GameObjectType.Consumable);
            //var speedPickup = new GameObject("rapidFire_pickup", GameObjectType.Consumable);
            var speedRenderer = new ModelRenderer();
            speedRenderer.Material = speedMaterial;
            speedPickup.AddComponent(speedRenderer);
            speedRenderer.Model = Content.Load<Model>("Assets/Models/Pickups/SpeedPickup");
            //speedRenderer.Model = Content.Load<Model>("Assets/Models/Pickups/RapidFirePickup");

            //upsize the model a little because the lightning bolt is quite small
            speedPickup.Transform.SetScale(8f, 8f, 8f);
            speedPickup.Transform.SetRotation(0, 90, 0);
            speedPickup.Transform.SetTranslation(-630, 130, 260);

            //Rapid-Fire Pickup Transform
            //speedPickup.Transform.SetRotation(90, 0, 0);
            //speedPickup.Transform.SetScale(3f, 3f, 3f);
            //speedPickup.Transform.SetTranslation(-630, 130, 250);
            level.Add(speedPickup);


            //Health Kit Pickup (represented by a health kit box)
            var healthMaterial = new BasicMaterial("model material");
            healthMaterial.Texture = Content.Load<Texture2D>("Assets/Textures/Pickups/HealthKitTexture");
            healthMaterial.Shader = new BasicShader();

            var healthPickup = new GameObject("health_pickup", GameObjectType.Consumable);
            var healthRenderer = new ModelRenderer();
            healthRenderer.Material = healthMaterial;
            healthPickup.AddComponent(healthRenderer);
            healthRenderer.Model = Content.Load<Model>("Assets/Models/Pickups/HealthKit");
            healthPickup.Transform.SetTranslation(65, -22, -60);
            healthPickup.Transform.SetScale(5f, 5f, 5f);
            level.Add(healthPickup);

            #endregion Archetype

            var count = 0;
            for (var i = 0; i <= 1; i += 1)
            {
                var clone = healthPickup.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                if(i == 0)
                {
                    clone.Transform.SetTranslation(-80, -22, -175);
                    clone.Transform.SetScale(5f, 5f, 5f);
                }
                else if (i == 1)
                {
                    clone.Transform.SetTranslation(195, -22, -175);
                    clone.Transform.SetScale(5f, 5f, 5f);
                }
                level.Add(clone);
            }
        }

        private void InitializeTurrets(Scene level)
        {
            var turretMaterial = new BasicMaterial("model material");
            //Placeholder texture - not the final one!
            turretMaterial.Texture = Content.Load<Texture2D>("Assets/Demo/Textures/grey");
            turretMaterial.Shader = new BasicShader();

            var turret = new GameObject("turret", GameObjectType.NPC);
            var turretRenderer = new ModelRenderer();
            turretRenderer.Material = turretMaterial;
            turret.AddComponent(turretRenderer);
            turretRenderer.Model = Content.Load<Model>("Assets/Models/Turret");
            //Far Left in Center Room
            turret.Transform.SetTranslation(-80, -22, -15);
            turret.Transform.SetRotation(0, 90, 0);
            turret.Transform.SetScale(3f, 3f, 3f);
            level.Add(turret);

            var count = 0;
            //Go up to 11
            for (var i = 0; i <= 7; i += 1)
            {
                var clone = turret.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                //Far Left Hall, Mid Right
                if (i == 0)
                {
                    clone.Transform.SetTranslation(-105, -22, -75);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(10f, 3f, 3f);
                }
                //Far Right in Center Room
                else if (i == 1)
                {
                    clone.Transform.SetTranslation(210, -22, -75);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(10f, 3f, 3f);
                }
                //Left Entrance to Center Room
                else if (i == 2)
                {
                    clone.Transform.SetTranslation(25, -22, 15);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Right Entrance to Center Room
                else if (i == 3)
                {
                    clone.Transform.SetTranslation(105, -22, 15);
                    clone.Transform.SetScale(10f, 3f, 3f);
                }
                //Far Right hall, Top Left
                else if (i == 4)
                {
                    clone.Transform.SetTranslation(240, -22, -175);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Right hall, Top Right
                else if (i == 5)
                {
                    clone.Transform.SetTranslation(490, -22, -100);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Left Hall, Bottom Left
                else if (i == 6)
                {
                    clone.Transform.SetTranslation(-480, -22, 50);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Left Hall, Top Left
                else if (i == 7)
                {
                    clone.Transform.SetTranslation(-480, -22, -200);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                level.Add(clone);
            }
        }

        /// <summary>
        /// Add demo game objects based on user-defined vertices and indices
        /// </summary>
        /// <param name="level"></param>
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
        /// add wall objects as rescaled cubes
        /// </summary>
        /// <param name="level"></param>
        private void InitializeWalls(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["brick"];
            material.Shader = new BasicShader();

            var archetypalWall = new GameObject("wall", GameObjectType.Architecture);
            var renderer = new MeshRenderer();
            renderer.Material = material;
            archetypalWall.AddComponent(renderer);
            renderer.Mesh = new CubeMesh();

            #endregion Archetype

            #region Middle Room Walls
            //front left wall
            var clone = archetypalWall.Clone() as GameObject;
            clone.Name = $"front left -{clone.Name}";
            clone.Transform.SetTranslation(-20, 0, 0);
            clone.Transform.SetScale(150, 50, 3);
            level.Add(clone);

            //front right wall
            var clone2 = archetypalWall.Clone() as GameObject;
            clone2.Name = $"front right -{clone.Name}";
            clone2.Transform.SetTranslation(150, 0, 0);
            clone2.Transform.SetScale(150, 50, 3);
            level.Add(clone2);

            //right side
            var clone3 = archetypalWall.Clone() as GameObject;
            clone3.Name = $"right side -{clone.Name}";
            clone3.Transform.SetTranslation(225, 0, -99);
            clone3.Transform.Rotate(0, 270, 0);
            clone3.Transform.SetScale(200, 50, 3);
            level.Add(clone3);

            //left side
            var clone4 = archetypalWall.Clone() as GameObject;
            clone4.Name = $"right side -{clone.Name}";
            clone4.Transform.SetTranslation(-95, 0, -99);
            clone4.Transform.Rotate(0, 90, 0);
            clone4.Transform.SetScale(200, 50, 3);
            level.Add(clone4);

            //back left wall
            var clone5 = archetypalWall.Clone() as GameObject;
            clone5.Name = $"back left -{clone.Name}";
            clone5.Transform.SetTranslation(-20, 0, -199);
            clone5.Transform.SetScale(150, 50, 3);
            level.Add(clone5);

            //back right wall
            var clone6 = archetypalWall.Clone() as GameObject;
            clone6.Name = $"back right -{clone.Name}";
            clone6.Transform.SetTranslation(150, 0, -199);
            clone6.Transform.SetScale(150, 50, 3);
            level.Add(clone6);
            #endregion

            #region Main Courtyard Walls
            //front main wall
            var clone7 = archetypalWall.Clone() as GameObject;
            clone7.Name = $"main front -{clone.Name}";
            clone7.Transform.SetTranslation(0, 0, 300);
            clone7.Transform.SetScale(1000, 50, 3);
            level.Add(clone7);

            //left main wall
            var clone8 = archetypalWall.Clone() as GameObject;
            clone8.Name = $"main left -{clone.Name}";
            clone8.Transform.SetTranslation(-500, 35, 0);
            clone8.Transform.Rotate(0, 90, 0);
            clone8.Transform.SetScale(600, 120, 3);
            level.Add(clone8);

            //right main wall
            var clone9 = archetypalWall.Clone() as GameObject;
            clone9.Name = $"main right -{clone.Name}";
            clone9.Transform.SetTranslation(500, 0, 0);
            clone9.Transform.Rotate(0, 270, 0);
            clone9.Transform.SetScale(600, 50, 3);
            level.Add(clone9);

            //back main wall
            var clone10 = archetypalWall.Clone() as GameObject;
            clone10.Name = $"main back -{clone.Name}";
            clone10.Transform.SetTranslation(0, 35, -300);
            clone10.Transform.SetScale(1000, 120, 3);
            level.Add(clone10);
            #endregion

            #region Balcony Walls
            //back balcony wall
            var clone11 = archetypalWall.Clone() as GameObject;
            clone11.Name = $"balcony back -{clone.Name}";
            clone11.Transform.SetTranslation(0, 150, -400);
            clone11.Transform.SetScale(1200, 100, 3);
            level.Add(clone11);

            //left balcony wall
            var clone12 = archetypalWall.Clone() as GameObject;
            clone12.Name = $"balcony left -{clone.Name}";
            clone12.Transform.SetTranslation(-600, 150, -50);
            clone12.Transform.Rotate(0, 90, 0);
            clone12.Transform.SetScale(700, 100, 3);
            level.Add(clone12);
            #endregion

        }

        /// <summary>
        /// Add floors using quads
        /// </summary>
        /// <param name="activeScene"></param>
        private void InitializeFloors(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["floor"];
            material.Shader = new BasicShader();

            var archetypalQuad = new GameObject("floor", GameObjectType.Skybox);
            var renderer = new MeshRenderer();
            renderer.Material = material;
            archetypalQuad.AddComponent(renderer);
            renderer.Mesh = new QuadMesh();

            #endregion Archetype

            //Main floor
            GameObject clone = archetypalQuad.Clone() as GameObject;
            clone.Name = $"main floor -{clone.Name}";
            material.Texture = textureDictionary["floor"];
            clone.Transform.Translate(0, -25, 0);
            clone.Transform.Scale(1000, 600, 0);
            clone.Transform.Rotate(270, 0, 0);
            level.Add(clone);

            //Balcony left floor
            //GameObject clone1 = archetypalQuad.Clone() as GameObject;
            //clone1.Name = $"main floor -{clone.Name}";
            //material.Texture = textureDictionary["floor"];
            //clone1.Transform.Translate(0, 120, 0);
            //clone1.Transform.Scale(100, 25, 0);
            //clone1.Transform.Rotate(270, 0, 0);
            //level.Add(clone1);
        }

        /// <summary>
        /// Set application data, input, title and scene manager
        /// </summary>
        private void InitializeEngine(string gameTitle, int width, int height)
        {
            //set game title
            Window.Title = gameTitle;

            //instanciate scene manager to store all scenes
            sceneManager = new SceneManager(this);

            //initialize global application data
            Application.Main = this;
            Application.Content = Content;
            Application.GraphicsDevice = _graphics.GraphicsDevice; //TODO - is this necessary?
            Application.GraphicsDeviceManager = _graphics;
            Application.SceneManager = sceneManager;

            //instanciate render manager to render all drawn game objects using preferred renderer (e.g. forward, backward)
            renderManager = new RenderManager(this, new ForwardRenderer());

            //instanciate screen (singleton) and set resolution etc
            Screen.GetInstance().Set(width, height, true, false);

            //instanciate input components and store reference in Input for global access
            Input.Keys = new KeyboardComponent(this);
            Input.Mouse = new MouseComponent(this);
            Input.Gamepad = new GamepadComponent(this);

            //add all input components to component list so that they will be updated and/or drawn
            //Q. what would happen is we commented out these lines?
            Components.Add(sceneManager); //add so SceneManager::Update() will be called
            Components.Add(renderManager); //add so RenderManager::Draw() will be called
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
            font = Content.Load<SpriteFont>("Assets/Fonts/Arial");
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

#if DEMO
            gun.Update();
            //DemoFind();
            fps.Update(gameTime);
#endif
        }

#if DEMO

        private void DemoFind()
        {
            /*
            //lets look for an object - note - we can ONLY look for object AFTER SceneManager::Update has been called
            if (cObject == null)
                cObject = sceneManager.Find(gameObject => gameObject.Name.Equals("Clone - cube - 2"));

            //the ? is short for (if cObject != null) then...

            cObject?.Transform.Rotate(0,
                Time.Instance.UnscaledDeltaTimeMs * 3 / 60.0f, 0);
            */
        }

#endif

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            fps.DrawFps(_spriteBatch, font, new Vector2(10f, 10f), Color.MonoGameOrange);
            _spriteBatch.End();


            base.Draw(gameTime);
            playerUI.DrawUI(gameTime);
        }

        #endregion Update & Draw

#if DEMO

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

    }
}