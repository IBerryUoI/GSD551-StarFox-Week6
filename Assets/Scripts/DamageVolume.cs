using System;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [Header("Damage")]
    
    [Tooltip("The amount of health removed when the player enters this volume")]
    [Min(1)]
    [SerializeField]
    private int damageAmount = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
        {
            return;
        }

        bool foundHealth = other.TryGetComponent(out Health playerHealth);
        if (foundHealth == true)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
