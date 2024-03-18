using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cowsins;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace cowsins
{
    public class ProjectileManager : NetworkBehaviour
    {
        [Header("References")]
        public EventManager events;
        public UIController uiController;
        public WeaponController weapon;
        
        [Header("Projectile")]
        public GameObject lightProjectile;
        public GameObject heavyProjectile;

        public async Task ShootProjectile()
        {
            Shoot();
            
            int i = 1;
            while (i < weapon.weapon.bulletsPerFire)
            {
                Invoke(nameof(Shoot), weapon.weapon.timeBetweenShots);
                
                i++;
            }
        }
        
        public void Shoot()
        {
            Debug.Log("Shooting");
            
            events.OnShoot.Invoke();

            if (weapon.resizeCrosshair && uiController.crosshair != null)
                uiController.crosshair.Resize(weapon.weapon.crosshairResize * 100);

            foreach (Transform p in weapon.firePoint)
            {
                Vector3 dir = CowsinsUtilities.GetSpreadDirection(weapon.weapon.spreadAmount, weapon.mainCamera);
                
                SpawnProjectileServerRpc(p.position, dir, weapon.weapon.projectileType, weapon.weapon.bulletDuration, weapon.weapon.damagePerBullet, weapon.weapon.speed);
            }
        }
        
        [ServerRpc]
        public void SpawnProjectileServerRpc(Vector3 firePoints, Vector3 dir, string projectileType, float bulletDuration, float damagePerBullet, float speed)
        {
            GameObject projectileGO = null;

            switch (projectileType)
            {
                case "light":
                    projectileGO = this.lightProjectile;
                    break;
                case "heavy":
                    projectileGO = this.heavyProjectile;
                    break;
            }
            
            GameObject projectile =
                Instantiate(projectileGO, firePoints, Quaternion.identity);
            projectile.AddComponent<Rigidbody>();
            projectile.AddComponent<BoxCollider>().center = new Vector3(0, 0, 0);
            projectile.GetComponent<BoxCollider>().isTrigger = true;
            projectile.AddComponent<Projectile>();

            ServerManager.Spawn(projectile);
            
            projectile.GetComponent<Projectile>().bulletLife = bulletDuration;
            projectile.GetComponent<Projectile>().projectileDamage = damagePerBullet;
            projectile.GetComponent<Projectile>().speed = speed;
            projectile.GetComponent<Projectile>().dir = dir;
        }
    }
}