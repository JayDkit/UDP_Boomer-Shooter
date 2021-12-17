using GDApp.Content.Scripts.Turrets.Bullets;
using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Graphics;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.App.Scripts.Player
{
    public class PlayerGun : Component
    {

        AudioEmitter soundEmitter = new AudioEmitter();
        GDLibrary.Managers.Cue shotSound = new GDLibrary.Managers.Cue("PlayerShot", Application.Main.Content.Load<SoundEffect>("Assets/Sounds/Shotgun_Blast")
                                                                         , SoundCategoryType.SoundByte, new Vector3(1f, 1f, 1f), false);

        Model gunMesh = Application.Main.Content.Load<Model>("Assets/Models/Guns/PlayerGun");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Textures/Props/Shotgun_Texture");

        GameObject shot;

        public PlayerGun()
        {
            shot = new GameObject("PlayerShot",GameObjectType.PlayerBullet);
            StandardBullet bulletScript = new StandardBullet(10f);
            shot.AddComponent(bulletScript);
            
            bulletScript.InitializeModel();

            Application.SoundManager.Add(shotSound);

            //shot.Transform.SetScale(0.001f, 0.001f, 0.001f);

            //shot.Transform.SetScale(110f, 110f, 110f);

        }

        public override void Update()
        {
            base.Update();

            if (Input.Mouse.WasJustClicked(GDLibrary.Inputs.MouseButton.Left))
            {
                shoot();
                //InstantiateFunctions.InstantiateGameObject(shot,Camera.Main.transform.LocalTranslation, Camera.Main.transform.LocalRotation);
            }

            setTranslationFromParent();
            soundEmitter.Position = gameObject.Transform.LocalTranslation;
        }

        private void shoot()
        {
            GameObject temp = InstantiateFunctions.InstantiateGameObject(shot, Camera.Main.transform.LocalTranslation, Camera.Main.transform.LocalRotation) as GameObject;

            temp.Transform.SetScale(0.1f, 0.1f, 0.1f);

            Collider colliderToAdd = new Collider(false, true);
            temp.AddComponent(colliderToAdd);
            colliderToAdd.AddPrimitive(new Sphere(
                temp.Transform.LocalTranslation,
                temp.Transform.LocalScale.X),
                new MaterialProperties(0, 0, 0)
                );
            colliderToAdd.Enable(false, 1);

            temp.GetComponent<StandardBullet>().collider = colliderToAdd;

            Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
        }

        public void InitializeModel()
        {
            gameObject.AddComponent(new ModelRenderer(gunMesh, new BasicMaterial("speed_material", shader, texture)));
            setTranslationFromParent();
            gameObject.Transform.SetScale(0.5f, 0.5f, 0.5f);
            gameObject.Transform.SetRotation(0,-90,0);
        }

        private void setTranslationFromParent()
        {
            //System.Diagnostics.Debug.WriteLine("Location: "+ Camera.Main.Transform.LocalTranslation);
            //gun.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(0, -3, -3));
            gameObject.Transform.SetTranslation(Camera.Main.Transform.WorldMatrix.Translation + new Vector3(1f,-3.3f, -3));
            //gun.Transform.SetRotation(Camera.Main.Transform.LocalRotation  + new Vector3(0, -90, 0));
            //new Vector3(MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X,-15,15))
            gameObject.Transform.SetRotation(new Vector3(MathHelper.Clamp(-Camera.Main.Transform.LocalRotation.X, -15, 15), -90 + Camera.Main.Transform.LocalRotation.Y, 0));
            //this.Transform.Rotate(0, -90 + Camera.Main.Transform.LocalRotation.Y, 0);
        }
    }
}
