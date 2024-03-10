using System.Collections;
using System.Collections.Generic;
using cowsins;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace cowsins
{
    public class ProjectileManager : NetworkBehaviour
    {
        public EventManager events;
        public UIController uiController;
        public WeaponController weapon;

        public void Shoot()
        {
            events.OnShoot.Invoke();

            Debug.Log("3");

            if (weapon.resizeCrosshair && uiController.crosshair != null)
                uiController.crosshair.Resize(weapon.weapon.crosshairResize * 100);

            Vector3 dir = CowsinsUtilities.GetSpreadDirection(weapon.weapon.spreadAmount, weapon.mainCamera);

            foreach (var p in weapon.firePoint)
            {
                GameObject projectile =
                    Instantiate(weapon.weapon.projectile.gameObject, p.position, p.transform.rotation);
                projectile.AddComponent<NetworkObject>();
                projectile.AddComponent<Rigidbody>().velocity = dir * weapon.weapon.speed;
                projectile.AddComponent<Projectile>().projectileDamage =
                    weapon.damagePerBullet * weapon.stats.damageMultiplier;
                projectile.AddComponent<Projectile>().bulletLife = weapon.weapon.bulletDuration;
                projectile.AddComponent<BoxCollider>().center = new Vector3(0, 0, 0);

                projectile.SetActive(true);

                InstanceFinder.ServerManager.Spawn(projectile);

                Debug.Log("4");

                projectile.SetActive(true);
            }
        }
    }
}