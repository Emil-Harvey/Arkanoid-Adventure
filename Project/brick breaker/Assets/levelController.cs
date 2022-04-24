using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum levelNumber {a1,b1,c1,d1,a2,b2,c2,d2,a3,b3,c3,d3,a4,b4,c4,d4 };

public class levelController : MonoBehaviour
{
    //[SerializeField]
    //private levelNumber levelSet;

    /// called by the UI buttons
    public void SetLevel(int level)
    {
        Application.LoadLevel(level);
        //ballScript.lives = 3;
    }


}
