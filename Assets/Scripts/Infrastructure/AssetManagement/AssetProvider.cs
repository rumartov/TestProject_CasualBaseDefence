using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        
        public GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent)
        {
            return Object.Instantiate(prefab, at, rotation, parent);
        }

        public GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation)
        {
            return Object.Instantiate(prefab, at, rotation);
        }

        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }
    }
}