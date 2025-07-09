using System.Net.Http.Headers;
using CropService;
using DealerService.Repositories;

namespace DealerService.Process
{
    public class DealerProcess  : IDealerProcess
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        public DealerProcess(IDealerRepository dealerRepository, HttpClient httpClient, IHttpContextAccessor ctx)
        {
            _dealerRepository = dealerRepository;
            _httpClient = httpClient;
            _context = ctx;
        }

        public async Task<Dealer> GetDealerByIdAsync(int dealerId)
        {
            return await _dealerRepository.GetDealerByIdAsync(dealerId);
        }

        public async Task<bool> UpdateDealerAsync(int dealerId, Dealer dealer)
        {
            return await _dealerRepository.UpdateDealerAsync(dealerId, dealer);
        }

        public async Task<List<Crop>> GetAllCropsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Items["Token"].ToString());
            var response = await _httpClient.GetAsync("http://localhost:5077/api/crops");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Crop>>();
            }
            return null;
        }

        public async Task<Crop> GetCropByNameAsync(string cropName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Items["Token"].ToString());
            var response = await _httpClient.GetAsync($"http://localhost:5077/api/crops/name/{cropName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Crop>();
            }
            return null;
        }

        public async Task<Crop> GetCropByIdAsync(int cropId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.HttpContext.Items["Token"].ToString());
            var response = await _httpClient.GetAsync($"http://localhost:5077/api/crops/{cropId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Crop>();
            }
            return null;
        }
        public async Task<bool> UpdateDealersStatusAsync(List<int> dealerIds, bool isActive)
        {
            return await _dealerRepository.UpdateDealersStatusAsync(dealerIds, isActive);
        }
        public async Task<List<Dealer>> GetAllDealersAsync()
        {
            return await _dealerRepository.GetAllDealersAsync();
        }
        public async Task<Dealer> RegisterDealer(Dealer dealer)
        {
            return await _dealerRepository.AddDealer(dealer);
        }
        public async Task<int?> GetDealerIdByUserIdAsync(int userId)
        {
            return await _dealerRepository.GetDealerIdByUserIdAsync(userId);
        }
    }
}
