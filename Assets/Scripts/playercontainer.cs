using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playercontainer : MonoBehaviour
{

    public TextMeshProUGUI scoretext;
    public Image healthBarFill;
    public Image chargeBarFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScoreText(int score)
    {
        scoretext.text = score.ToString();
    }

    public void updateChargeBar(float chargedmg, float maxchargedmg)
    {
        chargeBarFill.fillAmount = chargedmg / maxchargedmg;
    }
    public void updateHealthBar(int curHp, int maxHp)
    {
        healthBarFill.fillAmount = (float)curHp / (float)maxHp;
    }

    public void initialize(Color color)
    {
        scoretext.color = color;
        healthBarFill.color = color;
        scoretext.text = "0";
        healthBarFill.fillAmount = 1;
        chargeBarFill.fillAmount = 1;
    }

}
