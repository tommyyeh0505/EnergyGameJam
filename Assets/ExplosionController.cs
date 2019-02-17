using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] GameObject ExplosionPrefab;
    public void ExplodeAtLocation(Vector3 loc)
    {
        GameObject obj = Instantiate(ExplosionPrefab, loc, Quaternion.identity);
        Destroy(obj, 1.0f);
    }

}
