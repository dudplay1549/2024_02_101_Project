using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //각각의 아이템 개수를 저장하는 변수
    public int crystalCount = 0;                //크리스탈 개수
    public int plantCount = 0;                 //식물 개수
    public int bushCount = 0;                   //수풀 개수
    public int treeCount = 0;                   //나무 개수

    //아이템을 추가하는 함수, 아이템 종류에 따라 해당 아이템의 개수를 증가시킴
    public void AddItem(ItemType itemType)
    {
        //아이템 종류에 따른 동작 수행
        switch (itemType)
        {
            case ItemType.Crystal:
                crystalCount++; //크리스탈 개수 증가
                Debug.Log($"크리스탈 획득! 현재 개수 :{crystalCount}");           //현재 크리스탈 개수 출력
                break;
            case ItemType.Plant:
                plantCount++; //식물 개수 증가
                Debug.Log($"크리스탈 획득! 현재 개수 :{plantCount}");           //현재 식물 개수 출력
                break;
            case ItemType.Bush:
                bushCount++; //수풀 개수 증가
                Debug.Log($"크리스탈 획득! 현재 개수 :{bushCount}");           //현재 수풀 개수 출력
                break;
            case ItemType.Tree:
                treeCount++; //나무 개수 증가
                Debug.Log($"크리스탈 획득! 현재 개수 :{treeCount}");           //현재 나무 개수 출력
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            ShowInventory();                    //인벤토리 출력 함수 호출
        }
    }

    private void ShowInventory()
    {
        Debug.Log("====인벤토리====");
        Debug.Log($"크리스탈:{crystalCount}개");         //크리스탈 개수 출력
        Debug.Log($"식물:{crystalCount}개");           //식물 개수 출력
        Debug.Log($"수풀:{crystalCount}개");           //수풀 개수 출력
        Debug.Log($"나무:{crystalCount}개");           //나무 개수 출력
        Debug.Log("===================");
    }
}
