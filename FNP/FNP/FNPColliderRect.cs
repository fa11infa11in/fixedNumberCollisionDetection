
using FNM;
using System.Collections.Generic;

namespace FNP
{
	

	public class FNPColliderRect : FNPCollider2D
	{
		public FixedNumberVector3 size;
		public FixedNumberVector3[] axis;
		

		public override List<FixedNumberVector3> GetAllVertices()
		{
			FixedNumberVector3 leftTop = pos + size.z * axis[2] - size.x * axis[0];
			FixedNumberVector3 leftBottom = pos - size.z * axis[2] - size.x * axis[0];
			FixedNumberVector3 rightBottom = pos - size.z * axis[2] + size.x * axis[0];
			FixedNumberVector3 rightTop = pos + size.z * axis[2] + size.x * axis[0];

			List<FixedNumberVector3> ret = new List<FixedNumberVector3>();
            
			ret.Add(leftTop);
			ret.Add(leftBottom);
			ret.Add(rightBottom);
			ret.Add(rightTop);

			return ret;
		}

        public FNPColliderRect(FNPColliderConfig config)
        {
	        pos = config.pos;
	        size = config.size;
	        axis = new FixedNumberVector3[3];
            
	        axis[0] = config.axis[0];
            axis[1] = config.axis[1];
	        axis[2] = config.axis[2];
            
            this.Log("pos:" + pos.ToString() + " axis0: " + axis[0] +" axis1: " + axis[1] + " axis2: " + axis[2] );
        }


        public override bool DetectCircleCollision(FNPColliderCircle col, ref FixedNumberVector3 normal, ref FixedNumberVector3 borderAdjust)
        {
	      //  List<FixedNumberVector3> targetVertexColletion = GetAllVertices();
            List<FixedNumberVector3> normalList = new List<FixedNumberVector3>();
            
            normalList.Add(axis[0]);
            normalList.Add(axis[2]);
            
            normalList.Add(col.GetAxisForCylinder(this));

            FixedNumber minDis = FixedNumber.Zero;
            FixedNumberVector3 outDir = FixedNumberVector3.Zero;

            bool flag = FNPCollisionCalcAlgo.SeparatingAxisCalc(this, col, normalList, ref outDir, ref minDis);
            //
            // int cnt = 0;
            //
            // for (int i = 0; i < normalList.Count; i++)
            // {
            //     FixedNumber[] self = ProjectToAxis(normalList[i]);
            //     FixedNumber[] target = col.ProjectToAxis(normalList[i]);
            //     
            //     if ((self[1] < target[0]) || self[0] > target[1])
            //     {
            //         return false;
            //     }
            //     else
            //     {
            //         FixedNumber outLen = FixedNumber.Zero;
            //         if (self[1] >= target[0] && target[1] >= self[1])
            //         {
            //             outLen = self[1] - target[0];
            //
            //             if (cnt == 0)
            //             {
            //                 minDis = outLen;
            //                 outDir = normalList[i];
            //             }
            //             else
            //             {
            //                 if (minDis > outLen)
            //                 {
            //                     minDis = outLen;
            //                     outDir = normalList[i];
            //                 }
            //             }
            //
            //             cnt += 1;
            //
            //         }
            //         else if (target[1] >= self[0] && self[1] >= target[0])
            //         { 
            //             outLen =  target[1] - self[0];
            //              
            //             if (cnt == 0)
            //             {
            //                 minDis = outLen;
            //                 outDir = -1 * normalList[i];
            //             }
            //             else
            //             {
            //                 if (minDis > outLen)
            //                 {
            //                     minDis = outLen;
            //                     outDir = -1*normalList[i];
            //                 }
            //             }
            //
            //             cnt += 1;
            //         }
            //         else if (self[0] >= target[0] && self[1] <= target[1]   )
            //         {
            //             if (self[0] - target[0] >= target[1] - self[1])
            //             {
            //                 outLen = target[1] - self[1] + self[1] - self[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = -1 * normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = -1*normalList[i];
            //                     }
            //                 }
            //
            //                 cnt += 1;
            //             }
            //             else if (  self[0] - target[0] <= target[1] - self[1] )
            //             {
            //                 outLen = self[0] - target[0] + self[1] - self[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = normalList[i];
            //                     }
            //                 }
            //                 cnt += 1;
            //             }
            //         }
            //         else if (  self[0] <= target[0] && self[1] >= target[1]  )
            //         {
            //             if (target[0] - self[0] <= self[1] - target[1])
            //             {
            //                 outLen =  target[0] - self[0] + target[1] - target[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = -1 * normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = -1*normalList[i];
            //                     }
            //                 }
            //                 cnt += 1;
            //             }
            //             else if(  target[0] - self[0] >= self[1] - target[1] )
            //             {
            //                 outLen = self[1] - target[1] + target[1] - target[0];
            //                 
            //
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = normalList[i];
            //                     }
            //                 }
            //
            //                 cnt += 1;
            //             }
            //         }
            //     }
            // }

            if (flag == false)
	            return false;
            
            normal = -1 *  outDir;
            borderAdjust = -1 * outDir * minDis;
            
            return true;
        }

