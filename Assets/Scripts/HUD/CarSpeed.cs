using UnityEngine;
using TMPro;

public class CarSpeed : MonoBehaviour
{
    public Rigidbody carRigidbody;
    
    public TMP_Text speedHUD;

    void FixedUpdate()
    {
        float speed = Mathf.Round(carRigidbody.linearVelocity.magnitude * 3.6f);

        if(speed < 1f)
        {
            speed = 0f;
        }

        speedHUD.SetText(speed + " km/h");
    }
}
