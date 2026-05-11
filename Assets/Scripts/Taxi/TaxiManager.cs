using UnityEngine;
using TMPro;

public class TaxiManager : MonoBehaviour
{
    public TMP_Text pickpointHUD;
    public TMP_Text moneyHUD;

    string status;
    float money;

    string startPoint;

    void Start() 
    {
        status = "wfo";

        pickpointHUD.SetText("Waiting for order");
        
        UpdateMoney(0f);
    }

    public void Trigger(string name)
    {
        if(status == "wfo")
        {     
            startPoint = name;

            pickpointHUD.SetText("From: " + startPoint + "\nOrder in Progress");

            status = "oip";
        }
        else if(status == "oip")
        {     
            if(name != startPoint)
            {
                pickpointHUD.SetText("Waiting for order");

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
