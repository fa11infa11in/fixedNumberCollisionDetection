using System;
using FNM;
using System.Collections.Generic;
namespace FNP
{
	public class QuadtreeConfig
	{
		public int level;
		public FixedNumberVector3 bounds;
		public FixedNumberVector3 center;
	}
	public class Quadtree
	{
		public int MAX_OBJECTS = 2;
		public int MAX_LEVELS = 5;

		private int level;
		private List<FNPCollider2D> objects;
		private FixedNumberVector3 bounds; //x边长和z边长
		private FixedNumberVector3 center;
		private Quadtree[] nodes;


		public Quadtree(int level, FixedNumberVector3 bounds, FixedNumberVector3 center)
		{

			this.level = level;
			this.bounds = bounds;
			this.center = center;
			objects = new List<FNPCollider2D>();
			nodes = new Quadtree[4];
		}


		public Quadtree(QuadtreeConfig cfg)
		{
			this.level = cfg.level;
			this.bounds = cfg.bounds;
			this.center = cfg.center;
			objects = new List<FNPCollider2D>();
			nodes = new Quadtree[4];
		}

		public void Clear()
		{
			objects.Clear();

			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] == null)
					continue;

				nodes[i].Clear();
				nodes[i] = null;
			}
		}

		private void Split()
		{
			FixedNumber subx = bounds.x / 4;
			FixedNumber subz = bounds.z / 4;

			FixedNumber centerX = center.x;
			FixedNumber centerZ = center.z;

			nodes[0] = new Quadtree(level + 1, bounds / 2, new FixedNumberVector3(centerX + subx, 0, centerZ + subz));
			nodes[1] = new Quadtree(level + 1, bounds / 2, new FixedNumberVector3(centerX - subx, 0, centerZ + subz));
			nodes[2] = new Quadtree(level + 1, bounds / 2, new FixedNumberVector3(centerX - subx, 0, centerZ - subz));
			nodes[3] = new Quadtree(level + 1, bounds / 2, new FixedNumberVector3(centerX + subx, 0, centerZ - subz));

		}


		private static bool IsPointInRect(FixedNumberVector3 target, FixedNumberVector3 center,FixedNumberVector3 size)
		{
			FixedNumberVector3 vec = target - center;

			if (vec.x < size.x / 2
			    && vec.x > -1 * size.x / 2
			    && vec.z > -1 * size.z / 2
			    && vec.z < size.z / 2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool IsAllPointsInRect(FixedNumberVector3[] targets, FixedNumberVector3 center, FixedNumberVector3 size)
		{

			for (int i = 0; i < targets.Length; i++)
			{
				if (!IsPointInRect(targets[i], center, size))
				{
					return false;
				}
			}

			return true;
		}

		private int GetIndex(FNPCollider2D collider)
		{
			int index = -1;

			FixedNumber xMidPoint = center.x;
			FixedNumber zMidPoint = center.z;

			if (collider is FNPColliderCircle)
			{
				FNPColliderCircle cir = (FNPColliderCircle)collider;

				//当碰撞体是圆形时
				FixedNumber bottomPoint = collider.pos.z - cir.radius;
				FixedNumber topPoint = collider.pos.z + cir.radius;
				FixedNumber leftPoint = collider.pos.x - cir.radius;
				FixedNumber rightPoint = collider.pos.x + cir.radius;

				bool topQuadrant = bottomPoint > zMidPoint;
				bool bottomQuadrant = topPoint < zMidPoint;

				if (leftPoint > xMidPoint)
				{
					if (topQuadrant)
					{
						//在右上
						index = 0;
					}
					else if (bottomQuadrant)
					{
						//在右下
						index = 3;
					}
				}
				else if (rightPoint < xMidPoint)
				{
					if (topQuadrant)
					{
						//在左上
						index = 1;
					}
					else if (bottomQuadrant)
					{
						//在左下
						index = 2;
					}
				}

				return index;

			}
			else
			{
				FNPColliderRect squ = (FNPColliderRect)collider;
				FixedNumberVector3[] vertexs = squ.GetAllVertices().ToArray();

				FixedNumberVector3 centerRightTop = center + new FixedNumberVector3(bounds.x / 4, 0, bounds.z / 4);
				FixedNumberVector3 centerLeftTop = center + new FixedNumberVector3(-1 * bounds.x / 4, 0, bounds.z / 4);
				FixedNumberVector3 centerRightBottom = center + new FixedNumberVector3(bounds.x / 4, 0, -1 * bounds.z / 4);
				FixedNumberVector3 centerLeftBottom = center + new FixedNumberVector3(-1 * bounds.x / 4, 0, -1 * bounds.z / 4);

				if (IsAllPointsInRect(vertexs, centerRightTop, bounds / 2))
				{
					index = 0;
				}
				else if (IsAllPointsInRect(vertexs, centerRightBottom, bounds / 2))
				{
					index = 3;
				}
				else if (IsAllPointsInRect(vertexs, centerLeftTop, bounds / 2))
				{
					index = 1;
				}
				else if (IsAllPointsInRect(vertexs, centerLeftBottom, bounds / 2))
				{
					index = 2;
				}

				return index;
			}
		}

		public void Insert(FNPCollider2D collider)
		{
			if (nodes[0] != null)
			{
				int index = GetIndex(collider);

				if (index != -1)
				{
					nodes[index].Insert(collider);
					return;
				}
			}

			objects.Add(collider);

			if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
			{
				if (nodes[0] == null)
				{
					Split();
				}

				int i = objects.Count - 1;
				while (i >= 0)
				{
					int index = GetIndex(objects[i]);
					if (index != -1)
					{
						nodes[index].Insert(objects[i]);
						objects.RemoveAt(i);
					}

					i -= 1;
				}
			}

		}

		public void GetAllChildrenObjects( ref List<FNPCollider2D> list)
		{
			if (nodes[0] == null)
			{
				list.AddRange(objects);
				return;
			}
			
			for (int i = 0; i < nodes.Length; i++)
			{
				nodes[i].GetAllChildrenObjects(ref list);
			}
			
			list.AddRange(objects);
		}

		public void Retrieve(ref List<FNPCollider2D> potentialColliders, FNPCollider2D collider)
		{
			int index = GetIndex(collider);
			if (index != -1 && nodes[0] != null)
			{
				nodes[index].Retrieve(ref potentialColliders, collider);
			}
			
			potentialColliders.AddRange(objects);
		}

		public void RetrieveAll(ref List<FNPCollider2D> potential, FNPCollider2D collider)
		{
			int index = GetIndex(collider);
			if (index == -1)
			{
				GetAllChildrenObjects(ref potential);
				return;
			}
			else if (nodes[0] == null)
			{
				GetAllChildrenObjects(ref potential);
				return;
			}
			else 
			{
				nodes[index].RetrieveAll(ref potential, collider);
				potential.AddRange(objects);
			}
		}
		
	}
}

