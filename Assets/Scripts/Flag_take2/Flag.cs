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
    public OwnerTypes Owner { get; private set; } = OwnerTypes.None;    //旗の所有者

    private readonly ReactiveProperty<Color> _setColor = new ReactiveProperty<Color>( new Color(0.4f, 0.4f, 0.4f, 0.7f));
    public IObservable<Color> SetColor => _setColor;
    
    private readonly Subject<UniRx.Unit> _changeFlag = new Subject<UniRx.Unit>();
    public IObservable<UniRx.Unit> ChangeFlag => _changeFlag;
    
    private List<NetworkObject> _objectInFlagList = new List<NetworkObject>();  //旗内にいるゲームオブジェクトのリスト

    private OwnerTypes _myMode;  //ゲームモードを格納,敵味方の判定に使用 Host->OwnerTypes.Host  Client->OwnerTypes.Client
    
    [SerializeField] private bool _canChangeOT;

    [SerializeField] private ParticleSystem _particle_red;
    [SerializeField] private ParticleSystem _particle_blue;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    //旗の所有者情報と旗の色を更新
    private void updateOwner()
    {
        // 旗の範囲内にUnitがいるなら
        if (_objectInFlagList.Count > 0)
        {
            var baseOwner = _objectInFlagList.First().GetBehaviour<SimulationBehaviour>().Object.InputAuthority; //先頭要素のInputAuthorityを取得

            // 先頭要素のInputAuthorityと異なるInputAuthorityがある場合 -> 異なる陣営のユニットが中にいる
            if (_objectInFlagList.Exists((e) => e.GetBehaviour<SimulationBehaviour>().Object.InputAuthority != baseOwner))
            {
                Owner = OwnerTypes.None;
                _setColor.Value = new Color(0.4f, 0.4f, 0.4f, 0.7f);
                transform.GetChild(0).GetComponent<Renderer>().material.color = _setColor.Value;    //白色に変更
                transform.GetChild(1).GetComponent<Renderer>().material.color = _setColor.Value;    //白色に変更
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
                Debug.Log("フラグが変更："+changeBool);
                
                _setColor.Value = (_myMode == Owner) ? new Color(0, 0, 1, 0.7f) : new Color(1, 0, 0, 0.7f);   //味方なら青,敵なら赤
                
                transform.GetChild(0).GetComponent<Renderer>().material.color = _setColor.Value;    //色を変更
                transform.GetChild(1).GetComponent<Renderer>().material.color = _setColor.Value;    //色を変更
                transform.GetChild(2).GetComponent<Renderer>().material.SetColor(EmissionColor,_setColor.Value);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Unit")) return;  //Unit以外を除外

        NetworkObject obj = other.gameObject.GetComponent<NetworkObject>();

        if (!_objectInFlagList.Contains(obj))
            _objectInFlagList.Add(obj);

        updateOwner();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Unit")) return;

        NetworkObject obj = other.gameObject.GetComponent<NetworkObject>();

        _objectInFlagList.Remove(obj);   //外に出たオブジェクトをリストから削除

        updateOwner();
    }

    private void Start()
    {
        //敵味方の判断のためにホストかクライアントかを格納
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
        //リスト中でnullのものを削除＆所有者更新　
        //旗の範囲内からDestroyされたものも判定したい、最適なやり方がわからないのでわかる方に教えて頂けるとうれしい（OnTriggerExitでは反応しないかった）
        if (_objectInFlagList.RemoveAll(e => e.Equals(null)) > 0) updateOwner();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_FlagChangeEffect()
    {
        var particle = (_myMode == Owner) ? _particle_blue : _particle_red;
        _particle_blue.Stop(); _particle_red.Stop(); particle.Play();   //パーティクルの再生
        SoundManager.Instance.shotSe((_myMode == Owner) ? SeType.FlagGet : SeType.FlagLost);
    }
}
