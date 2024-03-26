using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Head : MonoBehaviour
{

    public float speed;
    public GameObject wall;
    public GameObject head;
    public GameObject horizontal;
    public GameObject vertical;
    public Body body;
    public float spawnOffset;

    private int horInput, verInput;
    private Vector3 target;
    private int bodyCount;
    private Vector3 direction;
    private int[][] map1, map2, map3;
    private int[][] currentMap;
    private Vector2 playerPos = new Vector2();
    private float distanceTraveled;
    private GameObject headGO;
    private bool[][] gmap;
    private Vector2 originalIndex = new Vector2();
    private Vector3 toward;
    private int bodyCountRequired;
    private Vector2 checkEnd;
    private Vector3 preDirection;
    private float totalDistanceTraveld = 0;

    // Start is called before the first frame update
    void Start()
    {
        bodyCount = 0;
        distanceTraveled = 0;
        bodyCountRequired = 0;
        preDirection = Vector3.zero;

        map1 = new int[][] {
            new int[] {1, 1, 1, 1, 1},
            new int[] {1, 0, 0, 0, 1},
            new int[] {1, 0, 2, 0, 1},
            new int[] {1, 0, 0, 0, 1},
            new int[] {1, 1, 1, 1, 1},
        };

        map2 = new int[][] {
            new int[] {-1, 1, 1, 1, -1, -1},
            new int[] {1, 0, 0, 0, 1, -1},
            new int[] {1, 0, 0, 0, 0, 1},
            new int[] {-1, 1, 0, 2, 0, 1},
            new int[] {-1, 1, 0, 0, 0, 1},
            new int[] {-1, 1, 1, 1, 1, 1},
        };

        map3 = new int[][] {
            new int[] {1, 1, 1, 1, 1, 1},
            new int[] {1, 0, 0, 0, 0, 1},
            new int[] {1, 0, 0, 2, 0, 1},
            new int[] {1, 0, 1, 1, 0, 1},
            new int[] {1, 0, 0, 0, 0, 1},
            new int[] {1, 1, 1, 1, 1, 1},
        };

        Map(map3);

    }

    private void Map(int[][] map)
    {
        currentMap = map;
        gmap = new bool[map.Length][];
        GameObject parent = new GameObject();
        for (int i = 0; i < map.Length; i++)
        {
            gmap[i] = new bool[map[i].Length];
            for (int j = 0; j < map[i].Length; j++)
            {
                gmap[i][j] = false;
                if (map[i][j] <= 0)
                {
                    if (map[i][j] == 0) bodyCountRequired++;
                    continue;
                }
                GameObject temp = null;
                if (map[i][j] == 1)
                {
                    temp = Instantiate(wall);
                }
                else if (map[i][j] == 2)
                {
                    temp = Instantiate(head);
                    temp.transform.position = Vector3.down * i + Vector3.right * j;
                    temp.transform.parent = parent.transform;
                    playerPos.x = i;
                    playerPos.y = j;
                    originalIndex.x = playerPos.x;
                    originalIndex.y = playerPos.y;
                    headGO = temp;

                }
                temp.transform.position = Vector3.down * i + Vector3.right * j + (map[i][j] == 0 ? Vector3.forward : Vector3.zero);
                temp.transform.parent = parent.transform;
                if (map[i][j] == 2) target = temp.transform.position;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        int i = (int)playerPos.x;
        int j = (int)playerPos.y;

        checkEnd = new Vector2(headGO.transform.up.y, headGO.transform.up.x);
        if (headGO && Vector3.Distance(headGO.transform.position, target) < 0.0000000000000000001f)
        {
            
            if ((gmap[(int)playerPos.x + (int)checkEnd.y][(int)playerPos.y + (int)checkEnd.x] ||
                currentMap[(int)playerPos.x + (int)checkEnd.y][(int)playerPos.y + (int)checkEnd.x] == 1) &&
                (currentMap[(int)playerPos.x - (int)checkEnd.y][(int)playerPos.y - (int)checkEnd.x] == 1 ||
                gmap[(int)playerPos.x - (int)checkEnd.y][(int)playerPos.y - (int)checkEnd.x]))
            {
                if (bodyCountRequired == 0)
                {
                    Debug.Log("Win");
                }
                else Debug.Log("Lose");
                Debug.Break();
            }

            horInput = (int)Input.GetAxisRaw("Horizontal");
            verInput = (int)Input.GetAxisRaw("Vertical");

            preDirection = direction;

            if (verInput != 0 && direction.y == 0)
            {

                while (i >= 0 && i < currentMap.Length && currentMap[i][j] != 1 && !gmap[i][j])
                {
                    i -= verInput;
                }
                i += verInput;
                if (playerPos.x != i)
                {
                    target = headGO.transform.position + Vector3.up * Mathf.Abs(i - playerPos.x) * verInput;
                    originalIndex.x = playerPos.x;
                    originalIndex.y = playerPos.y;
                    playerPos.x = i;
                    headGO.transform.up = Vector3.up * verInput;
                    direction = verInput * Vector3.up;
                }
            }
            else if (horInput != 0 && direction.x == 0)
            {
                while (j >= 0 && j < currentMap[i].Length && currentMap[i][j] != 1 && !gmap[i][j])
                {
                    j += horInput;
                }
                j -= horInput;

                if (playerPos.y != j)
                {
                    target = headGO.transform.position + Vector3.right * Mathf.Abs(j - playerPos.y) * horInput;
                    originalIndex.x = playerPos.x;
                    originalIndex.y = playerPos.y;
                    playerPos.y = j;
                    headGO.transform.up = Vector3.right * horInput;
                    direction = horInput * Vector3.right;
                }
            }


            bodyCount = 0;
            totalDistanceTraveld += distanceTraveled;
            distanceTraveled = 0;
        }

        toward = Vector3.MoveTowards(headGO.transform.position, target, speed * Time.deltaTime);

        float distance = (toward - headGO.transform.position).magnitude;
        distanceTraveled += distance;

        if (distanceTraveled > bodyCount)
        {
            int iIndex = (int)originalIndex.x - verInput * bodyCount;
            int jIndex = (int)originalIndex.y + horInput * bodyCount;
            gmap[iIndex][jIndex] = true;

            Vector3 spawnPos = iIndex * Vector3.down + jIndex * Vector3.right + Vector3.forward;

            if (preDirection != Vector3.zero && bodyCount == 0)
            {
                Body b = Instantiate(body, spawnPos, Quaternion.identity);
                b.Init(direction, preDirection);
            }
            else
            {
                Instantiate(Mathf.Abs(direction.x) == 1 ? horizontal : vertical, spawnPos, Quaternion.identity);
            }
            bodyCount++;
            bodyCountRequired--;
        }

    }

    private void LateUpdate()
    {
        headGO.transform.position = toward;
    }
}
