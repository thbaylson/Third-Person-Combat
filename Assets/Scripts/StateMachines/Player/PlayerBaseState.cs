using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        // Use the CharacterController Component to move the player
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + motion) * deltaTime);
    }

    protected void FaceTarget()
    {
        if(stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookAtVector = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookAtVector.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookAtVector);
    }
}
