using System;
using FNM;
using System.Collections.Generic;

namespace FNP
{
	public class FNPColliderPhysicsEnv
	{
		private List<FNPCollider2D> staticColliders;//环境中的静态碰撞体

		//public List<FNPColliderConfig> staticColliderConfigList;
		
		public Quadtree quadtreeRoot;

		public FNPColliderPhysicsEnv()
		{
			staticColliders = new List<FNPCollider2D>();
		}

		public void StaticCollidersInit( List< FNPColliderConfig > configs,QuadtreeConfig qc = null )
		{
			for (int i = 0; i < configs.Count; i++)
			{
				
				if(configs[i].type == ColliderType.Rect )
				{
					//静态方形初始化
					FNPColliderRect collider = new FNPColliderRect( configs[i]);
					staticColliders.Add(collider);
				}
				else
				{
					//静态圆形初始化
					FNPColliderCircle collider = new FNPColliderCircle(configs[i]);
					staticColliders.Add(collider);
				}
			}
			
			if (qc != null)
			{
				quadtreeRoot = new Quadtree(qc);
				for (int i = 0; i < staticColliders.Count; i++)
				{
					quadtreeRoot.Insert( staticColliders[i] );
				}
			}
		}

		public List<FNPCollider2D> GetEnvColliders()
		{
			return staticColliders;
		}
		
		public List<FNPCollider2D> GetPotentialCollider(FNPCollider2D collider = null)
		{
			if (quadtreeRoot == null)
				return staticColliders;

			if (collider == null)
				return staticColliders;

			List<FNPCollider2D> ret = new List<FNPCollider2D>();
			quadtreeRoot.RetrieveAll(ref ret, collider);
			return ret;
		}

	}
}

