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
            var material = new BasicMaterial("model material", new BasicShader(Application.Content), Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey"));
            var renderer = new ModelRenderer(Application.Main.Content.Load<Model>("Assets/Models/cube"), material);
            this.AddComponent(renderer);
            //gun.Transform.SetScale(0.5f, 0.5f, 0.5f);
            //gun.Transform.SetRotation(0, -90, 0);
        }

        public override void Update()
        {

        }
    }
}
