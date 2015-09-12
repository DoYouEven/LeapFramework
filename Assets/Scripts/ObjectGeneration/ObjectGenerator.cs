using UnityEngine;
using System.Collections;
using PathologicalGames;

public class ObjectGenerator : MonoBehaviour
{
    static public ObjectGenerator instance;

    // Use this for initialization

    public float sqrHorizon;
    public float horizon {  get { return Mathf.Sqrt(sqrHorizon); } }
    public float adjacentOffset;
    [MinMaxRangeAttribute(1,10)]
    public MinMaxRange LadderSizeRange;
    //
    // The distance behind the camera that the objects will be removed and added back to the object pool
    public float removeHorizon = -25;



    private Vector3 moveDirection;
    private Vector3 spawnDirection;
    // private ObjectSpawnData spawnData;
    //private PlayerController playerController;
    ObjectManager objectManager;
    InfiniteObjectHistory objectHistory;
    PlayerController playerController;
    private Transform playerTransform;
    /// <summary>
    /// Temp code
    /// </summary>
    private float[] LadderSizes;

    private int activeSlots;
    private int fakeSlots;
    private int slotCount;
    public void Awake()
    {
        instance = this;

    }

    public void Init()
    {
        objectHistory = InfiniteObjectHistory.instance;
        objectManager = ObjectManager.instance;
        moveDirection = Vector3.up;
        slotCount = objectManager.slotCount;
        spawnDirection = Vector3.up;
        activeSlots = GetStartingSlot();
    }

    public void StartGame()
    {
        playerController = PlayerController.instance;
         playerTransform = playerController.transform;
    }


    /// <summary>
    /// Logic, FIrst spawn the first slot, then spawn one to either the right or left, have atleast one main chain, can have multiple fake chains.
    /// </summary>
    /// <param name="activateImmediately"></param>
    public void SpawnObjectRun(bool activateImmediately)
    {



        // spawn the center objects
        InfiniteObject2D prevPlatform = objectHistory.GetTopInfiniteObject(activeSlots);
        InfiniteObject2D prevFakePlatform = null;
        int fakeSlot  = -99;
        
     while ((prevPlatform == null || (Vector3.Scale(prevPlatform.transform.position, spawnDirection)).sqrMagnitude < sqrHorizon))
        {

            Vector3 position = IndexToGlobalPosition(activeSlots);
            if (prevPlatform != null)
            {
            
                activeSlots = GetNextActiveSlot(prevPlatform.Slot);
                if(activeSlots == prevPlatform.Slot)
                    position = new Vector3(IndexToGlobalPosition(activeSlots).x, prevPlatform.transform.position.y  + Vector3.Dot(prevPlatform.GetSize(), Vector3.up), 0);
                else
                position = new Vector3(IndexToGlobalPosition(activeSlots).x, prevPlatform.transform.position.y - adjacentOffset + Vector3.Dot(prevPlatform.GetSize(), Vector3.up), 0);
                Debug.Log(activeSlots);

                //position += IndexToGlobalPosition(prevPlatform.Slot);
            }
           
            LadderObject ladderObject = SpawnObjects(activeSlots, position) as LadderObject;
            prevPlatform = objectHistory.GetTopInfiniteObject(activeSlots);

            //Checking if a fake chain exists


      /*
            if (Random.value > 0.5f)
            {
                prevFakePlatform = objectHistory.GetTopInfiniteObject(fakeSlot);
                if (prevFakePlatform != null)
                {
                    fakeSlot = GetNextActiveSlot(fakeSlot);
                    position = new Vector3(IndexToGlobalPosition(fakeSlot).x, prevFakePlatform.transform.position.y - adjacentOffset + Vector3.Dot(prevFakePlatform.GetSize(), Vector3.up), 0);
                    LadderObject fakeLadderObject = SpawnObjects(fakeSlot, position) as LadderObject;
                }
                else
                {
                    prevFakePlatform = prevPlatform;
                    fakeSlot = GetNextFakeSlot(prevPlatform.Slot, activeSlots);
                    if (fakeSlot != -99)
                    {
                        position = new Vector3(IndexToGlobalPosition(fakeSlot).x, prevFakePlatform.transform.position.y - adjacentOffset + Vector3.Dot(prevFakePlatform.GetSize(), Vector3.up), 0);
                        LadderObject fakeLadderObject = SpawnObjects(fakeSlot, position) as LadderObject;
                    }
                }
            }
            
           */

            //******Fake SpawnAlgorithm
            
          
     

        }
        /*


             if (platform == null)
                 return;

             PlatformSpawned(platform, ObjectLocation.Center, spawnDirection, activateImmediately);
             prevPlatform = infiniteObjectHistory.GetTopInfiniteObject(ObjectLocation.Center, false);

             if (spawnFullLength)
                 SpawnObjectRun(activateImmediately);
         }


         */
    }

