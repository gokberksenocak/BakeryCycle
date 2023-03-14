using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    private int money;
    public void IncreaseMoney()
    {
        money += 5;
        _moneyText.text = money.ToString();
    }

}
