using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdsAuth.Data
{
    public class SimpleAdDb
    {
        private readonly string _connectionString;

        public SimpleAdDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSimpleAd(SimpleAd ad)
        {
            using var ctx = new SimpleAdsContext(_connectionString);
            ctx.Ads.Add(ad);
            ctx.SaveChanges();
        }

        public List<SimpleAd> GetAds()
        {
            using var ctx = new SimpleAdsContext(_connectionString);
            return ctx.Ads.OrderByDescending(a => a.Date).Include(a => a.User).ToList();
        }

        public List<SimpleAd> GetAdsForUser(int userId)
        {
            using var ctx = new SimpleAdsContext(_connectionString);
            return ctx.Ads.Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Date)
                .Include(a => a.User)
                .ToList();
        }

        public int GetUserIdForAd(int adId)
        {
            using var ctx = new SimpleAdsContext(_connectionString);
            return ctx.Ads.FirstOrDefault(a => a.Id == adId).UserId;
        }

        public void Delete(int id)
        {
            using var ctx = new SimpleAdsContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"DELETE FROM Ads WHERE Id = {id}");
        }
    }
}