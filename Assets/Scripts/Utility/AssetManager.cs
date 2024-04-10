using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility
{
    public class AssetManager : IDisposable
    {
        public async Task<GameObject> InstantiateAsync(string assetId, Transform transform)
        {
            var asyncOperationHandle = Addressables.InstantiateAsync(assetId,transform);
            if (asyncOperationHandle.Task != null) {
                    await asyncOperationHandle.Task;
            }
            else {
                    return null;
            }
            
            return asyncOperationHandle.Result == null ? null : asyncOperationHandle.Result;
        }
        
        
        public async Task<T> LoadAssetAsync<T>(string assetId)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(assetId);
            if (asyncOperationHandle.Task != null) {
                await asyncOperationHandle.Task;
            }
            else {
                return default;
            }
            
            return asyncOperationHandle.Result == null ? default : asyncOperationHandle.Result;
        }
        
        public void Dispose()
        {
        }
    }
}