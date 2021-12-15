using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Turrets
{
    public class Turret : Component
    {

        public GDLibrary.Managers.Cue shotSound = new GDLibrary.Managers.Cue("shotSound", Application.Main.Content.Load<SoundEffect>("Assets/Sounds/Turret_Shooting_Short")
                                                                         , SoundCategoryType.SoundByte, new Vector3(1f, 1f, 0.3f), false);
        public AudioEmitter soundEmitter = new AudioEmitter();
        Model turretMesh = Application.Main.Content.Load<Model>("Assets/Models/Turret");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");
        public void InitializeModel()
        {

            GameObject.AddComponent(new ModelRenderer(turretMesh, new BasicMaterial("turret_material", shader, texture)));

            //gun.Transform.SetScale(0.5f, 0.5f, 0.5f);
            //gun.Transform.SetRotation(0, -90, 0);

            Application.SoundManager.Add(shotSound);
            soundEmitter.Position = GameObject.Transform.LocalTranslation;
        }

        public override void Update()
        {

        }
    }
}
