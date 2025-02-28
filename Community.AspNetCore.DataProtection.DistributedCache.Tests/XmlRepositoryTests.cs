using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace Community.AspNetCore.DataProtection.DistributedCache.Tests
{
    public class XmlRepositoryTests
    {
        private readonly IXmlRepository _xmlRepository;

        public XmlRepositoryTests()
        {
            var collection = new ServiceCollection();

            collection
                .AddDistributedMemoryCache()
                .AddDataProtection()
                .PersistKeysToDistributedCache();

            _xmlRepository = collection
                .BuildServiceProvider()
                .GetRequiredService<IXmlRepository>();
        }

        [Fact]
        public void StoreElement()
        {
            var element = XElement.Parse("<key id=\"23735faa-2346-4d29-9ad0-21d44324da2a\"/>");

            _xmlRepository.StoreElement(element, "Key1");

            IEnumerable<string> actual = _xmlRepository.GetAllElements().Select(e => e.ToString());
            IEnumerable<string> expected = [element.ToString()];

            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void StoreElement_MultipleKeys()
        {
            var element1 = XElement.Parse("<key id=\"23735faa-2346-4d29-9ad0-21d44324da2a\"/>");
            var element2 = XElement.Parse("<key id=\"eb705aff-3cfb-4eaf-aa00-a7f0deaf9fdf\"/>");

            _xmlRepository.StoreElement(element1, "Key1");
            _xmlRepository.StoreElement(element2, "Key2");

            var actual = _xmlRepository.GetAllElements().Select(e => e.ToString());
            var expected = new XElement[] { element1, element2 }.Select(e => e.ToString());

            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
