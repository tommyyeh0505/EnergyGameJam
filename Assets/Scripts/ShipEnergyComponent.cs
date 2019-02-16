using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnergyComponent : MonoBehaviour {
    [SerializeField] public float energy = 100f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public bool HasEnergy()
    {
        return (energy > 0);
    }

    public void ReduceEnergy(float reductionAmt)
    {
        if (reductionAmt > 0)
        {
            if (energy > reductionAmt)
            {
                energy -= reductionAmt;
            }
            else
            {
                energy = 0;
            }
        }
        
    }
}
