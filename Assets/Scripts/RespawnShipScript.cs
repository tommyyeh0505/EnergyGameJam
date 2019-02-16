using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipEnergyComponent))]
public class RespawnShipScript : MonoBehaviour
{
    public GameObject shipType;

    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
}
