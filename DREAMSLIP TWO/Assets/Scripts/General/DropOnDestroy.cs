using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    public GameObject dropItem;
    private void OnDestroy()
    {
        // Adding a buffer to the drop position
        Vector3 dropPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(dropItem, dropPos, new Quaternion(0, 0, 0, 0));
    }
}
