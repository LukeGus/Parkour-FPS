using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Projectile", menuName = "Weapon System/Projectile", order = 2)]
public class Projectile_SO : ScriptableObject
{
    [Header("References")]
    public GameObject projectilePrefab;
    public GameObject bulletHolePrefab;
    
    [Header("Stats")] 
    public float damage;
    public float damageRange;
    public float speed;
    
    [Header("Other")]
    public float lifeTime;
    public bool useGravity;
    
    [Header("Sound Effects")] 
    public AudioClip naturalSound;
    public AudioClip hitSound;
}

#if UNITY_EDITOR

[CustomEditor(typeof(Projectile_SO))]
public class Projectile_SO_Editor : Editor
{
    private string[] tabs = { "References", "Stats", "Other", "Sound Effects" };
    private int currentTab = 0;

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Ensure SerializedObject is up to date

        Projectile_SO projectileSO = (Projectile_SO)target;

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
                DrawReferencesTab(projectileSO);
                break;
            case 1:
                DrawStatsTab(projectileSO);
                break;
            case 2:
                DrawOtherTab(projectileSO);
                break;
            case 3:
                DrawSoundEffectsTab(projectileSO);
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to SerializedObject
    }

    // Helper methods for drawing tabs
    private void DrawReferencesTab(Projectile_SO projectileSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        GameObject newProjectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", projectileSO.projectilePrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Projectile Prefab");
            projectileSO.projectilePrefab = newProjectilePrefab;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        GameObject newBulletHolePrefab = (GameObject)EditorGUILayout.ObjectField("Bullet Hole Prefab", projectileSO.bulletHolePrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Bullet Hole Prefab");
            projectileSO.bulletHolePrefab = newBulletHolePrefab;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    private void DrawStatsTab(Projectile_SO projectileSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        float newDamage = EditorGUILayout.FloatField("Damage", projectileSO.damage);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Damage");
            projectileSO.damage = newDamage;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        float newDamageRange = EditorGUILayout.FloatField("Damage Range", projectileSO.damageRange);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Damage Range");
            projectileSO.damageRange = newDamageRange;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        float newSpeed = EditorGUILayout.FloatField("Speed", projectileSO.speed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Speed");
            projectileSO.speed = newSpeed;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    private void DrawOtherTab(Projectile_SO projectileSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        float newLifeTime = EditorGUILayout.FloatField("Life Time", projectileSO.lifeTime);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Life Time");
            projectileSO.lifeTime = newLifeTime;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        bool newUseGravity = EditorGUILayout.Toggle("Use Gravity", projectileSO.useGravity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Use Gravity");
            projectileSO.useGravity = newUseGravity;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    private void DrawSoundEffectsTab(Projectile_SO projectileSO)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        AudioClip newNaturalSound = (AudioClip)EditorGUILayout.ObjectField("Natural Sound", projectileSO.naturalSound, typeof(AudioClip), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Natural Sound");
            projectileSO.naturalSound = newNaturalSound;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }
        
        EditorGUI.BeginChangeCheck();
        AudioClip newHitSound = (AudioClip)EditorGUILayout.ObjectField("Hit Sound", projectileSO.hitSound, typeof(AudioClip), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projectileSO, "Changed Hit Sound");
            projectileSO.hitSound = newHitSound;
            EditorUtility.SetDirty(projectileSO); // Mark the asset as dirty
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
}

#endif