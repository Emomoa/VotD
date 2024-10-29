using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Volume settings
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public float volumeStep = 0.1f; 
    public float mutedVolume = 0f;

    public AudioMixer sfxMixer;
    public AudioMixer ambienceMixer;

    void Start()
    {
        
        keywords.Add("increase volume", IncreaseVolume);
        keywords.Add("lower volume", DecreaseVolume);
        keywords.Add("mute volume", MuteVolume);
        keywords.Add("unmute volume", UnmuteVolume);
        keywords.Add("maximum volume", MaxVolume);
        keywords.Add("minimum volume", MinVolume);
        keywords.Add("stop", PauseGame);
        keywords.Add("resume", ResumeGame);
        keywords.Add("increase ambience", IncreaseAmbience);
        keywords.Add("lower ambience", LowerAmbience);
        keywords.Add("increase sound effects", IncreaseSfx);
        keywords.Add("lower sound effects", LowerSfx);


        
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();

        // Set the initial volume
        UpdateVolume();
    }

    private void OnDestroy()
    {
        
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
            volume = mutedVolume;
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

    private void LowerAmbience()
    {
        float currentVolume;
        if (ambienceMixer.GetFloat("MasterVolume", out currentVolume))
        {
            ambienceMixer.SetFloat("MasterVolume", currentVolume - 10f);
        }
    }

    private void IncreaseAmbience()
    {
        float currentVolume;
        if (ambienceMixer.GetFloat("MasterVolume", out currentVolume))
        {
            ambienceMixer.SetFloat("MasterVolume", currentVolume + 10f);
        }
    }

    private void LowerSfx()
    {
        float currentVolume;
        if (sfxMixer.GetFloat("MasterVolume", out currentVolume))
        {
            sfxMixer.SetFloat("MasterVolume", currentVolume - 10f);
        }
    }

    private void IncreaseSfx()
    {
        float currentVolume;
        if (sfxMixer.GetFloat("MasterVolume", out currentVolume))
        {
            sfxMixer.SetFloat("MasterVolume", currentVolume + 10f);
        }
    }

}
