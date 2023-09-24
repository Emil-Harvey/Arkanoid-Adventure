using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class gameSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer_;
    public void SetVolume(bool sfx, float value)
    {
        if (sfx) { audioMixer_.SetFloat("sfx_volu", Mathf.Log10(value) * 50); }
        else { audioMixer_.SetFloat("music_volu", Mathf.Log10(value) * 50); }
    }
    public void SetSFXVolume(float value)
    {
        audioMixer_.SetFloat("sfx_volu", Mathf.Log10(value) * 50);     
    }
    public void SetMusicVolume(float value)
    {
        audioMixer_.SetFloat("music_volu", Mathf.Log10(value) * 50);
    }
    public void SetAudioQuality(float value)
    {
        /// really a binary switch
        if (value > 0.5){
            audioMixer_.SetFloat("qual_lp", 22000);
            audioMixer_.SetFloat("qual_hp", 40);
            Debug.Log("high qual!");
        }
        else{
            audioMixer_.SetFloat("qual_lp", 6000);
            audioMixer_.SetFloat("qual_hp", 1320);
            Debug.Log("low qual!");
        }
    }
    public void QuitGame()
    {
        HighscoreManager.Save();
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
