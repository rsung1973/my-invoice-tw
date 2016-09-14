using System;

namespace Business.ResourceManagement
{
	/// <summary>
	/// Summary description for IResourceCleaner.
	/// </summary>
	public interface IResourceCleaner
	{
		void ReleaseResource(object resourceID);
	}
}
