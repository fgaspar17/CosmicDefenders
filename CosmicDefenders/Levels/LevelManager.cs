namespace CosmicDefenders.Levels;

internal class LevelManager
{
    public LevelModel CurrentLevel { get; private set; }
    public bool Uninitilized { get; private set; } = true;
    private List<LevelModel> _levels = new List<LevelModel>();

    public LevelManager()
    {
        _levels.Add(new LevelModel()
        {
            Level = 1,
            EnemySpeedBoost = 0f,
            EnemyNumber = 30,
            EnemyShootBoost = 0f,
            EnemyBoss = false,
        });

        _levels.Add(new LevelModel()
        {
            Level= 2,
            EnemySpeedBoost = 2f,
            EnemyNumber = 30,
            EnemyShootBoost = 0f,
            EnemyBoss = false,
        });

        _levels.Add(new LevelModel()
        {
            Level = 3,
            EnemySpeedBoost = 2f,
            EnemyNumber = 33,
            EnemyShootBoost = 0f,
            EnemyBoss = false,
        });

        _levels.Add(new LevelModel()
        {
            Level = 4,
            EnemySpeedBoost = 2f,
            EnemyNumber = 33,
            EnemyShootBoost = 2f,
            EnemyBoss = false,
        });

        _levels.Add(new LevelModel()
        {
            Level = 5,
            EnemySpeedBoost = 5f,
            EnemyNumber = 36,
            EnemyShootBoost = 2f,
            EnemyBoss = true,
        });
    }

    public void LevelUp()
    {
        if (Uninitilized)
        {
            CurrentLevel = _levels.FirstOrDefault(lv => lv.Level == 1)!;
            Uninitilized = false;
        }
        else
        {
            CurrentLevel = _levels.FirstOrDefault(lv => lv.Level == CurrentLevel.Level + 1)!;
        }
    }
}