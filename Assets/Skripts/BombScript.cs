using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class BombScript : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 100f;


    ArrayList allHtTargets = new ArrayList();
    private void OnCollisionEnter(Collision other)
    {
        ExplosionDamage(transform.position, explosionRadius);
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 4f);
    }


    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        allHtTargets.Add(hitColliders);
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (!rb)
                continue;

            NavMeshAgent agent = hitCollider.GetComponent<NavMeshAgent>();
            if (!agent)
                continue;
            EnemyBehaviour eb = agent.GetComponent<EnemyBehaviour>();
            ThirdPersonCharacter tp = agent.GetComponent<ThirdPersonCharacter>();
            eb.enabled = false;
            agent.enabled = false;
            rb.AddExplosionForce(explosionForce, center, explosionRadius);
            StartCoroutine(EnableScripts(eb, agent));

        }
    }

    IEnumerator EnableScripts(EnemyBehaviour enemyBehaviour, NavMeshAgent agent)
    {
        Debug.Log("Vefore Scripts enbaled!");
        yield return new WaitForSeconds(1f);
        enemyBehaviour.enabled = true;
        agent.enabled = true;
        Debug.Log("Scripts enbaled!");

    }

}

