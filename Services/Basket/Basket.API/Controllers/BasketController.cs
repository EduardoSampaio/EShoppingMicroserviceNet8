using System.Net;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IMediator mediator, ILogger<BasketController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);
        return Ok(basket);
    }
    
    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        
        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }
    
    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        var query = new DeleteBasketByUserNameQuery(userName);
        return Ok(await _mediator.Send(query));
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        //Get existing basket with username
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        if (basket == null)
        {
            return BadRequest();
        }

        //remove the basket
        var deleteQuery = new DeleteBasketByUserNameQuery(basketCheckout.UserName);
        await _mediator.Send(deleteQuery);
        return Accepted();
    }
}