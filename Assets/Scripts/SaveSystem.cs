using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private SaveData activeSave;

    private void Awake()
    {
        instance = this;

        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.hotf";

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Save was successful");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.hotf";
        print(path);

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(path, FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Load was successful");
        }
        else
        {
            activeSave = new SaveData();
            activeSave.audioOptions[0] = -10f;
            activeSave.audioOptions[1] = 0f;
            activeSave.audioOptions[2] = -20f;
            activeSave.audioOptions[3] = 0f;

            for (int i = 0; i < activeSave.levelsUnlocked.Length; i++)
            {
                activeSave.levelsUnlocked[i] = false;
            }
            activeSave.levelsUnlocked[0] = true;
        }
    }

    public void DeleteSaveData()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.hotf";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public SaveData GetActiveSave()
    {
        return activeSave;
    }
}

[System.Serializable]
public class SaveData
{
    public float[] audioOptions = new float[10];
    public bool[] levelsUnlocked = new bool[10];
    public float[] highScoresForEndsLevels = new float[5];
}
