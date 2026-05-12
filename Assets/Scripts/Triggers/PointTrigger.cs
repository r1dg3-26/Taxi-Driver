using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    public TaxiManager manager;

    void OnTriggerEnter(Collider other) 
    {
        if(other.attachedRigidbody != null &&
            other.attachedRigidbody.CompareTag("Car"))
        {
            manager.Trigger(gameObject);
        }
    }
}
