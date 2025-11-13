using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class SpaceShipBulletExplosion
{
    const int COLUMNS = 5;
    const int ROWS = 8;

    private Sprite Sprite { get; set; }
    private int PositionX { get; set; } = 0;
    private int PositionY { get; set; } = 0;
    private int Width { get; set; }
    private int Height { get; set; }
    public bool IsFinished { get; set; } = false;

    public SpaceShipBulletExplosion(float positionX, float positionY)
    {
        Texture texture = new(Path.Combine("Assets", "StarOut01LtBlue.png"));
        Width = (int)texture.Size.X / COLUMNS;
        Height = (int)texture.Size.Y / ROWS;
        Sprite = new(texture, new IntRect(PositionX, PositionY, Width, Height));
        FloatRect bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2f, bounds.Height / 2f);
        Sprite.Position = new Vector2f(positionX, positionY);
        Sprite.Scale = new Vector2f(0.3f, 0.3f);
    }

    public void Update()
    {
        PositionX += Width;
        if (PositionX >= Sprite.Texture.Size.X)
        {
            PositionX = 0;
            PositionY += Height;
            if (PositionY >= Sprite.Texture.Size.Y)
            {
                IsFinished = true;
            }
        }
        Sprite.TextureRect = new IntRect(PositionX, PositionY, (int)Sprite.Texture.Size.X / COLUMNS, (int)Sprite.Texture.Size.Y / ROWS);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}