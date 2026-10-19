using UnityEngine;

public class BossChaseState : ChaseState
{
    public BossChaseState(Unit unit, StateMachine<State> sm, ChaseStateSO so) : base(unit, sm, so) { }

    public override void FrameUpdate()
    {
        
    }
}
