using UnityEngine;
using System.Collections;

namespace cowsins
{
    public class Lootbox : Interactable
    {
        [SerializeField] private GameObject[] loot;

        [SerializeField, Min(0)] private int price;

        [SerializeField] private float delayToReceiveLoot;

        [SerializeField] private float minSpawnAngle, maxSpawnAngle, spawnDistance;

        [SerializeField] private bool directionDependsOnPlayerPosition;

        [SerializeField] private EventManager eventManager;

        private Animation anim;

        private AudioSource audioSource;

        private string baseInteractText;

        private void Update()
        {
            if (eventManager == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (playerObject != null)
                {
                    eventManager = playerObject.GetComponent<EventManager>();
                }
            }
            
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }
            
            if (weaponController == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (playerObject != null)
                {
                    weaponController = playerObject.GetComponentInChildren<WeaponController>();
                }
            }
        }
        
        private void Start()
        {
            anim = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
            baseInteractText = interactText;
            if (price != 0) interactText += $" [{price}]";
        }
        public override void Highlight()
        {
            if (price == 0) return;

            if (CoinManager.Instance.CheckIfEnoughCoins(price))
                interactText = baseInteractText + "<color=#" + ColorUtility.ToHtmlStringRGB(Color.green) + ">" + $" [{price}]" + "</color>";
            else interactText = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.red) + ">" + $" [{price}]" + "</color>";


        }
        public override void Interact()
        {
            if (price != 0 && CoinManager.Instance.useCoins && CoinManager.Instance.CheckIfEnoughCoins(price) || price == 0)
                StartCoroutine(GetLoot());
        }

        private IEnumerator GetLoot()
        {
            if (price != 0)
            {
                CoinManager.Instance.RemoveCoins(price);
                eventManager.OnCoinsChange?.Invoke(CoinManager.Instance.coins);
            }
            yield return new WaitForSeconds(delayToReceiveLoot);
            GameObject lootObject = null;

            lootObject = loot[Random.Range(0, loot.Length)];

            SpawnSelectedLoot(lootObject);

            anim.Play();
            audioSource.Play();

            Destroy(GetComponent<Lootbox>());
            gameObject.layer = LayerMask.NameToLayer("Default");
            StopAllCoroutines();
        }

        private void SpawnSelectedLoot(GameObject loot)
        {
            float spawnAngle = Random.Range(minSpawnAngle, maxSpawnAngle);
            Quaternion spawnRotation = Quaternion.Euler(0f, spawnAngle, 0f); // Rotate around the y-axis
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 spawnDirection = directionDependsOnPlayerPosition ? (player.position - transform.position).normalized * spawnDistance : spawnRotation * -transform.right;

            Vector3 spawnPosition = transform.position + spawnDirection * spawnDistance;

            Instantiate(loot, spawnPosition, spawnRotation);
        }

        private void OnDrawGizmosSelected()
        {
            if (directionDependsOnPlayerPosition) return;

            Gizmos.color = Color.blue;

            Vector3 forward = -transform.right * spawnDistance;

            Quaternion minRotation = Quaternion.Euler(0f, minSpawnAngle, 0f);
            Vector3 minDirection = minRotation * forward;

            Quaternion maxRotation = Quaternion.Euler(0f, maxSpawnAngle, 0f);
            Vector3 maxDirection = maxRotation * forward;

            Gizmos.DrawLine(transform.position, transform.position + minDirection);
            Gizmos.DrawLine(transform.position, transform.position + maxDirection);
        }
    }


}