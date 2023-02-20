using System.Collections.Generic;
using FNM;

namespace FNP
{
    public static class FNPCollisionCalcAlgo
    {
        public static bool SeparatingAxisCalc(FNPCollider2D selfcol, FNPCollider2D targetcol, List<FixedNumberVector3> normalList, ref FixedNumberVector3 outDir, ref FixedNumber minDis)
        {
            int cnt = 0;
            
            for (int i = 0; i < normalList.Count; i++)
            {
                FixedNumber[] self = selfcol.ProjectToAxis(normalList[i]);
                FixedNumber[] target = targetcol.ProjectToAxis(normalList[i]);

                if ((self[1] <= target[0]) || self[0] >= target[1])
                {
                    return false;
                }
                else
                {
                    FixedNumber outLen = FixedNumber.Zero;
                    if (self[1] > target[0] && target[1] > self[1])
                    {
                         outLen = self[1] - target[0];

                         if (cnt == 0)
                         {
                             minDis = outLen;
                             outDir = normalList[i];
                         }
                         else
                         {
                             if (minDis > outLen)
                             {
                                 minDis = outLen;
                                 outDir = normalList[i];
                             }
                         }

                         cnt += 1;

                    }
                    else if (target[1] > self[0] && self[1] > target[0])
                    { 
                         outLen =  target[1] - self[0];
                         
                         if (cnt == 0)
                         {
                             minDis = outLen;
                             outDir = -1 * normalList[i];
                         }
                         else
                         {
                             if (minDis > outLen)
                             {
                                 minDis = outLen;
                                 outDir = -1*normalList[i];
                             }
                         }

                         cnt += 1;
                    }
                    else if (self[0] > target[0] && self[1] < target[1]   )
                    {
                        if (self[0] - target[0] > target[1] - self[1])
                        {
                            outLen = target[1] - self[1] + self[1] - self[0];
                            if (cnt == 0)
                            {
                                minDis = outLen;
                                outDir = -1 * normalList[i];
                            }
                            else
                            {
                                if (minDis > outLen)
                                {
                                    minDis = outLen;
                                    outDir = -1*normalList[i];
                                }
                            }

                            cnt += 1;
                        }
                        else if (  self[0] - target[0] < target[1] - self[1] )
                        {
                            outLen = self[0] - target[0] + self[1] - self[0];
                            if (cnt == 0)
                            {
                                minDis = outLen;
                                outDir = normalList[i];
                            }
                            else
                            {
                                if (minDis > outLen)
                                {
                                    minDis = outLen;
                                    outDir = normalList[i];
                                }
                            }
                            cnt += 1;
                        }
                    }
                    else if (  self[0] < target[0] && self[1] > target[1]  )
                    {
                        if (target[0] - self[0] < self[1] - target[1])
                        {
                            outLen =  target[0] - self[0] + target[1] - target[0];
                            if (cnt == 0)
                            {
                                minDis = outLen;
                                outDir = -1 * normalList[i];
                            }
                            else
                            {
                                if (minDis > outLen)
                                {
                                    minDis = outLen;
                                    outDir = -1*normalList[i];
                                }
                            }
                            cnt += 1;
                        }
                        else if(  target[0] - self[0] > self[1] - target[1] )
                        {
                            outLen = self[1] - target[1] + target[1] - target[0];
                            

                            if (cnt == 0)
                            {
                                minDis = outLen;
                                outDir = normalList[i];
                            }
                            else
                            {
                                if (minDis > outLen)
                                {
                                    minDis = outLen;
                                    outDir = normalList[i];
                                }
                            }

                            cnt += 1;
                        }
                    }
                    
                    
                }
            }

            return true;
        }
    }
}