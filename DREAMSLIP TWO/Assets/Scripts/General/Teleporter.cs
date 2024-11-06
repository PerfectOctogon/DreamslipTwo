using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform thingToTeleport;
    public Transform placeToTeleport;

    public void teleport()
    {
        thingToTeleport.position = placeToTeleport.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            //Debug.Log("Something hit!");
            other.transform.position = placeToTeleport.position;
            other.GetComponent<CharacterController>().enabled = true;
            other.GetComponent<Player>().Damage(20);
        }
    }
}
