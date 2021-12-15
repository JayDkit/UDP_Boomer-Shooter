using GDLibrary;
using Microsoft.Xna.Framework;
using GDLibrary.Core;
using System;
using GDApp.Content.Scripts.Turrets.Bullets;

namespace GDApp.Content.Scripts.Turrets
{
    public class StandardTurret : Turret
    {
        public GameObject bulletPrefab;
        public int count = 1;
        public Vector3 shotRotation;
        public Random random;
        private int soundCount = 0;
        private int soundCooldown = 2;

        public override void Start()
        {
            random = new Random();
        }

        public override void Update()
        {
            base.Update();
            if(count % 120 == 0)
            {
                Shoot();
            }
            
            if(soundCount != 0 && soundCooldown % 2 == 0)
            {
                Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
                soundCount--;
            }


            soundCooldown++;
            count++;
        }

        public void Shoot()
        {
            //Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
            //InsantiateFunctionExtensions.InstantiateGameObject(bulletPrefab, this.transform.LocalTranslation, this.transform.LocalRotation);

            //Pattern 1

            for (int x = 0; x < 5; x++)
            {
                shotRotation = GameObject.Transform.LocalRotation;
                int temp = random.Next(2);
                shotRotation += new Vector3(random.Next(-45,45), random.Next(-45, 45), random.Next(-45, 45)); ;
                for (int y = 0; y < 10; y++)
                {
                    //Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
                    InsantiateFunctionExtensions.InstantiateGameObject(bulletPrefab, this.transform.LocalTranslation + new Vector3(0,4,0),
                                tempRotation(new Vector3((float)(random.Next(-10, 10) * random.NextDouble()),
                                                        (float)(random.Next(-10, 10) * random.NextDouble()),
                                                        (float)(random.Next(-10, 10) * random.NextDouble()))));
                }
            }

            soundCount = 10;
        }

        private Vector3 tempRotation(Vector3 add)
        {
            Vector3 temp = shotRotation;
            temp += add;
            return temp;
        }
    }
}
