using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using remitee.DTOs;
using remitee.Helpers;
using remitee.Models;
using remitee.Repositories.Interfaces;
using remitee.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace remitee.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly decimal _fee = 10;
        private readonly IConfiguration _configuration;
       

        public QuoteService(IQuoteRepository quoteRepository, IConfiguration configuration)
        {
            _quoteRepository = quoteRepository;
            _configuration = configuration;
        }
        public async Task<ResponseDTO> AmountToSend(string sourceCurrency, string targetCurrency, decimal amount)
        {
            Quote sourceQuote = await _quoteRepository.GetById(sourceCurrency);
            Quote targetQuote = await _quoteRepository.GetById(targetCurrency);

            decimal dollarAmount = amount / targetQuote.Amount;

            decimal result = dollarAmount * sourceQuote.Amount / (1 - _fee / 100);

            return new ResponseDTO() { 
                Currency = sourceCurrency,
                Amount = result
            };

        }
        public async Task<ResponseDTO> AmountToReceive(string sourceCurrency, string targetCurrency, decimal amount)
        {
            Quote sourceQuote = await _quoteRepository.GetById(sourceCurrency);
            Quote targetQuote = await _quoteRepository.GetById(targetCurrency);

            decimal dollarAmount = amount / sourceQuote.Amount;

            decimal result = dollarAmount * targetQuote.Amount * (1 - _fee / 100);

            return new ResponseDTO()
            {
                Currency = targetCurrency,
                Amount = result
            };
        }

        public  async Task UpdateQuote()
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{_configuration["HOST_API"]}/live";
            url = $"{url}?access_key={_configuration["API_KEY"]}";

            try
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    ResponseAPI responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(content);

                    List<Quote> quotes = await MapQuote(responseAPI);
                    await _quoteRepository.UpdateAll(quotes);

                }
            }
            catch (Exception)
            {
                throw new AppException("Error API", 500);
            }

        }

        private static async Task<List<Quote>> MapQuote(ResponseAPI responseAPI)
        {
            List<Quote> quotes = new List<Quote>();

            var properties = responseAPI.quotes.GetType().GetProperties();
            foreach (var prop in properties)
            {
                string id = prop.Name;
                decimal amount = (decimal) prop.GetValue(responseAPI.quotes);

                quotes.Add(
                    new Quote()
                    {
                        Id = id,
                        Amount = amount
                    });
            }

            return quotes;
        }

    }
}
