using System.Collections;
using System.Collections.Generic;
using cowsins;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileManager : NetworkBehaviour
{
    public EventManager events;
    public UIController uiController;
    public WeaponController weapon;

    public Camera mainCamera;

    public void Shoot()
    {
        events.OnShoot.Invoke();

        if (weapon.resizeCrosshair && uiController.crosshair != null)
            uiController.crosshair.Resize(weapon.weapon.crosshairResize * 100);

        Transform hitObj;

        //This defines the first hit on the object
        Vector3 dir = CowsinsUtilities.GetSpreadDirection(weapon.weapon.spreadAmount, mainCamera);
        Ray ray = new Ray(mainCamera.transform.position, dir);

        if (Physics.Raycast(ray, out weapon.hit, weapon.weapon.bulletRange, weapon.hitLayer))
        {
            float dmg = weapon.damagePerBullet * weapon.stats.damageMultiplier;
            weapon.Hit(weapon.hit.collider.gameObject.layer, dmg, weapon.hit, true);
            hitObj = weapon.hit.collider.transform;

            //Handle Penetration
            Ray newRay = new Ray(weapon.hit.point, ray.direction);
            RaycastHit newHit;

            if (Physics.Raycast(newRay, out newHit, weapon.penetrationAmount, weapon.hitLayer))
            {
                if (hitObj != newHit.collider.transform)
                {
                    float dmg_ = weapon.damagePerBullet * weapon.stats.damageMultiplier * weapon.weapon.penetrationDamageReduction;
                    weapon.Hit(newHit.collider.gameObject.layer, dmg_, newHit, true);
                }
            }

            // Handle Bullet Trails
            if (weapon.weapon.bulletTrail == null) return;

            foreach (var p in weapon.firePoint)
            {
                TrailRenderer trail = Instantiate(weapon.weapon.bulletTrail, p.position, Quaternion.identity);

                StartCoroutine(weapon.SpawnTrail(trail, weapon.hit));
            }
        }
    }
}
