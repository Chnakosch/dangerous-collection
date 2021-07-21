using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameObject Canvas, Health, Ammo, Player, ReloadTimer, ReloadText;
    public GameObject BatteryCheck, WheelCheck, FuelCheck;
    private GameObject Car;

    private float reloadTimer = 0;
    void Start()
    {
        Car = GameObject.Find("CarBody");

        if(Canvas.activeSelf)
        {
            BatteryCheck.SetActive(false);
            WheelCheck.SetActive(false);
            FuelCheck.SetActive(false);
        }
    }
    void Update()
    {
        if(Canvas.activeSelf)
        {
            Health.transform.localScale = new Vector3(GetComponent<PlayerController>().health*0.01f,1,1);
            if(GetComponent<PlayerController>().weaponInHand != null)
                Ammo.GetComponent<UnityEngine.UI.Text>().text = GetComponent<PlayerController>().weaponInHand.GetComponent<WeaponScript>().currentAmmo.ToString() + " / " + 
                                                                GetComponent<PlayerController>().weaponInHand.GetComponent<WeaponScript>().maxAmmo.ToString();

            if(Input.GetButtonDown("Reload") && !GetComponent<PlayerController>().reloading)
                reloadTimer = GetComponent<PlayerController>().weaponInHand.GetComponent<WeaponScript>().reloadTime;       

            if(GetComponent<PlayerController>().reloading)
            {
                ReloadTimer.SetActive(true);
                ReloadText.SetActive(true);        
                ReloadTimer.GetComponent<UnityEngine.UI.Text>().text = (reloadTimer -= Time.deltaTime).ToString("F1");
            }
            else
            {
                ReloadTimer.SetActive(false);
                ReloadText.SetActive(false);
            }
            
            if(GetComponent<PlayerController>().inventory != null)
            {
                if(GetComponent<PlayerController>().inventory.Equals(Car.GetComponent<CarScript>().neededBattery))
                    BatteryCheck.SetActive(true);
                if(GetComponent<PlayerController>().inventory.Equals(Car.GetComponent<CarScript>().neededWheel))
                    WheelCheck.SetActive(true);
                if(GetComponent<PlayerController>().inventory.Equals(Car.GetComponent<CarScript>().neededFuel))
                    FuelCheck.SetActive(true);
            }
            
            if(Car.GetComponent<CarScript>().collectedStuff.Exists(o=>o == Car.GetComponent<CarScript>().neededBattery))
                BatteryCheck.GetComponent<UnityEngine.UI.Text>().color = Color.red;
            if(Car.GetComponent<CarScript>().collectedStuff.Exists(o=>o == Car.GetComponent<CarScript>().neededWheel))
                WheelCheck.GetComponent<UnityEngine.UI.Text>().color = Color.red;
            if(Car.GetComponent<CarScript>().collectedStuff.Exists(o=>o == Car.GetComponent<CarScript>().neededFuel))
                FuelCheck.GetComponent<UnityEngine.UI.Text>().color = Color.red;
        }
    }
}
