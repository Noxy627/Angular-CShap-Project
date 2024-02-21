using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.Models.DTO;
using CodePulse.Models.Domain;
using CodePulse.Data;
using CodePulse.Repositories.Interfaces;

namespace CodePulse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class CategoriesController : ControllerBase
  {
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
      this.categoryRepository = categoryRepository;
    }

    [HttpPost]

    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
    {
      var category = new Category
      {
        Name= request.Name,
        UrlHandle=request.UrlHandle,
      };

      await categoryRepository.CreateAsync(category);

      var response = new CategoryDto
      {
        Id = category.Id,
        Name =category.Name,
        UrlHandle=category.UrlHandle,
      };

      return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
      var categories = await categoryRepository.GetAllAsync();

      var response = new List<CategoryDto>();

      foreach (var category in categories)
      {
        response.Add(new CategoryDto
        {
          Id= category.Id,
          Name = category.Name,
          UrlHandle=category.UrlHandle,
        }); 
      }

      return Ok(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
      var eCategory = await categoryRepository.GetByIdAsync(id);

      if (eCategory == null)
      {
        return NotFound();
      }

      var response = new CategoryDto
      {
        Id = eCategory.Id,
        Name = eCategory.Name,
        UrlHandle = eCategory.UrlHandle,
      };
        return Ok(response);  
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateRequestDto request)
    {
      //Create a new Domain Model

      var category = new Category
      {
        Id = id,
        Name = request.Name,
        UrlHandle = request.UrlHandle,
      };

      category = await categoryRepository.UpdateAsync(category);

      if(category == null)
      {
        return NotFound();
      }

      //Convert the domain model 
      var response = new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle,
      };
      return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
      var category = await categoryRepository.DeleteAsync(id);

      if(category == null)
      {
        return NotFound();
      }

      var response = new CategoryDto
      { Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle,
      };

      return Ok(response);
    }
  }
}
