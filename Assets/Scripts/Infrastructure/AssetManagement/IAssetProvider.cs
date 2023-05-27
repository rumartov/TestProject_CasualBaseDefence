using Services;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path);
        GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent);
        GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation);
        
        GameObject Instantiate(GameObject prefab, Transform parent);
    }
}