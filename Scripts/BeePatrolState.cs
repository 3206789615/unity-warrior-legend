using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    private Bee bee;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        bee = (Bee)enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        bee.startX = bee.transform.position.x;
    }
    public override void LogicUpdate()
    {
        if (bee.BeeFoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
            return;
        }

        float leftBoundary = bee.startX - bee.patrolDistance;
        float rightBoundary = bee.startX + bee.patrolDistance;

        if (bee.transform.position.x >= rightBoundary)
        {
            // 你当前的代码中，scale.x = 1 表示向左
            bee.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (bee.transform.position.x <= leftBoundary)
        {
            // scale.x = -1 表示向右
            bee.transform.localScale = new Vector3(-1, 1, 1);
        }

        float bob = Mathf.Sin(Time.time * bee.bobSpeed) * bee.bobHeight;
        bee.moveDir = new Vector2(currentEnemy.faceDir.x, bob);
    }

    public override void OnExit()
    {
       
    }

    public override void PhysicsUpdate()
    {
        
    }
}
