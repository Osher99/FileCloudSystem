using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.Infra
{
    public interface IFileRepositroy
    {

        Task<File> GetFile(int fileId);

        Task<IEnumerable<File>> GetAllFiles(int userId);

        Task<bool> AddFile(File file);

        Task<bool> DeleteFile(int fileId);

        Task<File> GetFile(int fileID, int userID);  

    }
}
