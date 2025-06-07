using System;
using UniRx;

[Serializable]
public class ReactiveGameStateProperty : ReactiveProperty<GameState>
{
    public ReactiveGameStateProperty()
    {
    }

    public ReactiveGameStateProperty(GameState initState) : base(initState)
    {
    }
}