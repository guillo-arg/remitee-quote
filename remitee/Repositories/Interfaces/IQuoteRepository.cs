using remitee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Repositories.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote> GetById(string sourceCurrency);
        Task UpdateAll(List<Quote> quotes);
    }
}
