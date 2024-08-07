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
        if (selectedItemIndex == -1)                                // 아무것도 선택 안하면 return
        {
            return;
        }

        ShopItem item = items[selectedItemIndex];

        if (item.isPurchased && item.isOneTimePurchase)             // 한번만 구매할 수 있는 물건을 다시 사면
        {
            GameObject clone = Instantiate(buyOnlyOne);             // 더이상 못산다는 문구 출력
            clone.transform.SetParent(shopPanel.transform);         // 캔버스 안에서
            clone.transform.localPosition = Vector3.zero;           // 정가운데서
            return;
        }

        if (DataManager.instance.jam >= item.price)
        {
            DataManager.instance.jam -= item.price;
            item.isPurchased = true;
            
            switch (selectedItemIndex)
            {
                case 0:                                             // 0번이면 맵 레이더 생성
                    DataManager.instance.mapRader.SetActive(true);
                    break;
                case 1:                                             // 1번이면 포션 갯수 증가
                    DataManager.instance.potion++;
                    break;
                case 2:                                             // 2번이면 맥스 점프 증가
                    DataManager.instance.maxJumpCount++;
                    break;
            }

            GameObject clone = Instantiate(suceesbuy);          // 구매완료 이미지 생성
            clone.transform.SetParent(shopPanel.transform);     // 캔버스 안으로
            clone.transform.localPosition = Vector3.zero;       // 정 가운데에서 생성
            UpdatePlayerMoneyText();                            // 보유 금액 업데이트
            SelectItem(selectedItemIndex);                      // 구매한 아이템 업데이트
        }
        else
        {
            GameObject clone = Instantiate(dontMoney);          // 금액 부족 이미지 생성
            clone.transform.SetParent(shopPanel.transform);     // 캔버스 안으로 
            clone.transform.localPosition = Vector3.zero;       // 정 가운데서 생성
        }
    }
}
