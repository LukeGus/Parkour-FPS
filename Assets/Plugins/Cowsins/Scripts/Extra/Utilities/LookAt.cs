using UnityEngine;

namespace cowsins
{
    public class LookAt : MonoBehaviour
    {
        private Transform player;

        private void Update()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (player != null && player.gameObject.activeInHierarchy)
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(targetPosition);
            }
        }
    }
}