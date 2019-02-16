using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnergyComponent : MonoBehaviour
{
    [SerializeField] public float energy = 100.0f;
    [SerializeField] public float energyGain = 1.0f;
    [SerializeField] public float maxEnergy = 100.0f;
    [SerializeField] public float minEnergy = 0.0f;

    private void Update()
    {
        GainEnergy(energyGain * Time.deltaTime);
    }

    public bool HasEnergyRemaining(float reduction)
    {
        return energy - reduction > minEnergy;
    }

    public void GainEnergy(float gain)
    {
        energy = Mathf.Clamp(energy + gain, minEnergy, maxEnergy);
    }

    public void ReduceEnergy(float reduction)
    {
        energy = Mathf.Clamp(energy - reduction, minEnergy, maxEnergy);
    }

    public void ResetEnergy() {
        energy = maxEnergy;
    }
}
