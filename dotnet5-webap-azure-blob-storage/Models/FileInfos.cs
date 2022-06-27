using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlob.Api.Models
{
    public class FileInfos
    {
        public string Filename { get; set; }
        public string AzureId { get; set; }
        public string AzureName { get; set; }
        public DateTimeOffset Created { get; set; }
        public string ContainerName { get; set; }
        public string AccoutName { get; set; }

    }
}
