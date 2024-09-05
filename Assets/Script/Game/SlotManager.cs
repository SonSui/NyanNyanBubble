using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    // UI��O���[�v�A�G�t�F�N�g�̎Q��
    public GameObject Group;
    public GameObject[] groupPrefabs;
    public List<GameObject> wordGroups;
    public GameObject[] wordEffect;
    public GameObject specialGroup;
    public WordManager wordManager;

    // ���t���[���̍X�V����
    void Update()
    {
        // ����O���[�v�����݂��A�q�v�f������ꍇ
        if (specialGroup != null && specialGroup.transform.childCount > 0)
        {
            Transform firstChild = specialGroup.transform.GetChild(0);
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(true);
            }
        }
    }

    // �X���b�g��ǉ�����֐�
    public GameObject AddSlot(string word, int type = 0)
    {
        int length = word.Length;
        int n = wordGroups.Count;
        Vector3 pos = Group.transform.position + new Vector3(0, -2.0f * n, 0);

        // �w��̃v���n�u���C���X�^���X�����Ĕz�u
        GameObject newGroup = Instantiate(groupPrefabs[length - 1], pos, Quaternion.identity);
        newGroup.transform.SetParent(Group.transform);

        foreach (Transform child in newGroup.transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        wordGroups.Add(newGroup);
        Debug.Log("Group now :" + wordGroups.Count);
        return newGroup;
    }

    // ����X���b�g��ǉ�����֐�
    public void AddSpecialSlot(string word, int type)
    {
        int length = word.Length;
        Vector3 pos = specialGroup.transform.position;

        // �w��̃v���n�u���C���X�^���X�����Ĕz�u
        GameObject newGroup = Instantiate(groupPrefabs[length - 1], pos, Quaternion.identity);
        newGroup.transform.SetParent(specialGroup.transform);
        int sortOrder = -1;

        foreach (Transform child in newGroup.transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = sortOrder;
        }

        // �q���g�ɕύX
        ChangeToHint(newGroup, word, type);

        // ���łɓ���O���[�v�����݂���ꍇ�A�V�����O���[�v���\��
        if (specialGroup.transform.childCount > 1)
        {
            newGroup.SetActive(false);
        }
    }

    // �O���[�v���q���g�ɕύX����֐�
    private void ChangeToHint(GameObject group_, string word, int type)
    {
        GameObject wordM = GameObject.Find("WordManager");
        int c = group_.transform.childCount;
        GameObject newHira = Group;

        for (int i = 0; i < c; i++)
        {
            Transform child = group_.transform.GetChild(i);
            Color32 c32 = new Color32(112, 119, 255, 255);
            child.GetComponent<SpriteRenderer>().color = c32;

            newHira = wordM.GetComponent<WordManager>().SpawnHiragana(word[i]);
            newHira.transform.position = child.position;
            newHira.GetComponent<Hiragana>().NoSelectOption();

            Color cl = newHira.GetComponent<SpriteRenderer>().color;
            cl.a = 0.5f;
            newHira.GetComponent<SpriteRenderer>().color = cl;
            newHira.transform.SetParent(child.transform);
        }

        Vector3 pos = group_.transform.position + new Vector3(1.3f * c, 0, 0);
        GameObject we = Instantiate(wordEffect[type], pos, Quaternion.identity);
        we.transform.SetParent(newHira.transform);

        group_.GetComponent<GroupState>().isHint = true;
        group_.GetComponent<GroupState>().thisWord = word;
    }

    // �O���[�v���폜����֐�
    public void RemoveSlot(GameObject Group)
    {
        Group.GetComponent<GroupState>().isDistroying = true;
        wordGroups.Remove(Group);
    }

    // �X���b�g���ړ�����֐�
    public void MoveSlot(int type = 0)
    {
        switch (type)
        {
            case 0:
                int n = wordGroups.Count;
                for (int i = 0; i < n; i++)
                {
                    Vector3 targetPos = new Vector3(0, -2.0f * i, 0);
                    StartCoroutine(MoveTo(wordGroups[i], targetPos));
                }
                Debug.Log("First word group removed.");
                break;
            case 1:
                // ���̃P�[�X��ǉ�����ꍇ�̏���
                break;
        }
    }

    // �w��ʒu�ɃO���[�v���ړ�����R���[�`��
    private IEnumerator MoveTo(GameObject wordGroup_, Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = wordGroup_.transform.position;
        targetPosition += Group.transform.position;

        while (time < 0.25f)
        {
            wordGroup_.transform.position = Vector3.Lerp(startPosition, targetPosition, time * 4);
            time += Time.deltaTime;
            yield return null;
        }

        wordGroup_.transform.position = targetPosition;
    }

    // ����X���b�g���폜����֐�
    public void RemoveSpecialSlot(GameObject Group)
    {
        Group.GetComponent<GroupState>().isDistroying = true;
        Group.GetComponent<GroupState>().isSpecial = true;
        wordGroups.Remove(Group);

        int r = Random.Range(0, 100);
        if (r < 50)
        {
            wordManager.AddSpecialWord(1);
        }
        else if(r<70)
        {
            wordManager.AddSpecialWord(2);
        }
        else
        {
            wordManager.AddSpecialWord(0);
        }
    }

    // �S�ẴO���[�v���N���A����֐�
    public void ClearAllGroup()
    {
        int leng = Group.transform.childCount;
        wordGroups.Clear();

        for (int i = leng - 1; i >= 0; i--)
        {
            GameObject toDelete = Group.transform.GetChild(i).gameObject;
            Destroy(toDelete);
        }

        leng = specialGroup.transform.childCount;
        for (int i = leng - 1; i >= 0; i--)
        {
            GameObject toDelete = specialGroup.transform.GetChild(i).gameObject;
            Destroy(toDelete);
        }
    }
}
