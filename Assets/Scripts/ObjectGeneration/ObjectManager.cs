using PathologicalGames;
using System.Collections.Generic;
using UnityEngine;



public class ObjectManager : MonoBehaviour
{

    public bool showGrid;
    public float slotDistance;
    public int slotCount;
    public Transform ladderParent;
    static public ObjectManager instance;
    public List<LadderObject> ladders;
    public Transform gameAnchor;

    private SpawnPool ladderSpawnPool;
    public void Init()
    {
        ladderSpawnPool = PoolManager.Pools[ladders[0].PoolName];
    }
    public void Awake()
    {
        instance = this;
    }

    public SpawnPool GetObjectPool()
    {
        return ladderSpawnPool;
    }
    public void DeactivateObject(Transform ladderObject)
    {
        foreach (Transform child in ladderObject)
        {
            ladderSpawnPool.Despawn(child,ladderObject);
        }
        ladderSpawnPool.Despawn(ladderObject,ladderObject.parent);

    }
    public InfiniteObject2D GetLadderFromPool(int size,Vector3 pos)
    {
        //Spawn the core Ladder
        LadderObject ladderObject = ladderSpawnPool.Spawn(ladders[0].transform).GetComponent<LadderObject>();

        Transform ladderTransform = ladderObject.transform;
        ladderTransform.position = pos;
        
        //Spawn TopPeice
        Transform topLadder = ladderSpawnPool.Spawn(ladders[0].topLadder);
       topLadder.SetParent(ladderTransform,false);
        topLadder.localPosition = Vector3.zero;
        Transform bottomLadder = ladderSpawnPool.Spawn(ladders[0].bottomLadder);
        ladderObject.SetLadderSize(size);

 
        Vector3 spriteSize = ladderObject.GetCenterSize();
        //Vector3 startPosition = Vector3.Scale((topLadder.position - spriteSize * i), Vector3.up);
        for (int i = 1; i <= size; i++)
        {

            Transform infiniteObject = ladderSpawnPool.Spawn(ladders[0].mainLadder);

            infiniteObject.SetParent(ladderTransform);
            infiniteObject.localPosition = Vector3.Scale( 1* spriteSize * i, Vector3.up);
            
        }

        bottomLadder.SetParent(ladderTransform);
        bottomLadder.localPosition = Vector3.Scale(1 * spriteSize * (size + 1), Vector3.up);
        ladderObject.UpdateCollider();
        return ladderObject;
    }
    private InfiniteObject2D GetObjectsFromPool(GameObject prefab)
    {
        SpawnPool sp = GetObjectPool();
        return sp.Spawn(prefab).GetComponent<InfiniteObject2D>();
    }
    public void AssignParent(InfiniteObject2D infiniteObject, ObjectType objectType)
    {
        switch (objectType)
        {
            case ObjectType.Ladder:
                infiniteObject.SetParent(ladderParent);
                break;

        }
    }
    public int GetTotalObjectCount()
    {
        return ladders.Count;


    }


    public int GetStartingSlot()
    {
        return slotCount / 2;
    }
    public Vector2 getAnchorPos()
    {
        return gameAnchor.position;
    }
}