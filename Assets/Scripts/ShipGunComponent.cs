using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGunComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabBullet;
    private ShipEnergyComponent energyComponent;

    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();        
    }

    void Update()
    {
        if (Input.GetButton("shoot"))
        {
            Instantiate(prefabBullet);
        }
    }
}
