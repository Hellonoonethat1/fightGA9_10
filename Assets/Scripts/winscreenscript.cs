using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class winscreenscript : MonoBehaviour
{
    public TextMeshProUGUI winMessages;
    public Color[] player_colors;
    // Start is called before the first frame update
    void Start()
    {
      
        winMessages.color = player_colors[PlayerPrefs.GetInt("colorIndex", 0)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
