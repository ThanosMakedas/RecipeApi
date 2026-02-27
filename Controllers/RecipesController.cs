using Microsoft.AspNetCore.Mvc;
using RecipeApi.Models.DTOs;
using RecipeApi.Services;

namespace RecipeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var recipes = await _recipeService.GetAllRecipesAsync();
        return Ok(recipes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);
        if (recipe is null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Search term cannot be empty.");

        var recipes = await _recipeService.SearchRecipesAsync(q);
        return Ok(recipes);
    }

    [HttpGet("difficulty/{level}")]
    public async Task<IActionResult> GetByDifficulty(string level)
    {
        var recipes = await _recipeService.GetRecipesByDifficultyAsync(level);
        return Ok(recipes);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecipeDto dto)
    {
        var recipe = await _recipeService.CreateRecipeAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRecipeDto dto)
    {
        var recipe = await _recipeService.UpdateRecipeAsync(id, dto);
        if (recipe is null)
            return NotFound();

        return Ok(recipe);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _recipeService.DeleteRecipeAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}