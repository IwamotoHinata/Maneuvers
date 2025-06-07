
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Unit
{
    public class ChainbuffHeroSkill : HeroSkill
    {
        //このスクリプトはチェインバフユニットだけでなくチェインバフの影響を受けるすべてのユニットに付与されることを想定して作成しています
        //他のユニットとチェインするためにはユニットの子オブジェクトとして"ChainbuffArea"という空のオブジェクトを置き、
        //それに"ChainbuffArea"というタグをつけ、コンポーネントとしてcolliderをつける必要があります。

        private CharacterStatus MyCharacterStatus;
        private CharacterBaseData MyCharacterBaseData;
        private float MaxHp;
        private float healValue = 3;
        private float healHp;
        private GameObject ChainAffectCharacter;
        private SphereCollider sc;
        private int BeforeChainbuffState;
        private int UnitsNum; //味方ユニットの数
        private GameObject ChainCharacter;//自分のチェインバフエリア
        private ChainbuffHeroSkill thisCs; //このスクリプトを格納する。
        private int ChainbuffUnitType;
        private OwnerType Ownertype;
        private int ChainbuffState;//自分のチェイン状態。チェインバフを階層としてみた時自分がどの立ち位置にいるかを数値で示す。数値が小さいほどチェインバフの大元に近い。
        private ChainbuffHeroSkill[] ChainUnitsCs; //このオブジェクトとチェインしているユニットのChainAffectedHeroSkillスクリプトを格納
        private bool ChainbuffActive; //自分がチェインバフ状態か否か
        private ChainbuffHeroSkill ChainAffectCharacterCs; //チェインバフをかけてきた味方のチェインバフに関するスクリプト
        private PlayerDeck deck;


        // Start is called before the first frame update
        void Start()
        {
            //子オブジェクトのチェインバフエリアを先に見つけておく
            ChainCharacter = transform.Find("ChainbuffArea").gameObject;
            sc = ChainCharacter.GetComponent<SphereCollider>();
            thisCs = GetComponent<ChainbuffHeroSkill>();

            MyCharacterBaseData = GetComponent<CharacterBaseData>();
            MyCharacterStatus = GetComponent<CharacterStatus>();

            MyCharacterBaseData
            .OninitialSetting
            .Where(value => value == true)
            .Subscribe(_ =>
            {
                Init();
            }).AddTo(this);

            StartCoroutine(HpHealLoop());
            StartCoroutine(ChangeChainbuffState()); //チェインバフ状態の数値変更があったらチェインバフユニットを除く他の触れているチェインバフ状態のユニットに変更を通達
        }

        private void Init()
        {

            deck = FindObjectOfType<PlayerDeck>();
            UnitsNum = deck.MyPlayerCharacterDeck.getSelectedDeck().Unit.Length;
            ChainUnitsCs = new ChainbuffHeroSkill[UnitsNum];

            MaxHp = MyCharacterBaseData.MaxHP; 
            ChainbuffUnitType = (int)UnitType.Chainbuff;

            //チェインバフのユニットだった時の処理
            if (MyCharacterBaseData.MyCharacterID == ChainbuffUnitType)
            {
                sc.enabled = true;
                ChainbuffState = 0;
            }
            //そうじゃなかった時の処理
            else
            {
                sc.enabled = false;
                ChainbuffState = UnitsNum;
            }
            ChainbuffActive = false;
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {

            //自分のチェインバフエリア以外のチェインバフエリアに触れた時
            if (other.gameObject.CompareTag("ChainbuffArea") && other.transform.root.gameObject != this.gameObject)
            {

                Debug.Log("新たなチェインバフに触れた");
                BeforeChainbuffState = ChainbuffState;
                ChainAffectCharacter = other.transform.root.gameObject; //触れたチェインバフエリアの親であるユニットを取得

                if (MyCharacterBaseData.GetCharacterOwnerType() != ChainAffectCharacter.GetComponent<CharacterBaseData>().GetCharacterOwnerType())
                    return;

                ChainAffectCharacterCs = ChainAffectCharacter.GetComponent<ChainbuffHeroSkill>();

                //もし触れた相手が自分よりもチェインバフの大元に近いなら自分の状態を書き換える
                if (ChainAffectCharacterCs.ChainbuffState < ChainbuffState)
                {
                    ChainbuffState = ChainAffectCharacterCs.ChainbuffState + 1;
                }

                //自分がチェインバフエリアに触れている相手を記録する
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] == null)
                    {
                        ChainUnitsCs[i] = ChainAffectCharacterCs;
                        break;
                    }
                }
                //チェインバフ状態の時子オブジェクトのcolliderをアクティブにする
                ColliderChange();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //自分のチェインバフエリア以外のチェインバフエリアから離れるとき
            if (other.gameObject.CompareTag("ChainbuffArea") && other.transform.root.gameObject != this.gameObject)
            {
                //万が一触れた相手がチェインバフ状態じゃなかったとき処理をやめる
                if (ChainbuffState == UnitsNum)
                    return;

                BeforeChainbuffState = ChainbuffState;
                ChainAffectCharacter = other.transform.root.gameObject; //触れたチェインバフエリアの親であるユニットを取得
                ChainAffectCharacterCs = ChainAffectCharacter.GetComponent<ChainbuffHeroSkill>();//相手のチェインバフ状態を取得

                if (MyCharacterBaseData.GetCharacterOwnerType() != ChainAffectCharacter.GetComponent<CharacterBaseData>().GetCharacterOwnerType())
                    return;

                //触れているものを記録する配列から外す
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] == ChainAffectCharacterCs)
                    {
                        ChainUnitsCs[i] = null;
                        break;
                    }
                }

                ChainbuffStateChange();

                //相手側で処理できてなかったら代わりにやっておく
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainAffectCharacterCs.ChainUnitsCs[i] == thisCs)
                    {
                        ChainAffectCharacterCs.ChainUnitsCs[i] = null;
                        ChainAffectCharacterCs.ExitChainStateChange();//ステートの書き換えを行う関数
                    }

                }

                //子オブジェクトのcolliderを非アクティブにする
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                    ColliderChange();

            }
        }

        private void ChainbuffStateChange()
        {
            //チェインバフ本体で何にも触れていない状態であればチェイン状態を解除する（チェインバフエリアは残したまま）
            if (MyCharacterBaseData.MyCharacterID == ChainbuffUnitType)
            {
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] != null)
                        return;

                    if (i == UnitsNum - 1)
                    {
                        if(ChainbuffActive != false)
                        {
                            ChainbuffActive = false;
                            MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                            MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                        }
                    }
                }
            }

            else
            {
                int MinState = UnitsNum;
                //触れているチェインバフ状態のものがあればそれの下の階層の状態になり、何にも触れていなければチェインバフ状態の数値をチェインしていない状態の数値にする
                for (int i = 0; i < UnitsNum; i++)
                {
                    if (ChainUnitsCs[i] != null)
                    {
                        if (ChainUnitsCs[i].ChainbuffState < MinState)
                            MinState = ChainUnitsCs[i].ChainbuffState;
                    }

                    if (i == UnitsNum - 1)
                    {
                        if (MinState != UnitsNum) ChainbuffState = MinState + 1;

                        else ChainbuffState = UnitsNum;
                        break;
                    }
                }
            }
        }

        public void ExitChainStateChange()
        {

            BeforeChainbuffState = ChainbuffState;

            ChainbuffStateChange();

            //子オブジェクトのcolliderを非アクティブにする
            if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                ColliderChange();
        }

        private void ColliderChange()
        {
            //チェインバフ状態の時子オブジェクトのcolliderをアクティブにする
            if (ChainbuffState < UnitsNum && ChainbuffActive != true)
            {
                sc.enabled = true;
                ChainbuffActive = true;//ステータスを変更
                MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                Debug.Log("チェインバフ発動");
            }
            //子オブジェクトのcolliderを非アクティブにする
            else if (ChainbuffState == UnitsNum && ChainbuffActive != false)
            {
                sc.enabled = false;
                ChainbuffActive = false;
                MyCharacterStatus.ChainbuffStaticHitRateChange(ChainbuffActive);
                MyCharacterStatus.ChainbuffMoveHitRateChange(ChainbuffActive);
                Debug.Log("チェインバフ解除");
            }
        }
        IEnumerator ChangeChainbuffState()
        {
            yield return new WaitUntil(() => BeforeChainbuffState != ChainbuffState);//チェインバフの状態が変わったら
            //Debug.Log("連絡開始");
            for (int i = 0; i < UnitsNum; i++)
            {
                if (ChainUnitsCs[i] != null)
                {
                    // Debug.Log(" 連絡前　状態変化前：" + Before + "状態変化後：" + current);
                    ChainUnitsCs[i].ChainbuffStateChangeReport();//触れている味方に状態を変更したことを伝える

                    if (ChainbuffState == UnitsNum)
                    {
                        // Debug.Log("チェインバフ解除準備");
                        ChainUnitsCs[i] = null;
                    }
                }
            }
            StartCoroutine(ChangeChainbuffState()); //チェインバフ状態の数値変更があったらチェインバフユニットを除く他の触れているチェインバフ状態のユニットに変更を通達
        }

        //他のユニットのチェインバフ状態の数値の変更があった際その変動を記録
        public void ChainbuffStateChangeReport()
        {
            
            //自分がチェインバフかつ触れていたユニットがチェインバフ状態でなかったら
            for (int i = 0; i < UnitsNum; i++)
            {
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                {
                    if (ChainUnitsCs[i] != null && ChainUnitsCs[i].ChainbuffState == UnitsNum)
                    {
                        ChainUnitsCs[i] = null;
                    }
                }
            }

            ChainbuffStateChange();

            if (ChainbuffState == UnitsNum)
            {
                for (int i = 0; i < UnitsNum; i++)
                {
                     if (ChainUnitsCs[i] != null)
                    {
                        ChainUnitsCs[i].ChainbuffStateChangeReport();
                        ChainUnitsCs[i] = null;
                    }
                }
                if (MyCharacterBaseData.MyCharacterID != ChainbuffUnitType)
                    ColliderChange();
            }
           // Debug.Log(" 記録変化前：" + BeforeChainbuffState + "記録変化後：" + ChainbuffState);

        }
        

        //チェインバフ効果による自動回復
        IEnumerator HpHealLoop()
        {
            yield return new WaitUntil(() => MyCharacterStatus.CurrentHp < MaxHp && ChainbuffActive);
            while (ChainbuffActive)
            {
                if (MyCharacterStatus.CurrentHp < MaxHp)
                {
                    healHp = MyCharacterStatus.CurrentHp + healValue;
                    if (healHp > MaxHp)
                        healHp = MaxHp;
                    MyCharacterStatus.ChangeHPValue(healHp);
                    //Debug.Log("チェインバフ回復中");
                    yield return new WaitForSeconds(1);
                    if (healHp >= MaxHp) break;
                }
            }
            StartCoroutine(HpHealLoop());
        }

         

        private void OnDestroy()
        {
            for (int i = 0; i < UnitsNum; i++)
            {
                if (ChainUnitsCs[i] != null)
                {
                    for (int j = 0; j < UnitsNum; j++)
                    {
                        if (ChainUnitsCs[i].ChainUnitsCs[j] == thisCs)
                        {
                            ChainUnitsCs[i].ChainUnitsCs[j] = null;
                            ChainUnitsCs[i].ExitChainStateChange();//ステートの書き換えを行う関数
                        }
                    }
                    ChainUnitsCs[i] = null;

                }

            }
        }
    }
}

