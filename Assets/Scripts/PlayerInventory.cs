using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //������ ������ ������ �����ϴ� ����
    public int crystalCount = 0;                //ũ����Ż ����
    public int plantCount = 0;                 //�Ĺ� ����
    public int bushCount = 0;                   //��Ǯ ����
    public int treeCount = 0;                   //���� ����

    //�������� �߰��ϴ� �Լ�, ������ ������ ���� �ش� �������� ������ ������Ŵ
    public void AddItem(ItemType itemType)
    {
        //������ ������ ���� ���� ����
        switch (itemType)
        {
            case ItemType.Crystal:
                crystalCount++; //ũ����Ż ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� :{crystalCount}");           //���� ũ����Ż ���� ���
                break;
            case ItemType.Plant:
                plantCount++; //�Ĺ� ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� :{plantCount}");           //���� �Ĺ� ���� ���
                break;
            case ItemType.Bush:
                bushCount++; //��Ǯ ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� :{bushCount}");           //���� ��Ǯ ���� ���
                break;
            case ItemType.Tree:
                treeCount++; //���� ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� :{treeCount}");           //���� ���� ���� ���
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
            ShowInventory();                    //�κ��丮 ��� �Լ� ȣ��
        }
    }

    private void ShowInventory()
    {
        Debug.Log("====�κ��丮====");
        Debug.Log($"ũ����Ż:{crystalCount}��");         //ũ����Ż ���� ���
        Debug.Log($"�Ĺ�:{crystalCount}��");           //�Ĺ� ���� ���
        Debug.Log($"��Ǯ:{crystalCount}��");           //��Ǯ ���� ���
        Debug.Log($"����:{crystalCount}��");           //���� ���� ���
        Debug.Log("===================");
    }
}
