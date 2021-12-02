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

        public override void Update()
        {
            
            base.Update();
            setTranslationFromParent();
        }

        public void InitializeModel(Scene level)
        {
            var material = new BasicMaterial("model material");
            material.Texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
            material.Shader = new BasicShader(Application.Content);
            var renderer = new ModelRenderer();
            renderer.Material = material;
            renderer.Model = Application.Main.Content.Load<Model>("Assets/Models/Guns/PlayerGun"); //Comment this
            //renderer.Model = Application.Main.Content.Load<Model>("Assets/Models/cube");
            this.AddComponent(renderer);
            setTranslationFromParent();
            this.Transform.SetScale(0.5f, 0.5f, 0.5f);
            this.Transform.SetRotation(0,-90,0);
        }

        private void setTranslationFromParent()
        {
            //System.Diagnostics.Debug.WriteLine("Location: "+ Camera.Main.Transform.LocalTranslation);
            //gun.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0, -3, -3));
            this.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0,-3, 0));
            //gun.Transform.SetRotation(Camera.Main.Transform.LocalRotation  + new Vector3(0, -90, 0));
            this.Transform.SetRotation(new Vector3(0, -90 + Camera.Main.Transform.LocalRotation.Y,MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X,-15,15)));
        }
    }
}
