using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeeChaseState : BaseState
{
    private Bee bee;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        bee = (Bee)enemy;
        //Debug.Log("Enter");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;       
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        Transform tempTarget = bee.BeeFoundPlayer();
        if (tempTarget == null)
        {
            //currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, tempTarget.position, currentEnemy.chaseSpeed * Time.deltaTime);
            //currentEnemy.Dir = (tempTarget.position - currentEnemy.transform.position).normalized;
            bee.moveDir = Vector2.zero;
            currentEnemy.anim.SetBool("isAttack", false);
            return;
        }

        Vector2 targetBody = (Vector2)tempTarget.position;
        Collider2D targetCol = tempTarget.GetComponent<Collider2D>();
        if (targetCol != null)
        {
            targetBody = targetCol.bounds.center;
        }

        // 蜜蜂原点在身体下方 centerOffset 处，同样对齐到身体中心再算距离。
        Vector2 beeBody = (Vector2)bee.transform.position + bee.centerOffset;

        Vector2 toTarget = targetBody - beeBody;
        float distance = toTarget.magnitude;

        if (distance > bee.attackDistance)
        {
            // 完整方向（含上下），向量除以长度得到单位向量
            bee.moveDir = toTarget / distance;
        }
        else
        {
            // 到了攻击距离就悬停，避免一直顶进玩家身体里抖动
            bee.moveDir = Vector2.zero;
        }

        // 贴图只按左右翻转
        if (toTarget.x > 0f)
            bee.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (toTarget.x < 0f)
            bee.transform.localScale = new Vector3(1f, 1f, 1f);

        currentEnemy.anim.SetBool("isAttack", distance <= bee.attackDistance);
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        
    }
    
}
