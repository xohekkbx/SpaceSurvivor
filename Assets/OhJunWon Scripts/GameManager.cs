using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class GameManager : MonoBehaviour
{
    public GameObject player;
    int rowNum = 3;
    int colNum = 3;
    public GameObject[] mapPrefab;
    public GameObject[] item;
    public GameObject[] monsterPrefab;
    List<GameObject> inActiveMonster = new List<GameObject>();
    List<GameObject> activeMonster = new List<GameObject>();
    GameObject[] map = new GameObject[9];
    public static float time;
    float gameOverTime = 60f;
    bool isGameOver;
    bool isMove;
    float cameraHeight; 
    float cameraWidth;
    float outlineboundXLeft;
    float outlineboundXRight;
    float outlineboundYUp;
    float outlineboundYDown;
    int timer;

    Vector3 currentPos; 
    int currentPosIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight* Camera.main.aspect;
        inActiveMonster = new List<GameObject>();
        activeMonster = new List<GameObject>();
        outlineboundXLeft = player.transform.position.x - cameraWidth;
        outlineboundXRight = player.transform.position.x + cameraWidth;
        outlineboundYDown = player.transform.position.y - cameraHeight;
        outlineboundYUp = player.transform.position.y + cameraHeight;
        for (int i = 0; i < mapPrefab.Length; i++)
        { map[i] = Instantiate(mapPrefab[i], player.transform.position + new Vector3(10 * (i / 3), 10 * (i % 3), 0), mapPrefab[i].transform.rotation); }
        isMove = false;
        isGameOver = false;
        time = 0.0f;
        for (int i = 0; i < 10000; i++)
        {
            GameObject monster = Instantiate(monsterPrefab[0]);
            monster.SetActive(false);
            inActiveMonster.Add(monster);
        }
        for (int i = 0; i < 100; i++) MonsterRespawn(RespawnPos());


    }

    // Update is called once per frame
    void Update()
    {
        Timer(time);
        Debug.Log("활성 개수: " + activeMonster.Count);
        Debug.Log("비활성 개수: " + inActiveMonster.Count);
        outlineboundXLeft = player.transform.position.x - cameraWidth;
        outlineboundXRight = player.transform.position.x + cameraWidth;
        outlineboundYDown = player.transform.position.y - cameraHeight;
        outlineboundYUp = player.transform.position.y + cameraHeight;
        time += Time.deltaTime;
        MonsterScreenOut();
        MonsterInvisible();
        if (!isMove) MapMove();
        GameOver();
       
    }
    void Timer(float time)
    {
        if (timer == time)
        {
            for (int i = 0; i < timer * 10; i++) MonsterRespawn(RespawnPos());
            timer++;
        }
    }

    void MapMove()
    {
        currentPos = map[currentPosIndex].transform.position;
        Debug.Log("현재 위치: " + currentPosIndex);
        if (player.transform.position.x - cameraWidth <= currentPos.x - map[currentPosIndex].GetComponent<RectTransform>().rect.width / 2)
        {
            if (currentPosIndex / 3 == 0)
            {
                map[6].transform.position = map[0].transform.position + new Vector3(-10, 0, 0);
                map[7].transform.position = map[1].transform.position + new Vector3(-10, 0, 0);
                map[8].transform.position = map[2].transform.position + new Vector3(-10, 0, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                { map[3 * (currentPosIndex / 3) + i - 3].transform.position = map[3 * (currentPosIndex / 3) + i].transform.position + new Vector3(-10, 0, 0); }
            }
            Debug.Log("맵 왼쪽 이동");
        }
        else if (player.transform.position.x + cameraWidth >= currentPos.x + map[currentPosIndex].GetComponent<RectTransform>().rect.width / 2)
        {
            if (currentPosIndex / 3 == 2)
            {
                map[0].transform.position = map[6].transform.position + new Vector3(10, 0, 0);
                map[1].transform.position = map[7].transform.position + new Vector3(10, 0, 0);
                map[2].transform.position = map[8].transform.position + new Vector3(10, 0, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                { map[3 * (currentPosIndex / 3) + i + 3].transform.position = map[3 * (currentPosIndex / 3) + i].transform.position + new Vector3(+10, 0, 0); }
            }
            Debug.Log("맵 오른쪽 이동");
        }

        if (player.transform.position.y - cameraHeight <= currentPos.y - map[currentPosIndex].GetComponent<RectTransform>().rect.height / 2)
        {
            if (currentPosIndex % 3 == 0)
            {
                map[2].transform.position = map[0].transform.position + new Vector3(0, -10, 0);
                map[5].transform.position = map[3].transform.position + new Vector3(0, -10, 0);
                map[8].transform.position = map[6].transform.position + new Vector3(0, -10, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                { map[currentPosIndex % 3 + (3 * i) - 1].transform.position = map[(currentPosIndex % 3) + (3 * i)].transform.position + new Vector3(0, -10, 0); }
            }
            Debug.Log("맵 아래쪽 이동");
        }
        else if (player.transform.position.y + cameraHeight >= currentPos.y + map[currentPosIndex].GetComponent<RectTransform>().rect.height / 2)
        {
            if (currentPosIndex % 3 == 2)
            {
                map[0].transform.position = map[2].transform.position + new Vector3(0, 10, 0);
                map[3].transform.position = map[5].transform.position + new Vector3(0, 10, 0);
                map[6].transform.position = map[8].transform.position + new Vector3(0, 10, 0);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                { map[currentPosIndex % 3 + 3 * i + 1].transform.position = map[(currentPosIndex % 3) + (3 * i)].transform.position + new Vector3(0, +10, 0); }
            }
            Debug.Log("맵 위쪽 이동");
        }



        if (player.transform.position.x <= currentPos.x - map[currentPosIndex].GetComponent<RectTransform>().rect.width / 2)
        {
            if (currentPosIndex / 3 == 0) currentPosIndex += 6;
            else currentPosIndex -= 3;
        }
        else if (player.transform.position.x >= currentPos.x + map[currentPosIndex].GetComponent<RectTransform>().rect.width / 2)
        {
            if (currentPosIndex / 3 == 2) currentPosIndex -= 6;
            else currentPosIndex += 3;
        }
        if (player.transform.position.y <= currentPos.y - map[currentPosIndex].GetComponent<RectTransform>().rect.height / 2)
        {
            if (currentPosIndex % 3 == 0) currentPosIndex += 2;
            else currentPosIndex -= 1;
        }
        else if (player.transform.position.y >= currentPos.y + map[currentPosIndex].GetComponent<RectTransform>().rect.height / 2)
        {
            if (currentPosIndex % 3 == 2) currentPosIndex -= 2;
            else currentPosIndex += 1;
        }

        currentPos = map[currentPosIndex].transform.position;
    }


    Vector2 RespawnPos()
    {
        Vector2 respawnPos = new Vector2(Random.Range(outlineboundXLeft - 10, outlineboundXRight + 10), Random.Range(outlineboundYDown - 10, outlineboundYUp + 10));
        while (respawnPos.x >= outlineboundXLeft - 2 && respawnPos.x <= outlineboundXRight + 2 && respawnPos.y >= outlineboundYDown - 2 && respawnPos.y <= outlineboundYUp + 2)
        {
            respawnPos[0] = Random.Range(outlineboundXLeft - 10, outlineboundXRight + 10);
            respawnPos[1] = Random.Range(outlineboundYDown - 10, outlineboundYUp + 10);
        }
        return respawnPos;
    }

    void MonsterRespawn(Vector2 respawnPos)
    {
        activeMonster.Add(inActiveMonster[0]);
        inActiveMonster[0].transform.position = respawnPos;
        inActiveMonster[0].SetActive(true);
        inActiveMonster.RemoveAt(0);
    }

    void MonsterInvisible()
    {
        for (int i = activeMonster.Count - 1; i >= 0; i--)
        {   
            if (activeMonster[i].activeSelf == false)
            {
                inActiveMonster.Add(activeMonster[i]);
                activeMonster.RemoveAt(i);
                Debug.Log(i + "번째 몬스터 안보이게");
            }
        }
    }

    void MonsterScreenOut()
    {

        for (int i = 0; i < activeMonster.Count; i++)
        {
            Debug.Log(activeMonster.Count);
            if (outlineboundXLeft - 10 >= activeMonster[i].transform.position.x || activeMonster[i].transform.position.x >= outlineboundXRight + 10 || activeMonster[i].transform.position.y >= outlineboundYUp + 10 || activeMonster[i].transform.position.y <= outlineboundYDown - 10 )
            {
                activeMonster[i].SetActive(false);
                Debug.Log(i + "번 몬스터 삭제");
            }
        }
    }

    void GameOver()
    {
        if (time >= gameOverTime)
        {
            Debug.Log(time + "게임 종료");
            isGameOver = true;
        }
    }
    

}
