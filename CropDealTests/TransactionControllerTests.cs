using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Controllers;
using TransactionService.Process;

namespace TransactionService.Tests
{
    [TestClass]
    public class TransactionControllerTests
    {
        private Mock<ITransactionProcess> _mockTransactionProcess;
        private TransactionController _transactionController;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionProcess = new Mock<ITransactionProcess>();
            _transactionController = new TransactionController(_mockTransactionProcess.Object);
        }

        // ✅ CreateTransaction - Affirmation
        [TestMethod]
        public async Task CreateTransaction_ShouldReturnOk_WhenTransactionIsCreated()
        {
            var transaction = new Transaction { TransactionId = 1, FarmerId = 101, DealerId = 202, CropId = 303, CropName = "Wheat", Quantity = 10, TotalAmount = 500.0, TransactionStatus = true, TransactionDate = DateTime.UtcNow };
            _mockTransactionProcess.Setup(p => p.CreateTransactionAsync(transaction)).ReturnsAsync(true);

            var result = await _transactionController.CreateTransaction(transaction);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Transaction created successfully.", okResult.Value);
        }

        // ❌ CreateTransaction - Negation
        [TestMethod]
        public async Task CreateTransaction_ShouldReturnBadRequest_WhenTransactionIsNull()
        {
            var result = await _transactionController.CreateTransaction(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        // ✅ GetTransactionsByFarmerAndDateRange - Affirmation
        [TestMethod]
        public async Task GetTransactionsByFarmerAndDateRange_ShouldReturnOk_WithTransactions()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { TransactionId = 1, FarmerId = 101, DealerId = 202, CropId = 303, CropName = "Wheat", Quantity = 10, TotalAmount = 500.0, TransactionStatus = true, TransactionDate = DateTime.UtcNow }
            };
            _mockTransactionProcess.Setup(p => p.GetTransactionsByFarmerAndDateRangeAsync(101, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(transactions);

            var result = await _transactionController.GetTransactionsByFarmerAndDateRange(101, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedTransactions = okResult.Value as List<Transaction>;
            Assert.IsNotNull(returnedTransactions);
            Assert.AreEqual(1, returnedTransactions.Count);
        }

        // ❌ GetTransactionsByFarmerAndDateRange - Negation
        [TestMethod]
        public async Task GetTransactionsByFarmerAndDateRange_ShouldReturnBadRequest_WhenInvalidDateRange()
        {
            var result = await _transactionController.GetTransactionsByFarmerAndDateRange(101, DateTime.UtcNow, DateTime.UtcNow.AddDays(-10));

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        // ✅ GetTransactionsByDealerAndDateRange - Affirmation
        [TestMethod]
        public async Task GetTransactionsByDealerAndDateRange_ShouldReturnOk_WithTransactions()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { TransactionId = 1, DealerId = 202, FarmerId = 101, CropId = 303, CropName = "Wheat", Quantity = 10, TotalAmount = 500.0, TransactionStatus = true, TransactionDate = DateTime.UtcNow }
            };
            _mockTransactionProcess.Setup(p => p.GetTransactionsByDealerAndDateRangeAsync(202, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(transactions);

            var result = await _transactionController.GetTransactionsByDealerAndDateRange(202, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedTransactions = okResult.Value as List<Transaction>;
            Assert.IsNotNull(returnedTransactions);
            Assert.AreEqual(1, returnedTransactions.Count);
        }

        // ❌ GetTransactionsByDealerAndDateRange - Negation
        [TestMethod]
        public async Task GetTransactionsByDealerAndDateRange_ShouldReturnBadRequest_WhenInvalidDateRange()
        {
            var result = await _transactionController.GetTransactionsByDealerAndDateRange(202, DateTime.UtcNow, DateTime.UtcNow.AddDays(-10));

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        // ✅ GetTransactionById - Affirmation
        [TestMethod]
        public async Task GetTransactionById_ShouldReturnOk_WhenTransactionExists()
        {
            var transaction = new Transaction { TransactionId = 1, FarmerId = 101, DealerId = 202, CropId = 303, CropName = "Wheat", Quantity = 10, TotalAmount = 500.0, TransactionStatus = true, TransactionDate = DateTime.UtcNow };
            _mockTransactionProcess.Setup(p => p.GetTransactionByIdAsync(1)).ReturnsAsync(transaction);

            var result = await _transactionController.GetTransactionById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedTransaction = okResult.Value as Transaction;
            Assert.IsNotNull(returnedTransaction);
            Assert.AreEqual(1, returnedTransaction.TransactionId);
        }

        // ❌ GetTransactionById - Negation
        [TestMethod]
        public async Task GetTransactionById_ShouldReturnNotFound_WhenTransactionDoesNotExist()
        {
            _mockTransactionProcess.Setup(p => p.GetTransactionByIdAsync(1)).ReturnsAsync((Transaction)null);

            var result = await _transactionController.GetTransactionById(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
