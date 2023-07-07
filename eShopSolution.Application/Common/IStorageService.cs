using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Common
{
	//Nhiệm vụ Save File + lấy thông tin File
	public interface IStorageService
	{
		string GetFileUrl(string fileName);
		Task SaveFileAsync (Stream mediaBinaryStream, string fileName);
		Task DeleteFileAsync (string fileName);
	}
}
