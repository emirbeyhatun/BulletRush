using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletRushGame
{
    public interface IDamagable
    {
        bool TakeDamage(int damage);
        bool IsDead();
        event System.Action OnDeath;
    }

   
}

