

public class EnemyData
{
    private string enemyName;
    public string EnemyName
    {
        get
        {
            return enemyName;
        }

        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "Scout";
            }
            enemyName = value;
        }
    }


    private int maximumHealth;
    public int MaximumHealth
    {
        get
        {
            return maximumHealth;
        }

        set
        {
            if (value < 1)
            {
                value = 1;
            }
            maximumHealth = value;
        }
    }
    
    private int scoreValue;
    public int ScoreValue
    {
        get
        {
            return scoreValue;
        }

        set
        {
            if (value <= 0)
            {
                value = 0;
            }
            scoreValue = value;
        }
    }
    
    private EnemyData() {}
    
    public EnemyData(string enemyName = "Scout", int maximumHealth = 2, int scoreValue = 100)
    {
        EnemyName = enemyName;
        MaximumHealth = maximumHealth;
        ScoreValue = scoreValue;
    }

    


}



