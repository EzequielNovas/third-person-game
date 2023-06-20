using UnityEngine;
using UnityEngine.UI;

public class BarHp : MonoBehaviour
{
    public int hpMax;
    public float currentHp;
    public Image imageHP;

    void Start()
    {
        currentHp = hpMax;
    }

    void Update()
    {
        CheckHp();
        if(currentHp <= 0)
      gameObject.SetActive(false);
    }

    public void CheckHp()
    {
        imageHP.fillAmount = currentHp / hpMax;
    }
}
