using com.organo.x4ever.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface ILocalFile
    {
        List<string> Messages { get; set; }
        List<FileDetail> Files { get; set; }

        Task<List<FileDetail>> UpdatePlayListAsync();
    }
}