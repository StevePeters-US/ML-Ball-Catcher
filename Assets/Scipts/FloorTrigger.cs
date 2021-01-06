using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private BoxAgent boxAgent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            boxAgent.UpdateReward(-0.1f);
            other.gameObject.SetActive(false);
        }
    }
}
