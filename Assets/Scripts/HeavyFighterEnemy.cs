using System;
using UnityEngine;

public class HeavyFighterEnemy : Enemy
{
    [Header("Heavy Fighter Movement")]
    
    [Tooltip("How quickly the Heavy Fighter moves toward the player (in the X and Y)")]
    [Min(0f)]
    [SerializeField]
    private float trackingSpeed = 2.5f;
    
    private Transform playerTarget;

    private void Awake()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");

        if (playerGO == null)
        {
            //Debug.LogWarning("Heavy Fighter Enemy could not find the Player on Awake()");
            return;
        }

        playerTarget = playerGO.transform;
    }
    
    protected override void Start()
    {
        base.InitializeEnemy();
        
        gameObject.name = enemyData.EnemyName;
    }

    protected override void UpdateBehavior()
    {
        if (playerTarget == null)
        {
            return;
        }

        TrackPlayer();
    }

    private void TrackPlayer()
    {
        Vector3 trackingPosition = playerTarget.position;
        trackingPosition.z = transform.position.z;

        float maximumMovement = trackingSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, trackingPosition, maximumMovement);
    }

    protected override EnemyData CreateEnemyData()
    {
        return EnemyProfiles.CreateHeavyFighter();
    }
    
}
