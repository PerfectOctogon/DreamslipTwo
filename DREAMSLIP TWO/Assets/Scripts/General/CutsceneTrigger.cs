using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneTimeline;
    //public GameObject cutsceneCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            other.gameObject.SetActive(false);
            cutsceneTimeline.Play();
            Destroy(gameObject);
        }
    }
}
