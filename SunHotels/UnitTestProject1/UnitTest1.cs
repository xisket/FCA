using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ClearAllData()
        {
            var testProviders = CrossCutting.Application.Resolve<Domain.IProviderRepository>();
            var providers = testProviders.All();
            Debug.Print("Providers:" + providers.Count());
            foreach (var provider in providers)
            {
                testProviders.DeleteProvider(provider.Id);
            }
            var testProducts = CrossCutting.Application.Resolve<Domain.IProductRepository>();
            var products = testProducts.All();
            Debug.Print("products:" + products.Count());
            foreach (var product in products)
            {
                testProducts.DeleteProduct(product.Id);
            }
        }

        [TestMethod]
        public void ResetAndFillTestData()
        {
            CrossCutting.Application.RegisterTypes();
            ClearAllData();

            var testProviders = CrossCutting.Application.Resolve<Domain.IProviderRepository>();
            testProviders.AddProvider("SunHotels");
            testProviders.AddProvider("Iberoservice");
            testProviders.AddProvider("Thomas Cook");
            testProviders.AddProvider("Happy Holidays");
            testProviders.AddProvider("HolidayTaxis");
            testProviders.AddProvider("Grandpa");
            testProviders.AddProvider("Barcelo");
            testProviders.AddProvider("Logitravel");
            testProviders.AddProvider("Juniper");
            testProviders.AddProvider("Riu Hotels");

            var testProducts = CrossCutting.Application.Resolve<Domain.IProductRepository>();
            testProducts.AddProduct("PALMA AQUARIUM", "Boosting more than 700 hundred different species of fish and reptile, in 55 aquariums, Palma Aquarium is a spectacular place to explore. ");
            testProducts.AddProduct("DRACH CAVES", "Visit one of the islands most popular tourist attractions, the ‘caves of Drach.’ Discovered in 1896, these beautiful caves are home to the largest underground lake in Europe ");
            testProducts.AddProduct("PORT OF POLLENSA & FORMENTOR", "Enjoy a fabulous tour of Puerto Pollensa firstly, located in Pollensa bay. The bay’s beach being one of the resorts main attractions and with a promenade in front of the beach where the majority of bars ");
            testProducts.AddProduct("LA GRANJA OF ESPORLES", "Visit this beautiful 17th century mansion, surrounded by lush vegetation, beautiful gardens and natural fountains. Here you will find a genuine living display of Mallorcan customs through the ages.  ");
            testProducts.AddProduct("SON AMAR", "Son Amar is a fabulous evening of music and dance, celebrating scenes from Africa, India, Ireland, London and Broadway theatres, not forgetting Son Amar’s homeland, Spain, where you will be entertained with the very best Spanish ballet and flamenco ");
            testProducts.AddProduct("ISLAND TOUR", "What better way to see where you are holidaying, than an island tour, incorporating many modes of transport including an old tram, an electric wooden train, and weather permitting a boat. Visit the  ");
            testProducts.AddProduct("CAVES ARTA", "Visit these underground caverns with spectacular stalagmites. Enjoy the special effects in the various chambers known as Hell, Purgatory and Paradise ");
            testProducts.AddProduct("DIGITHAMS", "The Coves dels Hams (literally “Fishhook Caves”) were discovered in 1905. On the 2nd of March of that same year, the speleologist and tourism pioneer, Pedro Caldentey ");
            testProducts.AddProduct("MARINELAND", "Marineland is a wonderful marine zoo, which is home to the most amazing dolphin show. Enjoy the parrot show or be amazed at the wonderful agility of the sealions during the sealion show. ");
            testProducts.AddProduct("VALLDEMOSSA HALF DAY", "During this half day tour you will visit the Carthusian Monastery, known as the place where Chopin and George Sand spent a winter in 1838. ");
            testProducts.AddProduct("PIRATES ADVENTURE", "Pirates adventure is the most popular evening here in Mallorca, and it has been entertaining guests for over 28 years. This swashbuckling mix of thrilling gymnastics, breathtaking ");
            testProducts.AddProduct("AQUALAND", "Aqualand El Arenal is a place where the whole family can enjoy themselves and where you can have fun in a different way. ");
            testProducts.AddProduct("CITY SIGHTSEEING BUS PALMA", "The sightseeing bus offers you a fantastic and unique way of exploring Palma, as your ticket is valid for 24 hours you can hop on and off, at its many stops along the way, doing so at your own pace and leisure");
            testProducts.AddProduct("WESTERN WATER PARK", "Get wet and go wild in one of the main waterparks in Mallorca. This wild western themed waterpark offers fun and thrills for the whole family");
            testProducts.AddProduct("KATMANDU PARK", "Katmandu Park is a fantastic place to visit for all ages, this new generation of thrill ride will immerse you into 5D and, 4D technology. ");
            testProducts.AddProduct("PALMA & VALLDEMOSA", "A visit to Mallorca isn’t complete without exploring its capital, founded some 2100 years ago by the Romans, Palma is a stunning place to visit. ");
            testProducts.AddProduct("ALCUDIA EXPERIENCE", "This fabulous tour takes you around one of the largest resorts in Alcudia on an open top bus, this resort; popular with families is a fantastic place to visit and enjoy. ");
            testProducts.AddProduct("HIDROPARK", "Visit the only waterpark in the North of Mallorca, Hidropark. Situated in the poipular resort of Alcudia, this waterpark boasts many amazing rides and slides ");
            testProducts.AddProduct("RANCHO GRANDE", "Take in the breathtaking and untouched scenery by horse back as the sun is setting in the sky, from Rancho Grande");
            testProducts.AddProduct("WESTER WATER PARK WITH TRANSFER", "Get wet and go wild in one of the main waterparks in Mallorca. This wild western themed waterpark offers fun and thrills for the whole family. All of the latest ");
            testProducts.AddProduct("MARINELAND WITH TRANSFER", "Marineland is a wonderful marine zoo, which is home to the most amazing dolphin show. Enjoy the parrot show or be amazed at the wonderful agility of the sealions ");

            var providers = testProviders.All();
            var products = testProducts.All();

            Random rnd = new Random();
            foreach (var product in products)
            {
                var first = rnd.Next(0, providers.Count() - 1);
                var second = rnd.Next(0, providers.Count() - 1);
                while (second == first)
                {
                    second = rnd.Next(0, providers.Count() - 1);
                }
                var third = rnd.Next(0, providers.Count() - 1);
                while (third == first || third == second)
                {
                    third = rnd.Next(0, providers.Count() - 1);
                }
                var fourst = rnd.Next(0, providers.Count() - 1);
                while (fourst == first || fourst == second || fourst == third)
                {
                    fourst = rnd.Next(0, providers.Count() - 1);
                }

                testProducts.AddProductProvider(product.Id, providers.ElementAt(first).Id);
                testProducts.AddProductProvider(product.Id, providers.ElementAt(second).Id);
                testProducts.AddProductProvider(product.Id, providers.ElementAt(third).Id);
                testProducts.AddProductProvider(product.Id, providers.ElementAt(fourst).Id);

            }
        }

        [TestMethod]
        public void TestDataLayer()
        {
            CrossCutting.Application.RegisterTypes();

            //TESTING PRODUCTS
            var testProducts = CrossCutting.Application.Resolve<Domain.IProductRepository>();
            var products = testProducts.All();
            Debug.Print("Products count:" + products.Count());
            foreach (var product in products)
            {
                var productById = testProducts.ById(product.Id);
                Assert.AreEqual(product.Id, productById.Id);
                if (productById.Providers == null)
                    Debug.Print("productById:" + productById.Name + ", Providers NULL:");
                else
                    Debug.Print("productById:" + productById.Name + ", Providers:" + productById.Providers.Count());
            }

            //TESTING PROVIDERS
            var testProviders = CrossCutting.Application.Resolve<Domain.IProviderRepository>();
            var providers = testProviders.All();
            Debug.Print("Providers count:" + providers.Count());

        }
    }
}
