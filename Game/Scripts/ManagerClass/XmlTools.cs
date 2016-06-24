using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class Data
{
    public int noteCount;            // 노트 개수
    public List<string> noteType;    // 노트 타입
    public List<int> noteScore;      // 노트 스코어
    public List<float> appearTime;   // 출현시간
    public List<string> esName;      // 노트 효과음 이름.
    public float monsterHP;            // 몬스터 HP
}

public static class XmlTools {
    public static XmlDocument xmlFile = new XmlDocument();
    public static Data data;

    public static IEnumerator LoadData(string songName)
    {
        data = new Data();
        TextAsset textAsset = (TextAsset)Resources.Load(songName, typeof(TextAsset));
        xmlFile.LoadXml(textAsset.text);

        XmlNode rootNode = xmlFile.ChildNodes[1];

        //노트 데이터 넣기.
        XmlNodeList noteList = xmlFile.SelectNodes("/" + rootNode.Name + "/Note");

        data.noteCount = int.Parse(noteList[0].ChildNodes[0].InnerText);

        if (data.noteCount != 0)
        {
            data.noteType = new List<string>(data.noteCount);
            data.noteScore = new List<int>(data.noteCount);
            data.appearTime = new List<float>(data.noteCount);
            data.esName = new List<string>(data.noteCount);

            for (int i = 0; i < data.noteCount; i++)
            {
                data.noteType.Add(noteList[0].ChildNodes[i + 1].ChildNodes[0].InnerText);
                data.appearTime.Add(float.Parse(noteList[0].ChildNodes[i + 1].ChildNodes[1].InnerText));
                data.noteScore.Add(int.Parse(noteList[0].ChildNodes[i + 1].ChildNodes[2].InnerText));
                data.esName.Add(noteList[0].ChildNodes[i + 1].ChildNodes[3].InnerText);
                data.monsterHP += int.Parse(noteList[0].ChildNodes[i + 1].ChildNodes[2].InnerText);
            }
        }
        yield return null;
    }
}
