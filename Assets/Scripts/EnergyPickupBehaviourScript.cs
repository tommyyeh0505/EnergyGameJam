using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickupBehaviourScript : MonoBehaviour
{
    private GameObject ship;
    [SerializeField] public float energyGain = 10.0f;
    [SerializeField] public float attractDistance = 10.0f;
    [SerializeField] public float attractSpeed = 5f;

    void Start()
    {
        ship = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (ship) {
            Vector2 shipPos = ship.transform.position;
            Vector2 pos = gameObject.transform.position;
            if (Vector2.Distance(shipPos, pos) < attractDistance)
            {
                Vector2 dir = (shipPos - pos).normalized;
                gameObject.transform.Translate(attractSpeed * dir * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            ShipEnergyComponent energyComponent = col.gameObject.GetComponent<ShipEnergyComponent>();
            if (energyComponent)
            {
                energyComponent.GainEnergy(energyGain);
            }
            Destroy(this.gameObject);
        }
    }
}
