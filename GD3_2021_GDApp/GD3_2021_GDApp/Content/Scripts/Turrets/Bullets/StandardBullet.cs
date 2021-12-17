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
        public float speed = 0.005f;

        public override void Update()
        {
            base.Update();
            Vector3 movement = this.Transform.Forward * speed * Time.Instance.DeltaTimeMs;
            this.Transform.Translate(movement);
        }
    }
}
