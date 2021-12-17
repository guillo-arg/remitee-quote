using remitee.Helpers;
using remitee.Models;
using remitee.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppRemiteeContext _appContext;
        private readonly string _baseId = "USD";
        public QuoteRepository(AppRemiteeContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Quote> GetById(string sourceCurrency)
        {
            string id = $"{_baseId}{sourceCurrency}";

            Quote quote = _appContext.Quotes.Where(x => x.Id == id).FirstOrDefault();

            if (quote is null)
            {
                throw new AppException("sourceCurrency not found", 404);
            }

            return quote;
        }

        public async Task UpdateAll(List<Quote> quotes)
        {
            List<Quote> quotesDB = _appContext.Quotes.ToList();
            List<Quote> quotesToAdd = new List<Quote>();

            foreach (var item in quotes)
            {
                var quoteDB = quotesDB.FirstOrDefault(x => x.Id == item.Id);
                if (quoteDB is null)
                {
                    quotesToAdd.Add(item);
                }
                else
                {
                    quoteDB.Amount = item.Amount;
                }
            }
            
            try
            {
                _appContext.Quotes.UpdateRange(quotesDB);
                _appContext.Quotes.AddRange(quotesToAdd);
                _appContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new AppException("Database error", 500);
            }
        }
    }
}
