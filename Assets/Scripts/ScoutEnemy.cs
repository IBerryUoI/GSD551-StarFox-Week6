using UnityEngine;

public class ScoutEnemy : Enemy
{
    [Header("Scout Movement")]
    
    [Tooltip("The maximum horizontal distance the Scout travels from its starting location")]
    [Min(0f)]
    [SerializeField]
    private float horizontalDistance = 2f;

    [Tooltip("How quickly the Scout advances through its horizontal movement")]
    [Min(0f)]
    [SerializeField]
    private float movementFrequency = 2f;

    private Vector3 startingLocalPosition;
    private float elapsedMovementTime;


    private void Awake()
    {
        startingLocalPosition = transform.localPosition;
    }

    protected override void Start()
    {
        base.InitializeEnemy();
        
        gameObject.name = enemyData.EnemyName;
    }
    
    protected override void UpdateBehavior()
    {
        elapsedMovementTime += Time.deltaTime;
        MoveScout();
    }
    

    private void MoveScout()
    {
        float movementTime = elapsedMovementTime * movementFrequency;

        float horizontalOffset = Mathf.Sin(movementTime) * horizontalDistance;

        Vector3 movementPosition = startingLocalPosition;
        movementPosition.x += horizontalOffset;

        transform.localPosition = movementPosition;
    }
}
