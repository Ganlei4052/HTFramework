using UnityEngine.AddressableAssets;

namespace HT.Framework
{
    /// <summary>
    /// 资源信息基类
    /// </summary>
    public abstract class ResourceInfoBase
    {
        /// <summary>
        /// AssetBundle的名称
        /// </summary>
        public string AssetBundleName;
        /// <summary>
        /// Asset的路径
        /// </summary>
        public string AssetPath;
        /// <summary>
        /// Resources文件夹中的路径
        /// </summary>
        public string ResourcePath;

        public string AssetReferenceName;
        
        public AssetReference AssetReferenceValue;

        public bool IsAssetReference = false;
        public ResourceInfoBase(string assetBundleName, string assetPath, string resourcePath,bool isAssetReference = false)
        {
            AssetBundleName = string.IsNullOrEmpty(assetBundleName) ? assetBundleName : assetBundleName.ToLower();
            AssetPath = assetPath;
            ResourcePath = resourcePath;
            IsAssetReference = isAssetReference;
        }

        public ResourceInfoBase(AssetReference assetReference,string assetReferenceName, bool isAssetReference = true)
        {
            AssetReferenceName = assetReferenceName;
            AssetReferenceValue = assetReference;
            IsAssetReference = isAssetReference;
        }
        /// <summary>
        /// 获取资源的Resource全路径
        /// </summary>
        internal string GetResourceFullPath()
        {
            return $"ResourcesPath: Resources/{ResourcePath}";
        }
        /// <summary>
        /// 获取资源的AssetBundle全路径
        /// </summary>
        internal string GetAssetBundleFullPath(string assetBundleRootPath)
        {
            return $"AssetBundlePath: {assetBundleRootPath}{AssetBundleName}  AssetPath:{AssetPath}";
        }
    }
}