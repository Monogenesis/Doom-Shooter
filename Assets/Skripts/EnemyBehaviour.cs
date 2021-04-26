using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class EnemyBehaviour : MonoBehaviour
{

    public int health = 100;
    public Transform player;
    NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public Animator animator;
    private bool killed;

    private Rigidbody rb;

    public AudioSource[] screamSources;

    private bool isSeeking;
    public float seekingRadius;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = 100;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit Reaction") || killed)
        {

            if (!screamSources[1].isPlaying)
                screamSources[1].Play();
            if (killed)
            {
                screamSources[1].Stop();
            }
        }
        agent.SetDestination(transform.position);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") && isSeeking)
        {
            agent.SetDestination(player.position);
        }

        if (agent.remainingDistance > agent.stoppingDistance && !killed && isSeeking)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!isSeeking && distanceToPlayer < seekingRadius)
        {
            isSeeking = true;
        }

    }

    public void healthDamage(int damage)
    {
        isSeeking = true;
        if (health > 0 && !killed)
        {
            health -= damage;
            if (Random.Range(0, 2) == 1)
                animator.SetTrigger("Hit");
        }
        else if (!killed)
        {
            health = 0;
            killed = true;
            rb.isKinematic = true;
            animator.SetTrigger("Killed");
            screamSources[0].Play();
            screamSources[1].Stop();
            StartCoroutine(DeleteAfterTime());
        }
    }


    IEnumerator DeleteAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
