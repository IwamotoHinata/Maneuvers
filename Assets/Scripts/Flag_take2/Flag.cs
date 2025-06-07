using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using Online;
using UniRx;
using UniRx.Triggers;

public enum OwnerTypes
{
    Client,
    Host,
    None,
}

public class Flag : NetworkBehaviour
{
    [Networked]
    public OwnerTypes Owner { get; private set; } = OwnerTypes.None;    //���̏��L��

    private readonly ReactiveProperty<Color> _setColor = new ReactiveProperty<Color>( new Color(0.4f, 0.4f, 0.4f, 0.7f));
    public IObservable<Color> SetColor => _setColor;
    
    private readonly Subject<UniRx.Unit> _changeFlag = new Subject<UniRx.Unit>();
    public IObservable<UniRx.Unit> ChangeFlag => _changeFlag;
    
    private List<NetworkObject> _objectInFlagList = new List<NetworkObject>();  //�����ɂ���Q�[���I�u�W�F�N�g�̃��X�g

    private OwnerTypes _myMode;  //�Q�[�����[�h���i�[,�G�����̔���Ɏg�p Host->OwnerTypes.Host  Client->OwnerTypes.Client
    
    [SerializeField] private bool _canChangeOT;

    [SerializeField] private ParticleSystem _particle_red;
    [SerializeField] private ParticleSystem _particle_blue;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    //���̏��L�ҏ��Ɗ��̐F���X�V
    private void updateOwner()
    {
        // ���͈͓̔���Unit������Ȃ�
        if (_objectInFlagList.Count > 0)
        {
            var baseOwner = _objectInFlagList.First().GetBehaviour<SimulationBehaviour>().Object.InputAuthority; //�擪�v�f��InputAuthority���擾

            // �擪�v�f��InputAuthority�ƈقȂ�InputAuthority������ꍇ -> �قȂ�w�c�̃��j�b�g�����ɂ���
            if (_objectInFlagList.Exists((e) => e.GetBehaviour<SimulationBehaviour>().Object.InputAuthority != baseOwner))
            {
                Owner = OwnerTypes.None;
                _setColor.Value = new Color(0.4f, 0.4f, 0.4f, 0.7f);
                transform.GetChild(0).GetComponent<Renderer>().material.color = _setColor.Value;    //���F�ɕύX
                transform.GetChild(1).GetComponent<Renderer>().material.color = _setColor.Value;    //���F�ɕύX
                transform.GetChild(2).GetComponent<Renderer>().material.SetColor(EmissionColor,_setColor.Value);
                //_changeFlag.OnNext(UniRx.Unit.Default);
            }
            else
            {
                var lastOwner = baseOwner == 1 ? OwnerTypes.Host : OwnerTypes.Client;
                var changeBool = Owner != lastOwner;
                Owner = lastOwner;
                if (changeBool && Owner!= OwnerTypes.None)
                {
                    //_changeFlag.OnNext(UniRx.Unit.Default);
                    if (GameLauncher.Runner.GameMode == GameMode.Host)
                    {
                        RPC_FlagChangeEffect();
                    }
                }
                Debug.Log("�t���O���ύX�F"+changeBool);
                
                _setColor.Value = (_myMode == Owner) ? new Color(0, 0, 1, 0.7f) : new Color(1, 0, 0, 0.7f);   //�����Ȃ��,�G�Ȃ��
                
                transform.GetChild(0).GetComponent<Renderer>().material.color = _setColor.Value;    //�F��ύX
                transform.GetChild(1).GetComponent<Renderer>().material.color = _setColor.Value;    //�F��ύX
                transform.GetChild(2).GetComponent<Renderer>().material.SetColor(EmissionColor,_setColor.Value);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Unit")) return;  //Unit�ȊO�����O

        NetworkObject obj = other.gameObject.GetComponent<NetworkObject>();

        if (!_objectInFlagList.Contains(obj))
            _objectInFlagList.Add(obj);

        updateOwner();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Unit")) return;

        NetworkObject obj = other.gameObject.GetComponent<NetworkObject>();

        _objectInFlagList.Remove(obj);   //�O�ɏo���I�u�W�F�N�g�����X�g����폜

        updateOwner();
    }

    private void Start()
    {
        //�G�����̔��f�̂��߂Ƀz�X�g���N���C�A���g�����i�[
        if (GameLauncher.Runner.GameMode == GameMode.Host)
        {
            _myMode = OwnerTypes.Host;

        }
        else if (GameLauncher.Runner.GameMode == GameMode.Client)
        {
            _myMode = OwnerTypes.Client;
        }
    }

    /*
    public override void Spawned()
    {
        base.Spawned();
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyUp(KeyCode.F))
            .Subscribe(_ => _changeFlag.OnNext(UniRx.Unit.Default))
            .AddTo(this);
    }
    */

    private void Update()
    {
        //���X�g����null�̂��̂��폜�����L�ҍX�V�@
        //���͈͓̔�����Destroy���ꂽ���̂����肵�����A�œK�Ȃ������킩��Ȃ��̂ł킩����ɋ����Ē�����Ƃ��ꂵ���iOnTriggerExit�ł͔������Ȃ��������j
        if (_objectInFlagList.RemoveAll(e => e.Equals(null)) > 0) updateOwner();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_FlagChangeEffect()
    {
        var particle = (_myMode == Owner) ? _particle_blue : _particle_red;
        _particle_blue.Stop(); _particle_red.Stop(); particle.Play();   //�p�[�e�B�N���̍Đ�
        SoundManager.Instance.shotSe((_myMode == Owner) ? SeType.FlagGet : SeType.FlagLost);
    }
}
