using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{

    public float timeBetweenAttacks = 2f;

    public int attackDamage = 10;

    public AudioSource attackSound;

    public GameObject player;

    public HealthComponent playerHealth;
    bool isPlayerInRange;

    float timer;

    private Animator anim;

    public LayerMask attackables;


    void Awake()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        playerHealth = player.GetComponent<HealthComponent>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && Physics.CheckSphere(transform.position, 1.3f, attackables) && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            Attack();
        }
    }
    void Attack()
    {
        timer = 0;
        if (attackSound != null)
            attackSound.Play();

        anim.SetTrigger("Attack");

        playerHealth.reduceHealth(attackDamage);

    }
}