using System.Collections.Generic;
using UnityEngine;

namespace Alice.Utils
{
    [System.Serializable]
    public class BoundingBox
    {
        public List<float> min, max;

        public Bounds AsBounds()
        {
            if (min is null || min.Count != 3 || max is null || max.Count != 3)
                return new Bounds();

            return new Bounds(
                new Vector3((min[0] + max[0])/2f,(min[1] + max[1])/2f, (min[2] + max[2])/2f),
                new Vector3(-min[0] + max[0],-min[1] + max[1], -min[2] + max[2]));
        }
    }
}