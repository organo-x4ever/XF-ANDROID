﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Services
{
    public interface IApplicationSettingService : IBaseService
    {
        ApplicationSetting Get();
        Task<ApplicationSetting> GetAsync();
    }
}
