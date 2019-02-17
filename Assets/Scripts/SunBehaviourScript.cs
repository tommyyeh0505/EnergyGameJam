using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbitalForceComponent))]
public class SunBehaviourScript : MonoBehaviour
{
    [SerializeField] public float energyGain;
    public GameObject energyCubePrefab;
    public float energySpawnVariance = 5f;
    private OrbitalForceComponent orbitComponent;
    private GameObject ship;

    private void Start()
    {
        orbitComponent = GetComponent<OrbitalForceComponent>();
        ship = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (orbitComponent && ship)
        {
            float distance = Vector2.Distance(orbitComponent.transform.position, ship.transform.position);
            if (orbitComponent.IsInOrbit(distance))
            {
                Instantiate(energyCubePrefab, (Vector2) transform.position + Random.insideUnitCircle * energySpawnVariance, Quaternion.identity);
            }
        }
    }
}
