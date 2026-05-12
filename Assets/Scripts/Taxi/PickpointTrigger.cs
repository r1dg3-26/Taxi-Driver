using UnityEngine;

public class PickpointTrigger : MonoBehaviour
{
    public TaxiManager manager;

    void OnTriggerEnter(Collider other) 
    {
        if(other.attachedRigidbody != null &&
            other.attachedRigidbody.CompareTag("Player"))
        {
            manager.Trigger(gameObject);
        }
    }
}
