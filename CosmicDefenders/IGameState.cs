using SFML.Graphics;

namespace CosmicDefenders;

internal interface IGameState
{
    void Clear(RenderWindow window);
    void Draw(RenderWindow window);
    void Update(RenderWindow window);
    int GetState();
}