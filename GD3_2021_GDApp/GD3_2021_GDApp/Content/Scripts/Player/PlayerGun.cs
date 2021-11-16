using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Player
{
    public class PlayerGun : GameObject
    {
        Main main;
        private GameObject gun;

        public PlayerGun(Main main)
        {
            this.main = main;
        }

        public override void Update()
        {
            
            base.Update();
            setTranslationFromParent();
        }

        public void InitializeModel(Scene level)
        {
            var material = new BasicMaterial("model material");
            material.Texture = main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
            material.Shader = new BasicShader();

            gun = new GameObject("playergun", GameObjectType.Player);
            var renderer = new ModelRenderer();
            renderer.Material = material;
            gun.AddComponent(renderer);
            renderer.Model = main.Content.Load<Model>("Assets/Models/Guns/PlayerGun");
            setTranslationFromParent();
            gun.Transform.SetScale(0.5f, 0.5f, 0.5f);
            gun.Transform.SetRotation(0,-90,0);
            level.Add(gun);
        }

        private void setTranslationFromParent()
        {
            //System.Diagnostics.Debug.WriteLine("Location: "+ Camera.Main.Transform.LocalTranslation);
            //gun.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0, -3, -3));
            gun.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0,-3, MathF.Cos(transform.LocalRotation.Y) * 2 - 2));
            //gun.Transform.SetRotation(Camera.Main.Transform.LocalRotation  + new Vector3(0, -90, 0));
            gun.Transform.SetRotation(new Vector3(0, -90 + Camera.Main.Transform.LocalRotation.Y,MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X,-15,15)));
        }
    }
}
