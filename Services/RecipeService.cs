using RecipeApi.Models;
using RecipeApi.Models.DTOs;
using RecipeApi.Repositories;

namespace RecipeApi.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _repository;

    public RecipeService(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string term)
    {
        return await _repository.SearchAsync(term);
    }

    public async Task<IEnumerable<Recipe>> GetRecipesByDifficultyAsync(string difficulty)
    {
        return await _repository.GetByDifficultyAsync(difficulty);
    }

    public async Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto)
    {
        var recipe = MapToRecipe(dto);
        return await _repository.AddAsync(recipe);
    }

    public async Task<Recipe?> UpdateRecipeAsync(int id, UpdateRecipeDto dto)
    {
        var recipe = MapToRecipe(dto);
        return await _repository.UpdateAsync(id, recipe);
    }

    public async Task<bool> DeleteRecipeAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static Recipe MapToRecipe(CreateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            PrepTimeMinutes = dto.PrepTimeMinutes,
            CookTimeMinutes = dto.CookTimeMinutes,
            Servings = dto.Servings,
            Difficulty = dto.Difficulty,
            Ingredients = dto.Ingredients.Select(MapToIngredient).ToList(),
            Instructions = dto.Instructions
        };
    }

    private static Recipe MapToRecipe(UpdateRecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            PrepTimeMinutes = dto.PrepTimeMinutes,
            CookTimeMinutes = dto.CookTimeMinutes,
            Servings = dto.Servings,
            Difficulty = dto.Difficulty,
            Ingredients = dto.Ingredients.Select(MapToIngredient).ToList(),
            Instructions = dto.Instructions
        };
    }

    private static Ingredient MapToIngredient(CreateIngredientDto dto)
    {
        return new Ingredient
        {
            Name = dto.Name,
            Quantity = dto.Quantity,
            Unit = dto.Unit
        };
    }
}