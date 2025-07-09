using CropService;
using DealerService.Controllers;
using DealerService.Process;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DealerService.Tests
{
    [TestClass]
    public class DealerControllerTests
    {
        private Mock<IDealerProcess> _mockDealerProcess;
        private DealerController _dealerController;

        [TestInitialize]
        public void Setup()
        {
            _mockDealerProcess = new Mock<IDealerProcess>();
            _dealerController = new DealerController(_mockDealerProcess.Object);
        }

        // Test: GetDealerById - Success
        [TestMethod]
        public async Task GetDealerById_ShouldReturnOk_WhenDealerExists()
        {
            // Arrange
            var dealer = new Dealer { DealerId = 1, UserId = 101, Location = "City A", AccountNumber = "123456", BankIfsccode = "IFSC001" };
            _mockDealerProcess.Setup(p => p.GetDealerByIdAsync(1)).ReturnsAsync(dealer);

            // Act
            var result = await _dealerController.GetDealerById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedDealer = okResult.Value as Dealer;
            Assert.IsNotNull(returnedDealer);
            Assert.AreEqual(1, returnedDealer.DealerId);
        }

        // Test: GetDealerById - Failure
        [TestMethod]
        public async Task GetDealerById_ShouldReturnNotFound_WhenDealerDoesNotExist()
        {
            // Arrange
            _mockDealerProcess.Setup(p => p.GetDealerByIdAsync(It.IsAny<int>())).ReturnsAsync((Dealer)null);

            // Act
            var result = await _dealerController.GetDealerById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // Test: UpdateDealer - Success
        [TestMethod]
        public async Task UpdateDealer_ShouldReturnNoContent_WhenDealerIsUpdated()
        {
            // Arrange
            var dealer = new Dealer { DealerId = 1, UserId = 101, Location = "City A", AccountNumber = "123456", BankIfsccode = "IFSC001" };
            _mockDealerProcess.Setup(p => p.UpdateDealerAsync(1, dealer)).ReturnsAsync(true);

            // Act
            var result = await _dealerController.UpdateDealer(1, dealer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // Test: UpdateDealer - Failure
        [TestMethod]
        public async Task UpdateDealer_ShouldReturnNotFound_WhenDealerDoesNotExist()
        {
            // Arrange
            var dealer = new Dealer { DealerId = 1, UserId = 101, Location = "City A", AccountNumber = "123456", BankIfsccode = "IFSC001" };
            _mockDealerProcess.Setup(p => p.UpdateDealerAsync(1, dealer)).ReturnsAsync(false);

            // Act
            var result = await _dealerController.UpdateDealer(1, dealer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // Test: GetAllCrops - Success
        [TestMethod]
        public async Task GetAllCrops_ShouldReturnOk_WhenCropsExist()
        {
            // Arrange
            var crops = new List<Crop>
            {
                new Crop { CropId = 1, Name = "Wheat", Quantity = 100 },
                new Crop { CropId = 2, Name = "Rice", Quantity = 200 }
            };
            _mockDealerProcess.Setup(p => p.GetAllCropsAsync()).ReturnsAsync(crops);

            // Act
            var result = await _dealerController.GetAllCrops();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrops = okResult.Value as List<Crop>;
            Assert.IsNotNull(returnedCrops);
            Assert.AreEqual(2, returnedCrops.Count);
        }

        // Test: GetAllCrops - Failure
        [TestMethod]
        public async Task GetAllCrops_ShouldReturnNotFound_WhenNoCropsExist()
        {
            // Arrange
            _mockDealerProcess.Setup(p => p.GetAllCropsAsync()).ReturnsAsync(new List<Crop>());

            // Act
            var result = await _dealerController.GetAllCrops();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // Test: GetCropByName - Success
        [TestMethod]
        public async Task GetCropByName_ShouldReturnOk_WhenCropExists()
        {
            // Arrange
            var crop = new Crop { CropId = 1, Name = "Wheat", Quantity = 100 };
            _mockDealerProcess.Setup(p => p.GetCropByNameAsync("Wheat")).ReturnsAsync(crop);

            // Act
            var result = await _dealerController.GetCropByName("Wheat");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrop = okResult.Value as Crop;
            Assert.IsNotNull(returnedCrop);
            Assert.AreEqual("Wheat", returnedCrop.Name);
        }

        // Test: GetCropByName - Failure
        [TestMethod]
        public async Task GetCropByName_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            // Arrange
            _mockDealerProcess.Setup(p => p.GetCropByNameAsync(It.IsAny<string>())).ReturnsAsync((Crop)null);

            // Act
            var result = await _dealerController.GetCropByName("UnknownCrop");
             
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // Test: GetCropById - Success
        [TestMethod]
        public async Task GetCropById_ShouldReturnOk_WhenCropExists()
        {
            // Arrange
            var crop = new Crop { CropId = 1, Name = "Wheat", Quantity = 100 };
            _mockDealerProcess.Setup(p => p.GetCropByIdAsync(1)).ReturnsAsync(crop);

            // Act
            var result = await _dealerController.GetCropById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrop = okResult.Value as Crop;
            Assert.IsNotNull(returnedCrop);
            Assert.AreEqual(1, returnedCrop.CropId);
        }

        // Test: GetCropById - Failure
        [TestMethod]
        public async Task GetCropById_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            // Arrange
            _mockDealerProcess.Setup(p => p.GetCropByIdAsync(It.IsAny<int>())).ReturnsAsync((Crop)null);

            // Act
            var result = await _dealerController.GetCropById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        // ✅ UpdateDealersStatus - Affirmation
        [TestMethod]
        public async Task UpdateDealersStatus_ShouldReturnOk_WhenDealersUpdatedSuccessfully()
        {
            var dealerIds = new List<int> { 1, 2, 3 };
            bool isActive = true;

            _mockDealerProcess.Setup(p => p.UpdateDealersStatusAsync(dealerIds, isActive))
                .ReturnsAsync(true);

            var result = await _dealerController.UpdateDealersStatus(dealerIds, isActive);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Dealers' status updated successfully.", okResult.Value);
        }

        // ❌ UpdateDealersStatus - Negation
        [TestMethod]
        public async Task UpdateDealersStatus_ShouldReturnNotFound_WhenNoDealersUpdated()
        {
            var dealerIds = new List<int> { 10, 20, 30 }; // IDs that do not exist
            bool isActive = false;

            _mockDealerProcess.Setup(p => p.UpdateDealersStatusAsync(dealerIds, isActive))
                .ReturnsAsync(false);

            var result = await _dealerController.UpdateDealersStatus(dealerIds, isActive);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        // ✅ GetAllDealers - Affirmation
        [TestMethod]
        public async Task GetAllDealers_ShouldReturnOk_WithListOfDealers()
        {
            var dealers = new List<Dealer>
            {
                new Dealer { DealerId = 1, UserId = 101, Location = "New York", AccountNumber = "123456", BankIfsccode = "IFSC001", IsActive = true },
                new Dealer { DealerId = 2, UserId = 102, Location = "Los Angeles", AccountNumber = "654321", BankIfsccode = "IFSC002", IsActive = false }
            };

            _mockDealerProcess.Setup(p => p.GetAllDealersAsync()).ReturnsAsync(dealers);

            var result = await _dealerController.GetAllDealers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedDealers = okResult.Value as List<Dealer>;
            Assert.IsNotNull(returnedDealers);
            Assert.AreEqual(2, returnedDealers.Count);
        }

        // ❌ GetAllDealers - Negation
        [TestMethod]
        public async Task GetAllDealers_ShouldReturnOk_WithEmptyList_WhenNoDealersExist()
        {
            _mockDealerProcess.Setup(p => p.GetAllDealersAsync()).ReturnsAsync(new List<Dealer>());

            var result = await _dealerController.GetAllDealers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedDealers = okResult.Value as List<Dealer>;
            Assert.IsNotNull(returnedDealers);
            Assert.AreEqual(0, returnedDealers.Count);
        }

        // ✅ AddDealer - Affirmation
        [TestMethod]
        public async Task AddDealer_ShouldReturnCreated_WhenDealerIsAddedSuccessfully()
        {
            // Arrange
            var dealer = new Dealer { DealerId = 1, UserId = 101, Location = "City A", AccountNumber = "123456", BankIfsccode = "IFSC001" };

            _mockDealerProcess.Setup(p => p.RegisterDealer(dealer)).ReturnsAsync(dealer);

            // Act
            var result = await _dealerController.AddDealer(dealer);

            // Assert
            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            Assert.AreEqual(nameof(_dealerController.AddDealer), createdAtResult.ActionName);

            var returnedDealer = createdAtResult.Value as Dealer;
            Assert.IsNotNull(returnedDealer);
            Assert.AreEqual(1, returnedDealer.DealerId);
        }

        [TestMethod]
        public async Task AddDealer_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var dealer = new Dealer { DealerId = 1, UserId = 101, Location = "City A", AccountNumber = "123456", BankIfsccode = "IFSC001" };

            // Simulate RegisterDealer returning null
            _mockDealerProcess.Setup(p => p.RegisterDealer(It.IsAny<Dealer>())).ReturnsAsync((Dealer)null);

            // Act
            var result = await _dealerController.AddDealer(dealer);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Dealer registration failed.", badRequestResult.Value);
        }




    }
}
