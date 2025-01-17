using UnityEngine;

namespace Mono_Pool
{
    public interface IPoolableObject
    {
        public void Reset();
        public void SetPlayerTransform(Transform player);
    }
}