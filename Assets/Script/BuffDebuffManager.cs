using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffAndDebuff {DAMAGEUP, LIFESTEAL, DAMAGEDOWN, SHIELDBREAK}

public class BuffDebuffManager : MonoBehaviour
{
	public static BuffDebuffManager instance;
	
	Dictionary<BuffAndDebuff, int> playerBuffDebuff = new Dictionary<BuffAndDebuff, int>();
	Dictionary<BuffAndDebuff, int> bossBuffDebuff = new Dictionary<BuffAndDebuff, int>();
	
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
    }

    public void applyBuffDebuffToPlayer(BuffAndDebuff buffDebuff)
	{
		if (playerBuffDebuff.ContainsKey(buffDebuff))
		{
			playerBuffDebuff[buffDebuff] += 1;
		}
		else
		{
			playerBuffDebuff.Add(buffDebuff, 1);
		}
	}
	
	public void applyBuffDebuffToBoss(BuffAndDebuff buffDebuff)
	{
		if (bossBuffDebuff.ContainsKey(buffDebuff))
		{
			bossBuffDebuff[buffDebuff] += 1;
		}
		else
		{
			bossBuffDebuff.Add(buffDebuff, 1);
		}
	}

}
