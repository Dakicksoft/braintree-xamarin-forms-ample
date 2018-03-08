using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BraintreeSample.Services.RequestProvider;

namespace BraintreeSample.Services.Funny
{
  public class FunnyService : IFunnyService
  {

    readonly IRequestProvider _requestProvider;

    public FunnyService(IRequestProvider requestProvider)
    {
      _requestProvider = requestProvider;
    }

    public async Task<List<string>> GetLorem(int paras=1)
    {
      return await _requestProvider.GetAsync<List<string>>($"https://baconipsum.com/api/?type=all-meat&paras={paras}&start-with-lorem=1");
    }
  }
}
