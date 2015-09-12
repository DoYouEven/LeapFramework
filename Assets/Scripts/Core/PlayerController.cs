using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    ObjectGenerator objectGenerator;
    ObjectManager objectManager;
    GameManager gameManager;
    private LayerMask ladderLayer;
    public float playerSpeed = 1;
    public float hopSpeed = 4;
    private int targetSlot;
    private Vector3 targetPosition;
    
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }
        
    void Start()
    {
        
        ladderLayer = LayerMask.NameToLayer("Ladder");
        gameManager = GameManager.instance ;
        objectGenerator = ObjectGenerator.instance;
        objectManager = ObjectManager.instance;
        targetSlot = objectManager.GetStartingSlot();
        targetPosition = new Vector2(objectGenerator.IndexToGlobalPosition(targetSlot).x, -objectGenerator.horizon/2);
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.IsPlaying())
        {
            objectGenerator.MoveObjects(playerSpeed * Time.deltaTime);
            RaycastHit2D hit;
            if ((hit = Physics2D.Raycast(this.transform.position, Vector3.forward, ladderLayer)))
            {

                
                // Debug.Log(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("Dead");
                //gameManager.GameOver();
                
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, hopSpeed* Time.deltaTime);
        }
    }

    public void ChangeSlots(bool right)
    {
        int slot = right ? targetSlot +1 : targetSlot -1;
        slot = Mathf.Clamp(slot, 0, objectManager.slotCount);
        if (slot != targetSlot)
        {
            targetSlot = slot;
            ChangeSlots(slot);
        }

    }
    public void ChangeSlots(int targetSlot)
    {
        gameManager.IncrementScore();
        targetPosition = new Vector2 (objectGenerator.IndexToGlobalPosition(targetSlot).x, transform.position.y);
    }

}
