using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class BundlesRSS
    {
        private readonly List<CoinsBundleSO> coinBundlesList;

        public BundlesRSS(List<CoinsBundleSO> coinBundlesList)
        {
            this.coinBundlesList = coinBundlesList;
            coinBundlesList.Sort((x, y) => x.BundleRarity.CompareTo(y.BundleRarity));
        }

        public CoinsBundleSO GetCoinsBundle(BundleRarity bundleRarity) => coinBundlesList.First(x => x.BundleRarity == bundleRarity);
    }
}