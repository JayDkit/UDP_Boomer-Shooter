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

        public void InitializeModel(Scene level)
        {
            var material = new BasicMaterial("model material");
            material.Texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
            material.Shader = new BasicShader();
            var renderer = new ModelRenderer();
            renderer.Material = material;
            this.AddComponent(renderer);
            renderer.Model = Application.Main.Content.Load<Model>("Assets/Models/cube");
            //gun.Transform.SetScale(0.5f, 0.5f, 0.5f);
            //gun.Transform.SetRotation(0, -90, 0);
        }

        public override void Update()
        {

        }
    }
}
