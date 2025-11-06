namespace CosmicDefenders.Entities;

internal interface IShooter
{
    bool TryShoot(out IBullet? bullet);
}