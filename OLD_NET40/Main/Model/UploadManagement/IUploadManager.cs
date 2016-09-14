using System;
namespace Model.UploadManagement
{
    public interface IUploadManager<TUploadItem>
     where TUploadItem : IItemUpload, new()
    {
        System.Collections.Generic.List<TUploadItem> ErrorList { get; }
        bool IsValid { get; }
        int ItemCount { get; }
        System.Collections.Generic.List<TUploadItem> ItemList { get; }
        void ParseData(Model.Security.MembershipManagement.UserProfileMember userProfile, string fileName, System.Text.Encoding encoding);
        bool Save();
    }
}
