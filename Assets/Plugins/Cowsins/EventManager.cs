using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cowsins
{
    public class EventManager : MonoBehaviour
    {
        public bool useDebug;
        
        public UnityEvent<float, float, bool> OnHealthChanged;
        public UnityEvent<float, float, float, float> BasicHealthUISetUp;
        public UnityEvent<float, float> HealthDisplayMethod;
        public UnityEvent<float, float> NumericHealthDisplayMethod;
        public UnityEvent<string> AllowedInteraction;
        public UnityEvent ForbiddenInteraction;
        public UnityEvent DisableInteractionUI;
        public UnityEvent<float> OnInteractionProgressChanged;
        public UnityEvent OnFinishInteractionProgress;
        public UnityEvent<WeaponController> OnGenerateInspectionUI;
        public UnityEvent<int> OnInitializeDashUI;
        public UnityEvent OnDashGained;
        public UnityEvent<int> OnDashUsed;
        public UnityEvent OnEnemyHit;
        public UnityEvent<string> OnEnemyKilled;
        public UnityEvent OnSpawn;
        public UnityEvent OnShoot;
        public UnityEvent OnDamaged;
        public UnityEvent OnDeath;
        public UnityEvent<bool, bool> OnDetectReloadMethod;
        public UnityEvent<float> OnHeatRatioChanged;
        public UnityEvent<int, int, bool, bool> OnBulletsChanged;
        public UnityEvent DisableWeaponUI;
        public UnityEvent<Weapon_SO> SetWeaponDisplay;
        public UnityEvent EnableWeaponDisplay;
        public UnityEvent<int> OnCoinsChange;
        public UnityEvent<Attachment, bool> OnAttachmentUIElementClicked;
        public UnityEvent<Attachment, int> OnAttachmentUIElementClickedNewAttachment;
        public UnityEvent<GameObject> OnEnableAttachmentUI;
        
        private void OnEnable()
        {
            OnHealthChanged.AddListener(HealthChanged);
            BasicHealthUISetUp.AddListener(HealthUISetUp);
            HealthDisplayMethod.AddListener(HealthDisplay);
            NumericHealthDisplayMethod.AddListener(NumericHealthDisplay);
            AllowedInteraction.AddListener(InteractionAllowed);
            ForbiddenInteraction.AddListener(InteractionForbidden);
            DisableInteractionUI.AddListener(DisableInteraction);
            OnInteractionProgressChanged.AddListener(InteractionProgressChanged);
            OnFinishInteractionProgress.AddListener(FinishInteractionProgress);
            OnGenerateInspectionUI.AddListener(GenerateInspectionUI);
            OnInitializeDashUI.AddListener(InitializeDashUI);
            OnDashGained.AddListener(DashGained);
            OnDashUsed.AddListener(DashUsed);
            OnEnemyHit.AddListener(EnemyHit);
            OnEnemyKilled.AddListener(EnemyKilled);
            OnSpawn.AddListener(Spawn);
            OnShoot.AddListener(Shoot);
            OnDamaged.AddListener(Damaged);
            OnDeath.AddListener(Death);
            OnDetectReloadMethod.AddListener(DetectReloadMethod);
            OnHeatRatioChanged.AddListener(HeatRatioChanged);
            OnBulletsChanged.AddListener(BulletsChanged);
            DisableWeaponUI.AddListener(DisableWeapon);
            SetWeaponDisplay.AddListener(WeaponDisplay);
            EnableWeaponDisplay.AddListener(EnableWeapon);
            OnCoinsChange.AddListener(CoinsChange);
            OnAttachmentUIElementClicked.AddListener(AttachmentUIElementClicked);
            OnAttachmentUIElementClickedNewAttachment.AddListener(AttachmentUIElementClickedNewAttachment);
            OnEnableAttachmentUI.AddListener(EnableAttachmentUI);
        }
        
        private void OnDisable()
        {
            OnHealthChanged.RemoveListener(HealthChanged);
            BasicHealthUISetUp.RemoveListener(HealthUISetUp);
            HealthDisplayMethod.RemoveListener(HealthDisplay);
            NumericHealthDisplayMethod.RemoveListener(NumericHealthDisplay);
            AllowedInteraction.RemoveListener(InteractionAllowed);
            ForbiddenInteraction.RemoveListener(InteractionForbidden);
            DisableInteractionUI.RemoveListener(DisableInteraction);
            OnInteractionProgressChanged.RemoveListener(InteractionProgressChanged);
            OnFinishInteractionProgress.RemoveListener(FinishInteractionProgress);
            OnGenerateInspectionUI.RemoveListener(GenerateInspectionUI);
            OnInitializeDashUI.RemoveListener(InitializeDashUI);
            OnDashGained.RemoveListener(DashGained);
            OnDashUsed.RemoveListener(DashUsed);
            OnEnemyHit.RemoveListener(EnemyHit);
            OnEnemyKilled.RemoveListener(EnemyKilled);
            OnDetectReloadMethod.RemoveListener(DetectReloadMethod);
            OnHeatRatioChanged.RemoveListener(HeatRatioChanged);
            OnBulletsChanged.RemoveListener(BulletsChanged);
            DisableWeaponUI.RemoveListener(DisableWeapon);
            SetWeaponDisplay.RemoveListener(WeaponDisplay);
            EnableWeaponDisplay.RemoveListener(EnableWeapon);
            OnCoinsChange.RemoveListener(CoinsChange);
            OnAttachmentUIElementClicked.RemoveListener(AttachmentUIElementClicked);
            OnAttachmentUIElementClickedNewAttachment.RemoveListener(AttachmentUIElementClickedNewAttachment);
            OnEnableAttachmentUI.RemoveListener(EnableAttachmentUI);
        }
        
        private void HealthChanged(float health, float shield, bool damage)
        {
            if (useDebug) Debug.Log($"Health Changed: {health}, Shield: {shield}, Damage: {damage}");
        }
        private void HealthUISetUp(float health, float shield, float maxHealth, float maxShield)
        {
            if (useDebug) Debug.Log($"Health UI Set Up: Health: {health}, Shield: {shield}, Max Health: {maxHealth}, Max Shield: {maxShield}");
        }
        private void HealthDisplay(float health, float shield)
        {
            if (useDebug) Debug.Log($"Health Display: Health: {health}, Shield: {shield}");
        }
        private void NumericHealthDisplay(float health, float shield)
        {
            if (useDebug) Debug.Log($"Numeric Health Display: Health: {health}, Shield: {shield}");
        }
        private void InteractionAllowed(string interaction)
        {
            if (useDebug) Debug.Log($"Interaction Allowed: {interaction}");
        }
        private void InteractionForbidden()
        {
            if (useDebug) Debug.Log("Interaction Forbidden");
        }
        private void DisableInteraction()
        {
            if (useDebug) Debug.Log("Disable Interaction");
        }
        private void InteractionProgressChanged(float progress)
        {
            if (useDebug) Debug.Log($"Interaction Progress Changed: {progress}");
        }
        private void FinishInteractionProgress()
        {
            if (useDebug) Debug.Log("Finish Interaction Progress");
        }
        private void GenerateInspectionUI(WeaponController weaponController)
        {
            if (useDebug) Debug.Log($"Generate Inspection UI: {weaponController}");
        }
        private void InitializeDashUI(int dashCount)
        {
            if (useDebug) Debug.Log($"Initialize Dash UI: {dashCount}");
        }
        private void DashGained()
        {
            if (useDebug) Debug.Log("Dash Gained");
        }
        private void DashUsed(int dashCount)
        {
            if (useDebug) Debug.Log($"Dash Used: {dashCount}");
        }
        private void EnemyHit()
        {
            if (useDebug) Debug.Log("Enemy Hit");
        }
        private void EnemyKilled(string enemyName)
        {
            if (useDebug) Debug.Log($"Enemy Killed: {enemyName}");
        }
        private void Spawn()
        {
            if (useDebug) Debug.Log("Spawn");
        }
        private void Shoot()
        {
            if (useDebug) Debug.Log("Shoot");
        }
        private void Damaged()
        {
            if (useDebug) Debug.Log("Damaged");
        }
        private void Death()
        {
            if (useDebug) Debug.Log("Death");
        }
        private void DetectReloadMethod(bool canReload, bool isReloading)
        {
            if (useDebug) Debug.Log($"Detect Reload Method: Can Reload: {canReload}, Is Reloading: {isReloading}");
        }
        private void HeatRatioChanged(float heatRatio)
        {
            if (useDebug) Debug.Log($"Heat Ratio Changed: {heatRatio}");
        }
        private void BulletsChanged(int currentBullets, int maxBullets, bool isReloading, bool canReload)
        {
            if (useDebug) Debug.Log($"Bullets Changed: Current Bullets: {currentBullets}, Max Bullets: {maxBullets}, Is Reloading: {isReloading}, Can Reload: {canReload}");
        }
        private void DisableWeapon()
        {
            if (useDebug) Debug.Log("Disable Weapon");
        }
        private void WeaponDisplay(Weapon_SO weapon)
        {
            if (useDebug) Debug.Log($"Weapon Display: {weapon}");
        }
        private void EnableWeapon()
        {
            if (useDebug) Debug.Log("Enable Weapon");
        }
        private void CoinsChange(int coins)
        {
            if (useDebug) Debug.Log($"Coins Change: {coins}");
        }
        private void AttachmentUIElementClicked(Attachment attachment, bool active)
        {
            if (useDebug) Debug.Log($"Attachment UI Element Clicked: {attachment}, Active: {active}");
        }
        private void AttachmentUIElementClickedNewAttachment(Attachment attachment, int attachmentIndex)
        {
            if (useDebug) Debug.Log($"Attachment UI Element Clicked New Attachment: {attachment}, Attachment Index: {attachmentIndex}");
        }
        private void EnableAttachmentUI(GameObject gameObject)
        {
            if (useDebug) Debug.Log($"Enable Attachment UI: {gameObject}");
        }
    }
}