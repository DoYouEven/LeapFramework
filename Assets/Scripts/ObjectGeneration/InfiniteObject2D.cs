using UnityEngine;



public enum ObjectType
{
    Ladder
}

public abstract class InfiniteObject2D : MonoBehaviour
{


    public string PoolName;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Collider[] childColliders;
    private ObjectType objectType;
    public BoxCollider2D collider;
    [SerializeField]
    private int slot;
    public int Slot {  get { return slot; } set { slot = value; } }
    public virtual void Init()
    {

    }

    
    public virtual void UpdateCollider()
    {
    }

    public virtual float GetLenght()
    {
        return 0;
    }
    public virtual void SetParent(Transform parent)
    {
        transform.parent = parent;

        
    }
    public virtual Vector3 GetSize()
    {
        return Vector3.zero;
    }
    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public Vector3 GetSpriteSize(Sprite sp = null)
    {
        return sp.bounds.size;
    }
    public virtual InfiniteObject2D GetInfiniteObjectParent()
    {
        if (transform.parent != null)
            return transform.parent.GetComponent<InfiniteObject2D>();
        else
            return this;
    }
    public virtual Transform GetInfiniteObjectParentTransform()
    {
        if (transform.parent != null)
            return transform.parent; 
        else
            return transform;
    }

    public virtual void Deactivate()
    {

        
    }

}