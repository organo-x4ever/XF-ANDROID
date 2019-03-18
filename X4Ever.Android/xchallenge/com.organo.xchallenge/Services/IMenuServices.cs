using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Services
{
    public interface IMenuServices : IBaseService
    {
        Task<List<Menu>> GetByApplicationAsync();
    }
}