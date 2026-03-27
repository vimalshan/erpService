using CurrencyManagement.Application.Currencies.Commands.CreateCurrency;
using CurrencyManagement.Application.Currencies.Commands.DeleteCurrency;
using CurrencyManagement.Application.Currencies.Commands.UpdateCurrency;
using CurrencyManagement.Application.Currencies.Queries.GetAllCurrencies;
using CurrencyManagement.Application.Currencies.Queries.GetCurrencyById;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;
using CurrencyManagement.Application.ExchangeRates.Queries.ConvertAmount;
using CurrencyManagement.Application.ExchangeRates.Queries.GetExchangeRate;
using CurrencyManagement.Application.OrganizationCurrencies.Commands.MapOrganizationCurrency;
using CurrencyManagement.Application.OrganizationCurrencies.Queries.GetOrganizationCurrencies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CurrencyManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all currencies
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IList<CurrencyDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCurrenciesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a currency by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CurrencyDto>> GetById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCurrencyByIdQuery(id), cancellationToken);
            return Ok(result);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates a new currency
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CurrencyDto>> Create(CreateCurrencyCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.CurrencyId }, result);
    }

    /// <summary>
    /// Updates a currency
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CurrencyDto>> Update(long id, UpdateCurrencyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.CurrencyId)
            return BadRequest("ID mismatch");

        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes a currency
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteCurrencyCommand(id), cancellationToken);
            return NoContent();
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeRatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets exchange rate for a currency pair
    /// </summary>
    [HttpGet("{fromCurrencyId}/{toCurrencyId}/{financialYear}/{month}")]
    public async Task<ActionResult<ExchangeRateDto>> GetRate(long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetExchangeRateQuery(fromCurrencyId, toCurrencyId, financialYear, month), cancellationToken);
            return Ok(result);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Sets/updates an exchange rate
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ExchangeRateDto>> SetRate(SetExchangeRateCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetRate), new { fromCurrencyId = result.FromCurrencyId, toCurrencyId = result.ToCurrencyId, financialYear = result.FinancialYear, month = result.Month }, result);
    }

    /// <summary>
    /// Converts an amount between currencies
    /// </summary>
    [HttpPost("convert")]
    public async Task<ActionResult<ConvertedAmountDto>> ConvertAmount(ConvertAmountQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return NotFound("Exchange rate not found");
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class OrganizationCurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationCurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets currencies mapped to an organization
    /// </summary>
    [HttpGet("{organizationId}")]
    public async Task<ActionResult<IList<OrganizationCurrencyDto>>> GetByOrganization(long organizationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrganizationCurrenciesQuery(organizationId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Maps a currency to an organization
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrganizationCurrencyDto>> Map(MapOrganizationCurrencyCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetByOrganization), new { organizationId = result.OrganizationId }, result);
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
