using SFML.Graphics;

namespace CosmicDefenders.Entities
{
    internal interface IBullet
    {
        float PositionX { get; set; }
        float PositionY { get; set; }

        void Draw(RenderWindow window);
        void Update();
    }
}