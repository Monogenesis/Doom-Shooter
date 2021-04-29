using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class EnemyBehaviour : MonoBehaviour
{

    public static int EnemyCount = 0;
    private Text enemyCounterLabel;
    public int health = 100;
    public Transform player;
    NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public Animator animator;
    private bool killed;

    private Rigidbody rb;

    public AudioSource[] screamSources;

    private bool isSeeking;
    public float seekingRadius = 30f;
    public float alertDetectionRadius = 10f;

    public LayerMask enemyLayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = 100;
        rb = GetComponent<Rigidbody>();
        enemyCounterLabel = GameObject.FindGameObjectWithTag("EnemyCounterLabel").GetComponent<Text>();
        EnemyCount = 0;
    }
    private void Start()
    {
        EnemyCount++;
        UpdateEnemyCountLabel();
    }
    private void UpdateEnemyCountLabel()
    {
        enemyCounterLabel.text = EnemyCount.ToString();
    }
    void Update()
    {
        if (agent.isOnNavMesh)
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

    }

    public void healthDamage(int damage)
    {
        isSeeking = true;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertDetectionRadius, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            EnemyBehaviour enemyBehaviour = hitCollider.GetComponent<EnemyBehaviour>();
            if (!enemyBehaviour)
                continue;

            enemyBehaviour.isSeeking = true;
        }

        if (health > 0 && !killed)
        {
            health -= damage;
            if (Random.Range(0, 4) == 1)
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
        EnemyCount--;
        UpdateEnemyCountLabel();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
