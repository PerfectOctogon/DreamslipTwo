using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    public ResetPoint[] resetPoints;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Tried resetting gameobject : " + other.gameObject.name);
        if (other.transform.tag.Equals("Player"))
        {
            //Debug.Log("Tried resetting player");
            int resetPointIndex = 0;
            foreach (ResetPoint resetPoint in resetPoints)
            {
                if (resetPoint.pointReached)
                {
                    resetPointIndex++;
                    continue;
                }

                break;
            }
            
            other.transform.GetComponent<CharacterController>().enabled = false;
            other.transform.position = resetPoints[resetPointIndex - 1].transform.position;
            other.transform.GetComponent<CharacterController>().enabled = true;
        }
        
    }
}
