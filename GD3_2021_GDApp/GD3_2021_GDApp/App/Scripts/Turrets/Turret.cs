using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Turrets
{
    public class Turret : GameObject
    {
        public Turret() : base("Turret", GameObjectType.NPC, true) //Change type
        {
        }

        Model turretMesh = Application.Main.Content.Load<Model>("Assets/Models/Turret");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
        public void InitializeModel(Scene level)
        {
            this.AddComponent(new ModelRenderer(turretMesh, new BasicMaterial("turret_material", shader, texture)));

            //gun.Transform.SetScale(0.5f, 0.5f, 0.5f);
            //gun.Transform.SetRotation(0, -90, 0);
        }

        public override void Update()
        {

        }
    }
}
