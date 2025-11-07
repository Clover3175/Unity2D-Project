using System.IO;
using UnityEngine;

public class SaveSystem 
{
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "ProjectSave.json");


    //데이터를 JSON형식으로 저장
    public static void Save(GameData data, bool prettyPrint = true)
    {
        //객체를 JSON형식으로(문자열로) 변환
        string json = JsonUtility.ToJson(data, prettyPrint);

        //문자열을 파일로 저장
        File.WriteAllText(SavePath, json);
    }
    //JSON 파일을 읽어서 객체로 돌리기(로드)
    public static bool TryLoad(out GameData data)
    {
        //저장파일이 있는지 확인
        if (!File.Exists(SavePath))
        {
            data = null;
            return false;
        }

        string json = File.ReadAllText(SavePath);

        data = JsonUtility.FromJson<GameData>(json);

        return true;
    }
}
