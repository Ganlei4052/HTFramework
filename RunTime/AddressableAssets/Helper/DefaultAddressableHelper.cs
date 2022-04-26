using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace HT.Framework
{
    public class DefaultAddressableHelper : IAddressableHelper
    {
        /// <summary>
        /// 单线下载中
        /// </summary>
        private bool _isLoading = false;

        /// <summary>
        /// 单线下载等待
        /// </summary>
        private WaitUntil _loadWait;

        /// <summary>
        /// 资源管理器
        /// </summary>
        public IModuleManager Module { get; set; }

        /// <summary>
        /// 缓存的所有资源加载器【address、加载器】
        /// </summary>
        public Dictionary<string, AsyncOperationHandle> AssetCache { get; private set; } =
            new Dictionary<string, AsyncOperationHandle>();

        /// <summary>
        /// 缓存的所有场景加载器【address、加载器】
        /// </summary>
        public Dictionary<string, AsyncOperationHandle> SceneCache { get; private set; } =
            new Dictionary<string, AsyncOperationHandle>();

        public void OnInit()
        {
        }

        public void OnReady()
        {
        }

        public void OnUpdate()
        {
        }

        public void OnTerminate()
        {
            UnLoadAllAsset();
            ClearMemory();
        }

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }

        public void SetLoader()
        {
            _loadWait = new WaitUntil(() => { return !_isLoading; });
        }

        /// <summary>
        /// 克隆预制体
        /// </summary>
        /// <param name="prefabTem">预制体模板</param>
        /// <param name="parent">克隆后的父级</param>
        /// <param name="isUI">是否是UI</param>
        /// <returns>克隆后的预制体</returns>
        private GameObject ClonePrefab(GameObject prefabTem, Transform parent, bool isUI)
        {
            GameObject prefab = prefabTem;

            if (parent)
            {
                prefab.transform.SetParent(parent);
            }

            if (isUI)
            {
                prefab.rectTransform().anchoredPosition3D = Vector3.zero;
                prefab.rectTransform().sizeDelta = Vector3.zero;
                prefab.rectTransform().anchorMin = prefabTem.rectTransform().anchorMin;
                prefab.rectTransform().anchorMax = prefabTem.rectTransform().anchorMax;
                prefab.transform.localRotation = Quaternion.identity;
                prefab.transform.localScale = Vector3.one;
            }
            else
            {
                prefab.transform.localPosition = prefabTem.transform.localPosition;
                prefab.transform.localRotation = Quaternion.identity;
                prefab.transform.localScale = Vector3.one;
            }

            prefab.SetActive(false);
            return prefab;
        }

        /// <summary>
        /// 加载资源（异步）
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="info">资源信息标记</param>
        /// <param name="onLoading">加载中事件</param>
        /// <param name="onLoadDone">加载完成事件</param>
        /// <param name="isPrefab">是否是加载预制体</param>
        /// <param name="parent">预制体加载完成后的父级</param>
        /// <param name="isUI">是否是加载UI</param>
        /// <returns>加载协程迭代器</returns>
        public IEnumerator LoadAssetAsync<T>(ResourceInfoBase info, HTFAction<float> onLoading, HTFAction<T> onLoadDone,
            bool isPrefab,
            Transform parent, bool isUI) where T : Object
        {
            //单线加载，如果其他地方在加载资源，则等待
            if (_isLoading)
            {
                yield return _loadWait;
            }

            Object asset = null;
            AsyncOperationHandle handle;

            if (AssetCache.ContainsKey(info.AssetPath))
            {
                handle = AssetCache[info.AssetPath];
            }
            else
            {
                if (isPrefab)
                {
                    handle = Addressables.InstantiateAsync(info.AssetPath);
                }
                else
                {
                    handle = Addressables.LoadAssetAsync<T>(info.AssetPath);
                }
            }

            if (handle.Status == AsyncOperationStatus.Failed)
            {
                throw new HTFrameworkException(HTFrameworkModule.Addressable,
                    handle.OperationException.Message);
            }

            while (!handle.IsDone)
            {
                onLoading?.Invoke(handle.PercentComplete);
                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                asset = handle.Result as Object;
                if (asset != null)
                {
                    AssetCache.Add(info.AssetPath, handle);

                    if (isPrefab)
                    {
                        asset = ClonePrefab(asset as GameObject, parent, isUI);
                    }

                    DataSetInfo dataSet = info as DataSetInfo;
                    if (dataSet != null && dataSet.Data != null)
                    {
                        asset.Cast<DataSetBase>().Fill(dataSet.Data);
                    }

                    onLoadDone?.Invoke(asset as T);
                }
                else
                {
                    onLoadDone?.Invoke(null);
                    throw new HTFrameworkException(HTFrameworkModule.Resource,
                        "请求：" + info.AssetPath + " 未下载到AB包！");
                }
            }

            asset = null;
            onLoading?.Invoke(1);
            //本线路加载资源结束
            _isLoading = false;
        }

        public IEnumerator LoadAssetAsync<T>(AssetReferenceInfo info, HTFAction<float> onLoading,
            HTFAction<T> onLoadDone) where T : Object
        {
            //单线加载，如果其他地方在加载资源，则等待
            if (_isLoading)
            {
                yield return _loadWait;
            }

            Object asset = null;
            AsyncOperationHandle handle;
            bool isAssetReference = info.IsAssetReference;
            if (!isAssetReference)
            {
                if (AssetCache.ContainsKey(info.AssetPath))
                {
                    handle = AssetCache[info.AssetPath];
                }
                else
                {
                    handle = Addressables.LoadAssetAsync<T>(info.AssetPath);
                }
            }
            else
            {
                if (AssetCache.ContainsKey(info.AssetReferenceName))
                {
                    handle = AssetCache[info.AssetReferenceName];
                }
                else
                {
                    handle = Addressables.LoadAssetAsync<T>(info.AssetReferenceValue);
                }
            }


            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Log.Error("info:" + info.AssetReferenceName);
                throw new HTFrameworkException(HTFrameworkModule.Addressable,
                    handle.OperationException.Message);
            }

            while (!handle.IsDone)
            {
                onLoading?.Invoke(handle.PercentComplete);
                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                asset = handle.Result as Object;
                if (asset != null)
                {
                    if (!isAssetReference)
                    {
                        AssetCache.Add(info.AssetPath, handle);
                    }
                    else
                    {
                        AssetCache.Add(info.AssetReferenceName, handle);
                    }
                    onLoadDone?.Invoke(asset as T);
                }
                else
                {
                    onLoadDone?.Invoke(null);
                    if (!isAssetReference)
                    {
                        throw new HTFrameworkException(HTFrameworkModule.Resource,
                            "请求：" + info.AssetPath + " 未下载到AB包！");
                    }
                    else
                    {
                        throw new HTFrameworkException(HTFrameworkModule.Resource,
                            "请求：" + info.AssetReferenceName + " 未下载到AB包！");
                    }
                    
                }
            }

            asset = null;
            onLoading?.Invoke(1);
            //本线路加载资源结束
            _isLoading = false;
        }

        /// <summary>
        /// 加载场景（异步）
        /// </summary>
        /// <param name="info">资源信息标记</param>
        /// <param name="onLoading">加载中事件</param>
        /// <param name="onLoadDone">加载完成事件</param>
        /// <returns>加载协程迭代器</returns>
        public IEnumerator LoadSceneAsync(SceneInfo info, HTFAction<float> onLoading, HTFAction onLoadDone)
        {
            if (SceneCache.ContainsKey(info.AssetPath))
            {
                Log.Warning(string.Format("加载场景失败：名为 {0} 的场景已加载", info.AssetPath));
                yield break;
            }

            //单线加载，如果其他地方在加载资源，则等待
            if (_isLoading)
            {
                yield return _loadWait;
            }

            //轮到本线路加载资源
            _isLoading = true;

            var handle = Addressables.LoadSceneAsync(info.AssetPath, LoadSceneMode.Additive);

            if (handle.Status == AsyncOperationStatus.Failed)
            {
                throw new HTFrameworkException(HTFrameworkModule.Addressable,
                    handle.OperationException.Message);
            }

            while (!handle.IsDone)
            {
                onLoading?.Invoke(handle.PercentComplete);
                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                SceneCache.Add(info.AssetPath, handle);
                onLoading?.Invoke(1);
            }

            onLoadDone?.Invoke();
            //本线路加载资源结束
            _isLoading = false;
        }

        public void UnLoadAsset(string address)
        {
            if (AssetCache.ContainsKey(address))
            {
                Addressables.Release(AssetCache[address]);
                AssetCache.Remove(address);
            }
            else
            {
                Log.Error("资源未加载：" + address + "如果是AssetReference请传入AssetReferenceName，Addressables则传入地址");
            }
        }

        public void UnLoadAllAsset()
        {
            foreach (var asyncOperationHandle in AssetCache)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }

            AssetCache.Clear();
        }

        public IEnumerator UnLoadScene(SceneInfo info)
        {
            if (!SceneCache.ContainsKey(info.AssetPath))
            {
                Log.Warning(string.Format("卸载场景失败：名为 {0} 的场景还未加载！", info.AssetPath));
                yield break;
            }

            var handle = Addressables.UnloadSceneAsync(SceneCache[info.AssetPath]);
            yield return handle;
            if (handle.IsDone)
            {
                Addressables.Release(SceneCache[info.AssetPath]);
                SceneCache.Remove(info.AssetPath);
            }
        }

        /// <summary>
        /// 卸载所有场景（异步）
        /// </summary>
        /// <returns>卸载协程迭代器</returns>
        public IEnumerator UnLoadAllScene()
        {
            foreach (var scene in SceneCache)
            {
                var handle = Addressables.UnloadSceneAsync(scene.Value);
                yield return handle;
                if (handle.IsDone)
                {
                    Addressables.Release(scene.Value);
                }
            }
            SceneCache.Clear();
            yield return null;
        }

        /// <summary>
        /// 清理内存，释放空闲内存（异步）
        /// </summary>
        /// <returns>协程迭代器</returns>
        public IEnumerator ClearMemory()
        {
            yield return Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}