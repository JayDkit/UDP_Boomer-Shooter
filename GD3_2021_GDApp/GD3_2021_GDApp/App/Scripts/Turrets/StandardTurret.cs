using GDLibrary;
using Microsoft.Xna.Framework;
using GDLibrary.Core;
using System;
using GDApp.Content.Scripts.Turrets.Bullets;
using GDLibrary.Components;
using JigLibX.Geometry;
using JigLibX.Collision;

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

        public Transform player;

        public override void Start()
        {
            random = new Random();
        }

        public override void Update()
        {
            base.Update();
            if (count % 120 == 0 && Vector3.Distance(player.LocalTranslation,gameObject.Transform.LocalTranslation) <= 20)
            {
                Shoot();
            }

            if (soundCount != 0 && soundCooldown % 2 == 0)
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
            random = new Random();

            for (int x = 0; x < 2; x++)
            {
                shotRotation = GameObject.Transform.LocalRotation + new Vector3(0,180,0);
                int temp = random.Next(2);
                shotRotation += new Vector3(random.Next(-15, 15), random.Next(-15, 15), random.Next(-15, 15)); ;
                for (int y = 0; y < 10; y++)
                {
                    //Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
                    GameObject tempGameOBJ = InstantiateFunctions.InstantiateGameObject(bulletPrefab, this.transform.LocalTranslation + new Vector3(0, 2, 0),
                                tempRotation(new Vector3((float)(random.Next(-10, 10) * random.NextDouble()),
                                                        (float)(random.Next(-10, 10) * random.NextDouble()),
                                                        (float)(random.Next(-10, 10) * random.NextDouble())))) as GameObject;

                    tempGameOBJ.Transform.SetScale(0.1f, 0.1f, 0.1f);

                    Collider colliderToAdd = new Collider(false, true);
                    tempGameOBJ.AddComponent(colliderToAdd);
                    colliderToAdd.AddPrimitive(new Sphere(
                        tempGameOBJ.Transform.LocalTranslation,
                        tempGameOBJ.Transform.LocalScale.X),
                        new MaterialProperties(0, 0, 0)
                        );
                    colliderToAdd.Enable(false, 1);

                    tempGameOBJ.GetComponent<StandardBullet>().collider = colliderToAdd;
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
