using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageKeyEventSec1 : BaseMessageKeyEvent
{
    [SerializeField] private CharacterView[] views;
	[SerializeField] private Text CharaName;
    private BGMControl bgm;
    private ScenarioAudio audio;
    private readonly Dictionary<string, CharacterView> viewDic = new Dictionary<string, CharacterView>();
    static int flg;
    private void Awake()
    {
        foreach (var v in views) viewDic.Add(v.name, v);
    }

    private void Start()
    {
        audio = GameManager.Instance.GetComponent<ScenarioAudio>();
        bgm = GameManager.Instance.GetComponent<BGMControl>();
		CharaName.text = "";
        flg = 1;
    }

    public override void Event(int key)
    {
      
        if (flg==1)

        {
            key += 1;
          
        }
        switch (key)
        {
            case 1:
              	bgm.PlayBGM("_bgmScenario1");
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
                viewDic["d"].SetCharacterImage(EmotionType.Empty);
                viewDic["s"].SetCharacterImage(EmotionType.Empty);
                viewDic["h"].SetCharacterImage(EmotionType.Empty);
                viewDic["sor"].SetCharacterImage(EmotionType.Empty);
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                viewDic["seri"].SetCharacterImage(EmotionType.Empty);
                break;

			case 5:
				audio.ShotSE("_seScenario_handClap");
				CharaName.text = "？？？";
				break;

            case 6:
				CharaName.text = "";
				audio.ShotSE("_seScenario_walk");
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 7:
				CharaName.text = "";
                break;

            case 9:
				CharaName.text = "ウィリアム";
                break;

            case 10:
				CharaName.text = "";
                break;

            case 11:
				CharaName.text = "ウィリアム";
                break;

            case 13:
				CharaName.text = "";
                break;

            case 14:
				CharaName.text = "式典スタッフ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 16:
				CharaName.text = "";
                break;

            case 17:
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				CharaName.text = "ウィリアム";
                break;

            case 19:
				CharaName.text = "";
				break;


            case 22:
				CharaName.text = "ウィリアム";
                break;

            case 25:
				CharaName.text = "";
                break;

            case 26:
				bgm.StopBGM();
				CharaName.text = "ウィリアム";
                break;

            case 28:
				CharaName.text = "";
                break;

            case 29:
				audio.ShotSE("_seScenario_explosion");
				bgm.ChangeBGM("_bgmScenario2");
                break;

            case 31:
                viewDic["mimi"].SetCharacterImage(EmotionType.cul);
                break;

            case 32:
				audio.ShotSE("_seScenario_ransha");
                break;

            case 33:
				audio.ShotSE("_seScenario_scream");
                break;

            case 34:
                viewDic["mimi"].SetCharacterImage(EmotionType.cul, true);
				CharaName.text = "ウィリアム";
                break;

            case 36:
				audio.ShotSE("_seScenario_robot1");
				CharaName.text = "";
                break;

            case 37:
				CharaName.text = "ウィリアム";
                break;

            case 38:
				audio.ShotSE("_seScenario_run");
				CharaName.text = "";
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                break;

            case 41:
                viewDic["mimi"].SetCharacterImage(EmotionType.cul, true);
                break;            

			case 42:
				CharaName.text = "ウィリアム";
                break;

            case 44:
				CharaName.text = "";
                break;

            case 45:
				CharaName.text = "ウィリアム";
                break;

            case 46:
				CharaName.text = "";
                break;

            case 47:
				CharaName.text = "ウィリアム";
                break;

            case 49:
				CharaName.text = "";
                break;

            case 50:
				CharaName.text = "暴走AI";
                viewDic["mimi"].SetCharacterImage(EmotionType.cul);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 51:
				CharaName.text = "";
                break;

            case 52:
				audio.ShotSE("_seScenario_shotpre");
				CharaName.text = "ウィリアム";
                viewDic["mimi"].SetCharacterImage(EmotionType.cul, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 53:
				audio.ShotSE("_seScenario_shot");
				CharaName.text = "";
				break;

            case 54:
				CharaName.text = "ウィリアム";
                break;

            case 56:
				CharaName.text = "";
                break;

            case 57:
				CharaName.text = "ウィリアム";
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                break;

            case 58:
				CharaName.text = "";
                break;

            case 59:
				CharaName.text = "ウィリアム";
                break;

            case 62:
				CharaName.text = "";
                break;

            case 63:
				CharaName.text = "ウィリアム";
                break;

            case 64:
				CharaName.text = "";
                break;

            case 66:
				CharaName.text = "ウィリアム";
                break;

            case 67:
				CharaName.text = "";
                break;

            case 68:
				audio.ShotSE("_seScenario_buon");
				CharaName.text = "ファラガット";
                viewDic["d"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 69:
				CharaName.text = "ウィリアム";
                viewDic["d"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 70:
                CharaName.text = "ファラガット";
                viewDic["d"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 71:
                CharaName.text = "ウィリアム";
                viewDic["d"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 72:
                CharaName.text = "ファラガット";
                viewDic["d"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 73:
                CharaName.text = "ウィリアム";
                viewDic["d"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				break;

            case 77:
                CharaName.text = "ファラガット";
                viewDic["d"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				break;

            case 80:
                CharaName.text = "ウィリアム";
                viewDic["d"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 81:
                CharaName.text = "ファラガット";
                viewDic["d"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				break;

            case 85:
                CharaName.text = "ウィリアム";
                viewDic["d"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
           		break;

            case 86:
				CharaName.text = "";
                viewDic["d"].SetCharacterImage(EmotionType.Empty);
                break;

            case 87:
                CharaName.text = "ウィリアム";
                break;

            case 88:
				audio.ShotSE("_seScenario_walk2");
				CharaName.text = "";
                break;

            case 89:
				bgm.ChangeBGM("_bgmScenario3");
                break;

            case 93:
                CharaName.text = "ウィリアム";
                break;

            case 94:
				CharaName.text = "";
                break;

            case 95:
                CharaName.text = "ウィリアム";
                break;

            case 96:
				CharaName.text = "";
                break;

            case 97:
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				CharaName.text = "男性";
                break;

            case 99:
                CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 100:
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				CharaName.text = "男性";
                break;

            case 101:
				audio.ShotSE("_seScenario_walk2");
				CharaName.text = "";
                break;

            case 102:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 103:
				CharaName.text = "";
                viewDic["mimi"].SetCharacterImage(EmotionType.cul2);
                break;

            case 104:
                CharaName.text = "ウィリアム";
                break;

            case 105:
				CharaName.text = "暴走AI";
				viewDic["mimi"].SetCharacterImage(EmotionType.cul);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 106:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["mimi"].SetCharacterImage(EmotionType.cul, true);
                break;

            case 107:
				audio.ShotSE("_seScenario_sniperShot");
				CharaName.text = "";
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
				viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                break;

            case 108:
				audio.ShotSE("_seScenario_sniperShot");
				break;

            case 109:
				CharaName.text = "？？？";
                viewDic["s"].SetCharacterImage(EmotionType.s1q);
                break;

            case 110:
				CharaName.text = "";
                viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 112:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 116:
				CharaName.text = "？？？";
				audio.ShotSE("_seScenario_sniperShot");
                viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 117:
    			CharaName.text = "";
				break;

            case 118:
            	CharaName.text = "？？？";
				break;

            case 119:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
         
            case 121:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;
         
            case 122:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 123:
    			CharaName.text = "";
				break;
       
            case 124:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 126:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 128:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 129:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 130:
				audio.ShotSE("_seScenario_walk2");
    			CharaName.text = "";
				break;
         
            case 131:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 132:
				CharaName.text = "";
                viewDic["h"].SetCharacterImage(EmotionType.h1q);
                break;

			case 133:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1q, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;


            case 135:
				CharaName.text = "？？？";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 136:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 138:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 139:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 141:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
      
            case 144:
				CharaName.text = "ベルタ";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;
          
            case 145:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 146:
				CharaName.text = "ベルタ";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 147:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
        
            case 150:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
    
            case 151:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 152:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 153:
				CharaName.text = "ベルタ";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 154:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
   
            case 155:
				CharaName.text = "ベルタ";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;
      
            case 156:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
      
            case 157:
				CharaName.text = "ベルタ";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;
    
            case 158:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
        
            case 159:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
    
            case 160:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 162:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 163:
				CharaName.text = "ウィリアム";
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
       
            case 164:
				CharaName.text = "ハクメイ";
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
     
            case 165:
				audio.ShotSE("_seScenario_walk2");
				CharaName.text = "";
                viewDic["h"].SetCharacterImage(EmotionType.Empty);
                viewDic["s"].SetCharacterImage(EmotionType.Empty);
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
                break;

            case 166:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 167:
				CharaName.text = "避難民たち";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;
        
            case 168:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 169:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;

            case 170:
				CharaName.text = "ハクメイ";
                viewDic["s"].SetCharacterImage(EmotionType.h1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 171:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 172:
                bgm.ChangeBGM("_bgmScenario4");
				audio.ShotSE("_seScenario_runPeople");
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
        
            case 173:
				CharaName.text = "ハクメイ";
                viewDic["s"].SetCharacterImage(EmotionType.h1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 174:
				CharaName.text = "ウィリアム";
                viewDic["s"].SetCharacterImage(EmotionType.h1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                break;
       
            case 175:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

       
            case 177:
				CharaName.text = "？？？";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1q);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;
       
            case 178:
				CharaName.text = "ベルタ";
				viewDic["sor"].SetCharacterImage(EmotionType.sor1q, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;
       
            case 179:
				CharaName.text = "？？？";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 180:
				CharaName.text = "ソーサレス";
                break;

            case 181:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 182:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 183:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 184:
				audio.ShotSE("DebufferSkill");
				CharaName.text = "";
                break;

            case 185:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 186:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 188:
				audio.ShotSE("_seScenario_punch");
				CharaName.text = "ハクメイ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 189:
				CharaName.text = "";
                break;
      
            case 190:
				CharaName.text = "ハクメイ";
                break;

            case 191:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

            case 192:
				CharaName.text = "ハクメイ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

            case 193:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

            case 195:
				audio.ShotSE("_seScenario_buon");
				CharaName.text = "ファラガット";
                viewDic["sor"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

            case 196:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

            case 198:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 199:
				audio.ShotSE("_seScenario_sniperShot");
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

			case 200:
				CharaName.text = "";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

			case 201:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

			case 202:
				CharaName.text = "ファラガット";
                viewDic["sor"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 203:
                break;

			case 204:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 205:
				CharaName.text = "ファラガット";
                viewDic["sor"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 206:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.s1);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 207:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.s1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 208:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.s1);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 209:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.s1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 210:
				CharaName.text = "ファラガット";
                viewDic["sor"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.s1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 211:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.s1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 212:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.s1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 213:
                break;

			case 214:
				CharaName.text = "ハクメイ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 215:
				audio.ShotSE("_seScenario_wa-pu");
				CharaName.text = "";
                break;

			case 216:
				CharaName.text = "ハクメイ";
                break;

			case 217:
				CharaName.text = "";
                break;

			case 218:
                break;

			case 219:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 220:
				audio.ShotSE("_seScenario_kuubaku");
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 221:
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
				CharaName.text = "";
                break;

			case 222:
				CharaName.text = "ファラガット";
                viewDic["sor"].SetCharacterImage(EmotionType.d1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 223:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.d1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 224:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 225:
                break;

			case 226:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.a1);
                break;

			case 227:
				audio.ShotSE("_seScenario_recon");
				CharaName.text = "ハクメイ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.h1);
				viewDic["s"].SetCharacterImage(EmotionType.a1, true);
                break;

			case 228:
				CharaName.text = "";
                viewDic["sor"].SetCharacterImage(EmotionType.Empty);
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
				viewDic["s"].SetCharacterImage(EmotionType.Empty);
				break;

			case 229:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

			case 230:
                break;

			case 231:
				CharaName.text = "";
                break;

			case 232:
				CharaName.text = "ウィリアム";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                break;

			case 233:
				CharaName.text = "ベルタ";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1, true);
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                break;

			case 234:
                break;

			case 236:
				audio.ShotSE("_seScenario_sniperShot");
				CharaName.text = "";
                viewDic["sor"].SetCharacterImage(EmotionType.Empty);
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
				viewDic["s"].SetCharacterImage(EmotionType.Empty);
                break;

			case 237:
				CharaName.text = "ソーサレス";
                viewDic["sor"].SetCharacterImage(EmotionType.sor1);
				break;

			case 238:
				CharaName.text = "";
                viewDic["sor"].SetCharacterImage(EmotionType.Empty);
				break;

			case 239:
				CharaName.text = "ベルタ";
				viewDic["s"].SetCharacterImage(EmotionType.s1);
				break;

			case 240:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
				break;

			case 241:
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.h1);
				break;

			case 242:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["s"].SetCharacterImage(EmotionType.h1, true);
				break;

			case 243:
				CharaName.text = "";
				break;

			case 244:
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.h1);
				break;

			case 245:
				audio.ShotSE("_seScenario_wa-pu");
				CharaName.text = "";
				break;

			case 246:
				CharaName.text = "ハクメイ";
				break;

			case 247:
				bgm.StopBGM();
				CharaName.text = "？？？";
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
                viewDic["seri"].SetCharacterImage(EmotionType.seri);
				viewDic["s"].SetCharacterImage(EmotionType.sor1, true);
				break;

			case 248:
				CharaName.text = "ソーサレス";
                viewDic["seri"].SetCharacterImage(EmotionType.seri);
                viewDic["s"].SetCharacterImage(EmotionType.sor1, true);
				break;

			case 249:
				break;

			case 250:
				CharaName.text = "セリオン";
                viewDic["seri"].SetCharacterImage(EmotionType.seri);
				viewDic["s"].SetCharacterImage(EmotionType.sor1, true);
				break;

			case 251:
				break;

			case 252:
				CharaName.text = "ソーサレス";
                viewDic["seri"].SetCharacterImage(EmotionType.seri);
                viewDic["s"].SetCharacterImage(EmotionType.sor1, true);
				break;

            case 253:
				CharaName.text = "セリオン";
                viewDic["seri"].SetCharacterImage(EmotionType.seri);
				viewDic["s"].SetCharacterImage(EmotionType.sor1, true);
				break;

            case 256:
                bgm.ChangeBGM("_bgmScenario1");
				CharaName.text = "";
                viewDic["seri"].SetCharacterImage(EmotionType.Empty);
                viewDic["s"].SetCharacterImage(EmotionType.Empty);
                break;

            case 257:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                break;

            case 258:
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                break;

            case 259:
                break;

            case 260:
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                break;

			case 261:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;

			case 262:
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;

			case 263:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;

			case 264:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;

			case 265:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;

			case 266:	
				CharaName.text = "？？？";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi);
				break;

			case 267:
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi, true);
				break;

			case 268:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi, true);
				break;

			case 269:	
				CharaName.text = "ミミちゃん";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi);
				break;

			case 270:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi, true);
				break;

			case 271:	
				CharaName.text = "ミミちゃん";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi);
				break;

            case 272:	
				CharaName.text = "守護知能";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2);
				break;

			case 273:	
				break;
				
			case 274:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2, true);
				break;
				
			case 275:	
				break;
				
			case 276:	
				CharaName.text = "守護知能";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2);
				break;
				
			case 277:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2, true);
				break;
				
			case 278:	
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2, true);
				break;
				
			case 279:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2, true);
				break;
				
			case 280:	
				break;
				
			case 281:	
				break;
				
			case 282:	
				CharaName.text = "守護知能";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi2);
				break;
				
			case 283:	
				break;
				
			case 284:	
				CharaName.text = "ミミちゃん";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi);
				break;

			case 285:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                viewDic["mimi"].SetCharacterImage(EmotionType.mimi, true);
				break;
				
			case 286:	
				break;
				
			case 287:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
				break;
				
			case 288:	
				break;
				
			case 289:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 290:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 291:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 292:	
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 293:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 294:	
				break;
				
			case 295:	
				break;
				
			case 296:	
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
                viewDic["h"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 297:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
                viewDic["h"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 298:	
				break;
				
			case 299:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
                viewDic["h"].SetCharacterImage(EmotionType.Empty);
                viewDic["d"].SetCharacterImage(EmotionType.Empty);
				break;

            case 300:
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                break;

            case 301:
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                break;

            case 302:
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
                break;

            case 305:
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
                break;

			case 306:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 307:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 308:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 309:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 310:	
				break;
				
			case 311:	
				break;
				
			case 312:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 313:	
				CharaName.text = "ウィリアム";
                viewDic["a"].SetCharacterImage(EmotionType.a1);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 314:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 315:	
				break;
				
			case 316:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 317:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 318:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 319:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 320:	
				CharaName.text = "ベルタ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 321:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 322:	
				CharaName.text = "ハクメイ";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1);
                viewDic["d"].SetCharacterImage(EmotionType.d2, true);
				break;
				
			case 323:	
				CharaName.text = "ファラガット";
                viewDic["a"].SetCharacterImage(EmotionType.a1, true);
				viewDic["s"].SetCharacterImage(EmotionType.s1, true);
                viewDic["mimi"].SetCharacterImage(EmotionType.h1, true);
                viewDic["d"].SetCharacterImage(EmotionType.d2);
				break;
				
			case 324:	
				CharaName.text = "";
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
				viewDic["s"].SetCharacterImage(EmotionType.Empty);
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                viewDic["d"].SetCharacterImage(EmotionType.Empty);
				break;
				
			case 325:	
				break;
				
			case 326:	
				break;
				
			case 328:
				bgm.StopBGM();
                viewDic["a"].SetCharacterImage(EmotionType.Empty);
                viewDic["d"].SetCharacterImage(EmotionType.Empty);
                viewDic["s"].SetCharacterImage(EmotionType.Empty);
                viewDic["h"].SetCharacterImage(EmotionType.Empty);
                viewDic["sor"].SetCharacterImage(EmotionType.Empty);
                viewDic["mimi"].SetCharacterImage(EmotionType.Empty);
                viewDic["seri"].SetCharacterImage(EmotionType.Empty);
				break;

        }
    }

    public override void EffectEvent(int key, Action action, Action action2)
    {
        var fade = GameManager.Instance.gameObject.GetComponent<ScenarioFade>();
        switch (key)
        {
            case 0:
                fade.SimpleFade(0.2f, BackGroundType.bg1, action, action2);
                break;
            case 28:
                fade.SimpleFade(0.2f, BackGroundType.bg2, action, action2);
                break;
            case 38:
                fade.SimpleFade(0.2f, BackGroundType.bg3, action, action2);
                break;
            case 88:
                fade.SimpleFade(1.0f, BackGroundType.bg4, action, action2);
                break;
            case 130:
                fade.SimpleFade(0.2f, BackGroundType.bg4, action, action2);
                break;
            case 165:
                fade.SimpleFade(1.0f, BackGroundType.bg4, action, action2);
                break;
            case 169:
                fade.SimpleFade(0.2f, BackGroundType.bg5, action, action2);
                break;
            case 219:
                fade.SimpleFade(1.0f, BackGroundType.bg6, action, action2);
                break;
            case 237:
                fade.SimpleFade(0.2f, BackGroundType.bg5, action, action2);
                break;
            case 246:
                fade.SimpleFade(1.0f, BackGroundType.bg4, action, action2);
                break;
            case 255:
                fade.SimpleFade(1.0f, BackGroundType.bg7, action, action2);
                break;
            case 323:
                fade.SimpleFade(1.0f, BackGroundType.bg8, action, action2);
                break;

        }
    }
}