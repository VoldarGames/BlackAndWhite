namespace Assets.Scripts.Interfaces
{
    public interface IKillable
    {
        bool IsDead { get; set; }
        bool CanBeDamaged { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        float NextCanBeDamaged { get; set; }

        void DoDamage(int damage);
        void Die();
        void CheckCanBeDamage(); 
    }
}
