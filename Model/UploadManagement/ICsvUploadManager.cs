using System;
using System.Text;

using Model.Security.MembershipManagement;

namespace Model.UploadManagement
{
    public interface ICsvUploadManager : IDisposable
    {
        bool IsValid { get; }
        void ParseData(UserProfileMember userProfile, string fileName, Encoding encoding);
        bool Save();
        int ItemCount
        {
            get;
        }
    }
}
