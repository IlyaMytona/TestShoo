﻿using Test.Interface;
using UnityEngine;
using Test.GameServices;
using Test.Controllers.TimeRemainings;


namespace Test.Controllers
{
    public sealed class AmmunitionLifeCycleController : BaseController, IExecute
    { 
        public void Execute()
        {
            var deltaTime = Time.deltaTime * Time.timeScale;

            foreach (var bullet in Services.Instance.LevelService.BulletAmmunitions)
            {
                if (!bullet.IsActive)
                {
                    continue;
                }
                bullet.Transform.Translate(Vector3.forward * (deltaTime * bullet.Force));
                bullet.TimeRemaining.AddTimeRemaining(bullet.TimeToDestruct);
                
            }

            foreach (var fractionBullet in Services.Instance.LevelService.FractionBulletAmmunitions)
            {
                if (!fractionBullet.IsActive)
                {
                    continue;
                }

                fractionBullet.Transform.Translate(Vector3.forward * (deltaTime * fractionBullet.Force));
                fractionBullet.TimeRemaining.AddTimeRemaining(fractionBullet.TimeToDestruct);
            }
        }
    }
}
