using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINALBOSS : Enemy
{
    [SerializeField] GameObject HEALTHBAR;

    Animator anim;
    public bool spottedPlayer;
    public bool chaseDistance;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateEnemyStates()
    {
        base.UpdateEnemyStates();
    }
}
