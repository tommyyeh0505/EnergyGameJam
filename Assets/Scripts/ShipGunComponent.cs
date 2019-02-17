using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShipGunComponent : MonoBehaviour
{
    [SerializeField] public GameObject prefabBullet;
    [SerializeField] public float bulletEnergyCost = 0.2f;
    [SerializeField] public float timeBetweenShots = 0.2f;
    [SerializeField] public AudioClip fireAudio;
    private AudioSource audioSource;

    private ShipEnergyComponent energyComponent;
    private Rigidbody2D body;
    private float lastTimeShot;

    void Start()
    {
        energyComponent = GetComponent<ShipEnergyComponent>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("shoot") && (Time.time - lastTimeShot) > timeBetweenShots)
        {
            if (body && energyComponent && energyComponent.HasEnergyRemaining(bulletEnergyCost))
            {
                Instantiate(prefabBullet, body.GetRelativePoint(new Vector2(0, 0.5f)), transform.rotation);
                energyComponent.ReduceEnergy(bulletEnergyCost);
                lastTimeShot = Time.time;
                if (audioSource)
                {
                    audioSource.PlayOneShot(fireAudio, 1);
                }
            }
        }
    }
}
