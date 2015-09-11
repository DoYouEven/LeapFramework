using UnityEngine;
using System.Collections;
using PathologicalGames;
public class Tests : MonoBehaviour {


    public GameObject topLadder;
    public GameObject bottomLadder;
    public GameObject mainLadder;
    public string poolName;
    SpawnPool shapesPool;

    public int ladderSize;

    public void CreateLadder(int n)
    {
        GameObject top = Instantiate(topLadder);
        GameObject bottom = Instantiate(bottomLadder);
       
        for (int i = 0; i < n; i++)
        {

            GameObject main = shapesPool.Spawn(mainLadder).gameObject;
            Vector3 spriteSize = GetSpriteSize(main.GetComponent<SpriteRenderer>().sprite);
            main.transform.position = Vector3.Scale((top.transform.position - spriteSize * i),Vector3.up);
        }


        bottom.transform.position = Vector3.Scale(top.transform.position - GetSpriteSize(mainLadder.GetComponent<SpriteRenderer>().sprite) * n,Vector3.up);

    }


    public Vector3 GetSpriteSize(Sprite sp)
    {
        return sp.bounds.size;
    }
	// Use this for initialization
	void Start () {
        shapesPool = PoolManager.Pools[this.poolName];
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.E))
        {
            CreateLadder(10);
        }

        
	
	}
}
