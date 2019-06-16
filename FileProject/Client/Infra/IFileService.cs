using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Client.Infra
{
    public interface IFileService
    {
        Task<IEnumerable<File>> GetAllFiles(int id);
        Task<bool> UploadFile(File file);
        Task<File> Browse();

        Task<byte[]> GetBytesAsync(StorageFile file);

        Task<bool> DeleteFile(int id);

        Task<bool> SaveFile(File downloadedFile);
    }
}
