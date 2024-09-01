using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Settings menu handler.
/// </summary>
public class gameSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public void SetVolume(bool sfx, float value)
    {
        if (sfx) { audioMixer.SetFloat("sfx_volu", Mathf.Log10(value) * 50); }
        else { audioMixer.SetFloat("music_volu", Mathf.Log10(value) * 50); }
    }
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("sfx_volu", Mathf.Log10(value) * 50);     
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("music_volu", Mathf.Log10(value) * 50);
    }
    public void SetAudioQuality(float value)
    {
        /// really a binary switch
        if (value > 0.5){
            audioMixer.SetFloat("qual_lp", 22000);
            audioMixer.SetFloat("qual_hp", 40);
            Debug.Log("high qual!");
        }
        else{
            audioMixer.SetFloat("qual_lp", 6000);
            audioMixer.SetFloat("qual_hp", 1320);
            Debug.Log("low qual!");
        }
    }
    public void QuitGame()
    {
        HighscoreManager.Save();
        Application.Quit();
    }

}
