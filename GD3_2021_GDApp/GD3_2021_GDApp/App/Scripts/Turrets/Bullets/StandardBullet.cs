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

        public override object Clone()
        {
            var clone = new StandardBullet();
            clone.ID = "GO-" + Guid.NewGuid();

            Component clonedComponent = null;
            Transform clonedTransform = null;

            foreach (Component component in components)
            {
                clonedComponent = clone.AddComponent((Component)component.Clone());
                clonedComponent.gameObject = clone;

                clonedTransform = clonedComponent as Transform;

                if (clonedTransform != null)
                    clonedComponent.transform = clonedTransform;
            }

            return clone;
        }
    }
}
