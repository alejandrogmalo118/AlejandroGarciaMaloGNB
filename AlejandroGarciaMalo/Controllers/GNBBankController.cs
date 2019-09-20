using AlejandroGarciaMalo.Models;
using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.JsonModels;
using AlejandroGarciaMalo.Models.UnitOfWork;
using AlejandroGarciaMalo.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GNBBankController : BaseController
    {
        #region ctor
        public GNBBankController(ILogger<BaseController> logger, IMapper mapper)
            : base(logger, mapper)
        { }
        #endregion

        #region Actions
        /// <summary>
        /// Retrieve all Rates
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRates()
        {
            Logger.LogDebug("'{0}' has been invoked", nameof(GetRates));

            var response = new ListResponse<Rate>();

            try
            {
                var data = await GetAllRates();

                response.Model = data;
                response.Message = $"Total of Rates: {data.Count()}";
            }
            catch (Exception ex)
            {
                ResponseError(response, ex, nameof(GetRates));
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves all Transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTransactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactions()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetTransactions));

            var response = new ListResponse<Transaction>();

            try
            {
                var data = await GetAllTransactions();

                response.Model = data;
                response.Message = $"Total of Transactions: {data.Count()}";
            }
            catch (Exception ex)
            {
                ResponseError(response, ex, nameof(GetTransactions));
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Get all transactions by Sku, with the amount in EUR
        /// </summary>
        /// <param name="Sku"></param>
        /// <returns></returns>
        [HttpGet("GetTransactionsBySku")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTransactionsBySku(string Sku)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetTransactionsBySku));

            var response = new ListResponse<Transaction>();
            var outputCurrency = "EUR";
            try
            {
                var dataRates = (await GetAllRates()).ToList();
                _dbContext = new MyDbContext();
                var dataTransactions = (await GetAllTransactions()).ToList();
                var filteredTransactions = dataTransactions.Where(t => t.Sku.Equals(Sku)).ToList();
                
                foreach (var transaction in filteredTransactions)
                {
                    List<Rate> listFinalRates = new List<Rate>();

                    if (transaction.Currency.Equals(outputCurrency))
                        continue;

                    Converter.SearchBestRate(transaction.Currency, outputCurrency, listFinalRates, dataRates);

                    for (int i = 0; i < listFinalRates.Count(); i++)
                    {
                        if (transaction.Currency.Equals(outputCurrency))
                            break;

                        var rate = listFinalRates.FirstOrDefault(r => r.From.Equals(transaction.Currency));

                        transaction.Currency = rate.To;
                        transaction.Amount = transaction.Amount * rate.RateValue;
                    }
                }
                
                response.Model = filteredTransactions;
                response.Message = $"Total of Transactions: {filteredTransactions.Count()} , Sum of all Amount: {filteredTransactions.Sum(t => t.Amount)} {outputCurrency}";
            }
            catch (Exception ex)
            {
                ResponseError(response, ex, nameof(GetTransactionsBySku));
            }

            return response.ToHttpResponse();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Method for getting Rates
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Rate>> GetAllRates()
        {
            IEnumerable<JsonRate> jsonRates = Enumerable.Empty<JsonRate>();

            if(Helper.CheckForInternetConnection())
            {
                jsonRates = await HelperApi<JsonRate>.GetFromApi(HelperConfig.UrlRates);
            }
            else
            {
                Logger?.LogWarning($"There was an error, the api cant connect to the server, retrieving data from sqlite...");

                using (var unitOfWork = new UnitOfWork(_dbContext))
                {
                    var dataDb = await unitOfWork.Rates.GetAll();

                    Logger.LogInformation("The rates have been retrieved successfully.");

                    return dataDb;
                }
            }

            // La conexión con la API externa ha tenido éxito, guardamos los resultados en nuestra base de datos local
            using (var unitOfWork = new UnitOfWork(_dbContext))
            {
                var dataDb = await unitOfWork.Rates.GetAll();
                unitOfWork.Rates.DeleteRange(dataDb);
                unitOfWork.Complete();

                var rates = _mapper.Map<List<Rate>>(jsonRates);

                unitOfWork.Rates.AddRange(rates);
                unitOfWork.Complete();

                Logger.LogInformation("The rates have been retrieved successfully.");

                return rates;
            }
        }

        /// <summary>
        /// Method for getting Transactions
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            IEnumerable<JsonTransaction> jsonTransactions = Enumerable.Empty<JsonTransaction>();

            if (Helper.CheckForInternetConnection())
            {
                jsonTransactions = await HelperApi<JsonTransaction>.GetFromApi(HelperConfig.UrlTransactions);
            }
            else
            {
                Logger?.LogWarning($"There was an error, the api cant connect to the server, retrieving data from sqlite...");

                using (var unitOfWork = new UnitOfWork(_dbContext))
                {
                    var dataDb = await unitOfWork.Transactions.GetAll();

                    Logger.LogInformation("The Transactions have been retrieved successfully.");

                    return dataDb;
                }
            }
            
            // La conexión con la API externa ha tenido éxito, guardamos los resultados en nuestra base de datos local
            using (var unitOfWork = new UnitOfWork(_dbContext))
            {
                var dataDb = await unitOfWork.Transactions.GetAll();
                unitOfWork.Transactions.DeleteRange(dataDb);
                unitOfWork.Complete();

                var transactions = _mapper.Map<List<Transaction>>(jsonTransactions);

                unitOfWork.Transactions.AddRange(transactions);
                unitOfWork.Complete();

                Logger?.LogInformation("The transactions have been retrieved successfully.");

                return transactions;
            }
        }

        /// <summary>
        /// Method for resolving error
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        private void ResponseError<T>(ListResponse<T> response, Exception ex, string method)
        {
            response.DidError = true;
            response.ErrorMessage = "There was an internal error, please contact to technical support.";

            Logger?.LogCritical($"There was an error on '{method}' invocation: {ex}");
        }

        #endregion
    }
}