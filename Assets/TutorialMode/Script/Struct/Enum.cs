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
        Minion, //�~�j�I��
        Basic, //��{��
        Assault, //�ˌ���
        Recon, //���R��
        Debuffer, //�f�o�b�t�@�[
        Sniper, //�X�i�C�p�[
        Tank, //�^���N
        Chainbuff, //�`�F�C���o�t
        Funneler, //�t�@���l���𓊂�����l
        Funnel, //�t�@���l��
        Stealth, //�X�e���X
    }
}
