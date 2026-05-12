using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using TMPro;

public class TaxiManager : MonoBehaviour
{
    public TMP_Text pointHUD;
    public TMP_Text moneyHUD;

    string status;
    float money;

    List<GameObject> points;
    int ppIndex;
    int doIndex;
    GameObject pickpoint;
    GameObject dropOff;

    void Awake()
    {
        points = new List<GameObject>(
            FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(o => o.CompareTag("Point")));
    }

    void Start() 
    {
        status = "wfo";

        pointHUD.SetText("Waiting for order");
        
        UpdateMoney(0f);
    }

    public void StartRandomOrder()
    {
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
            pointHUD.SetText("From: " + pickpoint.name + "\nTo: " + dropOff.name + "\nHeading to pickpoint");

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

                pointHUD.SetText("From: " + pickpoint.name + "\nTo: " + dropOff.name + "\nHeading to drop-off");

                status = "htd";
            }
        }
        else if(status == "htd")
        {     
            if(trigger == dropOff)
            {
                trigger.GetComponent<MeshRenderer>().enabled = false;

                pointHUD.SetText("Waiting for order");

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
