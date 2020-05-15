﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	private GameManager gameManager;
	//private ItemReader itemReader;
	
	public List<ItemType> ItemsGot;
	public Dictionary<ItemType,int> ItemsOwn;
	
    public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		ItemsGot=new List<ItemType>();
		ItemsOwn=new Dictionary<ItemType,int>();
		InitItems();

		//test
		for(int i=0;i<(int)ItemType.NUM;i++)
			GetItem((ItemType)i,10);
    }
	public void InitItems()
	{
		for(int i=0;i<(int)ItemType.NUM;i++)
			GetItem((ItemType)i,0);
	}
	
	public void GetItem(ItemType itemType, int num)
	{
		if(ItemsOwn.ContainsKey(itemType))
		{
			ItemsOwn[itemType]+=num;
		}
		else
		{
			ItemsOwn[itemType]=num;
			if(!ItemsGot.Contains(itemType))
			{
				ItemsGot.Add(itemType);
			}
		}
	}
	
	public void ConsumeItem(ItemType itemType, int num)
	{
		if(ItemsOwn.ContainsKey(itemType)&&ItemsOwn[itemType]>=num)
		{
			ItemsOwn[itemType]-=num;
			if(ItemsOwn[itemType]==0)
				ItemsOwn.Remove(itemType);
		}
		else
			Debug.Log("consumeitem error");
	}

}
