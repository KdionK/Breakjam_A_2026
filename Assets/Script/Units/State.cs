using UnityEngine;

public class State
{
    protected Unit unit;
    protected IStateMachine stateMachine;

    protected State(Unit unit, IStateMachine stateMachine)
    {
        this.unit = unit;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
    }

    public virtual void FrameUpdate()
    {

    }
    public virtual void PhysicUpdate()
    {
    
    }

    public virtual void CheckTransition()
    {
        
    }
}
