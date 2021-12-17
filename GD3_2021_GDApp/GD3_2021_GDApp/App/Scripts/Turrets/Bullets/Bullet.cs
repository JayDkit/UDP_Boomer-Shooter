using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Graphics;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Turrets.Bullets
{
    public class Bullet : Component
    {
        
        Model bulletMesh = Application.Main.Content.Load<Model>("Assets/Models/sphere");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/red");

        int damage = 1;
        int lifeTime = 10 * 16;

        public Collider collider;

        public Bullet()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            lifeTime--;
            if (lifeTime <= 0)
            {
                object[] parameters = { GameObject };
                EventDispatcher.Raise(new EventData(EventCategoryType.GameObject, EventActionType.OnRemoveObject, parameters));
            }
        }
        
        public void InitializeModel()
        {
            ModelRenderer modelRenderer = new ModelRenderer(bulletMesh, new BasicMaterial("turret_material", shader, texture));
            gameObject.AddComponent(modelRenderer);
            //gameObject.Transform.SetScale(0.001f, 0.001f, 0.001f);

        }

    }
}
