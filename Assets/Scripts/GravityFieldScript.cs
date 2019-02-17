using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldScript : MonoBehaviour
{
    List<GravityComponentScript> affected = new List<GravityComponentScript>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RegisterAffected(GravityComponentScript toRegister)
    {
        this.affected.Add(toRegister);
    }

    public void UnregisterAffected(GravityComponentScript toUnregister)
    {
        this.affected.Remove(toUnregister);
    }

    public List<GravityComponentScript> GetAffected()
    {
        return affected;
    }
}
