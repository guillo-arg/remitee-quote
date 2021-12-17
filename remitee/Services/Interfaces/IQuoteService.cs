using remitee.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Services.Interfaces
{
    public interface IQuoteService
    {
        Task<ResponseDTO> AmountToSend(string sourceCurrency, string targetCurrency, decimal amount);
        Task UpdateQuote();
        Task<ResponseDTO> AmountToReceive(string sourceCurrency, string targetCurrency, decimal amount);
    }
}