    private int GetNextFakeSlot(int previousActive,int activeSlot)
    {

        int slots = -99;

        if (activeSlots < previousActive)
        {
            slots = previousActive + 1 == objectManager.slotCount ? -99 : previousActive + 1;
        }
        else
        {
            slots = previousActive - 1 == -1? -99 : previousActive -1;
        }


        return slots;
    }
    private int GetNextActiveSlot(int previousActiveSlot)
    {
        int slots;
        if (Random.value < 0.5f)
        {

            slots = previousActiveSlot  == objectManager.slotCount - 1 ?  previousActiveSlot -1 :previousActiveSlot + 1;
            
               
        }
        else
            slots = previousActiveSlot == 0 ? previousActiveSlot + 1 : previousActiveSlot - 1;


        slots = Mathf.Clamp(slots, 0, objectManager.slotCount);
        return slots;

    }

    private InfiniteObject2D SpawnObjects(int slot, Vector3 position)
    {
        return SpawnLadder(slot, LadderSizeRange.GetRandomValueInt(), position);

    }
    private void LadderSpawned(InfiniteObject2D infinteObject2D)
    {

    }

    private InfiniteObject2D SpawnLadder(int slot, int n, Vector3 position)
    {

        InfiniteObject2D ladderObj = objectManager.GetLadderFromPool(n, position);
        ladderObj.Slot = slot;
        ladderObj.transform.position = position;
        InfiniteObject2D prevTopPlatform = objectHistory.ObjectSpawned(slot, ObjectType.Ladder, ladderObj);
        if (prevTopPlatform != null)
        {
            prevTopPlatform.SetParent(ladderObj.transform);
        }
        else
        {
            objectHistory.SetBottomInfiniteObject(slot, ladderObj);
        }
        return ladderObj;

    }

    /// <summary>
    /// Since right now we dont have
    /// </summary>

    public Vector3 IndexToGlobalPosition(int slot, float z = -1)
    {
        float x;
        float y;

        float slotMidPoint = objectManager.slotDistance / 2;

        x = objectManager.slotDistance * slot;


        y = 0;
        return transform.TransformPoint(new Vector3(x + slotMidPoint,-Mathf.Sqrt(sqrHorizon)/2+ 0.8f, z));

    }

    int GetStartingSlot()
    {
        return objectManager.slotCount / 2;
    }

    public void MoveObjects(float moveDistance)
    {
        if (moveDistance == 0)
            return;

        // the distance to move the objects
        Vector3 delta = moveDirection * moveDistance;


        // only move the top most platform/scene of each ObjectLocation because all of the other objects are children of these two
        // objects. Only have to check the bottom-most platform/scene as well to determine if it should be removed
        InfiniteObject2D infiniteObject = null;
        Transform objectTransform = null;
        LadderObject ladderObject = null;
        for (int i = 0; i < 2; ++i)
        { // loop through the platform and scenes
            for (int j = 0; j < slotCount; ++j)
            {
                // move
                infiniteObject = objectHistory.GetTopInfiniteObject(j);
                if (infiniteObject != null)
                {
                    objectTransform = infiniteObject.transform;
                    Vector3 pos = objectTransform.position;
                    pos -= delta;
                    objectTransform.position = pos;
                    
                    // check for removal.. there will always be a bottom object if there is a top object
                    infiniteObject = objectHistory.GetBottomInfiniteObject(j);
                    if (playerTransform.InverseTransformPoint(infiniteObject.transform.position).y < removeHorizon)
                    {
                     
                        objectHistory.ObjectRemoved(j);
                        objectManager.DeactivateObject(infiniteObject.transform);
                    }
                    
                }
            }


        }


        //  dataManager.AddToScore(moveDistance);
           SpawnObjectRun(true);

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
