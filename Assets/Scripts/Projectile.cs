using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")] 
    
    [Tooltip("The RigidBody use to move the projectile through the scene.")]
    [SerializeField]
    private Rigidbody projectileRigidBody;

    [Header("Movement")] 
    
    [Tooltip("How quickly it moves through the world. Units per second.")]
    [SerializeField]
    private float movmentSpeed = 40f;

    [Tooltip("How long the projectile lives for")]
    [SerializeField]
    private float lifetime = 5f;

    [Header("Damage")]
    [Tooltip("The amount of health removed when the projectile hits a valid target")]
    [Min(1)]
    [SerializeField]
    private int damageAmount = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CombatTarget") == false)
        {
            return;
        }

        bool foundHealth = other.TryGetComponent(out Health targetHealth);
        if (foundHealth)
        {
            targetHealth.TakeDamage(damageAmount);
        }
        
        //Debug.Log("Projectile overlapped: " + other.gameObject.name);
        
        Destroy(gameObject);
    }


    public void Initialize(Vector3 direction)
    {
        Vector3 normalizedDirection = direction.normalized;

        transform.forward = normalizedDirection;

        projectileRigidBody.linearVelocity = normalizedDirection * movmentSpeed;
    }
}
