using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

//enum levelNumber {a1,b1,c1,d1,a2,b2,c2,d2,a3,b3,c3,d3,a4,b4,c4,d4 };

public class levelController : MonoBehaviour
{
    //[SerializeField]
    //private levelNumber levelSet;

   // [SerializeField] AudioMixer audioMixer_;

    /// called by the UI buttons
    public void SetLevel(int level)
    {
        Application.LoadLevel(level);
        //ballScript.lives = 3;
    }
    /*
    public void SetSfxVolume(float value)
    {
        audioMixer_.SetFloat("sfx_volu", Mathf.Log10(value) * 50);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer_.SetFloat("music_volu", Mathf.Log10(value) * 50);
    }
    */
}
