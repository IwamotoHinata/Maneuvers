using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;

public class TextPath
{
    private static Dictionary<string, string> pathDic = new Dictionary<string, string>()
    {
        {"sec1","sec_scenarios/sec1"},
        {"sec2","sec_scenarios/sec2"},
        {"sec3","sec_scenarios/sec3"},
        {"sec4","sec_scenarios/sec4"},
        {"sec5","sec_scenarios/sec5"},
        {"sec6","sec_scenarios/sec6"},
        {"sec7","sec_scenarios/sec7"},
        {"sec8","sec_scenarios/sec8"},
        {"sec9","sec_scenarios/sec9"},
        {"sec10","sec_scenarios/sec10"},
        {"sec11","sec_scenarios/sec11"},
        {"sec12","sec_scenarios/sec12"},
        {"sec13","sec_scenarios/sec13"},
    };

    public static string Sec1()
    {
        return pathDic["sec1"];
    }
    public static string Sec2()
    {
        return pathDic["sec2"];
    }
    public static string Sec3()
    {
        return pathDic["sec3"];
    }
    public static string Sec4()
    {
        return pathDic["sec4"];
    }
    public static string Sec5()
    {
        return pathDic["sec5"];
    }
    public static string Sec6()
    {
        return pathDic["sec6"];
    }
    public static string Sec7()
    {
        return pathDic["sec7"];
    }
    public static string Sec8()
    {
        return pathDic["sec8"];
    }
    public static string Sec9()
    {
        return pathDic["sec9"];
    }
    public static string Sec10()
    {
        return pathDic["sec10"];
    }
    public static string Sec11()
    {
        return pathDic["sec11"];
    }
    public static string Sec12()
    {
        return pathDic["sec12"];
    }
    public static string Sec13()
    {
        return pathDic["sec13"];
    }
}
