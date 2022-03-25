using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VendingMachine;
using VendingMachine.Models;

namespace VendingMachineTests
{
    [TestClass]
    public class CoinsRegisterTests
    {
        [TestMethod()]
        public void ReturnMoney_IfRequiredSetOfCoinsNotAvailable_Test()
        {
            var initialCoins = new Dictionary<CoinDenomination, int>
            {
                { CoinDenomination.Fifties, 0 },
                { CoinDenomination.Twenties, 0 },
                { CoinDenomination.Tens, 0 },
                { CoinDenomination.Fives, 0 },
                { CoinDenomination.Ones, 10 }
            };
            CoinsRegister.LoadCoinsRegister(initialCoins);

            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            var returnAmount = CoinsRegister.CalculateChange(50, 35);

            Assert.IsNotNull(returnAmount);
            Assert.IsTrue(returnAmount.Count == 1);
            Assert.IsTrue(returnAmount.TryGetValue(CoinDenomination.Fifties, out var amountOfCoins) && amountOfCoins == 1);
        }
        
        [TestMethod()]
        public void ChangeReturned_IfRequiredSetOfCoinsAvailable_Test()
        {
            var initialCoins = new Dictionary<CoinDenomination, int>
            {
                { CoinDenomination.Fifties, 0 },
                { CoinDenomination.Twenties, 0 },
                { CoinDenomination.Tens, 0 },
                { CoinDenomination.Fives, 0 },
                { CoinDenomination.Ones, 15 }
            };
            CoinsRegister.LoadCoinsRegister(initialCoins);

            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            var change = CoinsRegister.CalculateChange(50, 35);

            Assert.IsNotNull(change);
            Assert.IsTrue(change.Count == 1);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Ones, out var amountOfCoins) && amountOfCoins == 15);
        }
        
        [TestMethod()]
        public void SmallestNumberOfCoinsReturned_Test()
        {
            var initialCoins = new Dictionary<CoinDenomination, int>
            {
                { CoinDenomination.Fifties, 0 },
                { CoinDenomination.Twenties, 1 },
                { CoinDenomination.Tens, 1 },
                { CoinDenomination.Fives, 2 },
                { CoinDenomination.Ones, 10 }
            };
            CoinsRegister.LoadCoinsRegister(initialCoins);

            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            var change = CoinsRegister.CalculateChange(150, 110);

            Assert.IsNotNull(change);
            Assert.IsTrue(change.Count == 3);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Twenties, out var amountOfTwenties) && amountOfTwenties == 1);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Tens, out var amountOfTens) && amountOfTens == 1);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Fives, out var amountOfFives) && amountOfFives == 2);
        }
        
        [TestMethod()]
        public void RightAmountOfCoinsInCoinsRegister_AfterChangePayout_Test()
        {
            var initialCoins = new Dictionary<CoinDenomination, int>
            {
                { CoinDenomination.Fifties, 0 },
                { CoinDenomination.Twenties, 1 },
                { CoinDenomination.Tens, 1 },
                { CoinDenomination.Fives, 2 },
                { CoinDenomination.Ones, 10 }
            };
            CoinsRegister.LoadCoinsRegister(initialCoins);

            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);
            CoinsRegister.InsertCoinToRegister(CoinDenomination.Fifties);

            var change = CoinsRegister.CalculateChange(150, 110);
            
            Assert.IsNotNull(change);
            //We should have 3x50, 10x1
            Assert.IsTrue(initialCoins.TryGetValue(CoinDenomination.Fifties, out var amountOfFifties) && amountOfFifties == 3);
            Assert.IsTrue(initialCoins.TryGetValue(CoinDenomination.Ones, out var amountOfOnes) && amountOfOnes == 10);
            //We are paying 1x20, 1x10, 2x5
            Assert.IsTrue(change.Count == 3);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Twenties, out var amountOfTwenties) && amountOfTwenties == 1);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Tens, out var amountOfTens) && amountOfTens == 1);
            Assert.IsTrue(change.TryGetValue(CoinDenomination.Fives, out var amountOfFives) && amountOfFives == 2);

        }
    }
}