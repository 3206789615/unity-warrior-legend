using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BaseState
{    

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {      
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            
        }
        else
        {
            currentEnemy.wait = false;            
        }
    }
    
    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
       
    }

}
