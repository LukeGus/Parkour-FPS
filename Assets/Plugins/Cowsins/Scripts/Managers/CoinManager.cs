using FishNet.Object;
using UnityEngine;

namespace cowsins
{
    public class CoinManager : NetworkBehaviour
    {
        public static CoinManager Instance; // Singleton instance of the CoinManager
        
        [SerializeField] private EventManager eventManager; // Reference to the event manager

        [Tooltip("Whether the game should use coins.")]
        public bool useCoins; // Flag to indicate if the game uses coins

        public int coins { get; private set; } // Property to access the coin count (read-only from outside)

        private void Awake()
        {
            Invoke("SetInstance", 1);
        }

        private void SetInstance()
        {
#if !UNITY_SERVER
            if (Instance == null && NetworkDisabler.Instance.isOwner)
            {
                Instance = this;
            }
# else
            if (Instance == null)
            {
                Instance = this;
            }
#endif
        }

        private void Start()
        {
            eventManager.OnCoinsChange?.Invoke(coins);
            
            eventManager.OnCoinsChange.AddListener(AddCoins);
        }

        // Add coins to the total count
        public void AddCoins(int amount)
        {
            coins += Mathf.Abs(amount); // Add positive amount of coins
        }

        // Remove coins from the total count
        public void RemoveCoins(int amount)
        {
            coins -= Mathf.Abs(amount); // Subtract positive amount of coins

            if (coins <= 0) // Ensure coins don't go negative
            {
                coins = 0; // Set coins to zero if they go below
            }
        }

        // Check if there are enough coins
        public bool CheckIfEnoughCoins(int amount)
        {
            return coins >= amount; // Return whether there are enough coins
        }

        // Purchase action, checking and deducting coins if sufficient
        public bool CheckIfEnoughCoinsAndPurchase(int amount)
        {
            if (coins >= amount) // Check if enough coins are available
            {
                RemoveCoins(amount); // Deduct coins from the total
                return true; // Purchase successful
            }
            return false; // Not enough coins for purchase
        }

        // Reset the coin count to zero
        public void ResetCoins()
        {
            coins = 0; // Set coins to zero
        }
    }
}
