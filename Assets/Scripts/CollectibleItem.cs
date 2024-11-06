using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������������ ������ ��ũ������
public class CollectibleItem : MonoBehaviour
{
    public ItemType itemType;                   //������ ���� (�� ; ũ����Ż, �Ĺ�, ��Ǯ, ����)
    public string itemName;                     //������ �̸�
    public float respawnTime = 30.0f;           //������ �ð�(�������� �ٽ� ������ �� ������ ��� �ð�)
    public bool canCollect = true;              //���� ���� ����(������ �� �ִ��� ���θ� ��Ÿ��)

    //�������� �����ϴ� �޼���, playerInventory�� ���� �κ��丮�� �߰�
    public void CollectItem(PlayerInventory inventory)
    {
        if (!canCollect) return;

        inventory.AddItem(itemType);               //�������� �κ��丮�� �߰�
        Debug.Log($"{itemName}���� �Ϸ�");         //������ ���� �Ϸ� �޼��� ���
        StartCoroutine(RespawnRoutine());          //������ ������ �ڷ�ƾ ����
    }

    //������ �������� ó���ϴ� �ڷ�ƾ
    private IEnumerator RespawnRoutine()
    {
        canCollect = false;         //���� �Ұ����� ���·� ����
        GetComponent<MeshRenderer>().enabled = false;           //��Ƽ���� MeshRenderer�� ���� ������ �ʰ� ��
        GetComponent<MeshCollider>().enabled = false;           //��Ƽ���� MeshCollider�� ���� ������ �ʰ� ��

        //�����ä��� ������ �ð� �����Ѥ� ���
        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshRenderer>().enabled = true;            //�������� MeshRenderer�� �ٽ� ���̰� ��
        GetComponent<MeshCollider>().enabled = true;            //�������� MeshCollider�� �ٽ� ���̰� ��
        canCollect = true;                                      //���� ���� ���·� ����
    }
}