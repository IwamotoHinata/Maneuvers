using System;
using Fusion;
using Online;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ShowUICanvas : MonoBehaviour
{
    [Header("Timer")] [SerializeField] private TMP_Text timerText;
    [SerializeField] private Timer timer;


    [Header("Leader")] 
    private BasicLeader leader;
    [SerializeField] private Image leaderImage;
    [SerializeField] private Image ultSlider;
    [SerializeField] private Image ultCompleteImage;


    [Header("Flag")] [SerializeField] private Flag flag1;
    [SerializeField] private Flag flag2;
    [SerializeField] private Flag flag3;

    //左から123です(旗と配置が違います)。
    [SerializeField] private Image havingFlagImage1;
    [SerializeField] private Image havingFlagImage2;
    [SerializeField] private Image havingFlagImage3;

    
    //上から12345
    [Header("UnitIcon")] [SerializeField] private Image[] iconImages;
    [SerializeField] private Image[] skillIconImages;
    private PlayerDeck _playerDeck;
    
    private AsyncOperationHandle<Sprite> _unitIconHandle;
    private AsyncOperationHandle<Sprite> _skillIconHandle;
    private AsyncOperationHandle<Sprite> _leaderIconHandle;
    private void Start()
    {
        timer
            .Time
            .Subscribe(x => timerText.text = SecToMinString(x))
            .AddTo(this);


        if (GameLauncher.Runner.GameMode == GameMode.Host)
        {
            HostFlagObserver();
        }
        else
        {
            ClientFlagObserver();
        }

        _playerDeck = FindObjectOfType<PlayerDeck>();
        SetUnitIcon();
        SetLeaderIcon();
    }

    private void HostFlagObserver()
    {
        flag1
            .SetColor
            .Subscribe(x => havingFlagImage2.color = x)
            .AddTo(this);

        flag2
            .SetColor
            .Subscribe(x => havingFlagImage3.color = x)
            .AddTo(this);

        flag3
            .SetColor
            .Subscribe(x => havingFlagImage1.color = x)
            .AddTo(this);
    }

    private void ClientFlagObserver()
    {
        flag1
            .SetColor
            .Subscribe(x => havingFlagImage2.color = x)
            .AddTo(this);

        flag2
            .SetColor
            .Subscribe(x => havingFlagImage1.color = x)
            .AddTo(this);

        flag3
            .SetColor
            .Subscribe(x => havingFlagImage3.color = x)
            .AddTo(this);
    }

    public void SetLeaderObserver(LeaderObserver leaderObserver)
    {
        leader = leaderObserver.Leader[RoomPlayer.Local.MyLeader].GetComponent<BasicLeader>();
        leader
            .UltPoint
            .Subscribe(SetUltGauge)
            .AddTo(this);
    }

    private String SecToMinString(int x)
    {
        int min = x / 60;
        int sec = x % 60;
        return min.ToString("00") + ":" + sec.ToString("00");
    }
    
    private void SetUltGauge(float ultPoint)
    {
        ultSlider.fillAmount = (ultPoint / leader.maxUltPoint) * 0.31f;
        if (ultPoint >= leader.maxUltPoint)
        {
            ultCompleteImage.color = Color.green;
        }
        else
        {
            ultCompleteImage.color = new Color(0.4f,0.4f,0.4f);
        }
    }
    
    
    private async void SetLeaderIcon()
    {
        var leaders = _playerDeck.MyPlayerCharacterDeck.getSelectedDeck().Leader;
        String leaderPath = "";
        switch (leaders)
        {
            case 0:
                leaderPath = "UI/Icon/LeaderIcon/Guardian.png";
                break;
            case 1:
                leaderPath= "UI/Icon/LeaderIcon/Origin.png";
                break;
            case 2:
                leaderPath = "UI/Icon/LeaderIcon/Cultist.png";
                break;
        }
        _leaderIconHandle = Addressables.LoadAssetAsync<Sprite>(leaderPath);
        await _leaderIconHandle.Task;
        leaderImage.sprite = _leaderIconHandle.Result;
    }
    private async void SetUnitIcon()
    {
        var units = _playerDeck.MyPlayerCharacterDeck.getSelectedDeck().Unit;
        String unitPath = "";
        String skillPath = "";
        for (int i = 0; i < iconImages.Length; i++)
        {
            switch (units[i])
            {
                case 0:
                    unitPath = "UI/Icon/UnitIcon/Basic.png";
                    skillPath = "UI/Icon/SkillIcon/Basic.png";
                    break;
                case 1:
                    unitPath = "UI/Icon/UnitIcon/Assault.png";
                    skillPath = "UI/Icon/SkillIcon/Assault.png";
                    break;
                case 2:
                    unitPath = "UI/Icon/UnitIcon/Recon.png";
                    skillPath = "UI/Icon/SkillIcon/Recon.png";
                    break;
                case 3:
                    unitPath = "UI/Icon/UnitIcon/Debuff.png";
                    skillPath = "UI/Icon/SkillIcon/Debuff.png";
                    break;
                case 4:
                    unitPath = "UI/Icon/UnitIcon/Sniper.png";
                    skillPath = "UI/Icon/SkillICon/Sniper/png";
                    break;
                case 5:
                    unitPath = "UI/Icon/UnitIcon/Tank.png";
                    skillPath = "UI/Icon/SkillIcon/Tank.png";
                    break;
                case 6:
                case 9:
                case 10:
                    unitPath = null;
                    skillPath = null;
                    break;
            }
            _unitIconHandle = Addressables.LoadAssetAsync<Sprite>(unitPath);
            await _unitIconHandle.Task;
            iconImages[i].sprite = _unitIconHandle.Result;
            _skillIconHandle = Addressables.LoadAssetAsync<Sprite>(skillPath);
            await _skillIconHandle.Task;
            skillIconImages[i].sprite = _skillIconHandle.Result;
        }
    }
}