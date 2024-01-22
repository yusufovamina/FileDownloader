using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    internal class Image
    {
        public Image(int id, string filename, byte[] imageBytes)
        {
            Id = id;
            FileName = filename;
            ImageBytes = imageBytes;
        }
        public int Id { get; private set; }
        public string FileName { get; private set; }
        public byte[] ImageBytes { get; private set; }
    }
}
