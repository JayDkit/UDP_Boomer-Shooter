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

        public override void Update()
        {
            base.Update();
            if(count % 120 == 0)
            {
                Shoot();
            }
            
            count++;
        }

        public void Shoot()
        {
            Application.SoundManager.Play3D(shotSound.ID, Application.playerListener, soundEmitter);
            InsantiateFunctionExtensions.InstantiateGameObject(bulletPrefab, this.transform.LocalTranslation, this.transform.LocalRotation);
        }
    }
}
