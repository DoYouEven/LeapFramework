using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteObjectHistory : MonoBehaviour
{



    public static InfiniteObjectHistory instance;
    private List<int>[] objectSpawnIndex;
    private ObjectManager objectManager;
    private ObjectGenerator objectGenerator;
    private InfiniteObject2D[] topObject;
    private InfiniteObject2D[] bottomObject;
    private float[] lastObjectSpawnDistance;
    private float[] totalDistance;
    private int activeSlot;
    private int centerSlot;
     void Awake()
    {
        instance = this;
    }
    void Start()
    {
        objectManager = ObjectManager.instance;
        objectGenerator = ObjectGenerator.instance;
    }

    public void Init()
    {
        if(objectManager == null)
            objectManager = ObjectManager.instance;

        int slotCount = objectManager.slotCount;
        
        topObject = new InfiniteObject2D[slotCount];
        bottomObject = new InfiniteObject2D[slotCount];
        lastObjectSpawnDistance = new float[slotCount];
        totalDistance = new float[slotCount];
        centerSlot = slotCount / 2;
    }



    /// <summary>
    /// This method should be called everytime an object is spawned in the game
    /// </summary>
    /// <param name="index"></param>
    /// <param name="locationOffset"></param>
    /// <param name="slot"></param>
    /// <param name="angle"></param>
    /// <param name="objectType"></param>
    /// <param name="infiniteObject"></param>
    /// <returns></returns>
    public InfiniteObject2D ObjectSpawned (int slot,  ObjectType objectType, InfiniteObject2D infiniteObject)

    {
        Debug.Log("Spawing LSots " + slot);
        lastObjectSpawnDistance[slot] = totalDistance[slot] + infiniteObject.GetSize().y;
        //totalDistance += infiniteObject.GetSize().y;
        InfiniteObject2D previousObject = null;
      
        previousObject = topObject[slot];
        topObject[slot] = infiniteObject;

        return previousObject;
    }

    public void SetBottomInfiniteObject(int location, InfiniteObject2D infiniteObject)
    {


        bottomObject[location]= infiniteObject;
        
    }
    public InfiniteObject2D GetBottomInfiniteObject(int slot)
    {
        return bottomObject[slot];
    }
    public InfiniteObject2D GetTopInfiniteObject(int slot)
    {
        if (slot > topObject.Length-1 || slot < 0)
            return null;
        return topObject[slot];
    }
    public void ObjectRemoved(int slot)
    {
        bottomObject[slot] = bottomObject[slot].GetInfiniteObjectParent();
        if (bottomObject[slot] == null)
        {
            topObject[slot] = null;
        }
    }
    public float GetTotalDistance()
    {


        return totalDistance[0];
    }


    
    public void SetActiveSlot(int location)
	{
        activeSlot = location;
	}
    // Update is called once per frame
    void Update()
    {

    }
}
