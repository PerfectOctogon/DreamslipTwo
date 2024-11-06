using UnityEngine;

public class TransferControl : MonoBehaviour
{
    public GameObject player;
    public GameObject cutsceneCamera;

    
    public void transferControlToPlayer()
    {
        cutsceneCamera.SetActive(false);
        player.SetActive(true);
        StartCoroutine(HintBox.ShowHint("Use WASD to move"));
    }
}
