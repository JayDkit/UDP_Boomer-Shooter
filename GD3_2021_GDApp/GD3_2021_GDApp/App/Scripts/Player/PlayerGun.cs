using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.App.Scripts.Player
{
    public class PlayerGun : GameObject
    {

        Model gunMesh = Application.Main.Content.Load<Model>("Assets/Models/Guns/PlayerGun");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Textures/Props/Shotgun_Texture");

        Scene scene;

        public PlayerGun(Scene scene) : base("Gun", GameObjectType.Player, false) //Change type
        {
            this.scene = scene;
        }

        public override void Update()
        {
            base.Update();

            if (Input.Mouse.WasJustClicked(GDLibrary.Inputs.MouseButton.Left))
            {
                var bullet = new Bullet(Camera.Main.transform.LocalTranslation, Camera.Main.Forward);
                scene.Add(bullet);
            }

            setTranslationFromParent();
        }

        public void InitializeModel(Scene level)
        {
            this.AddComponent(new ModelRenderer(gunMesh, new BasicMaterial("speed_material", shader, texture)));
            setTranslationFromParent();
            this.Transform.SetScale(0.5f, 0.5f, 0.5f);
            this.Transform.SetRotation(0,-90,0);
        }

        private void setTranslationFromParent()
        {
            //System.Diagnostics.Debug.WriteLine("Location: "+ Camera.Main.Transform.LocalTranslation);
            //gun.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0, -3, -3));
            this.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(1f,-3.3f, -3));
            //gun.Transform.SetRotation(Camera.Main.Transform.LocalRotation  + new Vector3(0, -90, 0));
            //new Vector3(MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X,-15,15))
            this.Transform.SetRotation(new Vector3(MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X, -15, 15), -90 + Camera.Main.Transform.LocalRotation.Y, 0));
            //this.Transform.Rotate(0, -90 + Camera.Main.Transform.LocalRotation.Y, 0);
        }
    }
}
