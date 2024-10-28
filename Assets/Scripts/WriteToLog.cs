using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteToLog : MonoBehaviour
{
    private string filename = "";
    private int _sessionCounter;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
        _sessionCounter = PlayerPrefs.GetInt("SessionCounter", 0);
        _sessionCounter++;
        PlayerPrefs.SetInt("SessionCounter", _sessionCounter);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
        // Save sessoion counter to PlayerPrefs
        PlayerPrefs.SetInt("SessionCounter", _sessionCounter);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/LogFile.text";
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(filename, true);
        
        tw.WriteLine("Play session: " + PlayerPrefs.GetInt("SessionCounter", 0) + "," + " [" + DateTime.Now + "]" + "," + logString + ",");
        
        tw.Close();
    }
}
