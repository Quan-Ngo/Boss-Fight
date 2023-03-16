using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
	
    public Animator animator;
	public AudioClip[] audioClips;
	private AudioSource audioSource;
    [SerializeField] private BuffManager BuffManager;

    //Player Statistics
    [SerializeField] private int AP = 5;
    public int Damage = 5;
    public float Lifesteal = 0f;
	public int Defense = 4;
	[SerializeField] private int MaxHealth;
    private int Health;
	private int Block = 0;

    //Player Temp Statistics
    public int TempDamage = 0;
    public int TempDefense;
    public float TempLifesteal = 0f;

    //Player UI Elements
    [SerializeField] private Text APText;
    [SerializeField] private HealthBar HealthBar;
	
	private bool animationLock;


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
		
		Health = MaxHealth;
        HealthBar.SetMaxHealth(Health);
		animationLock = false;
		audioSource = GetComponent<AudioSource>();
    }

    public void turnStart()
    {
        AP = 5;
        APText.text = ("AP: " + AP.ToString());

        BuffManager.updateBuffs();
    }

    public void attack()
    {
        if (AP >= 1 && !animationLock) {
            updateAP(-1);
			audioSource.clip = audioClips[0];
			playAnimationClip("attack");
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }
	
	public void dealDamage()
	{
		int damageDealt;
		damageDealt = Damage + TempDamage;
		
		if (damageDealt < 0)
		{
			damageDealt = 0;
		}
		
		BossManager.Instance.takeDamage(damageDealt);

		if(Lifesteal + TempLifesteal > 0)
		{
			int DmgToHP = (int) ( (damageDealt) * (Lifesteal + TempLifesteal) );
			heal(DmgToHP);
		}
	}
		
    public void defend()
    {
        if (AP >= 2 && !animationLock)
        {
            updateAP(-2);
			audioSource.clip = audioClips[2];
			audioSource.Play();
            Block += Defense + TempDefense; //Temporary Value for Block
            HealthBar.SetBlock(Block);
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void buff()
    {
        if (AP >= 2 && !animationLock)
        {
            updateAP(-2);
			audioSource.clip = audioClips[1];
			playAnimationClip("buff");
            Buff DamageBuff = new Buff("DamageBuff" ,Type.Buff, Stats.Damage, 1, -1, "Damage incresed by 1 per stack.", 1);
            BuffManager.addBuff(DamageBuff);
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    public void lifesteal()
    {
        if (AP >= 3 && !animationLock)
        {
            updateAP(-3);
			audioSource.clip = audioClips[1];
			playAnimationClip("buff");
            Buff LifestealBuff = new Buff("LifestealBuff", Type.Buff, Stats.Lifesteal, 34, 3, "Attacks heal for 30% of damage dealt.", -1);
            BuffManager.addBuff(LifestealBuff);
        }
        else
        {
            Debug.Log("Not Enough AP");
        }
    }

    private void updateAP(int amount)
    {
        AP += amount;
        APText.text = ("AP: " + AP.ToString());
    }

    public void takeDamage(int Damage)
    {
		animator.SetTrigger("GetHit");
        if (Block >= Damage)
        {
            Block -= Damage;
            HealthBar.SetBlock(Block);
        }
        else
        {
            Damage -= Block;
            Block = 0;
            HealthBar.SetBlock(Block);
            Health -= Damage;
            HealthBar.SetHealth(Health);
        }
		
		if (Health <= 0)
		{
			audioSource.clip = audioClips[3];
			playAnimationClip("death");
		}
    }

    public void heal(int amount)
    {
        if (Health + amount >= MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += amount;
        }

        HealthBar.SetHealth(Health);
    }

    public void addBuff(Buff buff)
    {
        BuffManager.addBuff(buff);
    }

    public void removeBuff(Buff buff)
    {
        BuffManager.removeBuff(buff);
    }

    public void addStatBuff(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                TempDefense += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Damage:
                TempDamage += buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Lifesteal:
                TempLifesteal += ((float) (buff.buffValue.Value)) / 100f;
                break;
        }

        ActionManager.Instance.updateTips();
    }

    public void removeStatBuff(Buff buff)
    {
        switch (buff.buffValue.Stat)
        {
            case Stats.Block:
                TempDefense -= buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Damage:
                TempDamage -= buff.buffValue.Value * buff.Stacks;
                break;
            case Stats.Lifesteal:
                TempLifesteal -= ((float)(buff.buffValue.Value)) / 100f;
                break;
        }

        ActionManager.Instance.updateTips();
    }

    public void endAnimationLock()
	{
		animationLock = false;
	}
	
	private void playAnimationClip(string animationToPlay)
	{
		animationLock = true;
		
		switch (animationToPlay)
		{
			case "attack":
				animator.SetTrigger("Attack");
				break;
			case "buff":
				animator.SetTrigger("Buff");
				break;
			case "death":
				animator.SetTrigger("Death");
				break;
		}
	}
}
