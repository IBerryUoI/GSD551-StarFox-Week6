
public static class EnemyProfiles 
{
    public static EnemyData CreateScout()
    {
        return new EnemyData("Scout", 2, 100);
    }

    public static EnemyData CreateHeavyFighter()
    {
        return new EnemyData("Heavy Fighter", 6, 200);
    }
    
}
