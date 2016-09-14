using System;
using System.Text;

using Model.Security.MembershipManagement;

namespace Model.InvoiceManagement
{
    public interface IGoogleUploadManager : IDisposable
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
