using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace csharp
{
    [TestFixture]
    public class GildedRoseTest
    {
        public void Item_has_Sellln()
        {
            var item = new Item();

            Assert.NotNull(item.SellIn);
        }

        public void Item_has_Quality()
        {
            var item = new Item();

            Assert.NotNull(item.SellIn);
        }

        [Test]
        public void SellIn_DecreasesAfterUpdate()
        {
            IList<Item> items = new List<Item> { new Item { Name = "foo", SellIn = 10, Quality = 10 } };
            var sellInStart = items[0].SellIn;
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            Assert.AreEqual(sellInStart, items[0].SellIn + 1);
        }

        [Test]
        public void Quality_DecreasesAfterUpdate()
        {
            IList<Item> items = new List<Item> { new Item { Name = "foo", SellIn = 10, Quality = 10 } };
            var qualityStart = items[0].Quality;
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            Assert.Less(items[0].Quality, qualityStart);
        }

        [Test]
        public void Quality_DegradesFasterAfterSellInDate()
        {
            IList<Item> items = new List<Item> { new Item { Name = "foo", SellIn = 1, Quality = 10 } };
            GildedRose app = new GildedRose(items);

            var quality = items[0].Quality;
            app.UpdateQuality();
            var normalDegradeRate = quality - items[0].Quality;

            quality = items[0].Quality;
            app.UpdateQuality();
            var doubleDegradeRate = quality - items[0].Quality;

            Assert.AreEqual(normalDegradeRate * 2, doubleDegradeRate);
        }

        [Test]
        public void Quality_NotNegative()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Cheese", SellIn = 0, Quality = 0 },
                new Item { Name = "Conjured Cursed Robe", SellIn = -10, Quality = 0 },
                new Item { Name = "Conjured Cursed Robe of Despair", SellIn = -10, Quality = 1 },
                new Item { Name = "Sulfuras", SellIn = -10, Quality = -5 },
                new Item { Name = "Aged Brie", SellIn = 5, Quality = -4 }
            };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            Assert.GreaterOrEqual(items[0].Quality, 0);
        }

        [Test]
        public void Quality_AgedBrieIncreasesInQuality()
        {
            IList<Item> items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 10, Quality = 5 } };
            GildedRose app = new GildedRose(items);
            var qualityStart = items[0].Quality;

            app.UpdateQuality();
            

            Assert.Greater(items[0].Quality, qualityStart);
        }

        [Test]
        public void Quality_NotAbove50()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 },
                new Item { Name = "Aged Brie", SellIn = -10, Quality = 49 },
                new Item { Name = "Random bone", SellIn = 5, Quality = 60 },
                new Item { Name = "Conjured bone", SellIn = 5, Quality = 160 }
            };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();
            foreach (var item in items)
            {
                Assert.LessOrEqual(item.Quality, 50);
            }

        }

        [Test]
        public void Quality_SulfurasNotDecreases()
        {
            IList<Item> items = new List<Item> { new Item { Name = "Sulfuras", SellIn = 5, Quality = 40 } };
            GildedRose app = new GildedRose(items);
            var qualityStart = items[0].Quality;

            app.UpdateQuality();

            Assert.AreEqual(items[0].Quality, qualityStart);

        }

        [Test]
        public void SellIn_SulfurasNotDecreases()
        {
            IList<Item> items = new List<Item> { new Item { Name = "Sulfuras", SellIn = 5, Quality = 40 } };
            GildedRose app = new GildedRose(items);
            var sellInStart = items[0].SellIn;

            app.UpdateQuality();

            Assert.AreEqual(items[0].SellIn, sellInStart);
        }

        [Test]
        public void Quality_BackstagePassesNormallyIncreasesAfterUpdate()
        {
            IList<Item> items = new List<Item> { new Item { Name = "Backstage passes", SellIn = 30, Quality = 10 } };
            var qualityStart = items[0].Quality;
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            Assert.Greater(items[0].Quality, qualityStart);
        }

        [Test]
        public void Quality_BackstagePassesIncreaseInValueBy2WhenSellIn10OrLess()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Backstage passes", SellIn = 10, Quality = 10 },
            };
            GildedRose app = new GildedRose(items);
            var lastQuality = items[0].Quality;

            while (items[0].SellIn > 5)
            {
                app.UpdateQuality();
                var change = lastQuality - items[0].Quality;
                Assert.AreEqual(change, -2);
                lastQuality = items[0].Quality;
            }
        }

        [Test]
        public void Quality_BackstagePassesIncreaseInValueBy3WhenSellIn5OrLess()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Backstage passes", SellIn = 5, Quality = 10 },
            };

            GildedRose app = new GildedRose(items);
            var lastQuality = items[0].Quality;

            while (items[0].SellIn > 0)
            {
                app.UpdateQuality();
                var change = lastQuality - items[0].Quality;
                Assert.AreEqual(change, -3);
                lastQuality = items[0].Quality;
            }
        }


        [Test]
        public void Quality_BackstagePassesWorth0AfterSellInDate()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Backstage passes", SellIn = 1, Quality = 10 },
                new Item { Name = "Backstage passes", SellIn = 0, Quality = 10 }
            };

            GildedRose app = new GildedRose(items);
            app.UpdateQuality();

            Assert.AreNotEqual(items[1].Quality, items[0].Quality);
            Assert.AreEqual(items[1].Quality, 0);
        }

        [Test]
        public void Quality_ConjuredDegradeTwiceAsFast()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Conjured Robe", SellIn = 10, Quality = 10 },
                new Item { Name = "Normal Robe", SellIn = 10, Quality = 10 }
            };

            GildedRose app = new GildedRose(items);
            var startingQuality = items[0].Quality;
            app.UpdateQuality();

            var conjuredQualityChange = items[0].Quality - startingQuality;
            var normalQualityChange = items[1].Quality - startingQuality;

            Assert.AreEqual(normalQualityChange * 2, conjuredQualityChange);
        }

        public void Quality_ConjuredDegradeTwiceAsFastAfterSellIn()
        {
            IList<Item> items = new List<Item>
            {
                new Item { Name = "Conjured Robe", SellIn = -3, Quality = 10 },
                new Item { Name = "Normal Robe", SellIn = -3, Quality = 10 }
            };

            GildedRose app = new GildedRose(items);
            var startingQuality = items[0].Quality;
            app.UpdateQuality();

            var conjuredQualityChange = items[0].Quality - startingQuality;
            var normalQualityChange = items[1].Quality - startingQuality;

            Assert.AreEqual(normalQualityChange * 2, conjuredQualityChange);
        }
    }
}
