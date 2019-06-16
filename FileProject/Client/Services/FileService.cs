using Client.Infra;
using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Client.Services
{
    public class FileService : IFileService
    {
        private readonly string BaseUri;
        public FileService()
        {
            BaseUri = "https://localhost:44339/api/file";
        }

        public async Task<IEnumerable<File>> GetAllFiles(int id)
        {
            IEnumerable<File> files;
            using (var client = new HttpClient())
            {
                var getOnlineUsers = await client.GetAsync($"{BaseUri}/{id}");
                var result = await getOnlineUsers.Content.ReadAsStringAsync();
                files = JsonConvert.DeserializeObject<IEnumerable<File>>(result);
            }
            return files;
        }

            public async Task<bool> UploadFile(File file)
        {
            var content = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync($"{BaseUri}/", content); //
                if (result.IsSuccessStatusCode == true)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> DeleteFile(int fileId)
        {
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync($"{BaseUri}/{fileId}");
                if (result.IsSuccessStatusCode == true)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<File> Browse()
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                byte[] myFile = await GetBytesAsync(file);
                var File = new File()
                {
                    Path = file.Path,
                    FileEnding = file.FileType,
                    FileName = file.DisplayName,
                    Content = myFile,
                    DateUploaded = DateTime.Now
                };
                return File;
            }
            return null;
        }

        public async Task<byte[]> GetBytesAsync(StorageFile file)
        {
            byte[] fileBytes = null;
            if (file == null) return null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }

            public async Task<bool> SaveFile(File downloadedFile)
            {
                FileSavePicker savePicker = new FileSavePicker();
                //savePicker.DefaultFileExtension = downloadedFile.FileEnding;
                savePicker.FileTypeChoices.Add("Original extension", new List<string>() { downloadedFile.FileEnding });
                savePicker.FileTypeChoices.Add("New extension", new List<string>() { "." });
                savePicker.SuggestedFileName = downloadedFile.FileName;
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                var file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    await FileIO.WriteBytesAsync(file, downloadedFile.Content);
                    return true;
                }
                return false;
        }
    }
}