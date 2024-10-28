using System.IO;
using UnityEngine;

[System.Serializable]
public class GameParameters
{
    public float walkSpeed;
    public float sneakSpeed;
    public float acceleration;
    public float ghostSpeed;
    public float attackInterval;
}

public class ParameterLoader : MonoBehaviour
{
    public GameParameters parameters;

    void Awake()
    {
        LoadParameters();
    }

    void LoadParameters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "gameParameters.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            parameters = JsonUtility.FromJson<GameParameters>(jsonData);
        }
        else
        {
            Debug.LogError("Parameter file not found at " + path);
        }
    }
}