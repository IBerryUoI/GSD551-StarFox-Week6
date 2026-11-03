using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    
    [Tooltip("The health component initialized from the selected enemy profile.")]
    [SerializeField]
    private Health myHealth;
    
    public EnemyData enemyData
    {
        get;
        private set;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateBehavior();
    }

    protected virtual void UpdateBehavior()
    {
    }

    protected void InitializeEnemy()
    {
        enemyData = CreateEnemyData();
        
        myHealth.Initialize(enemyData.MaximumHealth);

        LogEnemyData();
    }

    protected virtual EnemyData CreateEnemyData()
    {
        return EnemyProfiles.CreateScout();
    }
    
    private void LogEnemyData()
    {
        Debug.Log("Created " + enemyData.EnemyName +
                  "| Health: " + enemyData.MaximumHealth +
                  "| Score: " + enemyData.ScoreValue) ;
    }
}
