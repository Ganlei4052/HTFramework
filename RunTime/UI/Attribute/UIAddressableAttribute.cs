using System;

namespace HT.Framework
{
    /// <summary>
    /// UI可寻址资源标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UIAddressableAttribute : Attribute
    {
        public string AssetPath { get; private set; }
        public UIType EntityType { get; private set; }
        public string WorldUIDomainName { get; private set; }
        public string AssetBundleName { get; private set; }
        public string ResourcePath { get; private set; }

        public UIAddressableAttribute(string assetBundleName, string assetPath, string resourcePath, UIType entityType = UIType.Overlay, string worldUIDomainName = "World")
        {
            AssetPath = assetPath;
            EntityType = entityType;
            WorldUIDomainName = worldUIDomainName;
        }
    }
}