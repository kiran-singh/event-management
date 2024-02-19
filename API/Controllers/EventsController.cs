using System.Net;
using AutoMapper;
using EventManagement.API.Models;
using EventManagement.API.ViewModels;
using EventManagement.Domain;
using EventManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

public class EventsController : BaseController
{
    private readonly IBaseRepository<Event> _repository;
    private readonly IMapper _mapper;

    public EventsController(IBaseRepository<Event> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var entities = await _repository.ListAllAsync();

        var viewModels = _mapper.Map<List<EventListViewModel>>(entities);
        
        return Ok(viewModels);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        var viewModel = _mapper.Map<EventDetailsViewModel>(entity);
        
        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateEvent createEvent)
    {
        var entity = _mapper.Map<Event>(createEvent);

        var result = await _repository.AddOrUpdateAsync(entity);
        
        return result
            ? StatusCode((int) HttpStatusCode.Created)
            : StatusCode((int) HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] UpdateEvent updateEvent)
    {
        var entity = _mapper.Map<Event>(updateEvent);
        
        var result = await _repository.AddOrUpdateAsync(entity);
        
        return result
            ? NoContent()
            : StatusCode((int) HttpStatusCode.InternalServerError);
    }
}