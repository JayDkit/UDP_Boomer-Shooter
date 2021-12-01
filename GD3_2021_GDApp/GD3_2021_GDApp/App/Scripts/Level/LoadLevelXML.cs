using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GDApp.Content.Scripts.Level
{
    /// <summary>
    /// Load all the level placed objects data from a prefix data file (Level1.xml),
    /// that was generated using using the object data.
    /// To make life easier for the level designer
    /// </summary>
    [DataContract]
    public class LoadLevelXML
    {
        private Dictionary<string, Texture2D> textureDictionary;
        Scene level;

        private string type;
        private Vector3 localTranslation;
        private Vector3 localRotation;
        private Vector3 localScale;

        private GameObject archetypalWall;
        private GameObject archetypalFloor;
        private GameObject archetypalSpeedPickup;
        private GameObject archetypalHealthPickup;
        private GameObject archetypalTurret;

        QuadMesh quadMesh = new QuadMesh();
        CubeMesh cubeMesh = new CubeMesh();
        Model turretMesh = Application.Main.Content.Load<Model>("Assets/Models/Turret");
        Model healthMesh = Application.Main.Content.Load<Model>("Assets/Models/Pickups/HealthKit");
        Model speedMesh = Application.Main.Content.Load<Model>("Assets/Models/Pickups/SpeedPickup");
        BasicShader shader = new BasicShader(Application.Content, false, true);

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [DataMember]
        public Vector3 LocalTranslation
        {
            get { return localTranslation; }
            set { localTranslation = value; }
        }

        [DataMember]
        public Vector3 LocalRotation
        {
            get { return localRotation; }
            set { localRotation = value; }
        }

        [DataMember]
        public Vector3 LocalScale
        {
            get { return localScale; }
            set { localScale = value; }
        }


        public LoadLevelXML() : this("none", Vector3.Zero, Vector3.Zero, Vector3.One)
        {
            textureDictionary = new Dictionary<string, Texture2D>();
            LoadTextures();
            preLoadAllTypesOfModels();
        }

        public LoadLevelXML(Scene level) : this("none", Vector3.Zero, Vector3.Zero, Vector3.One)
        {
            textureDictionary = new Dictionary<string, Texture2D>();
            this.level = level;
            LoadTextures();
            preLoadAllTypesOfModels();
        }

        public LoadLevelXML(string type, Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            this.type = type;
            localTranslation = translation;
            localRotation = rotation;
            localScale = scale;
        }
      
        public void LoadTextures()
        {
            //walls
            textureDictionary.Add("brick", Application.Main.Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/brick"));
            textureDictionary.Add("floor", Application.Main.Content.Load<Texture2D>("Assets/Textures/Architecture/Floors/TarmacTexture"));

            //Turrets + Pickups (Non-Props)
            textureDictionary.Add("speed_pickup", Application.Main.Content.Load<Texture2D>("Assets/Textures/Pickups/ElectricityTexture"));
            textureDictionary.Add("health_pickup", Application.Main.Content.Load<Texture2D>("Assets/Textures/Pickups/HealthKitTexture"));
            textureDictionary.Add("rapidFire_pickup", Application.Main.Content.Load<Texture2D>("Assets/Textures/Pickups/BrassTexture"));
            textureDictionary.Add("turret", Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey"));
        }

        public void setGroundFloor() //Setting the ground floor manually
        {
            var floor = new GameObject("floor", GameObjectType.Environment, false);
            floor.Transform.Translate(0, 0, -40);
            floor.Transform.Scale(100, 95, 0);
            floor.Transform.Rotate(-90, 0, 0);
            floor.AddComponent(new MeshRenderer(quadMesh, new BasicMaterial("groundfloor_material", shader, textureDictionary["floor"])));
            level.Add(floor);
        }

        private void preLoadAllTypesOfModels()
        {
            //Floor
            archetypalFloor = new GameObject("floor", GameObjectType.Environment, true);
            archetypalFloor.AddComponent(new MeshRenderer(cubeMesh, new BasicMaterial("floor_material", shader, textureDictionary["turret"]))); //Texture needs to be changed to be changed
            //Wall
            archetypalWall = new GameObject("wall", GameObjectType.Environment, true);
            archetypalWall.AddComponent(new MeshRenderer(cubeMesh, new BasicMaterial("wall_material", shader, textureDictionary["brick"])));
            //Turret
            archetypalTurret = new GameObject("turret", GameObjectType.NPC, true);
            archetypalTurret.AddComponent(new ModelRenderer(turretMesh, new BasicMaterial("turret_material", shader, textureDictionary["turret"]))); //Placeholder texture - not the final one!
            //Pickups - HEALTH
            archetypalHealthPickup = new GameObject("health_pickup", GameObjectType.Consumable, true);
            archetypalHealthPickup.AddComponent(new ModelRenderer(healthMesh, new BasicMaterial("health_material", shader, textureDictionary["health_pickup"])));
            //Pickups - SPEEDUP
            archetypalSpeedPickup = new GameObject("speed_pickup", GameObjectType.Consumable, true);
            archetypalSpeedPickup.AddComponent(new ModelRenderer(speedMesh, new BasicMaterial("speed_material", shader, textureDictionary["speed_pickup"])));
        }

        private void InitializeModel(int id, string type, Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            switch (type)
            {
                case "Wall":
                    var cloneWall = archetypalWall.Clone() as GameObject;
                    cloneWall.Name = $"{type}-{id}";
                    cloneWall.Transform.Transform.SetTranslation(translation);
                    cloneWall.Transform.Transform.SetRotation(rotation);
                    cloneWall.Transform.SetScale(scale);
                    level.Add(cloneWall);
                    break;
                case "Turret":
                    var cloneTurret = archetypalTurret.Clone() as GameObject;
                    cloneTurret.Name = $"{type}-{id}";
                    cloneTurret.Transform.Transform.SetTranslation(translation);
                    cloneTurret.Transform.Transform.SetRotation(rotation);
                    cloneTurret.Transform.SetScale(scale);
                    level.Add(cloneTurret);
                    break;
                case "Floor":
                    var cloneFloor = archetypalFloor.Clone() as GameObject;
                    cloneFloor.Name = $"{type}-{id}";
                    cloneFloor.Transform.Transform.SetTranslation(translation);
                    cloneFloor.Transform.Transform.SetRotation(rotation);
                    cloneFloor.Transform.SetScale(scale);
                    level.Add(cloneFloor);
                    break;
                case "Pickup-Health":
                    var cloneHealth = archetypalHealthPickup.Clone() as GameObject;
                    cloneHealth.Name = $"{type}-{id}";
                    cloneHealth.Transform.Transform.SetTranslation(translation);
                    cloneHealth.Transform.Transform.SetRotation(rotation);
                    cloneHealth.Transform.SetScale(scale);
                    level.Add(cloneHealth);
                    break;
                case "Pickup-Speed":
                    var cloneSpeed= archetypalSpeedPickup.Clone() as GameObject;
                    cloneSpeed.Name = $"{type}-{id}";
                    cloneSpeed.Transform.Transform.SetTranslation(translation);
                    cloneSpeed.Transform.Transform.SetRotation(rotation);
                    cloneSpeed.Transform.SetScale(scale);
                    level.Add(cloneSpeed);
                    break;
                default:
                    break;
            }

        }

        public void LoadLevelFromXML()
        {
            //Path is like this so that the file can be place properly in the asset folders.
            var readList = GDLibrary.Utilities.SerializationUtility.Load("../../../Content/Assets/Data/Level1.xml", typeof(List<LoadLevelXML>)) as List<LoadLevelXML>; 
            //System.Diagnostics.Debug.WriteLine("HERE WAE HAVE: " + readList[0].type + " - " + readList[0].localTranslation);
            for (int i = 0; i < readList.Count; i++)
            {
                InitializeModel(i, readList[i].type, readList[i].localTranslation, readList[i].localRotation, readList[i].localScale);
            }
        }

    }
}
