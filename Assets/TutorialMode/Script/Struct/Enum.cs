namespace Tutorial
{
    public enum OwnerType_offline
    {
        Player,
        Enemy,
        None,
    }

    public enum SpawnState_offline
    {
        Auto,
        Manual,
    }

    public enum UnitState_offline
    {
        None,
        Idle,
        Attack,
        Move,
        MoveAttack,
        VigilanceMove,
        Reload,
        Dead,
    }

    public enum UnitType_offline
    {
        Minion, //ミニオン
        Basic, //基本兵
        Assault, //突撃兵
        Recon, //リコン
        Debuffer, //デバッファー
        Sniper, //スナイパー
        Tank, //タンク
        Chainbuff, //チェインバフ
        Funneler, //ファンネルを投げつける人
        Funnel, //ファンネル
        Stealth, //ステルス
    }
}
