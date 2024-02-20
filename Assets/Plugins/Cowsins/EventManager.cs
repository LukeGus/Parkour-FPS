using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cowsins
{
    public class EventManager : MonoBehaviour
    {
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
        
        private void HealthChanged(float currentHealth, float maxHealth, bool isHealing)
        {
            Debug.Log("HealthChanged");
        }
        private void HealthUISetUp(float maxHealth, float maxShield, float currentHealth, float currentShield)
        {
            Debug.Log("HealthUISetUp");
        }
        private void HealthDisplay(float currentHealth, float maxHealth)
        {
            Debug.Log("HealthDisplay");
        }
        private void NumericHealthDisplay(float currentHealth, float maxHealth)
        {
            Debug.Log("NumericHealthDisplay");
        }
        private void InteractionAllowed(string interaction)
        {
            Debug.Log("InteractionAllowed");
        }
        private void InteractionForbidden()
        {
            Debug.Log("InteractionForbidden");
        }
        private void DisableInteraction()
        {
            Debug.Log("DisableInteraction");
        }
        private void InteractionProgressChanged(float progress)
        {
            Debug.Log("InteractionProgressChanged");
        }
        private void FinishInteractionProgress()
        {
            Debug.Log("FinishInteractionProgress");
        }
        private void GenerateInspectionUI(WeaponController weaponController)
        {
            Debug.Log("GenerateInspectionUI");
        }
        private void InitializeDashUI(int maxDashes)
        {
            Debug.Log("InitializeDashUI");
        }
        private void DashGained()
        {
            Debug.Log("DashGained");
        }
        private void DashUsed(int remainingDashes)
        {
            Debug.Log("DashUsed");
        }
        private void EnemyHit()
        {
            Debug.Log("EnemyHit");
        }
        private void EnemyKilled(string enemyName)
        {
            Debug.Log("EnemyKilled");
        }
        private void DetectReloadMethod(bool isReloading, bool isReloadingMagazine)
        {
            Debug.Log("DetectReloadMethod");
        }
        private void HeatRatioChanged(float heatRatio)
        {
            Debug.Log("HeatRatioChanged");
        }
        private void BulletsChanged(int currentBullets, int maxBullets, bool isReloading, bool isReloadingMagazine)
        {
            Debug.Log("BulletsChanged");
        }
        private void DisableWeapon()
        {
            Debug.Log("DisableWeapon");
        }
        private void WeaponDisplay(Weapon_SO weapon)
        {
            Debug.Log("WeaponDisplay");
        }
        private void EnableWeapon()
        {
            Debug.Log("EnableWeapon");
        }
        private void CoinsChange(int coins)
        {
            Debug.Log("CoinsChange");
        }
        private void AttachmentUIElementClicked(Attachment attachment, bool isEquipped)
        {
            Debug.Log("AttachmentUIElementClicked");
        }
        private void AttachmentUIElementClickedNewAttachment(Attachment attachment, int attachmentIndex)
        {
            Debug.Log("AttachmentUIElementClickedNewAttachment");
        }
        private void EnableAttachmentUI(GameObject attachment)
        {
            Debug.Log("EnableAttachmentUI");
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
    }
}