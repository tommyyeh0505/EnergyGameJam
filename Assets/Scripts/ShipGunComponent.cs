using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGunComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabBullet;
    [SerializeField] public float bulletEnergyCost = 10.0f;
    private ShipEnergyComponent energyComponent;
    private Rigidbody2D body;

    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();
        body = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (Input.GetButton("shoot") && energyComponent && energyComponent.HasEnergyRemaining(bulletEnergyCost))
        {
            if (body)
            {
                Instantiate(prefabBullet, body.GetRelativePoint(new Vector2(0, 0.5f)), transform.rotation);
            }
            energyComponent.ReduceEnergy(bulletEnergyCost * Time.deltaTime);
        }
    }
}
