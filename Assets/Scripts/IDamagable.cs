using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public interface IDamagable
    {
        bool TakeDamage(int damage, out bool isDead);
        bool IsDead();
        event System.Action OnDeath;
    }

   
}

