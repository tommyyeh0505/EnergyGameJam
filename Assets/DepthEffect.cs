using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthEffect : MonoBehaviour
{
    Vector3 defaultPosition;

    public Vector3 maxOffset = new Vector3(1,1,0);
    public Vector3 minOffset = new Vector3(-1, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newPosX = MapValue(Input.mousePosition.x, 0, Screen.width, minOffset.x, maxOffset.x);
        float newPosY = MapValue(Input.mousePosition.y, 0, Screen.height, minOffset.y, maxOffset.y);
        this.gameObject.transform.position = new Vector3(newPosX, newPosY, 0);
    }

    public float MapValue(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
