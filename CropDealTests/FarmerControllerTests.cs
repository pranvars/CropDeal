using CropService;
using FarmerService;
using FarmerService.Controllers;
using FarmerService.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmerServiceTests
{
    [TestClass]
    public class FarmerControllerTests
    {
        private Mock<IFarmerProcess> _mockFarmerProcess;
        private FarmerController _farmerController;

        [TestInitialize]
        public void Setup()
        {
            _mockFarmerProcess = new Mock<IFarmerProcess>();
            _farmerController = new FarmerController(_mockFarmerProcess.Object);
        }

        // ✅ Test 1: GetFarmerByIdAsync - Success
        [TestMethod]
        public async Task GetFarmerByIdAsync_ShouldReturnOk_WhenFarmerExists()
        {
            // Arrange
            var farmer = new Farmer { FarmerId = 1, UserId = 100, Location = "Texas", AccountNumber = "1234567890", BankIfsccode = "IFSC001" };
            _mockFarmerProcess.Setup(p => p.GetFarmerByIdAsync(1)).ReturnsAsync(farmer);

            // Act
            var result = await _farmerController.GetFarmerById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
            Assert.AreEqual(farmer, okResult.Value);
        }

        // ❌ Test 2: GetFarmerByIdAsync - Farmer Not Found
        [TestMethod]
        public async Task GetFarmerByIdAsync_ShouldReturnNotFound_WhenFarmerDoesNotExist()
        {
            // Arrange
            _mockFarmerProcess.Setup(p => p.GetFarmerByIdAsync(999)).ReturnsAsync((Farmer)null);

            // Act
            var result = await _farmerController.GetFarmerById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // ✅ Test 3: UpdateFarmerAsync - Success
        [TestMethod]
        public async Task UpdateFarmerAsync_ShouldReturnNoContent_WhenFarmerIsUpdated()
        {
            // Arrange
            var farmer = new Farmer { FarmerId = 1, UserId = 100, Location = "Texas", AccountNumber = "1234567890", BankIfsccode = "IFSC001" };
            _mockFarmerProcess.Setup(p => p.UpdateFarmerAsync(1, farmer)).ReturnsAsync(true);

            // Act
            var result = await _farmerController.UpdateFarmer(1, farmer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // ❌ Test 4: UpdateFarmerAsync - Farmer Not Found
        [TestMethod]
        public async Task UpdateFarmerAsync_ShouldReturnNotFound_WhenFarmerDoesNotExist()
        {
            // Arrange
            var farmer = new Farmer { FarmerId = 1, UserId = 100, Location = "Texas", AccountNumber = "1234567890", BankIfsccode = "IFSC001" };
            _mockFarmerProcess.Setup(p => p.UpdateFarmerAsync(1, farmer)).ReturnsAsync(false);

            // Act
            var result = await _farmerController.UpdateFarmer(1, farmer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        // ✅ Test 5: PublishCropAsync - Success
        [TestMethod]
        public async Task PublishCropAsync_ShouldReturnOk_WhenCropIsPublishedSuccessfully()
        {
            // Arrange
            var crop = new Crop { CropId = 1, Name = "Wheat", Quantity = 100 };
            _mockFarmerProcess.Setup(p => p.PublishCropAsync(crop)).ReturnsAsync(true);

            // Act
            var result = await _farmerController.PublishCrop(crop);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        // ❌ Test 6: PublishCropAsync - Failure
        [TestMethod]
        public async Task PublishCropAsync_ShouldReturnBadRequest_WhenCropPublicationFails()
        {
            // Arrange
            var crop = new Crop { CropId = 1, Name = "Wheat", Quantity = 100 };
            _mockFarmerProcess.Setup(p => p.PublishCropAsync(crop)).ReturnsAsync(false);

            // Act
            var result = await _farmerController.PublishCrop(crop);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        // ✅ UpdateFarmersStatus - Affirmation
        [TestMethod]
        public async Task UpdateFarmersStatus_ShouldReturnOk_WhenFarmersUpdatedSuccessfully()
        {
            var farmerIds = new List<int> { 1, 2, 3 };
            _mockFarmerProcess.Setup(p => p.UpdateFarmersStatusAsync(farmerIds, true)).ReturnsAsync(true);

            var result = await _farmerController.UpdateFarmersStatus(farmerIds, true);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Farmers' status updated successfully.", okResult.Value);
        }

        // ❌ UpdateFarmersStatus - Negation
        [TestMethod]
        public async Task UpdateFarmersStatus_ShouldReturnNotFound_WhenNoFarmersFound()
        {
            var farmerIds = new List<int> { 4, 5, 6 };
            _mockFarmerProcess.Setup(p => p.UpdateFarmersStatusAsync(farmerIds, false)).ReturnsAsync(false);

            var result = await _farmerController.UpdateFarmersStatus(farmerIds, false);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // ✅ GetAllFarmers - Affirmation
        [TestMethod]
        public async Task GetAllFarmers_ShouldReturnOk_WithListOfFarmers()
        {
            var farmers = new List<Farmer>
            {
                new Farmer { FarmerId = 1, UserId = 201, Location = "Texas", AccountNumber = "987654", BankIfsccode = "IFSC003", IsActive = true },
                new Farmer { FarmerId = 2, UserId = 202, Location = "California", AccountNumber = "321987", BankIfsccode = "IFSC004", IsActive = false }
            };

            _mockFarmerProcess.Setup(p => p.GetAllFarmersAsync()).ReturnsAsync(farmers);

            var result = await _farmerController.GetAllFarmers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedFarmers = okResult.Value as List<Farmer>;
            Assert.IsNotNull(returnedFarmers);
            Assert.AreEqual(2, returnedFarmers.Count);
        }

        // ❌ GetAllFarmers - Negation
        [TestMethod]
        public async Task GetAllFarmers_ShouldReturnOk_WithEmptyList_WhenNoFarmersExist()
        {
            _mockFarmerProcess.Setup(p => p.GetAllFarmersAsync()).ReturnsAsync(new List<Farmer>());

            var result = await _farmerController.GetAllFarmers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedFarmers = okResult.Value as List<Farmer>;
            Assert.IsNotNull(returnedFarmers);
            Assert.AreEqual(0, returnedFarmers.Count);
        }

        // ✅ Test: AddFarmer - Success
        [TestMethod]
        public async Task AddFarmer_ShouldReturnCreated_WhenFarmerIsRegisteredSuccessfully()
        {
            // Arrange
            var farmer = new Farmer { FarmerId = 1, UserId = 201, Location = "Village X", AccountNumber = "987654", BankIfsccode = "IFSC999" };

            _mockFarmerProcess.Setup(p => p.RegisterFarmer(It.IsAny<Farmer>())).ReturnsAsync(farmer);

            // Act
            var result = await _farmerController.AddFarmer(farmer);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnedFarmer = createdResult.Value as Farmer;
            Assert.IsNotNull(returnedFarmer);
            Assert.AreEqual(1, returnedFarmer.FarmerId);
        }

        // ❌ Test: AddFarmer - Failure (Null Farmer)
        [TestMethod]
        public async Task AddFarmer_ShouldReturnBadRequest_WhenFarmerIsNull()
        {
            // Act
            var result = await _farmerController.AddFarmer(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Farmer information is required.", badRequestResult.Value);
        }

    }
}
