using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace BraintreeSample.Services.Media
{
  public class MediaService : IMediaService
  {
    static bool _initialized;

    public MediaService()
    {

    }

    async Task<bool> Initialize()
    {
      return await CrossMedia.Current.Initialize();
    }

    public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
    {
      if (_initialized==false)
      {
        _initialized=await Initialize();
      }

      return await CrossMedia.Current.PickPhotoAsync(options);
    }

    public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
    {
      if (_initialized == false)
      {
        _initialized = await Initialize();
      }

      return await CrossMedia.Current.TakePhotoAsync(options);
    }

    public async Task<MediaFile> PickVideoAsync()
    {
      if (_initialized == false)
      {
        _initialized = await Initialize();
      }

      return await CrossMedia.Current.PickVideoAsync();
    }

    public async Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
    {
      if (_initialized == false)
      {
        _initialized = await Initialize();
      }

      return await CrossMedia.Current.PickVideoAsync();
    }

    public bool IsCameraAvailable => CrossMedia.Current.IsCameraAvailable;
    public bool IsTakePhotoSupported => CrossMedia.Current.IsTakePhotoSupported;
    public bool IsPickPhotoSupported => CrossMedia.Current.IsPickPhotoSupported;
    public bool IsTakeVideoSupported => CrossMedia.Current.IsTakeVideoSupported;
    public bool IsPickVideoSupported => CrossMedia.Current.IsPickVideoSupported;
  }
}
