using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDApp.Content.Scripts.Turrets.Bullets
{
    public class StandardBullet : Bullet
    {
        public float speed = 1f;

        public StandardBullet() { }
        public StandardBullet(float speedPar)
        {
           
            speed = speedPar;

        }

        public override void Update()
        {
            base.Update();
            Vector3 movement = gameObject.Transform.Forward * speed * Time.Instance.DeltaTimeMs;
            collider.Body.Velocity = movement;
        }
    }
}
