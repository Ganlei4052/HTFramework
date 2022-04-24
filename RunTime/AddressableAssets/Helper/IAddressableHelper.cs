using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace HT.Framework
{
    /// <summary>
    /// 可寻址资源系统管理助手
    /// </summary>
    public interface IAddressableHelper : IInternalModuleHelper
    {
        /// <summary>
        /// 缓存的所有资源加载器【address、加载器】
        /// </summary>
        Dictionary<string, AsyncOperationHandle> AssetCache { get; }

        /// <summary>
        /// 缓存的所有场景加载器【address、加载器】
        /// </summary>
        Dictionary<string, AsyncOperationHandle> SceneCache { get; }


        void SetLoader();

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
        IEnumerator LoadAssetAsync<T>(ResourceInfoBase info, HTFAction<float> onLoading, HTFAction<T> onLoadDone,
            bool isPrefab, Transform parent, bool isUI) where T : Object;
        
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
        IEnumerator LoadAssetAsync<T>(AssetReferenceInfo info, HTFAction<float> onLoading, HTFAction<T> onLoadDone) where T : Object;
        
        /// <summary>
        /// 加载场景（异步）
        /// </summary>
        /// <param name="info">资源信息标记</param>
        /// <param name="onLoading">加载中事件</param>
        /// <param name="onLoadDone">加载完成事件</param>
        /// <returns>加载协程迭代器</returns>
        IEnumerator LoadSceneAsync(SceneInfo info, HTFAction<float> onLoading, HTFAction onLoadDone);

        /// <summary>
        /// 卸载资源（异步，Resource模式：卸载未使用的资源，AssetBundle模式：卸载AB包）
        /// </summary>
        /// <param name="assetBundleName">AB包名称</param>
        /// <param name="unloadAllLoadedObjects">是否同时卸载所有实体对象</param>
        /// <returns>卸载协程迭代器</returns>
        IEnumerator UnLoadAsset(string assetBundleName, bool unloadAllLoadedObjects = false);

        /// <summary>
        /// 卸载所有资源（异步，Resource模式：卸载未使用的资源，AssetBundle模式：卸载AB包）
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否同时卸载所有实体对象</param>
        /// <returns>卸载协程迭代器</returns>
        IEnumerator UnLoadAllAsset(bool unloadAllLoadedObjects = false);

        /// <summary>
        /// 卸载场景（异步）
        /// </summary>
        /// <param name="info">资源信息标记</param>
        /// <returns>卸载协程迭代器</returns>
        IEnumerator UnLoadScene(SceneInfo info);

        /// <summary>
        /// 卸载所有场景（异步）
        /// </summary>
        /// <returns>卸载协程迭代器</returns>
        IEnumerator UnLoadAllScene();

        /// <summary>
        /// 清理内存，释放空闲内存（异步）
        /// </summary>
        /// <returns>协程迭代器</returns>
        IEnumerator ClearMemory();
    }
}