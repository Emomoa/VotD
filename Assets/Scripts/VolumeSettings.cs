using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq; // Add this namespace for ToArray()

public class VolumeSettings : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Volume settings
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public float volumeStep = 0.1f; // The amount to increase or decrease the volume
    public float mutedVolume = 0f;

    void Start()
    {
        // Initialize the keywords and their corresponding actions
        keywords.Add("increase volume", IncreaseVolume);
        keywords.Add("lower volume", DecreaseVolume);
        keywords.Add("mute volume", MuteVolume);
        keywords.Add("unmute volume", UnmuteVolume);
        keywords.Add("maximum volume", MaxVolume);
        keywords.Add("minimum volume", MinVolume);
        keywords.Add("pause", PauseGame);
        keywords.Add("resume", ResumeGame);


        // Convert keywords.Keys to an array and create the KeywordRecognizer
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();

        // Set the initial volume
        UpdateVolume();
    }

    private void OnDestroy()
    {
        // Clean up the recognizer when the object is destroyed
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.OnPhraseRecognized -= OnKeywordsRecognized;
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command recognized: " + args.text);
        keywords[args.text].Invoke();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void IncreaseVolume()
    {
        volume = Mathf.Clamp01(volume + volumeStep);
        UpdateVolume();
        Debug.Log("Volume increased to: " + volume);
    }

    private void DecreaseVolume()
    {
        volume = Mathf.Clamp01(volume - volumeStep);
        UpdateVolume();
        Debug.Log("Volume decreased to: " + volume);
    }

    private void MuteVolume()
    {
        mutedVolume = volume;
        volume = 0f;
        UpdateVolume();
        Debug.Log("Volume muted");
    }

    private void UnmuteVolume()
    {
        if (volume == 0f)
        {
            volume = mutedVolume; // Default unmuted volume
        }
        UpdateVolume();
        Debug.Log("Volume unmuted");
    }

    private void MaxVolume()
    {
        volume = 1f;
        UpdateVolume();
        Debug.Log("Volume set to maximum");
    }

    private void MinVolume()
    {
        volume = 0f;
        UpdateVolume();
        Debug.Log("Volume set to minimum");
    }

    private void UpdateVolume()
    {
        AudioListener.volume = volume;
    }
}
