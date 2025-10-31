using SFML.Graphics;

namespace CosmicDefenders.Entities.Enemies;

public interface IEnemy
{
    int Width { get; }
    float PositionY { get; }
    float PositionX { get; }
    int Height { get; }

    public void PositionEnemy(float x, float y);
    public void Draw(RenderWindow window);
}