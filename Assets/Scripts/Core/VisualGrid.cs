
using UnityEngine;
using System.Collections;

/// <summary>
/// Make sure objectManager and objectGenerator are in the scene
/// </summary>
public class VisualGrid : MonoBehaviour
{
	[Range(1,50)] public int refreshRate = 7;
	int internalTimer = 0;
	 public ObjectManager grid;
    public ObjectGenerator objectGenerator;
	 public VectorFrame Corners = new VectorFrame(Vector2.zero);
	
	[HideInInspector] public float ratio{
		get{
			return 1;
			}
	}
	[HideInInspector] public float yOffset{
		get{
			return 0;
		}
	}

	[HideInInspector]
	public Vector3 TileSize2D {
		get {
			return new Vector3(grid.slotDistance*ratio, Mathf.Sqrt(objectGenerator.sqrHorizon), 0.01f);
		}
	}
	/*
	[HideInInspector]
	public Vector3 TileSize2DPadded {
		get {
			return new Vector3(gm.size*ratio*(1f-(gm.paddingPercentage/100)),
			                   gm.size*ratio*(1f-(gm.paddingPercentage/100)), 0.01f);
		}
	}*/
	
	public Vector3 this[int x, int y] {
		get {
		
			return Corners.CalculatePosFromTopLeft(x, y, grid.slotDistance,grid.getAnchorPos() );
		
			}
		}

	
	public struct VectorFrame
	{
		public Vector2 TopL;
		public Vector2 TopR;
		
		public Vector2 BottomL;
		public Vector2 BottomR;
		
		public float z;
		
		public VectorFrame(Vector2 kickstart){
			TopL = TopR = BottomL = BottomR = kickstart;
			z = 0;
		}
		
		public VectorFrame(Vector2 topL, Vector2 topR, Vector2 bottomL, Vector2 bottomR)
		: this(topL, topR, bottomL, bottomR, 0f)
		{
		}
		
		public VectorFrame(Vector2 topL, Vector2 topR, Vector2 bottomL, Vector2 bottomR, float frameZ)
		{
			TopL = topL;
			TopR = topR;
			BottomL = bottomL;
			BottomR = bottomR;
			z = frameZ;
		}
		
		public VectorFrame Set(Vector2 topL, Vector2 topR, Vector2 bottomL, Vector2 bottomR, float frameZ = 0f)
		{
			TopL = topL;
			TopR = topR;
			BottomL = bottomL;
			BottomR = bottomR;
			z = frameZ;
			
			return this;
		}

		public VectorFrame Set(Vector2 topL)
		{
			TopL = topL;
			
			return this;
		}
		
		public Vector3 CalculatePosFromTopLeft(int x, int y, float step,Vector2 anchor)
		{
            return new Vector3(anchor.x + ((x * step) + (step * 0.5f)), anchor.y - ((y * step) + (step * 0.5f)), z);
		}
		
		public Vector3 CalculatePosFromBottomLeft(int x, int y, float step)
		{
			return new Vector3(BottomL.x + ((x * step) + (step * 0.5f)), BottomL.y + ((y * step) + (step * 0.5f)), z);
		}
		public Vector3 CalculatePosFromBottomLeftHex(int x, int y, float step)
		{
			Vector3 pos = CalculatePosFromBottomLeft(x,y,step);
			if(x%2 == 0){ // displacement for hexagon type
				pos.Set(pos.x*0.865f, pos.y + (0.25f * step) , pos.z );
			} else {
				pos.Set(pos.x*0.865f, pos.y - (0.25f * step) , pos.z );
			}
			return pos;
		}
		
		public Vector3 CalculatePosFromTopRight(int x, int y, float step)
		{
			return new Vector3(TopR.x - ((x * step) + (step * 0.5f)), TopR.y - ((y * step) + (step * 0.5f)), z);
		}
		
		public Vector3 CalculatePosFromBottomRight(int x, int y, float step)
		{
			return new Vector3(BottomR.x - ((x * step) + (step * 0.5f)), BottomR.y + ((y * step) + (step * 0.5f)), z);
		}
	}



	// ####################################
	// start of functions
	// ####################################
	
	// values that is refreshed each call
	private void SetValues()
	{
		// for refresh rate... reduce performance of Unity editor
		internalTimer++;
		if(internalTimer < refreshRate){
			return;
		}
		internalTimer = 0;
		
		float halfWidth = grid.slotDistance * 0.5f;
		float halfHeight = grid.slotCount * grid.slotDistance * 0.5f;
		
		Vector3 TempPos = grid.transform.position;
		
		Corners.Set(
			new Vector2(TempPos.x - halfWidth, TempPos.y + halfHeight),
			new Vector2(TempPos.x + halfWidth, TempPos.y + halfHeight),
			new Vector2(TempPos.x - halfWidth, TempPos.y - halfHeight),
			new Vector2(TempPos.x + halfWidth, TempPos.y - halfHeight),
			TempPos.z
			);
	}
	
	public void OnDrawGizmos()
	{		
		if (grid != null) {
			SetValues(); // refresh all values that is needed
			
			
		
			if (grid.showGrid ){
				for (int x = 0; x < grid.slotCount; x++)
				{
					
						if (grid.showGrid) { // show grid box
							Gizmos.color = Color.white;
							Gizmos.DrawWireCube(this[x, 0], TileSize2D);
                        
						}
					
						
					
				}
			}
		}
	}
}