using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraintreeSample.Services.Funny
{
    public interface IFunnyService
    {
      Task<List<string>> GetLorem(int paras=1);
    }
}
