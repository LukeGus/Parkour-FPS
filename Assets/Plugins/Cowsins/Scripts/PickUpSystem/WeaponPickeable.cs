using UnityEngine;
using UnityEditor;

namespace cowsins
{
    public class WeaponPickeable : Pickeable
    {
        [Tooltip("Which weapon are we grabbing")] public Weapon_SO weapon;

        [HideInInspector] public int currentBullets, totalBullets;
        
        private UIController uiController;

        private AttachmentIdentifier_SO Barrel,
            Scope,
            Stock,
            Grip,
            Magazine,
            Flashlight,
            Laser;
        public override void Start()
        {
            base.Start();
            if (dropped) return;
            GetVisuals();
            currentBullets = weapon.magazineSize;
            totalBullets = weapon.totalMagazines * weapon.magazineSize;
            SetDefaultAttachments();
        }
        
        public void Update()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }
        }    

        public override void Interact()
        {
            base.Interact();

            weaponController = player.GetComponent<WeaponController>();

            if (!CheckIfInventoryFull())
            {
                Destroy(gameObject);
                return;
            }

            Weapon_SO oldWeapon = weaponController.weapon;
            int saveBulletsLeftInMagazine = weaponController.id.bulletsLeftInMagazine;
            int saveTotalBullets = weaponController.id.totalBullets;
            weaponController.ReleaseCurrentWeapon();

            // Instantiating the selected weapon
            var weaponPicked = Instantiate(weapon.weaponObject, weaponController.weaponHolder);
            weaponPicked.transform.localPosition = weapon.weaponObject.transform.localPosition;
            //Assign the weapon to the inventory
            weaponController.inventory[weaponController.currentWeapon] = weaponPicked;

            weaponController.weapon = weapon;
            // Apply attachments to the weapon
            ApplyAttachments(weaponController);
            //Since this slot is selected, let�s unholster it
            weaponController.UnHolster(weaponController.inventory[weaponController.currentWeapon].gameObject, true);
            // Set bullets
            WeaponIdentification curWeapon = weaponController.inventory[weaponController.currentWeapon].GetComponent<WeaponIdentification>();
            curWeapon.bulletsLeftInMagazine = currentBullets;
            curWeapon.totalBullets = totalBullets;
            //UI
            weaponController.slots[weaponController.currentWeapon].weapon = weapon;
            weaponController.slots[weaponController.currentWeapon].GetImage();
            //Now, let�s set the new weapon graphics on the pickeable
            currentBullets = saveBulletsLeftInMagazine;
            totalBullets = saveTotalBullets;
            
/*

#if UNITY_EDITOR
            uiController.crosshair.GetComponent<CrosshairShape>().currentPreset = inv.weapon.crosshairPreset;
            CowsinsUtilities.ApplyPreset(uiController.crosshair.GetComponent<CrosshairShape>().currentPreset, uiController.crosshair.GetComponent<CrosshairShape>());
#endif

*/

            weapon = oldWeapon;
            image.sprite = weapon.icon;
            Destroy(graphics.transform.GetChild(0).gameObject);
            GetVisuals();
        }

        private bool CheckIfInventoryFull()
        {
            weaponController = player.GetComponentInChildren<WeaponController>();
            
            for (int i = 0; i < weaponController.inventorySize; i++)
            {
                if (weaponController.inventory[i] == null) // Inventory is, indeed, not full, so there is room for a new weapon.
                {
                    // Instantiating the selected weapon
                    var weaponPicked = Instantiate(weapon.weaponObject, weaponController.weaponHolder);
                    weaponPicked.transform.localPosition = weapon.weaponObject.transform.localPosition;
                    //Assign the weapon to the inventory
                    weaponController.inventory[i] = weaponPicked;
                    //Since this slot is selected and it was empty, let�s unholster it
                    if (weaponController.inventory[weaponController.currentWeapon] == weaponController.inventory[i])
                    {
                        weaponController.inventory[i].gameObject.SetActive(true);
                        weaponController.weapon = weapon;
                        ApplyAttachments(weaponController);
                        weaponController.UnHolster(weaponPicked.gameObject, true);
                    }
                    else weaponController.inventory[i].gameObject.SetActive(false);
                    // Set bullets
                    WeaponIdentification curWeapon = weaponController.inventory[i].GetComponent<WeaponIdentification>();
                    curWeapon.bulletsLeftInMagazine = currentBullets;
                    curWeapon.totalBullets = totalBullets;
                    //UI
                    weaponController.slots[i].weapon = weapon;
                    weaponController.slots[i].GetImage();
                    
/*
                    
#if UNITY_EDITOR
                    if (inv.weapon != null)
                    {
                        uiController.crosshair.GetComponent<CrosshairShape>().currentPreset = inv.weapon.crosshairPreset;
                        CowsinsUtilities.ApplyPreset(uiController.crosshair.GetComponent<CrosshairShape>().currentPreset, uiController.crosshair.GetComponent<CrosshairShape>());
                    }
#endif

*/

                    // Don�t return true
                    return false;
                }
            }
            // Inventory is full, we�ll check what to do then
            return true;

        }

        public override void Drop(WeaponController wcon, Transform orientation)
        {
            base.Drop(wcon, orientation);

            currentBullets = wcon.id.bulletsLeftInMagazine;
            totalBullets = wcon.id.totalBullets;
            weapon = wcon.weapon;
            GetVisuals();
        }

        // Applied the default attachments to the weapon
        private void SetDefaultAttachments()
        {
            DefaultAttachment defaultAttachments = weapon.weaponObject.defaultAttachments;
            Barrel = defaultAttachments.defaultBarrel?.attachmentIdentifier;
            Scope = defaultAttachments.defaultScope?.attachmentIdentifier;
            Stock = defaultAttachments.defaultStock?.attachmentIdentifier;
            Grip = defaultAttachments.defaultGrip?.attachmentIdentifier;
            Flashlight = defaultAttachments.defaultFlashlight?.attachmentIdentifier;
            Magazine = defaultAttachments.defaultMagazine?.attachmentIdentifier;
            Laser = defaultAttachments.defaultLaser?.attachmentIdentifier;
        }
        /// <summary>
        /// Stores the attachments on the WeaponPickeable so they can be accessed later in case the weapon is picked up.
        /// </summary>
        public void SetPickeableAttachments(Attachment b, Attachment sc, Attachment st, Attachment gr, Attachment mag, Attachment fl, Attachment ls)
        {
            Barrel = b?.attachmentIdentifier;
            Scope = sc?.attachmentIdentifier;
            Stock = st?.attachmentIdentifier;
            Grip = gr?.attachmentIdentifier;
            Magazine = mag?.attachmentIdentifier;
            Flashlight = fl?.attachmentIdentifier;
            Laser = ls?.attachmentIdentifier;
        }
        public void GetVisuals()
        {
            // Get whatever we need to display
            interactText = weapon._name;
            image.sprite = weapon.icon;
            // Manage graphics
            Destroy(graphics.transform.GetChild(0).gameObject);
            Instantiate(weapon.pickUpGraphics, graphics);
        }

        // Equips all the appropriate attachyments on pick up
        public void ApplyAttachments(WeaponController weaponController)
        {
            WeaponIdentification wp = weaponController.inventory[weaponController.currentWeapon];

            var attachments = new[] { Barrel, Scope, Stock, Grip, Magazine, Flashlight, Laser };
            foreach (var attachment in attachments)
            {
                (Attachment atc, int id) = GetAttachmentID(attachment, wp);
                weaponController.AssignNewAttachment(atc, id);
            }
        }

        /// <summary>
        /// Grabs the attachment object and the id given an attachment identifier
        /// </summary>
        /// <param name="atcToCheck">Attachment object to get information about returned.</param>
        /// <param name="wID">Weapon Identification that holds the attachments</param>
        /// <returns></returns>
        private (Attachment, int) GetAttachmentID(AttachmentIdentifier_SO atcToCheck, WeaponIdentification wID)
        {
            // Check for compatibility
            for (int i = 0; i < wID.compatibleAttachments.barrels.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.barrels[i].attachmentIdentifier) return (wID.compatibleAttachments.barrels[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.scopes.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.scopes[i].attachmentIdentifier) return (wID.compatibleAttachments.scopes[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.stocks.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.stocks[i].attachmentIdentifier) return (wID.compatibleAttachments.stocks[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.grips.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.grips[i].attachmentIdentifier) return (wID.compatibleAttachments.grips[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.magazines.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.magazines[i].attachmentIdentifier) return (wID.compatibleAttachments.magazines[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.flashlights.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.flashlights[i].attachmentIdentifier) return (wID.compatibleAttachments.flashlights[i], i);
            }
            for (int i = 0; i < wID.compatibleAttachments.lasers.Length; i++)
            {
                if (atcToCheck == wID.compatibleAttachments.lasers[i].attachmentIdentifier) return (wID.compatibleAttachments.lasers[i], i);
            }

            // Return an error
            return (null, -1);
        }
    }

#if UNITY_EDITOR

    [System.Serializable]
    [CustomEditor(typeof(WeaponPickeable))]
    public class WeaponPickeableEditor : Editor
    {
        private string[] tabs = { "Basic", "References", "Effects", "Events" };
        private int currentTab = 0;

        override public void OnInspectorGUI()
        {
            serializedObject.Update();
            WeaponPickeable myScript = target as WeaponPickeable;

            Texture2D myTexture = Resources.Load<Texture2D>("CustomEditor/WeaponPickeable_CustomEditor") as Texture2D;
            GUILayout.Label(myTexture);

            EditorGUILayout.BeginVertical();
            currentTab = GUILayout.Toolbar(currentTab, tabs);
            EditorGUILayout.Space(10f);
            EditorGUILayout.EndVertical();
            #region variables

            if (currentTab >= 0 || currentTab < tabs.Length)
            {
                switch (tabs[currentTab])
                {
                    case "Basic":
                        EditorGUILayout.LabelField("CUSTOMIZE YOUR WEAPON PICKEABLE", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("weapon"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("interactText"));
                        break;
                    case "References":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("image"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("graphics"));

                        break;
                    case "Effects":
                        EditorGUILayout.LabelField("EFFECTS", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotates"));
                        if (myScript.rotates) EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("translates"));
                        if (myScript.translates) EditorGUILayout.PropertyField(serializedObject.FindProperty("translationSpeed"));
                        break;
                    case "Events":
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("events"));
                        break;

                }
            }

            #endregion

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}