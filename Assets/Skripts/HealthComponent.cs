using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HealthComponent : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public bool isDead;


    public HealthBarScript healthBar;


    private void Start()
    {
        maxHealth = health;
        UpdateProperties();
    }

    public void changeMaxHealth(int value)
    {
        maxHealth += value;
        if (maxHealth < health)
            health = maxHealth;
        UpdateProperties();
    }

    public int increaseHealth(int value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateProperties();
        return health;
    }

    public int reduceHealth(int value)
    {
        health -= value;
        if (health <= 0)
        {
            isDead = true;
        }
        if (isDead && gameObject.name == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
        UpdateProperties();
        return health;
    }

    private void UpdateProperties()
    {
        if (healthBar)
        {
            healthBar.UpdateMaxHealth();
            healthBar.UpdateHealth();
        }
    }

}
