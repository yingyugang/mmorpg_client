using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAttribute : MonoBehaviour {


	public float maxHealth = 1000;
	public float currentHealth = 1000;
	
	public float maxPower = 1000;
	public float currentPower = 1000;

	public bool powerRecoverAble = true;
	public float powerRecover = 100;//10 per second;
	public bool healthRecoverAble = true;
	public float healthRecover = 10;//10 per second;

	public Dictionary<Collection,int> items;

	public GUIBar guiHealthBar;
	public GUIBar.BarSide barSide;

	void Start()
	{
		if (guiHealthBar == null) 
		{
			guiHealthBar = gameObject.AddComponent<GUIBar>();	
			guiHealthBar.side = barSide;
		}
	}
	
	void Update()
	{
		guiHealthBar.healthRadiu = currentHealth / maxHealth;
		guiHealthBar.powerRadiu = currentPower / maxPower;
		if(powerRecoverAble)RecoverPower();
		if(healthRecoverAble)RecoverHealth();
	}
	
	void RecoverPower()
	{
		currentPower = Mathf.Clamp (currentPower + powerRecover * Time.deltaTime,0,maxPower);
	}
	
	void RecoverHealth()
	{
		currentHealth = Mathf.Clamp (currentHealth + healthRecover * Time.deltaTime,0,maxHealth);
	}
	
	public void AdjustPower(float power)
	{
		currentPower = Mathf.Clamp (currentPower + power,0,maxPower);
	}
	
	public void AdjustPowerByPercent(float percent)
	{
		AdjustPower (maxPower * percent);
	}
	
	public void AdjustHealth(float damage)
	{
		currentHealth = Mathf.Clamp (currentHealth + damage,0,maxHealth);
		if (currentHealth <= 0) 
		{
			healthRecoverAble = false;
//			if(onDead!=null)
//				onDead();
		}
	}
	
	public void AdjustHealthByPercent(float percent)
	{
		AdjustHealth (maxHealth * percent);
	}

	public float GetHealthPercent()
	{
		return currentHealth / maxHealth;
	}

}
