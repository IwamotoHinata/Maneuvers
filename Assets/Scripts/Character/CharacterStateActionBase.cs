using Fusion;

public abstract class CharacterStateActionBase : NetworkBehaviour
{
    public abstract void StartStateAction();
    public abstract void StateAction();
    public abstract void EndStateAction();
}