        public override bool DetectRectCollision(FNPColliderRect col, ref FixedNumberVector3 normal, ref FixedNumberVector3 borderAdjust)
        {
            
            List<FixedNumberVector3> normalList = new List<FixedNumberVector3>();
            
            normalList.Add(axis[0]);
            normalList.Add(axis[2]);

            normalList.Add(col.axis[0]);
            normalList.Add(col.axis[2]);

            
            FixedNumber minDis = FixedNumber.Zero;
            FixedNumberVector3 outDir = FixedNumberVector3.Zero;

            bool flag = FNPCollisionCalcAlgo.SeparatingAxisCalc(this, col, normalList, ref outDir, ref minDis);

            if (flag == false)
                return false;
            #region MyRegion
            // int cnt = 0;
            // for (int i = 0; i < normalList.Count; i++)
            // {
            //     FixedNumber[] self = ProjectToAxis(normalList[i]);
            //     FixedNumber[] target = col.ProjectToAxis(normalList[i]);
            //
            //     if ((self[1] <= target[0]) || self[0] >= target[1])
            //     {
            //         return false;
            //     }
            //     else
            //     {
            //         FixedNumber outLen = FixedNumber.Zero;
            //         if (self[1] > target[0] && target[1] > self[1])
            //         {
            //              outLen = self[1] - target[0];
            //
            //              if (cnt == 0)
            //              {
            //                  minDis = outLen;
            //                  outDir = normalList[i];
            //              }
            //              else
            //              {
            //                  if (minDis > outLen)
            //                  {
            //                      minDis = outLen;
            //                      outDir = normalList[i];
            //                  }
            //              }
            //
            //              cnt += 1;
            //
            //         }
            //         else if (target[1] > self[0] && self[1] > target[0])
            //         { 
            //              outLen =  target[1] - self[0];
            //              
            //              if (cnt == 0)
            //              {
            //                  minDis = outLen;
            //                  outDir = -1 * normalList[i];
            //              }
            //              else
            //              {
            //                  if (minDis > outLen)
            //                  {
            //                      minDis = outLen;
            //                      outDir = -1*normalList[i];
            //                  }
            //              }
            //
            //              cnt += 1;
            //         }
            //         else if (self[0] > target[0] && self[1] < target[1]   )
            //         {
            //             if (self[0] - target[0] > target[1] - self[1])
            //             {
            //                 outLen = target[1] - self[1] + self[1] - self[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = -1 * normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = -1*normalList[i];
            //                     }
            //                 }
            //
            //                 cnt += 1;
            //             }
            //             else if (  self[0] - target[0] < target[1] - self[1] )
            //             {
            //                 outLen = self[0] - target[0] + self[1] - self[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = normalList[i];
            //                     }
            //                 }
            //                 cnt += 1;
            //             }
            //         }
            //         else if (  self[0] < target[0] && self[1] > target[1]  )
            //         {
            //             if (target[0] - self[0] < self[1] - target[1])
            //             {
            //                 outLen =  target[0] - self[0] + target[1] - target[0];
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = -1 * normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = -1*normalList[i];
            //                     }
            //                 }
            //                 cnt += 1;
            //             }
            //             else if(  target[0] - self[0] > self[1] - target[1] )
            //             {
            //                 outLen = self[1] - target[1] + target[1] - target[0];
            //                 
            //
            //                 if (cnt == 0)
            //                 {
            //                     minDis = outLen;
            //                     outDir = normalList[i];
            //                 }
            //                 else
            //                 {
            //                     if (minDis > outLen)
            //                     {
            //                         minDis = outLen;
            //                         outDir = normalList[i];
            //                     }
            //                 }
            //
            //                 cnt += 1;
            //             }
            //         }
            //         
            //         
            //     }
            // }
            #endregion

            normal = -1 *  outDir;
            borderAdjust = -1 * outDir * minDis;
            
            return true;
        }


        
        

        public override FixedNumber[] ProjectToAxis(FixedNumberVector3 axis)
        {

	       List< FixedNumberVector3> vertices = GetAllVertices();
	        
	        FixedNumber init = FixedNumberVector3.DotProduct(vertices[0], axis);
            
	        FixedNumber minPoint = init;
	        FixedNumber maxPoint = init;

	        for (int i = 1; i < vertices.Count; i++)
	        {
		        FixedNumber cur = FixedNumberVector3.DotProduct(vertices[i], axis);

		        minPoint = cur < minPoint ? cur : minPoint;
		        maxPoint = cur > maxPoint ? cur : maxPoint;
	        }

	        return new FixedNumber[] { minPoint, maxPoint };
        }
	}
}

