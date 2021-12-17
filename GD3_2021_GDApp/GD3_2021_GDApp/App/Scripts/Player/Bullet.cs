using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;
using GDLibrary.Graphics;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp.App.Scripts.Player
{
    class Bullet : GameObject
    {
        Model bulletMesh = Application.Main.Content.Load<Model>("Assets/Models/sphere");
        BasicShader shader = new BasicShader(Application.Content, false, true);
        Texture2D texture = Application.Main.Content.Load<Texture2D>("Assets/Demo/Textures/grey");

        Collider collider;

        public float speed = 0.1f;
        public int damage = 1;
        public int lifeTime = 6 * 16;

        Vector3 forwardCamera;
        public Bullet(Vector3 position, Vector3 forward) : base("Bullet", GameObjectType.Bullet, true)
        {
            this.forwardCamera = forward;
            this.AddComponent(new ModelRenderer(bulletMesh, new BasicMaterial("turret_material", shader, texture)));
            this.Transform.SetTranslation(position);
            this.Transform.SetScale(0.1f, 0.1f, 0.1f);
            
            collider = new Collider();
            this.AddComponent(collider);
            collider.AddPrimitive(new Sphere(
                this.Transform.LocalTranslation,
                this.Transform.LocalRotation.X / 2.0f),
                new MaterialProperties(0, 0, 0)
                );
            collider.Enable(false, 1);
            
        }

        public override void Update()
        {
            base.Update();
            var temp = new Vector3(-forwardCamera.X, -forwardCamera.Y, forwardCamera.Z);
            Vector3 movement = temp * speed * Time.Instance.DeltaTimeMs;
            this.Transform.Translate(movement);

                lifeTime--;
                if (lifeTime <= 0)
                {
                object[] parameters = { this };
                EventDispatcher.Raise(new EventData(EventCategoryType.GameObject, EventActionType.OnRemoveObject, parameters));
                }
        }
    }
}
