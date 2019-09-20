using AutoMapper;
using AlejandroGarciaMalo.Models;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.JsonModels;
using AlejandroGarciaMalo.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AlejandroGarciaMalo.UnitTest
{
    public class UnitTest1
    {
        public DbContextOptions<MyDbContext> options;
        private readonly IMapper _mapper;

        public UnitTest1()
        {
            options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "Test1")
                .Options;

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMappingProfiles());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async void GetRatesFromApi()
        {
            // Arrange
            HelperConfig.UrlRates = "http://quiet-stone-2094.herokuapp.com/rates.json";

            // Act
            var value = await HelperApi<JsonRate>.GetFromApi(HelperConfig.UrlRates);

            var rates = _mapper.Map<List<Rate>>(value);

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(value.Count(), rates.Count());
            Assert.NotNull(rates.FirstOrDefault());
            Assert.NotEqual(0, rates.FirstOrDefault().RateValue);
        }

        [Fact]
        public async void GetTransactionsFromApi()
        {
            // Arrange
            HelperConfig.UrlTransactions = "http://quiet-stone-2094.herokuapp.com/transactions.json";

            // Act
            var value = await HelperApi<JsonTransaction>.GetFromApi(HelperConfig.UrlTransactions);

            var transactions = _mapper.Map<List<Transaction>>(value);

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(value.Count(), transactions.Count());
            Assert.NotNull(transactions.FirstOrDefault());
            Assert.NotEqual(0, transactions.FirstOrDefault().Amount);
        }

        [Fact]
        public async void InsertRateDB()
        {
            using (var context = new MyDbContext(options))
            {
                var rate = new Rate()
                {
                    From = "EUR",
                    To = "CAD",
                    RateValue = 0.78f
                };

                context.Rates.Add(rate);
                await context.SaveChangesAsync();
            }
            
            using (var context = new MyDbContext(options))
            {
                var count = await context.Rates.CountAsync();
                Assert.Equal(1, count);
                
                var u = await context.Rates.FirstOrDefaultAsync(user => user.From.Equals("EUR"));
                Assert.NotNull(u);

                var query = context.GetRates();
                Assert.NotNull(query);
                var queryRate = context.GetRateByIndexAsync(new Rate { From = "EUR", To = "CAD" });
                Assert.NotNull(queryRate);
            }
        }

        [Fact]
        public async void InsertTransactionDB()
        {
            using (var context = new MyDbContext(options))
            {
                var transaction = new Transaction()
                {
                    Sku = "I2103",
                    Amount = 15.8f,
                    Currency = "USD"
                };

                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
            
            using (var context = new MyDbContext(options))
            {
                var count = await context.Transactions.CountAsync();
                Assert.Equal(1, count);

                var u = await context.Transactions.FirstOrDefaultAsync(user => user.Sku.Equals("I2103"));
                Assert.NotNull(u);
            }
        }
    }
}
