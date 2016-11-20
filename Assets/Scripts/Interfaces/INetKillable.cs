﻿namespace Assets.Scripts.Interfaces
{
    public interface INetKillable
    {
        bool IsDead { get; set; }
        bool CanBeDamaged { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        float NextCanBeDamaged { get; set; }

        void CmdDoDamage(int damage);
        void CmdDie();
        void CheckCanBeDamage(); 
    }
}