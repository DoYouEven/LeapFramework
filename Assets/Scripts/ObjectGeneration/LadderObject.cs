using UnityEngine;
using System.Collections;

public class LadderObject : InfiniteObject2D
{



    public GameObject topLadder;
    public GameObject bottomLadder;
    public GameObject mainLadder;

    public float colliderSizeFactor = 1.3f;
    private int ladderStepCount;
    public override void Init()
    {

    }
    public override void UpdateCollider()
    {
        Vector2 size =new Vector2( ObjectManager.instance.slotDistance, GetSize().y ) * colliderSizeFactor;
        collider.size = size ;
        collider.offset = new Vector2(0, 0.5f);
    }
    public void SetLadderSize(int n)
    {
        ladderStepCount = n;
    }
    public override float GetLenght()
    {
        return base.GetLenght();
    }
    public override Vector3 GetSize()
    {
        return GetSpriteSize(mainLadder.GetComponent<SpriteRenderer>().sprite) * ladderStepCount + GetSpriteSize(topLadder.GetComponent<SpriteRenderer>().sprite)*2;
    }

    public  Vector3 GetCenterSize()
    {
        return GetSpriteSize(mainLadder.GetComponent<SpriteRenderer>().sprite);
    }






}
