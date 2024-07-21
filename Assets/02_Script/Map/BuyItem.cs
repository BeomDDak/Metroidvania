using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    // 아이템 저장
[System.Serializable]
public class ShopItem
{
    public string name;
    public int price;
    public bool isOneTimePurchase;
    public bool isPurchased;
}

public class BuyItem : MonoBehaviour
{
    // 아이템 저장한거 리스트로 만들기
    public List<ShopItem> items = new List<ShopItem>();

    // 아이템 이름 텍스트
    public TextMeshProUGUI[] itemTexts;

    // 선택한 아이템 텍스트
    public TextMeshProUGUI selectedItemText;

    // 플레이어 잼 표시 텍스트
    public TextMeshProUGUI playerJamText;

    // 선택할 아이템
    private int selectedItemIndex = -1;

    public GameObject shopPanel;
    public GameObject buyOnlyOne;
    public GameObject dontMoney;
    public GameObject suceesbuy;

    void Start()
    {
        UpdatePlayerMoneyText();
    }

    // 보유한 금액
    void UpdatePlayerMoneyText()
    {
        playerJamText.text = DataManager.instance.jam.ToString();
    }

    // 아이템 선택 버튼
    public void SelectItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < items.Count)
        {
            selectedItemIndex = itemIndex;
            ShopItem item = items[itemIndex];
            selectedItemText.text = item.name;
        }
    }

    // 구매 버튼
    public void PurchaseSelectedItem()
    {
        if (selectedItemIndex == -1)
        {
            return;
        }

        ShopItem item = items[selectedItemIndex];

        if (item.isPurchased && item.isOneTimePurchase)
        {
            GameObject clone = Instantiate(buyOnlyOne);
            clone.transform.SetParent(shopPanel.transform);
            clone.transform.localPosition = Vector3.zero;
            return;
        }

        if (DataManager.instance.jam >= item.price)
        {
            DataManager.instance.jam -= item.price;
            item.isPurchased = true;
            
            switch (selectedItemIndex)
            {
                case 0:
                    DataManager.instance.mapRader.SetActive(true);
                    break;
                case 1:
                    DataManager.instance.potion++;
                    break;
                case 2:
                    DataManager.instance.maxJumpCount++;
                    break;
            }

            GameObject clone = Instantiate(suceesbuy);
            clone.transform.SetParent(shopPanel.transform);
            clone.transform.localPosition = Vector3.zero;
            UpdatePlayerMoneyText();
            SelectItem(selectedItemIndex); // 구매한 아이템 업데이트
        }
        else
        {
            GameObject clone = Instantiate(dontMoney);
            clone.transform.SetParent(shopPanel.transform);
            clone.transform.localPosition = Vector3.zero;
        }
    }
}
