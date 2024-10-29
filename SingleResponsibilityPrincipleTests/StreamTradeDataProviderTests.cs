using Microsoft.Data.SqlClient;
using SingleResponsibilityPrinciple.Contracts;
using SingleResponsibilityPrinciple;
using System.Reflection;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class StreamTradeDataProviderTests
    {

        private int countStrings(IEnumerable<string> collectionOfStrings)
        {
            // count the trades
            int count = 0;
            foreach (var trade in collectionOfStrings)
            {
                count++;
            }
            return count;
        }


        [TestMethod()]
        public void TestSize1()
        {
            //Arrange
            ILogger logger = new ConsoleLogger("TestSize1.txt");
            Stream tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.trades_1good.txt");

            ITradeDataProvider tradeProvider = new StreamTradeDataProvider(tradeStream, logger);

            //Act
            IEnumerable<string> trades = tradeProvider.GetTradeData();

            //Assert
 
            Assert.AreEqual(countStrings(trades), 1);
        }
        [TestMethod()]
        public void TestSize5()
        {
            //Arrange
            ILogger logger = new ConsoleLogger("TestSize5.txt");
            Stream tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.trades_5good.txt");

            ITradeDataProvider tradeProvider = new StreamTradeDataProvider(tradeStream, logger);

            //Act
            IEnumerable<string> trades = tradeProvider.GetTradeData();

            //Assert

            Assert.AreEqual(countStrings(trades), 5);
        }

    }
}