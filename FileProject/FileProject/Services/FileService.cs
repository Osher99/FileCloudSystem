using Common;
using FileProject.DAL;
using FileProject.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepositroy _fileRepo;


        public FileService(IFileRepositroy fileRepo)
        {
            _fileRepo = fileRepo;
        }
        public async Task<bool> AddFile(File file)
        {
            if (!await _fileRepo.AddFile(file))
                return false;
            return true;
        }

        public Task<bool> DeleteFile(int fileId)
        {
             return _fileRepo.DeleteFile(fileId);           
        }

        public async Task<IEnumerable<File>> GetUserFiles(int id)
        {
            IEnumerable<File> allFiles = await _fileRepo.GetAllFiles(id);
            
            return allFiles;
        }

        public async Task<File> GetFile(int FileID, int UserID)
        {
            var file = await _fileRepo.GetFile(FileID, UserID);
            return file;
        }
        public async Task<File> GetFile(int FileID)
        {
            var file = await _fileRepo.GetFile(FileID);
            return file;
        }
    }
}
