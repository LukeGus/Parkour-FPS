using UnityEngine;

namespace cowsins
{
    public class LookAt : MonoBehaviour
    {
        private Transform player;
        
        void Update()
        {
            if (player == null)
            {
                GameObject mainPlayerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (mainPlayerObject != null)
                {
                    GameObject playerObject = GameObject.Find("Player");

                    if (playerObject != null)
                    {
                        player = playerObject.transform;
                    }
                }
            }

            if (player != null)
            {
                transform.LookAt(player);
            }
        }
    }
}