using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    // ������ ����
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
    // ������ �����Ѱ� ����Ʈ�� �����
    public List<ShopItem> items = new List<ShopItem>();

    // ������ �̸� �ؽ�Ʈ
    public TextMeshProUGUI[] itemTexts;

    // ������ ������ �ؽ�Ʈ
    public TextMeshProUGUI selectedItemText;

    // �÷��̾� �� ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI playerJamText;

    // ������ ������
    private int selectedItemIndex = -1;

    public GameObject shopPanel;
    public GameObject buyOnlyOne;
    public GameObject dontMoney;
    public GameObject suceesbuy;

    void Start()
    {
        UpdatePlayerMoneyText();
    }

    // ������ �ݾ�
    void UpdatePlayerMoneyText()
    {
        playerJamText.text = DataManager.instance.jam.ToString();
    }

    // ������ ���� ��ư
    public void SelectItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < items.Count)
        {
            selectedItemIndex = itemIndex;
            ShopItem item = items[itemIndex];
            selectedItemText.text = item.name;
        }
    }

    // ���� ��ư
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
            SelectItem(selectedItemIndex); // ������ ������ ������Ʈ
        }
        else
        {
            GameObject clone = Instantiate(dontMoney);
            clone.transform.SetParent(shopPanel.transform);
            clone.transform.localPosition = Vector3.zero;
        }
    }
}
