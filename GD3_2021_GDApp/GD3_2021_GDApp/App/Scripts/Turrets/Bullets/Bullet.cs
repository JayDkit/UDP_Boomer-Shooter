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
        public Bullet() : base("Bullet", GameObjectType.Player, true) //Change type
        {
        }

        Model bulletMesh = Application.Main.Content.Load<Model>("Assets/Models/sphere");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
        public void InitializeModel(Scene level)
        {
            this.AddComponent(new ModelRenderer(bulletMesh, new BasicMaterial("turret_material", shader, texture)));

        }

    }
}
