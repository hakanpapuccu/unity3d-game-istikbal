using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int heal;
    [SerializeField] private Gun gun;
    [SerializeField] private TextMeshProUGUI healText;

    public void Damage(int damage)
    {
        heal -= damage;
        HealText(heal);
        if (heal <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        GameManager.instance.Lose();
    }
    public void HealText(int value)
    {
        healText.text = value.ToString();
    }
}
