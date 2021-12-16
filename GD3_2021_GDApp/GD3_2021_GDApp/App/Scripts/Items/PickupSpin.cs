using GDLibrary;
using GDLibrary.Components;
using System;

namespace GDApp.App.Scripts
{
    public class PickupSpin : Component
    {
        public PickupSpin()
        {

        }

        public override void Awake(GameObject gameObject)
        {
            base.Awake(gameObject);
            transform = gameObject.Transform;
        }
        int i = 0;
        public override void Update()
        {
            if (i >= 360)
            {
                i = 0;
            }
            transform.SetRotation(0, i++, 0);
        }

    }
}
