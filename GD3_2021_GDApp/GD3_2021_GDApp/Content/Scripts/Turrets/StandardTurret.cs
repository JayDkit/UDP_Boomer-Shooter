using GDLibrary;
using Microsoft.Xna.Framework;
using GDLibrary.Core;
using System;
using GDApp.Content.Scripts.Turrets.Bullets;

namespace GDApp.Content.Scripts.Turrets
{
    public class StandardTurret : Turret
    {
        public StandardBullet bulletPrefab;
        public int count = 1;

        public override void Update()
        {
            base.Update();
            if(count % 60 == 0)
            {
                Shoot();
            }
            
            count++;
        }

        public void Shoot()
        {
            InsantiateFunctionExtensions.InstantiateGameObject(bulletPrefab, this.transform.LocalTranslation, this.transform.LocalRotation);
        }
    }
}
