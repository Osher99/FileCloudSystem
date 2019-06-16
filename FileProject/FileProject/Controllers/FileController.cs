using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using FileProject.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFiles(int id)
        {
            try
            {
                var listOfFiles = await _fileService.GetUserFiles(id);
                if (listOfFiles != null)
                    return new ObjectResult(listOfFiles);
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

      
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] File file)
        {
            try
            {
                bool DidLogIn = await _fileService.AddFile(file);
                if (DidLogIn)
                    return new ObjectResult(true);
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("{FileID}/{UserID}")]
        public async Task<IActionResult> Get(int FileID, int UserID)
        {
            try
            {
                var File = await _fileService.GetFile(FileID, UserID);
                if (File != null)
                    return new ObjectResult(File);
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                if (await _fileService.DeleteFile(id))
                return NoContent();
            return BadRequest();
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}