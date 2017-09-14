using System.Collections.Generic;
using System.Data;

namespace csharp
{
    public class GildedRose
    {
        IList<Item> Items;

        public GildedRose(IList<Item> items)
        {
            this.Items = items;
        }

        public void UpdateQuality()
        {
            UpdateAllQuality(Items);
            UpdateAllSellIn(Items);
        }

        private static void UpdateAllQuality(IList<Item> items)
        {
            foreach (var item in items)
            {
                if (item.Name.Contains("Sulfuras"))
                    continue;
                if (item.Name.Contains("Conjured"))
                    UpdateConjuredQuality(item);
                else if (!item.Name.Contains("Sulfuras"))
                    UpdateQuality(item);
            }
        }

        private static void UpdateQuality(Item item)
        {
            if (item.Name.Contains("Aged Brie"))
                UpdateBrieQuality(item);
            else if (item.Name.Contains("Backstage passes"))
                UpdatePassesQuality(item);
            else
                NormalUpdateQuality(item);

            ApplyQualityConstrains(item, 0, 50);
        }

        private static void NormalUpdateQuality(Item item)
        {
            item.Quality = item.SellIn > 0 ? item.Quality - 1 : item.Quality - 2;
        }

        private static void UpdateConjuredQuality(Item item)
        {
            UpdateQuality(item);
            UpdateQuality(item);
        }

        private static void UpdateBrieQuality(Item item)
        {
            item.Quality = item.SellIn > 0 ? item.Quality + 1 : item.Quality + 2;
        }

        private static void UpdatePassesQuality(Item item)
        {
            if (item.SellIn > 10)
                item.Quality = item.Quality + 1;
            else if (item.SellIn > 5)
                item.Quality = item.Quality + 2;
            else if (item.SellIn > 0)
                item.Quality = item.Quality + 3;
            else 
                item.Quality = 0;
        }

        private static void UpdateAllSellIn(IList<Item> items)
        {
            foreach (var item in items)
            {
                UpdateSellIn(item);
            }
        }

        private static void ApplyQualityConstrains(Item item, int lower, int upper)
        {
            item.Quality = item.Quality > upper ? upper : item.Quality;
            item.Quality = item.Quality < lower ? lower : item.Quality;
        }

        private static void UpdateSellIn(Item item)
        {
            if (!item.Name.Contains("Sulfuras"))
            {
                item.SellIn -= 1;
            }
        }
    }

    
}
