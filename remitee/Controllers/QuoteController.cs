using Microsoft.AspNetCore.Mvc;
using remitee.DTOs;
using remitee.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;
        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }


        [HttpGet("AmountToSend")]
        public async Task<IActionResult> AmountToSend(string sourceCurrency, string targetCurrency, decimal amount)
        {
            ResponseDTO response = await _quoteService.AmountToSend(sourceCurrency, targetCurrency, amount);

            return Ok(response);
        }

        [HttpGet("AmountToReceive")]
        public async Task<IActionResult> AmountToReceive(string sourceCurrency, string targetCurrency, decimal amount)
        {
            ResponseDTO response = await _quoteService.AmountToReceive(sourceCurrency, targetCurrency, amount);

            return Ok(response);
        }

    }
}
