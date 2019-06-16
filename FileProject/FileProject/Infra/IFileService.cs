using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.Infra
{
    public interface IFileService
    {

        Task<IEnumerable<File>> GetUserFiles(int id);

        Task<bool> AddFile(File file);

        Task<bool> DeleteFile(int fileId);
        Task<File> GetFile(int FileID, int UserID);
        Task<File> GetFile(int FileID);


    }
}
