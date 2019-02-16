using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEnergyComponent))]
public class RespawnShipScript : MonoBehaviour
{
    public GameObject shipType;
    private ShipEnergyComponent energyComponent;

    // Start is called before the first frame update
    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("reset") && shipType)
        {
            GameObject ship = GameObject.FindGameObjectWithTag("Player");
            if (!ship)
            {
                Instantiate(shipType, Vector3.zero, Quaternion.identity);
            }
            energyComponent.ResetEnergy();
        }
    }
}
