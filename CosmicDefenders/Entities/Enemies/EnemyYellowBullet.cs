using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyYellowBullet : IBullet
{
    private RectangleShape Shape { get; set; }
    public float PositionX
    {
        get => Shape.Position.X;
        set => Shape.Position = new Vector2f(value, Shape.Position.Y);
    }

    public float PositionY
    {
        get => Shape.Position.Y;
        set => Shape.Position = new Vector2f(Shape.Position.X, value);
    }

    public EnemyYellowBullet(float positionX, float positionY)
    {
        Shape = new();
        Shape.Size = new Vector2f(5, 15);
        Shape.FillColor = Color.Yellow;
        Shape.Position = new Vector2f(positionX, positionY);

        SoundBuffer shootBuffer = new SoundBuffer(Path.Combine("Assets", "BulletEnemyShoot.mp3"));
        Sound shootSound = new Sound(shootBuffer);
        shootSound.Play();
    }

    public void Update()
    {
        Shape.Position = new Vector2f(Shape.Position.X, Shape.Position.Y + 2);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Shape);
    }
}