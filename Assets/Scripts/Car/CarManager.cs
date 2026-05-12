using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarManager : MonoBehaviour
{
    private Rigidbody rigidBody;
    
    public TMP_Text speedHUD;
    public TMP_Text hintHUD;
    public Slider fuelSlider;
    public Image fuelSliderImage;

    public float fuel;
    public float fuelConsumption = 0.0001f;
    public float fuelSpeed = 0.001f;

    public string status;

    public TaxiManager taxiManager;
    private CarInputActions carControls;

    void Awake()
    {
        carControls = new CarInputActions();
    }

    void OnEnable()
    {
        carControls.Enable();
    }

    void OnDisable()
    {
        carControls.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        fuel = 1f;
        status = "n";
    }

    void FixedUpdate()
    {
        float speed = Mathf.Round(rigidBody.linearVelocity.magnitude * 3.6f);

        if(speed < 1f)
        {
            speed = 0f;
        }

        speedHUD.SetText(speed + " km/h");

        if(fuel > 0f)
        {
            fuel -= fuelConsumption * speed * Time.deltaTime;
        }
        else
        {
            fuel = 0f;
        }

        fuelSlider.value = fuel;

        if(fuel >= 0.5f)
        {
            fuelSliderImage.color = Color.green;
        } 
        else if(fuel <= 0.5f && fuel >= 0.25f)
        {
            fuelSliderImage.color = Color.yellow;
        } 
        else if(fuel <= 0.25f && fuel != 0f)
        {
            fuelSliderImage.color = Color.red;

            hintHUD.SetText("Low fuel!\nYou need to refuel");
        } 
        else if(fuel == 0f)
        {
            fuelSliderImage.color = Color.red;

            hintHUD.SetText("No fuel!\nENJOY YOUR FATE");
        } 
    }

    public void Trigger(GameObject trigger)
    {
        if(status == "n")
        {
            taxiManager.enabled = false;

            hintHUD.SetText("To start fueling press ENTER");

            bool isConfirmed = carControls.HUD.Confirm.WasPressedThisFrame();
            if(isConfirmed)
            {
                status = "fp";
            }
        }
        else if(status == "fp")
        {
            if(fuel < 1f)
            {
                hintHUD.SetText("Fueling...");
                fuel += fuelSpeed;
            } 
            else
            {
                hintHUD.SetText("Fueled!\nTo continue working, leave fueling station");
            }
        } 
    }
}
