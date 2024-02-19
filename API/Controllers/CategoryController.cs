using System.Net;
using AutoMapper;
using EventManagement.API.Models;
using EventManagement.API.ViewModels;
using EventManagement.Domain;
using EventManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers;

public class CategoryController : BaseController
{
    private readonly IBaseRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryController(IBaseRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategory createCategory)
    {
        var category = _mapper.Map<Category>(createCategory);
        
        var result = await _repository.AddOrUpdateAsync(category);
        
        return result
            ? StatusCode((int) HttpStatusCode.Created)
            : StatusCode((int) HttpStatusCode.InternalServerError);
    }
    
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var entities = await _repository.ListAllAsync();

        var viewModels = _mapper.Map<List<CategoryViewModel>>(entities);
    
        return Ok(viewModels);
    }
    
    [HttpPut]
    public async Task<ActionResult> Put([FromBody] UpdateCategory updateEvent)
    {
        var entity = _mapper.Map<Category>(updateEvent);
        
        var result = await _repository.AddOrUpdateAsync(entity);
        
        return result
            ? NoContent()
            : StatusCode((int) HttpStatusCode.InternalServerError);
    }
}