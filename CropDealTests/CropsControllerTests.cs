using CropService;
using CropService.Controllers;
using CropService.Process;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CropDealTests
{
    [TestClass]
    public class CropsControllerTests
    {
        private Mock<ICropProcess> _mockCropProcess;
        private CropsController _cropController;
         
        [TestInitialize]
        public void Setup()
        {
            _mockCropProcess = new Mock<ICropProcess>();
            _cropController = new CropsController(_mockCropProcess.Object);
        }

        // ✅ GetAllCrops - Affirmation
        [TestMethod]
        public async Task GetAllCrops_ShouldReturnOk_WithCrops()
        {
            var crops = new List<Crop> { new Crop { CropId = 1, Name = "Wheat" } };
            _mockCropProcess.Setup(p => p.GetAllCropsAsync()).ReturnsAsync(crops);

            var result = await _cropController.GetAllCrops();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrops = okResult.Value as List<Crop>;
            Assert.IsNotNull(returnedCrops);
            Assert.AreEqual(1, returnedCrops.Count);
        }

        // ❌ GetAllCrops - Negation
        [TestMethod]
        public async Task GetAllCrops_ShouldReturnNotFound_WhenNoCropsExist()
        {
            _mockCropProcess.Setup(p => p.GetAllCropsAsync()).ReturnsAsync(new List<Crop>());

            var result = await _cropController.GetAllCrops();

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        // ✅ GetCropById - Affirmation
        [TestMethod]
        public async Task GetCropById_ShouldReturnOk_WhenCropExists()
        {
            var crop = new Crop { CropId = 1, Name = "Rice" };
            _mockCropProcess.Setup(p => p.GetCropByIdAsync(1)).ReturnsAsync(crop);

            var result = await _cropController.GetCropById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrop = okResult.Value as Crop;
            Assert.IsNotNull(returnedCrop);
            Assert.AreEqual(1, returnedCrop.CropId);
        }

        // ❌ GetCropById - Negation
        [TestMethod]
        public async Task GetCropById_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            _mockCropProcess.Setup(p => p.GetCropByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException("Crop not found."));

            var result = await _cropController.GetCropById(999);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        // ✅ GetCropByName - Affirmation
        [TestMethod]
        public async Task GetCropByName_ShouldReturnOk_WhenCropExists()
        {
            var crop = new Crop { CropId = 1, Name = "Wheat" };
            _mockCropProcess.Setup(p => p.GetCropByNameAsync("Wheat")).ReturnsAsync(crop);

            var result = await _cropController.GetCropByName("Wheat");

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedCrop = okResult.Value as Crop;
            Assert.IsNotNull(returnedCrop);
            Assert.AreEqual("Wheat", returnedCrop.Name);
        }

        // ❌ GetCropByName - Negation
        [TestMethod]
        public async Task GetCropByName_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            _mockCropProcess.Setup(p => p.GetCropByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new KeyNotFoundException("Crop not found."));

            var result = await _cropController.GetCropByName("NonExistent");

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        // ✅ CreateCrop - Affirmation
        [TestMethod]
        public async Task CreateCrop_ShouldReturnCreatedAtAction_WhenSuccessful()
        {
            var crop = new Crop { CropId = 1, Name = "Barley" };
            _mockCropProcess.Setup(p => p.CreateCropAsync(It.IsAny<Crop>())).ReturnsAsync(crop);

            var result = await _cropController.CreateCrop(crop);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var returnedCrop = createdAtActionResult.Value as Crop;
            Assert.IsNotNull(returnedCrop);
            Assert.AreEqual("Barley", returnedCrop.Name);
        }

        // ❌ CreateCrop - Negation
        [TestMethod]
        public async Task CreateCrop_ShouldReturnBadRequest_WhenCropIsNull()
        {
            _mockCropProcess.Setup(p => p.CreateCropAsync(null))
                .ThrowsAsync(new ArgumentNullException("Crop data cannot be null."));

            var result = await _cropController.CreateCrop(null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        // ✅ UpdateCrop - Affirmation
        [TestMethod]
        public async Task UpdateCrop_ShouldReturnNoContent_WhenSuccessful()
        {
            var crop = new Crop { CropId = 1, Name = "Soybean" };
            _mockCropProcess.Setup(p => p.UpdateCropAsync(It.IsAny<Crop>())).Returns(Task.CompletedTask);

            var result = await _cropController.UpdateCrop(1, crop);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // ❌ UpdateCrop - Negation
        [TestMethod]
        public async Task UpdateCrop_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            var crop = new Crop { CropId = 1, Name = "Soybean" };
            _mockCropProcess.Setup(p => p.UpdateCropAsync(It.IsAny<Crop>()))
                .ThrowsAsync(new KeyNotFoundException("Crop not found."));

            var result = await _cropController.UpdateCrop(1, crop);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // ✅ DeleteCrop - Affirmation
        [TestMethod]
        public async Task DeleteCrop_ShouldReturnNoContent_WhenSuccessful()
        {
            _mockCropProcess.Setup(p => p.DeleteCropAsync(1)).Returns(Task.CompletedTask);

            var result = await _cropController.DeleteCrop(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // ❌ DeleteCrop - Negation
        [TestMethod]
        public async Task DeleteCrop_ShouldReturnNotFound_WhenCropDoesNotExist()
        {
            _mockCropProcess.Setup(p => p.DeleteCropAsync(It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException("Crop not found."));

            var result = await _cropController.DeleteCrop(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
