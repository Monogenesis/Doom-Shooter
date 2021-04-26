using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileEnvironment : MonoBehaviour
{
    public int damage = 50;
    public float damageInterval = 1.5f;
    float timer;


    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        HealthComponent health = other.gameObject.GetComponent<HealthComponent>();

        if (health && timer > damageInterval)
        {
            health.reduceHealth(damage);
            timer = 0;
        }
    }

}
