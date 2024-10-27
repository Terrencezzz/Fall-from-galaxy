using UnityEngine;

public class BloaterBehaviour : EnemyBase
{
    protected override void Start()
    {
        maxHealth = 15;
        damageToPlayer = 5;
        speed = 2f;
        detectionRange = 10f; // Bloaters have shorter detection range
        base.Start();
    }
}
