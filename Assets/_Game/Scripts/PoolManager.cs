namespace LetsStartAKittyCult
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    public class PoolManager : MonoBehaviour
    {
        private static Dictionary<GameObject, Pool> managedPools =
            new Dictionary<GameObject, Pool>();


        public static void Add(Pool pool)
        {
            if (pool.Prefab == null)
            {
                Debug.LogError(
                    "Prefab of pool: " + pool.gameObject.name + " is empty! " +
                    "Can't add pool to managedPools Dictionary.");
                return;
            }

            if (managedPools.ContainsKey(pool.Prefab))
            {
                Debug.LogError(
                    "Pool with prefab " + pool.Prefab.name + " has already been added " +
                    "to managedPools Dictionary.");
                return;
            }

            managedPools.Add(pool.Prefab, pool);
        }


        public static Pool CreatePool(
            GameObject prefab,
            int preLoad,
            bool limit,
            int maxCount,
            GameObject poolTarget = null)
        {
            if (managedPools.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager already contains Pool for prefab: " + prefab.name);
                return managedPools[prefab];
            }

            if (poolTarget == null)
                poolTarget = new GameObject(prefab.name + " Pool");
            Pool poolComponent = poolTarget.AddComponent<Pool>();

            poolComponent.Prefab = prefab;
            poolComponent.PreloadAmount = preLoad;
            poolComponent.Limit = limit;
            poolComponent.MaxCount = maxCount;

            poolComponent.Awake();

            return poolComponent;
        }


        public static void DeactivatePool(GameObject prefab)
        {
            if (!managedPools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to deactivate: " + prefab.name);
                return;
            }

            List<GameObject> activeList = managedPools[prefab].Active.ToList();
            foreach (GameObject item in activeList)
                managedPools[prefab].Despawn(item);
        }


        public static void Despawn(GameObject instance)
        {
            GetPool(instance).Despawn(instance);
        }


        public static void DestroyAllInactive(bool limitToPreLoad)
        {
            foreach (GameObject prefab in managedPools.Keys)
                managedPools[prefab].DestroyUnused(limitToPreLoad);
        }


        public static void DestroyAllPools()
        {
            foreach (GameObject prefab in managedPools.Keys)
                DestroyPool(managedPools[prefab].gameObject);
        }


        public static void DestroyPool(GameObject prefab)
        {
            if (!managedPools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to destroy: " + prefab.name);
                return;
            }

            Destroy(managedPools[prefab].gameObject);
            managedPools.Remove(prefab);
        }


        public static Pool GetPool(GameObject instance)
        {
            foreach (GameObject prefab in managedPools.Keys)
            {
                if (managedPools[prefab].Active.Contains(instance))
                    return managedPools[prefab];

                if (managedPools[prefab].Inactive.Contains(instance))
                    return managedPools[prefab];
            }

            Debug.LogWarning("PoolManager couldn't find Pool for instance: " + instance.name);
            return null;
        }


        public static bool IsPooled(GameObject instance)
        {
            Pool pool = GetPool(instance);
            return pool != null;
        }


        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }


        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, position, Quaternion.identity);
        }


        public static GameObject Spawn(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation)
        {
            if (!managedPools.ContainsKey(prefab))
            {
                Debug.Log("New pool: " + prefab.name);
                CreatePool(prefab, 0, false, 0);
            }

            return managedPools[prefab].Spawn(position, rotation);
        }


        public void OnDestroy()
        {
            managedPools.Clear();
        }
    }
}