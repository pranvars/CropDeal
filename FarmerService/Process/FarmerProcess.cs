using System.Net.Http.Headers;
using CropService;
using FarmerService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmerService.Process
{
    public class FarmerProcess : IFarmerProcess
    {
        private readonly IFarmerRepository _farmerRepository;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;

        public FarmerProcess(IFarmerRepository farmerRepository, HttpClient httpClient, IHttpContextAccessor ctx)
        {
            _farmerRepository = farmerRepository;
            _httpClient = httpClient;
            _context = ctx;
        }

        public async Task<Farmer> GetFarmerByIdAsync(int farmerId)
        {
            return await _farmerRepository.GetFarmerByIdAsync(farmerId);
        }

        public async Task<bool> UpdateFarmerAsync(int farmerId, Farmer farmer)
        {
            return await _farmerRepository.UpdateFarmerAsync(farmerId, farmer);
        }

        public async Task<bool> PublishCropAsync(Crop crop)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Items["Token"].ToString());
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5077/api/crops", crop);
            return response.IsSuccessStatusCode;
        }
        //public async Task<bool> UpdateFarmersStatusAsync(List<Farmer> farmers)
        //{
        //    return await _farmerRepository.UpdateFarmersStatusAsync(farmers);
        //}

        //public async Task<bool> UpdateFarmersStatusAsync(bool isActive)
        //{
        //    return await _farmerRepository.UpdateFarmersStatusAsync(isActive);
        //}

        public async Task<bool> UpdateFarmersStatusAsync(List<int> farmerIds, bool isActive)
        {
            return await _farmerRepository.UpdateFarmersStatusAsync(farmerIds, isActive);
        }
        public async Task<List<Farmer>> GetAllFarmersAsync()
        {
            return await _farmerRepository.GetAllFarmersAsync();
        }
        public async Task<Farmer> RegisterFarmer(Farmer farmer)
        {
            return await _farmerRepository.AddFarmer(farmer);
        }
        public async Task<int?> GetFarmerIdByUserIdAsync(int userId)
        {
            return await _farmerRepository.GetFarmerIdByUserIdAsync(userId);
        }
    }
}
