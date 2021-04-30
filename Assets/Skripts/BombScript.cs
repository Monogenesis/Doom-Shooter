using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class BombScript : MonoBehaviour
{

    public GameObject explosionPrefab;


    ArrayList allHtTargets = new ArrayList();
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 4f);
    }



}

