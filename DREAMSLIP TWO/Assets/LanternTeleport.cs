using UnityEngine;

public class LanternTeleport : MonoBehaviour
{
    private GameObject player;
    private CharacterController playerCharacterController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BlasterBullet"))
        {
            if (player == null)
            {
                player = GameObject.Find("Player");
                playerCharacterController = player.GetComponent<CharacterController>();
            }
            playerCharacterController.enabled = false;
            player.transform.position = transform.position;
            playerCharacterController.enabled = true;
        }
    }
}
