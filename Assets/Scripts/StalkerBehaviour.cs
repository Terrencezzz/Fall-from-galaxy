using UnityEngine;

public class StalkerBehaviour : EnemyBase
{
    protected override void Start()
    {
        maxHealth = 5;
        damageToPlayer = 10;
        speed = 3.5f;
        detectionRange = 15f; // Stalkers have longer detection range
        base.Start();
    }
}
