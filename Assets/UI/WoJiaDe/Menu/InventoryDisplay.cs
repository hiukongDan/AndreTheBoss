﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
	public InventoryPanel inventory;
	public ItemDisplay Prefab_item;
	
	public float width;
	public float height;
	public float size;
	
	public Color color;
	public ItemType currentType;
	
	public Text name;
	public Text typename;
	public Text use;
	public Button usebutton;
	
	private Dictionary<ItemType,int> items;
	private int itemcount;
	
	private GameManager gameManager;
	private ItemReader itemReader;
	
	void Awake()
	{
		usebutton.onClick.AddListener(OnUseBtn);
	}
	
	public void OnEnable()
	{
        if (gameManager == null)
            gameManager = GameObject.FindObjectOfType<GameManager>();
		itemReader=new ItemReader();
		itemReader.ReadFile();
	}

	public void UpdateInventory()
	{
		items=inventory.itemsOwn;
		itemcount=items.Count;
		
		Dictionary<ItemType,int>.Enumerator en=items.GetEnumerator();
		if(this.transform.childCount>itemcount)
		{
			for(int i=itemcount;i<this.transform.childCount;i++)
			{
				Debug.Log("destroying child:----"+i);
				Transform child=this.transform.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
		for(int i=0;i<itemcount;i++)
		{
			if(en.MoveNext())
			{
				if(this.transform.childCount<=i)
				{
					ItemDisplay item=GenItem(i);
					item.type=en.Current.Key;
					item.num=en.Current.Value;
					item.UpdateItemDisplay();
				}
				else
				{
					ItemDisplay item=this.transform.GetChild(i).GetComponent<ItemDisplay>();
					item.type=en.Current.Key;
					item.num=en.Current.Value;
					item.UpdateItemDisplay();
				}
			}
		}
				
        this.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (itemcount<=12?3:(itemcount)/4+1)*height*UnityEngine.Screen.height); 
	}
	
	public ItemDisplay GenItem(int index)
	{
		ItemDisplay newitem=Instantiate<ItemDisplay>(Prefab_item);
		newitem.transform.SetParent(this.transform);
		newitem.inventory=this.GetComponent<InventoryDisplay>();
		UpdateItem(newitem,index);
		newitem.UpdateItemDisplay();
		return newitem;
	}
	
	public void UpdateItem(ItemDisplay item, int index)
	{
		item.index=index;
		item.size=size;
		item.width=width;
		item.height=height;
	}
	
	public void OnItemBtn()
	{
		/*for(int i=0;i<itemcount;i++)
		{
			ItemDisplay item=this.transform.GetChild(i).GetComponent<ItemDisplay>();
			item.gameObject.GetComponent<Image>().color=item.type!=currentType?Color.white:color;
		}*/
		UpdateInventory();
		for(int i=0;i<itemcount;i++)
		{
			ItemDisplay item=this.transform.GetChild(i).GetComponent<ItemDisplay>();
			if(item.type==currentType)
				item.gameObject.GetComponent<Image>().color=color;
		}
		
	}
	
	public void OnUseBtn()
	{
		Item useditem = itemReader.GetItemData(currentType);
		BuffEntry currentBuff=useditem.buff;
		Monster monster=gameManager.gameInteraction.menu.currentMonster;
		if(currentBuff==null)
			return;
		if(currentBuff.counter>=0)
			monster.addBuff(currentBuff.attributeType,currentBuff.value,currentBuff.counter);
		else
			monster.modifyAttribute(currentBuff.attributeType,currentBuff.value);
		monster.UpdateCurrentValue();
		inventory.itemManager.ConsumeItem(currentType, 1);
		UpdateInventory();
	}
	
	public void Update()
	{
		foreach(Transform child in transform)
		{
			child.GetComponent<ItemDisplay>().UpdatePosition();
		}
	}
}
