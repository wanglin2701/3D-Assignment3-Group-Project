using Unity.VisualScripting;

public abstract class BaseState
{
    //contain the instance of enemy class
    //instance of statemachine class

    public Enemy enemy;
    public MeleeEnemy meleeEnemy;
    public StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

}
