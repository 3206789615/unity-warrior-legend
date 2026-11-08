using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class Character : MonoBehaviour
{
    public VoidEvent newGameEvent;

    public float maxHealth;
    public float currentHealth;

    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public bool shellInvulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent onDie;
      

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter<=0)
            {
                invulnerable = false;
            }
        }
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            onDie?.Invoke();
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable || shellInvulnerable)
            return;
        if (currentHealth- attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            onDie?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    
}
