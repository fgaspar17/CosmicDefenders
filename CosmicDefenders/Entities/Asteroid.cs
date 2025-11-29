using CosmicDefenders.Entities.Enemies;
using CosmicDefenders.Entities.Player;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities;

internal class Asteroid
{
    private int _life = 3;
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

    public float Width { get => Sprite.GetGlobalBounds().Width; }
    public float Height { get => Sprite.GetGlobalBounds().Height; }
    public Asteroid(int positionX, int positionY)
    {
        // Painting the Asteroid
        Texture texture = new(Path.Combine("Assets", "AsteroidsArcade.png"));
        IntRect textureRect = new(64, 192, 64, 64);
        Sprite = new(texture, textureRect);
        Sprite.Position = new Vector2f(positionX, positionY);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }

    public void UpdateSprite()
    {
        Sprite.TextureRect = new IntRect(64 * (1 + (3 - _life)), 192, 64, 64);
    }

    internal bool CollidesWith(IBullet bullet, out bool destroyed)
    {
        bool collisionDetected = false;
        destroyed = false;

        Asteroid asteroid = this;
        if ((bullet.PositionX >= asteroid.PositionX && bullet.PositionX <= asteroid.PositionX + asteroid.Width)
            && bullet.PositionY >= asteroid.PositionY && bullet.PositionY <= asteroid.PositionY + asteroid.Height)
        {
            _life -= 1;
            collisionDetected = true;
            SoundBuffer hitBuffer = new SoundBuffer(Path.Combine("Assets", "PlayerHit.wav"));
            Sound hitSound = new Sound(hitBuffer);
            hitSound.Play();
            UpdateSprite();
        }

        if (_life <= 0)
            destroyed = true;

        return collisionDetected;
    }
}