namespace CosmicDefenders.Entities;

internal class AsteroidFactory
{
    private readonly int _asteroidWidth;
    private readonly int _asteroidMargin;

    public AsteroidFactory(int asteroidWidth = 64, int asteroidMargin = 100)
    {
        _asteroidWidth = asteroidWidth;
        _asteroidMargin = asteroidMargin;
    }

    public List<Asteroid> CreateAsteroids(int count, int startY, int windowWidth)
    {
        List<Asteroid> asteroids = new();

        int totalWidth = count * _asteroidWidth + (_asteroidMargin * (count - 1));
        int startX = (windowWidth - totalWidth) / 2;

        for (int i = 0; i < count; i++)
        {
            asteroids.Add(new Asteroid(startX, startY));
            startX += _asteroidWidth + _asteroidMargin;
        }

        return asteroids;
    }
}