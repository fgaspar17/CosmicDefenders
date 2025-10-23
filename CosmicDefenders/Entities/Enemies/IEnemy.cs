using SFML.Graphics;

namespace CosmicDefenders.Entities.Enemies;

public interface IEnemy
{
    int Width { get; }

    public void PositionEnemy(float x, float y);
    public void Draw(RenderWindow window);
}