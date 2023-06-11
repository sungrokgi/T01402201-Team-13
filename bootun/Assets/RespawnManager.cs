using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> mobPool = new List<GameObject>();
    public GameObject[] Mobs;
    public int ObjCnt = 1;
    // Start is called before the first frame update
    void Awake()
    {
        for (int j = 0; j < ObjCnt; j++)
            mobPool.Add(CreateObj(Mobs[Random.Range(0,Mobs.Length)],transform));
    }
    GameObject CreateObj(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }
    private void Start()
    {
        GameManager.instance.onPlay += PlayGame;
    }
    void PlayGame(bool isPlay)
    {
        if (isPlay)
        {
            for (int i = 0; i < mobPool.Count; i++)
                mobPool[i].SetActive(false);
            StartCoroutine(CreateMob());
        }
        else
            StopCoroutine(CreateMob());
    }
    IEnumerator CreateMob()
    {
        yield return new WaitForSeconds(0.5f);
        while (GameManager.instance.isPlay)
        {
            mobPool[DeactiveMob()].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f,3f));
        }
    }

    int DeactiveMob()
    {
        List<int> num = new List<int>();
        for (int i =0; i< mobPool.Count;i++)
        {
            if (!mobPool[i].activeSelf)
                num.Add(i);
        }
        int x = 0;
        if (num.Count > 0)
            x = num[Random.Range(0, num.Count)];
        return x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
