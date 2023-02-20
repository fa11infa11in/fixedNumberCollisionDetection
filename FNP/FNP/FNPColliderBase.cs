using System;
using System.Collections.Generic;
using FNM;

namespace FNP
{
    public class FNPColliderConfig
    {
        public ColliderType type;

        //for rect
        public FixedNumberVector3 pos;
        public FixedNumberVector3 size;
        public FixedNumberVector3[] axis;
        
        //for circle
        public FixedNumber radius;
    }

    public enum ColliderType
    {
        None,
        Rect,
        Circle,
    }
    
    public class CollsionInfo
    {
        public FNPCollider2D collider;
        public FixedNumberVector3 normal;
        public FixedNumberVector3 borderAdjust;
    }

    public abstract class FNPCollider2D
    {

        public FixedNumberVector3 pos;

        

        public virtual bool DetectCollision(FNPCollider2D col, ref FixedNumberVector3 normal, ref FixedNumberVector3 borderAdjust)
        {
            if (col is FNPColliderRect)
            {
                return DetectRectCollision((FNPColliderRect)col, ref normal, ref borderAdjust);
            }
            else if (col is FNPColliderCircle)
            {
                return DetectCircleCollision((FNPColliderCircle)col, ref normal, ref borderAdjust);
            }

            return false;
        }
        
        public abstract bool DetectCircleCollision(FNPColliderCircle col, ref FixedNumberVector3 normal,
            ref FixedNumberVector3 borderAdjust);
        
        public abstract bool DetectRectCollision(FNPColliderRect col, ref FixedNumberVector3 normal,
            ref FixedNumberVector3 borderAdjust);

        
        public virtual List<FixedNumberVector3> GetAllVertices()
        {
            return null;
        }

        public abstract FixedNumber[] ProjectToAxis(FixedNumberVector3 axis);

        public virtual void CalcCollidersInteraction(List<FNPCollider2D> colliderList, ref FixedNumberVector3 velocity,
            ref FixedNumberVector3 borderAdjust)
        {
            if( velocity == FixedNumberVector3.Zero )
            {return;}
            
            FixedNumberVector3 normal = FixedNumberVector3.Zero;
            FixedNumberVector3 adj = FixedNumberVector3.Zero;


            List<CollsionInfo> collisionInfoList = new List<CollsionInfo>();
            for (int i = 0; i <colliderList.Count; i++)
            {
                
                if (DetectCollision(colliderList[i], ref normal, ref adj))
                {
                    CollsionInfo info = new CollsionInfo()
                    {
                        collider = colliderList[i],
                        normal = normal,
                        borderAdjust = adj,
                    };
                    collisionInfoList.Add(info);
                }
            }

            if (collisionInfoList.Count == 1)
            {
                CollsionInfo info = collisionInfoList[0];
                velocity = CorrectVelocity(velocity, info.normal);
               // this.Log("碰撞法线： " + info.normal);
               // this.Log("边界修正： " + info.borderAdjust);
                borderAdjust = info.borderAdjust;
               // this.Log("单个：" + borderAdjust);
              // this.Log("单个碰撞体，校正速度 ： " + velocity.ToString());
            }
            else if( collisionInfoList.Count > 1 )
            {
                FixedNumberVector3 centerNormal = FixedNumberVector3.Zero;
                CollsionInfo closestNormalCollisionInfo = new CollsionInfo();
                FixedNumber maxAngle = CalcMaxNormalAngle(collisionInfoList, velocity, ref centerNormal, ref closestNormalCollisionInfo);
                FixedNumber angle = FixedNumberVector3.Angle(-1 * velocity, centerNormal);

                if (angle > maxAngle)
                {
                    velocity = ModifyVelocity(velocity, closestNormalCollisionInfo.normal);
                    FixedNumberVector3 adjustFusion = FixedNumberVector3.Zero;
                    for (int i = 0; i < collisionInfoList.Count; i++)
                    {
                        adjustFusion += collisionInfoList[i].borderAdjust;
                    }

                    borderAdjust = adjustFusion;
                }
                else
                {
                    velocity = FixedNumberVector3.Zero;
                    this.Log("can't move.");
                }
            }
            
            this.Log("final bj: " + borderAdjust);
            

        }
        
        public virtual FixedNumberVector3 ModifyVelocity(FixedNumberVector3 velocity, FixedNumberVector3 normal)
        {
            if (normal == FixedNumberVector3.Zero)
                return velocity;

            if (FixedNumberVector3.Angle(velocity, normal) > FixedNumber.Pi / 2 )
            {
                FixedNumber projectingLen = FixedNumberVector3.DotProduct(velocity, normal);
                if( projectingLen != 0)
                    velocity -= normal * projectingLen;
            }

            return velocity;

        }
        
        public virtual FixedNumber CalcMaxNormalAngle(List<CollsionInfo> infoList,FixedNumberVector3 velocity, ref FixedNumberVector3 centerNormal,ref CollsionInfo info)
        {
            for (int i = 0; i < infoList.Count; i++)
            {
                centerNormal += infoList[i].normal;
            }

            centerNormal /= infoList.Count;

            FixedNumber maxAngle = FixedNumber.Zero;
            FixedNumber velocityAngle = FixedNumber.Zero;
            for (int i = 0; i < infoList.Count; i++)
            {
                FixedNumber curNormalAngle = FixedNumberVector3.Angle(centerNormal, infoList[i].normal);
                maxAngle = curNormalAngle > maxAngle ? curNormalAngle : maxAngle;
                
                FixedNumber curVelocityAngle = FixedNumberVector3.Angle(velocity, infoList[i].normal);
                if (velocityAngle < curVelocityAngle)
                {
                    velocityAngle = curVelocityAngle;
                    info = infoList[i];
                }

            }

            return maxAngle;

        }

        public virtual FixedNumberVector3 CorrectVelocity(FixedNumberVector3 velocity, FixedNumberVector3 normal)
        {
            if (normal == FixedNumberVector3.Zero)
            {
                return velocity;
            }
            
            //确保是靠近不是远离
            if (FixedNumberVector3.Angle(normal, velocity) > (FixedNumber.Pi / 2))
            {
                FixedNumber prjLen = FixedNumberVector3.DotProduct(velocity, normal);
                if (prjLen != 0)
                {
                    velocity -= prjLen * normal;
                }
            }

            return velocity;
        }


    }
}

