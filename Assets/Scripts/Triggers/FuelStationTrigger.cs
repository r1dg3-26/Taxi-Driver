using UnityEngine;
using TMPro;

public class FuelStationTrigger : MonoBehaviour
{
    public CarManager carManager;
    public TaxiManager taxiManager;

    public TMP_Text hintHUD;

    void OnTriggerStay(Collider other) 
    {
        if(other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Car"))
        {
            carManager.Trigger(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Car"))
        {
            carManager.status = "n";

            hintHUD.SetText("");

            taxiManager.enabled = true;
        }
    }
}
