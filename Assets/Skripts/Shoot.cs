using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{

    public Camera cam;
    public GameObject muzzle;
    public Slider slider;
    public AudioSource shootAudio;

    [Header("Weapon Settings")]
    public float shootRate = 1f;
    public int damage = 5;

    public float magazineSize = 42;

    public float reloadTime = 2;

    float timer = 0f;

    private float ammunitionCount;
    private Text ammunitionCountLabel;
    private int playIndex;

    private AudioSource[] shootAudioSources;

    private float realoadTimer = 2;
    private bool needToReload;


    void Start()
    {
        ammunitionCount = magazineSize;
        ammunitionCountLabel = GameObject.FindGameObjectWithTag("AmmunitionCountLabel").GetComponent<Text>();
        slider.maxValue = reloadTime;
        slider.minValue = 0;
        UpdateAmmunitonCountLabel();
        shootAudioSources = new AudioSource[5];
        for (int i = 0; i < shootAudioSources.Length; i++)
        {
            shootAudioSources[i] = shootAudio;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;
        realoadTimer += Time.deltaTime;

        if (!needToReload && Input.GetKeyDown(KeyCode.R))
        {
            needToReload = true;
            StartCoroutine(Reload());
        }
        if (needToReload)
        {
            slider.value = reloadTime - realoadTimer;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!needToReload && timer >= shootRate)
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
                shootAudioSources[playIndex].Play();
                playIndex = (playIndex + 1) % 3;

                if (ammunitionCount <= 0)
                {
                    needToReload = true;
                    StartCoroutine(Reload());
                }
                else
                {
                    ammunitionCount--;
                }
                UpdateAmmunitonCountLabel();
            }
            else
            {
                muzzle.SetActive(false);
            }

        }
        else
        {
            muzzle.SetActive(false);
        }


    }

    private void UpdateAmmunitonCountLabel()
    {
        ammunitionCountLabel.text = ammunitionCount + "/∞";
    }
    IEnumerator Reload()
    {
        realoadTimer = 0;
        yield return new WaitForSeconds(reloadTime);
        ammunitionCount = magazineSize;
        needToReload = false;
        UpdateAmmunitonCountLabel();
    }
}
