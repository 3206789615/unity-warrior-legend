using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{    
    public float attackDistance = 1.2f; // 离玩家这个水平距离以内播攻击动画

    public float bobSpeed = 3f;   // 巡逻上下浮动的快慢
    public float bobHeight = 0.5f; // 浮动强度，0 就不上下动
    public float patrolDistance = 3f;
    public Vector2 moveDir;
    [HideInInspector] public float startX;
    protected override void Awake()
    {
        base.Awake();
        startX = transform.position.x;
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }
    public Transform BeeFoundPlayer()
    {
        Collider2D hit= Physics2D.OverlapCircle(transform.position + (Vector3)centerOffset, checkDistance, attackLayer);
        return hit != null ? hit.transform : null;
    }

    public override bool FoundPlayer()
    {
        return BeeFoundPlayer() != null;
    }

    public override void Move()
    {
        rb.velocity = moveDir * currentSpeed;
    }
    public override void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;

        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 1).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, checkDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, attackDistance);
    }
}
