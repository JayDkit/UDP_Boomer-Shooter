using GDLibrary;
using Microsoft.Xna.Framework;

namespace GDApp.Content.Scripts.Turrets.Bullets
{
    internal class AcceleratingBullet : Bullet
    {
        public float initialSpeed = 10f;
        public float acceleration = 1f;
        public float maxSpeed = 25f;
        public float speed;

        public void Start()
        {
            speed = initialSpeed;
        }

        public override void Update()
        {
            base.Update();
            Vector3 movement = this.Transform.Forward * speed * Time.Instance.DeltaTimeMs;
            this.Transform.Translate(movement);

            if(speed < maxSpeed)
            {
                speed += acceleration * Time.Instance.DeltaTimeMs;
            }
            if(speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            
        }
    }
}
