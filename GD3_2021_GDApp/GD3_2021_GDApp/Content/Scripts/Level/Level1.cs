using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDApp.Content.Scripts.Level
{
    public class Level1
    {
        private Dictionary<string, Texture2D> textureDictionary;
        private ContentManager Content;

        public Level1(ContentManager Content)
        {
            textureDictionary = new Dictionary<string, Texture2D>();
            this.Content = Content;
        }

        public void LoadTextures()
        {
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
        /// Add floors using quads
        /// </summary>
        /// <param name="activeScene"></param>
        public void InitializeFloors(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["floor"];
            material.Shader = new BasicShader(Application.Content);

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

        public void InitializeWalls(Scene level)
        {
            #region Archetype

            var material = new BasicMaterial("simple diffuse");
            material.Texture = textureDictionary["brick"];
            material.Shader = new BasicShader(Application.Content);

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

        public void InitializePickups(Scene level)
        {
            #region Archetype

            //Speed Pickup (represented by lightning bolt) - Commented out lines are to create Rapid-Fire pickup
            //(represented by a bullet), both are included so they can be swapped between for demonstration purposes
            //for CA2.
            var speedMaterial = new BasicMaterial("model material");
            speedMaterial.Texture = Content.Load<Texture2D>("Assets/Textures/Pickups/ElectricityTexture");
            //speedMaterial.Texture = Content.Load<Texture2D>("Assets/Textures/Pickups/BrassTexture");
            speedMaterial.Shader = new BasicShader(Application.Content);

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
            healthMaterial.Shader = new BasicShader(Application.Content);

            var healthPickup = new GameObject("health_pickup", GameObjectType.Consumable);
            var healthRenderer = new ModelRenderer();
            healthRenderer.Material = healthMaterial;
            healthPickup.AddComponent(healthRenderer);
            healthRenderer.Model = Content.Load<Model>("Assets/Models/Pickups/HealthKit");
            //healthRenderer.Model = Content.Load<Model>("Assets/Models/Pickups/HealthKit");
            healthPickup.Transform.SetTranslation(65, -22, -60);
            healthPickup.Transform.SetScale(5f, 5f, 5f);
            level.Add(healthPickup);

            #endregion Archetype

            var count = 0;
            for (var i = 0; i <= 1; i += 1)
            {
                var clone = healthPickup.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                if (i == 0)
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

        public void InitializeTurrets(Scene level)
        {
            var turretMaterial = new BasicMaterial("model material");
            //Placeholder texture - not the final one!
            turretMaterial.Texture = Content.Load<Texture2D>("Assets/Demo/Textures/grey");
            turretMaterial.Shader = new BasicShader(Application.Content);

            var turret = new GameObject("turret", GameObjectType.NPC);
            var turretRenderer = new ModelRenderer();
            turretRenderer.Material = turretMaterial;
            turret.AddComponent(turretRenderer);
            turretRenderer.Model = Content.Load<Model>("Assets/Models/Turret");
            //Far Left in Center Room
            turret.Transform.SetTranslation(-80, -25, -15);
            turret.Transform.SetRotation(0, 90, 0);
            turret.Transform.SetScale(3f, 3f, 3f);
            level.Add(turret);

            var count = 0;
            for (var i = 0; i <= 11; i += 1)
            {
                var clone = turret.Clone() as GameObject;
                clone.Name = $"{clone.Name} - {count++}";
                //Far Right in Center Room
                if (i == 0)
                {
                    clone.Transform.SetTranslation(210, -25, -75);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }

                #region Entrance To Center Room Turrets
                //Right Entrance to Center Room
                else if (i == 1)
                {
                    clone.Transform.SetTranslation(105, -25, 15);
                    clone.Transform.SetScale(3, 3f, 3f);
                }
                //Left Entrance to Center Room
                else if (i == 2)
                {
                    clone.Transform.SetTranslation(25, -25, 15);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                #endregion

                #region Far Right Hall Turrets
                //Far Right hall, Top Right
                else if (i == 3)
                {
                    clone.Transform.SetTranslation(490, -25, -100);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Right hall, Top Left
                else if (i == 4)
                {
                    clone.Transform.SetTranslation(240, -25, -175);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                #endregion

                #region Far Left Hall Turrets
                //Far Left Hall, Mid Right
                else if (i == 5)
                {
                    clone.Transform.SetTranslation(-105, -25, -75);
                    clone.Transform.SetRotation(0, -90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Left Hall, Bottom Left
                else if (i == 6)
                {
                    clone.Transform.SetTranslation(-480, -25, 80);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Far Left Hall, Top Left
                else if (i == 7)
                {
                    clone.Transform.SetTranslation(-480, -25, -200);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                #endregion

                #region Balcony Turrets
                //Left Balcony, Top Left
                else if (i == 8)
                {
                    clone.Transform.SetTranslation(-540, 95, -260);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }
                //Left Balcony, Bottom Left
                else if (i == 9)
                {
                    clone.Transform.SetTranslation(-540, 95, 20);
                    clone.Transform.SetRotation(0, 90, 0);
                    clone.Transform.SetScale(3f, 3f, 3f);
                }

                //Back Balcony, Left Side
                else if (i == 10)
                {
                    clone.Transform.SetTranslation(-30, 95, -360);
                    clone.Transform.SetScale(3, 3f, 3f);
                }

                //Back Balcony, Left Side
                else if (i == 11)
                {
                    clone.Transform.SetTranslation(160, 95, -360);
                    clone.Transform.SetScale(3, 3f, 3f);
                }
                #endregion

                level.Add(clone);
            }
        }
    }
}
