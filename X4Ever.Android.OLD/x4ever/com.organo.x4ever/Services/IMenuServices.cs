using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Models;

namespace com.organo.x4ever.Services
{
    public interface IMenuServices : IBaseService
    {
        Task<List<Menu>> GetByApplicationAsync();
    }
}