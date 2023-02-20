using System;
using System.Collections.Generic;
using FNM;
namespace FNP
{
	public class FNPColliderCircle : FNPCollider2D
	{

		public FixedNumber radius;

        public FNPColliderCircle (FNPColliderConfig config)
        {
            this.radius = config.radius;
            this.pos = config.pos;
        }


        public override bool DetectCircleCollision(FNPColliderCircle col, ref FixedNumberVector3 normal, ref FixedNumberVector3 borderAdjust)
        {
            FixedNumberVector3 dis = pos - col.pos;
            if (dis.SqMagnitude() > (radius + col.radius) * (radius + col.radius))
            {
                return false;
            }
            else
            {
                normal = dis.normalized;
                borderAdjust = normal * (radius + col.radius - dis.Magnitude());
                return true;
            }
        }

        public override bool DetectRectCollision(FNPColliderRect col, ref FixedNumberVector3 normal, ref FixedNumberVector3 borderAdjust)
        {
           
            List<FixedNumberVector3> normalList = new List<FixedNumberVector3>();
            
            normalList.Add(GetAxisForCylinder(col));
            
            normalList.Add(col.axis[0]);
            normalList.Add(col.axis[2]);
            

            FixedNumber minDis = FixedNumber.Zero;
            FixedNumberVector3 outDir = FixedNumberVector3.Zero;
            
            bool flag = FNPCollisionCalcAlgo.SeparatingAxisCalc(this, col, normalList, ref outDir, ref minDis);
            if (flag == false)
                return false;
            
            // int cnt = 0;
            //
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
            //         else if (target[1] > self[0] && self[1] > target[0])
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
            //     }
            // }
            
            normal = -1 *  outDir;
            borderAdjust = -1 * outDir * minDis;
            
            return true;

        }
        
        public FixedNumberVector3 GetAxisForCylinder(FNPColliderRect col)
        {
            List<FixedNumberVector3> vertices = col.GetAllVertices();

            FixedNumberVector3 center = pos; 
            FixedNumberVector3 minLenVec = FixedNumberVector3.Zero; 
            FixedNumber minlenSq = FixedNumber.Zero;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == 0)
                {
                    minLenVec = vertices[i] - center;
                    minlenSq = minLenVec.SqMagnitude();
                }
                else
                {
                    if (minlenSq > (vertices[i] - center).SqMagnitude())
                    {
                        minLenVec = vertices[i] - center;
                        minlenSq = minLenVec.SqMagnitude();
                    }
                }
            }
            
            return minLenVec.normalized;
        }

        public override FixedNumber[] ProjectToAxis(FixedNumberVector3 axis)
        {
            FixedNumber prjCen = FixedNumberVector3.DotProduct(pos, axis);
            return new FixedNumber[]
            {
                prjCen - radius, prjCen + radius
            };
        }
    }
}

