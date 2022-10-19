using CarRenting.Data;

namespace CarRenting.Services.Dealers
{
    public class DealerService : IDealerService
    {
        private readonly CarRentingDbContext data;

        public DealerService(CarRentingDbContext _data)
        {
            data = _data;
        }
               
        public bool IsDealer(string userId)
        {
            return data
                    .Dealers
                    .Any(d => d.UserId == userId);
        }

        public int IdByUser(string userId)
        {
            return data
                .Dealers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
        }
    }
}
