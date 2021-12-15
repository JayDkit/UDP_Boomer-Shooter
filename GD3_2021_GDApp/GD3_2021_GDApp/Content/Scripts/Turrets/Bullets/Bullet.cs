using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Turrets.Bullets
{
    public class Bullet : GameObject
    {

        public void InitializeModel(Scene level)
        {
            var material = new BasicMaterial("model material", new BasicShader(Application.Content), Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey"));

            var renderer = new ModelRenderer(Application.Main.Content.Load<Model>("Assets/Models/sphere"), material);
            this.AddComponent(renderer);
        }

    }
}
