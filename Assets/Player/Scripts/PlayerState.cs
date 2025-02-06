public abstract class PlayerState
{
    protected Player player;

    public PlayerState(Player player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
