using System;
using System.Threading.Tasks;

namespace jtbPromise
{
    public interface IAdmobInterstitialAds
    {
         Task Display(string adId);
    }
}
