using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using cowsins;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    public CameraEffects cameraEffects;
    public AudioSource audioSource;
    
    [Header("Scriptable Objects")] 
    public Weapon_SO[] weapons;
    public Projectile_SO[] projectiles;

    [Header("Controls")] 
    public PlayerInput playerInput;
    private InputAction fire;
    private InputAction reload;
    private InputAction aim;
    
    [Header("Private Variables")]
    private bool isFiring;
    private bool isAiming;
    private bool canFire = true;
    private float ammo;
    public Weapon_SO currentWeapon;
    public WeaponIdentification weaponIdentification;

    private void Awake()
    {
        playerInput = new PlayerInput();

        fire = playerInput.FPSPlayer.Fire;
        fire.Enable();
        fire.performed += ctx => StartFire();
        fire.canceled += ctx => StopFire();
        
        reload = playerInput.FPSPlayer.Reload;
        reload.Enable();
        reload.performed += ctx => StartReload();

        aim = playerInput.FPSPlayer.Aim;
        aim.Enable();
        aim.performed += ctx => StartAim();
        aim.canceled += ctx => StopAim();
        
        ammo = currentWeapon.maxAmmo;
    }

    private void OnDestroy()
    {
        fire.performed -= ctx => StartFire();
        
        reload.performed -= ctx => StartReload();
        
        aim.performed -= ctx => StartAim();
        aim.canceled -= ctx => StopAim();
    }
    
    private void EquipWeapon(string weaponName)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.name == weaponName)
            {
                currentWeapon = weapon;
                weaponIdentification = currentWeapon.GetComponent<WeaponIdentification>();
                
                ammo = currentWeapon.maxAmmo;
            }
        }
    }

    private void StartFire()
    {
        isFiring = true;

        InvokeRepeating("ProjectileShoot", 0, currentWeapon.fireRate);
    }

    private void StopFire()
    {
        isFiring = false;
        
        CancelInvoke("ProjectileShoot");
    }

    public void ProjectileShoot()
    {
        foreach (var firePoint in weaponIdentification.firePoints)
        {
            for (int i = 0; i < currentWeapon.shotsPerFire; i++)
            {
                if (ammo <= 0) return;
                if (!canFire) return;

                if (currentWeapon.fireSound != null) audioSource.PlayOneShot(currentWeapon.fireSound);
                
                cameraEffects.Recoil(currentWeapon.recoilX, currentWeapon.recoilY, currentWeapon.recoilZ);
                
                var muzzleFlash = Instantiate(currentWeapon.muzzleFlash, firePoint.position, firePoint.rotation);
                muzzleFlash.transform.parent = firePoint;
                Destroy(muzzleFlash, 1f);

                var x = Random.Range(-currentWeapon.bulletSpread, currentWeapon.bulletSpread);
                var y = Random.Range(-currentWeapon.bulletSpread, currentWeapon.bulletSpread);

                var projectile = Instantiate(currentWeapon.projectile.projectilePrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(x, y, 0f));

                projectile.layer = LayerMask.NameToLayer("Projectile");
                
                var projectileScript = projectile.AddComponent<Projectile>();

                projectileScript.bulletHolePrefab = currentWeapon.projectile.bulletHolePrefab;
                projectileScript.damage = currentWeapon.projectile.damage;
                projectileScript.damageRange = currentWeapon.projectile.damageRange;
                projectileScript.speed = currentWeapon.projectile.speed;
                projectileScript.lifeTime = currentWeapon.projectile.lifeTime;
                projectileScript.useGravity = currentWeapon.projectile.useGravity;

                ammo--;
                
                StartCoroutine(projectileScript.DestroyProjectile());
                
                RaycastHit hit;
                
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100f))
                {
                    if (hit.transform.tag != "LocalPlayer" && hit.transform.tag != "RemotePlayer")
                    {
                        var bulletHole = Instantiate(currentWeapon.projectile.bulletHolePrefab, hit.point + new Vector3(0f, 0f, -0.5f), Quaternion.LookRotation(-hit.normal));  
                        
                        bulletHole.AddComponent<DestroyAfterDelay>().delay = projectileScript.lifeTime;
                    }
                }
            }
        }
    }
    
    public void StartReload()
    {
        if (ammo == currentWeapon.maxAmmo) return;
        StartCoroutine(Reload());
        if (currentWeapon.reloadSound != null) audioSource.PlayOneShot(currentWeapon.reloadSound);
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        ammo = currentWeapon.maxAmmo;
    }
    
    public void StartAim()
    {
        isAiming = true;
        if (currentWeapon.aimSound != null) audioSource.PlayOneShot(currentWeapon.aimSound);
    }
    
    public void StopAim()
    {
        isAiming = false;
    }
}