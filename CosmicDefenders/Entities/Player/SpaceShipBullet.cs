using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class SpaceShipBullet : IBullet
{
    private Sprite Sprite { get; set; }
    public float PositionX
    {
        get => Sprite.Position.X;
        set => Sprite.Position = new Vector2f(value, Sprite.Position.Y);
    }

    public float PositionY
    {
        get => Sprite.Position.Y;
        set => Sprite.Position = new Vector2f(Sprite.Position.X, value);
    }

    public SpaceShipBullet(float positionX, float positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "bullet.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);

        SoundBuffer shootBuffer = new SoundBuffer(Path.Combine("Assets", "BulletSpaceShipShoot.mp3"));
        Sound shootSound = new Sound(shootBuffer);
        shootSound.Play();
    }

    public void Update()
    {
        Sprite.Position = new Vector2f(Sprite.Position.X, Sprite.Position.Y - 10);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}
