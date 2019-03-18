using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface ILocalFile
    {
        List<string> Messages { get; set; }
        List<FileDetail> Files { get; set; }

        Task<List<FileDetail>> UpdatePlayListAsync();
    }
}