using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class ScoreProfile
{
    public string profileName;
    public int[] scores = new int[16];
    int OverallScore;
    public void UpdateScore() {
        OverallScore = 0;
        foreach (int s in scores)
        {
            OverallScore += s;
        }
    }
}
public static class HighscoreManager
{
    public static ScoreProfile data = new ScoreProfile();
    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        var filepath = Application.persistentDataPath + "/Scores.sve";
        FileStream stream = new FileStream(filepath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

    }
    public static ScoreProfile Load()
    {
        var filepath = Application.persistentDataPath + "/Scores.sve";

        if (File.Exists(filepath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filepath, FileMode.Open);

            data = formatter.Deserialize(stream) as ScoreProfile;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + filepath);
            return null;
        }
    }

    public static void AddScoreToProfile(int Score, int level)
    {
        data.scores[level] = Score;
        data.UpdateScore();
    }
}
