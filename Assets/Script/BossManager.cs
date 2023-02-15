using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void testAttack()
    {
        PlayerManager.Instance.takeDamage(15);
        Debug.Log("Rawr, Die");
        TurnManager.Instance.changeTurn();
    }

    public void turnStart()
    {
        testAttack();
    }


}
