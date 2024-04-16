using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon System/Weapon", order = 1)]
public class Weapon_SO : ScriptableObject
{
    [Header("References")]
    public GameObject weaponPrefab;
    public Projectile_SO projectile;
    
    [Header("Stats")] 
    public float fireRate;
    public float reloadTime;
    public int timeBetweenShots;
    public int shotsPerFire;
    public float bulletSpread;
    public int maxAmmo;
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    
    [Header("Muzzle FLash")]
    public GameObject muzzleFlash;
    
    [Header("Sound Effects")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip aimSound;
}

#if UNITY_EDITOR

[CustomEditor(typeof(Weapon_SO))]
public class Weapon_SO_Editor : Editor
{
    private string[] tabs = { "References", "Stats", "Muzzle Flash", "Sound Effects" };
    private int currentTab = 0;

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Ensure SerializedObject is up to date

        Weapon_SO weaponSO = (Weapon_SO)target;

        // Tab selection buttons
        GUILayout.BeginHorizontal();
        for (int i = 0; i < tabs.Length; i++)
        {
            if (GUILayout.Button(tabs[i], GUILayout.Height(30)))
            {
                currentTab = i;
            }
        }
        GUILayout.EndHorizontal();

        // Content based on the selected tab
        switch (currentTab)
        {
            case 0:
                DrawReferencesTab(weaponSO);
                break;
            case 1:
                DrawStatsTab(weaponSO);
                break;
            case 2:
                DrawMuzzleFlashTab(weaponSO);
                break;
            case 3:
                DrawSoundEffectsTab(weaponSO);
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to SerializedObject
    }

    // Helper methods for drawing tabs
    private void DrawReferencesTab(Weapon_SO weaponSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        GameObject newWeaponPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon Prefab", weaponSO.weaponPrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Weapon Prefab");
            weaponSO.weaponPrefab = newWeaponPrefab;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUI.BeginChangeCheck();
        Projectile_SO newProjectile = (Projectile_SO)EditorGUILayout.ObjectField("Projectile", weaponSO.projectile, typeof(Projectile_SO), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Projectile");
            weaponSO.projectile = newProjectile;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    private void DrawStatsTab(Weapon_SO weaponSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Rates", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        float newFireRate = EditorGUILayout.FloatField("Fire Rate (Seconds)", weaponSO.fireRate);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Fire Rate");
            weaponSO.fireRate = newFireRate;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUI.BeginChangeCheck();
        float newReloadTime = EditorGUILayout.FloatField("Reload Time (Seconds)", weaponSO.reloadTime);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Reload Time");
            weaponSO.reloadTime = newReloadTime;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        int newTimeBetweenShots = EditorGUILayout.IntField("Time Between Shots (Seconds)", weaponSO.timeBetweenShots);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Time Between Shots");
            weaponSO.timeBetweenShots = newTimeBetweenShots;
            EditorUtility.SetDirty(weaponSO);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Shots", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        int newShotsPerFire = EditorGUILayout.IntField("Shots Per Fire", weaponSO.shotsPerFire);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Shots Per Fire");
            weaponSO.shotsPerFire = newShotsPerFire;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUI.BeginChangeCheck();
        float newBulletSpread = EditorGUILayout.FloatField("Bullet Spread", weaponSO.bulletSpread);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Bullet Spread");
            weaponSO.bulletSpread = newBulletSpread;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ammo", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        int newMaxAmmo = EditorGUILayout.IntField("Max Ammo", weaponSO.maxAmmo);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Max Ammo");
            weaponSO.maxAmmo = newMaxAmmo;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Recoil", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        float newRecoilX= EditorGUILayout.FloatField("Recoil X", weaponSO.recoilX);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Recoil X");
            weaponSO.recoilX = newRecoilX;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUI.BeginChangeCheck();
        float newRecoilY = EditorGUILayout.FloatField("Recoil Y", weaponSO.recoilY);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Recoil Y");
            weaponSO.recoilY = newRecoilY;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUI.BeginChangeCheck();
        float newRecoilZ = EditorGUILayout.FloatField("Recoil Z", weaponSO.recoilZ);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Recoil Z");
            weaponSO.recoilZ = newRecoilZ;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
    
    private void DrawMuzzleFlashTab(Weapon_SO weaponSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Muzzle Flash", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        GameObject newMuzzleFlash = (GameObject)EditorGUILayout.ObjectField("Muzzle Flash", weaponSO.muzzleFlash, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSO, "Changed Muzzle Flash");
            weaponSO.muzzleFlash = newMuzzleFlash;
            EditorUtility.SetDirty(weaponSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    private void DrawSoundEffectsTab(Weapon_SO weaponSo)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        AudioClip newFireSound = (AudioClip)EditorGUILayout.ObjectField("Fire Sound", weaponSo.fireSound, typeof(AudioClip), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSo, "Changed Fire Sound");
            weaponSo.fireSound = newFireSound;
            EditorUtility.SetDirty(weaponSo); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        AudioClip newReloadSound = (AudioClip)EditorGUILayout.ObjectField("Reload Sound", weaponSo.reloadSound, typeof(AudioClip), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSo, "Changed Reload Sound");
            weaponSo.reloadSound = newReloadSound;
            EditorUtility.SetDirty(weaponSo); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        AudioClip newAimSound = (AudioClip)EditorGUILayout.ObjectField("Aim Sound", weaponSo.aimSound, typeof(AudioClip), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(weaponSo, "Changed Aim Sound");
            weaponSo.aimSound = newAimSound;
            EditorUtility.SetDirty(weaponSo); // Mark the asset as dirty
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
}

#endif
