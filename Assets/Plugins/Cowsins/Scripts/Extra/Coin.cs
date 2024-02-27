using UnityEngine;

namespace cowsins
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private EventManager eventManager;
        
        [SerializeField] private int minCoins, maxCoins;
        [SerializeField] private UIController uiController;

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
        }

        [SerializeField] private AudioClip collectCoinSFX;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            int amountOfCoins = Random.Range(minCoins, maxCoins);
            uiController.UpdateCoinsPanel();
            eventManager.OnCoinsChange.Invoke(amountOfCoins);
            SoundManager.Instance.PlaySound(collectCoinSFX, 0, 1, false, 0);
            Destroy(this.gameObject);
        }
    }

}