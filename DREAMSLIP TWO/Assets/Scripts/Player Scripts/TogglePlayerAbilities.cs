using UnityEngine;

public class TogglePlayerAbilities : MonoBehaviour
{
    public int toggleID;
    public bool activate;


    public static void ToggleInsomniaBeamEmitter(bool active)
    {
        Camera.main.transform.GetChild(0).gameObject.SetActive(active);
        Camera.main.transform.GetChild(1).gameObject.SetActive(active);
        if (active)
        {
            FirstPersonPlayerMovement.playerWeaponEnabled = true;
            return;
        }
        FirstPersonPlayerMovement.playerWeaponEnabled = false;
    }


    public static void ToggleInsomniaBeamEmitter()
    {
        GameObject mainCamera = Camera.main.gameObject;
        bool activated = mainCamera.transform.GetChild(0).gameObject.activeInHierarchy;
        
        mainCamera.transform.GetChild(0).gameObject.SetActive(!activated);
        mainCamera.transform.GetChild(1).gameObject.SetActive(!activated);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.tag.Equals("Player")) return;
        switch (toggleID)
        {
            case 1:
                ToggleInsomniaBeamEmitter(activate);
                break;
        }
    }
}
