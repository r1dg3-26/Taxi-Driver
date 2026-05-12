using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using TMPro;

public class TaxiManager : MonoBehaviour
{
    public TMP_Text infoHUD;
    public TMP_Text moneyHUD;
    public TMP_Text hintHUD;

    string status;
    float money;

    List<GameObject> points;
    int ppIndex;
    int doIndex;
    GameObject pickpoint;
    GameObject dropOff;

    private CarInputActions carControls;

    void Awake()
    {
        carControls = new CarInputActions();

        points = new List<GameObject>(
            FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(o => o.CompareTag("Point")));
    }

    void OnEnable()
    {
        carControls.Enable();

        if(status == "wfo")
        {
            hintHUD.SetText("Press ENTER to place new order");
        }
    }

    void OnDisable()
    {
        carControls.Disable();
    }

    void Start() 
    {
        status = "wfo";

        infoHUD.SetText("Waiting for order");
        hintHUD.SetText("Press ENTER to place new order");
        
        UpdateMoney(0f);
    }

    void FixedUpdate()
    {
        bool isConfirmed = carControls.HUD.Confirm.WasPressedThisFrame();
        if(isConfirmed)
        {
            StartRandomOrder();
        }
    }

    public void StartRandomOrder()
    {
        hintHUD.SetText("");
        
        if(status == "wfo")
        {
            ppIndex = Random.Range(0, points.Count);

            do
            {
                doIndex = Random.Range(0, points.Count);
            }
            while(ppIndex == doIndex || Mathf.Abs(ppIndex - doIndex) < 5);
            
            pickpoint = points[ppIndex];
            dropOff = points[doIndex];

            pickpoint.GetComponent<MeshRenderer>().enabled = true;
            infoHUD.SetText("From: " + pickpoint.name + "\nTo: " + dropOff.name + "\nHeading to pickpoint");

            status = "htp";
        }
    }

    public void Trigger(GameObject trigger)
    {
        if(status == "htp")
        {     
            if(trigger == pickpoint)
            {
                trigger.GetComponent<MeshRenderer>().enabled = false;
    dropOff.GetComponent<MeshRenderer>().enabled = true;

                infoHUD.SetText("From: " + pickpoint.name + "\nTo: " + dropOff.name + "\nHeading to drop-off");

                status = "htd";
            }
        }
        else if(status == "htd")
        {     
            if(trigger == dropOff)
            {
                trigger.GetComponent<MeshRenderer>().enabled = false;

                infoHUD.SetText("Waiting for order");

                hintHUD.SetText("Press ENTER to place new order");

                UpdateMoney(100f);

                status = "wfo";
            }
        }
    }

    void UpdateMoney(float diff)
    {
        money += diff;

        moneyHUD.SetText("$" + money);
    }
}
