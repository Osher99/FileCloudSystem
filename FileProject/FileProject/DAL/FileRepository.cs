using Common;
using FileProject.Data;
using FileProject.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.DAL
{
    public class FileRepository: IFileRepositroy
    {
        public readonly UserContext _userContext;
        public FileRepository(UserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<File> GetFile(int fileId)
        {
            return _userContext.Files.Where(p => p.FileID == fileId).SingleOrDefault();
        }

        public async Task<IEnumerable<File>> GetAllFiles(int userId)
        {
            return _userContext.Files.Where(p => p.UserID == userId).ToList();
        }

        public async Task<bool> AddFile(File file)
        {
            _userContext.Files.Add(file);
            await _userContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFile(int fileId)
        {
            var selectedFile = await GetFile(fileId);

            if (selectedFile != null)
            {
                _userContext.Files.Remove(selectedFile);
                await _userContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<File> GetFile(int fileID, int userID)
        {
            var file = _userContext.Files.FirstOrDefault(u => u.FileID == fileID && u.UserID == userID);
            return file;
        }
    }
}
