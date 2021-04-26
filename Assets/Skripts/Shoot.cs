using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public Camera cam;
    public float shootRate = 1f;
    public int damage = 5;
    float timer = 0f;

    public AudioSource shootSound;
    [SerializeField]
    public AudioSource[] shootSources;
    private int playIndex;
    public GameObject muzzle;
    void Start()
    {

    }


    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (timer >= shootRate)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        EnemyBehaviour enemy = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                        enemy.healthDamage(damage);
                    }
                }
                timer = 0;
                muzzle.SetActive(true);
                shootSources[playIndex].Play();
                playIndex = (playIndex + 1) % 3;
            }
            else
            {
                muzzle.SetActive(false);
            }

        }


    }


}
