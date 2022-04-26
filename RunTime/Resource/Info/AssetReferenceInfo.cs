using UnityEngine.AddressableAssets;

namespace HT.Framework
{
    public class AssetReferenceInfo : ResourceInfoBase
    {
        public AssetReferenceInfo(string assetBundleName, string assetPath, string resourcePath, bool isAssetReference = false) : base(assetBundleName,
            assetPath, resourcePath, isAssetReference)
        {

        }
        public AssetReferenceInfo(AssetReference assetReference, string assetReferenceName, bool isAssetReference = true) : base(assetReference, assetReferenceName, isAssetReference)
        {

        }

    }
}