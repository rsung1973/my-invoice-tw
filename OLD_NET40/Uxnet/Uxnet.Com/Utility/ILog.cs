using System;

namespace Utility
{
	/// <summary>
	/// ILog ���K�n�y�z�C
	/// </summary>
	public interface ILog
	{
		string Subject
		{
			get;
		}

		string ToString();

	}

    public interface ILog2 : ILog
    {
        string GetFileName(string currentLogPath,string qName,ulong key);
    }
}
