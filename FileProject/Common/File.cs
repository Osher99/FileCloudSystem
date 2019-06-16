using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class File
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FileEnding { get; set; }

        public string Description { get; set; }
        public DateTime DateUploaded { get; set; }
        public byte[] Content { get; set; }

        public string Path { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
