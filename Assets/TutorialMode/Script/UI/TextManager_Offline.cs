using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class TextManager_Offline : MonoBehaviour
{
    public int Senario = 0;
    [SerializeField]
    private TextMeshProUGUI storytext;
    private TextAsset csvFile;
    private List<string[]> csvData = new List<string[]>();

    // Start is called before the first frame update
    void Start()
    {
        Senario = 1;

        csvFile = Resources.Load("CSV/hints1") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        //‚æ‚Ý‚±‚Þ
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(','));
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (Senario)
        {
            case 1:
                {
                    storytext.text = csvData[0][0];
                    break;
                }
            case 2:
                {
                    storytext.text = csvData[0][1];
                    break;
                }
            case 3:
                {
                    storytext.text = csvData[0][2];
                    break;
                }
            case 4:
                {
                    storytext.text = csvData[0][3];
                    break;
                }
            case 5:
                {
                    storytext.text = csvData[0][4];
                    break;
                }
            case 6:
                {
                    storytext.text = csvData[0][5];
                    break;
                }
            case 7:
                {
                    storytext.text = csvData[0][6];
                    break;
                }
        }
    }
    public void changeSenario()
    {
        Senario++;
    }
}