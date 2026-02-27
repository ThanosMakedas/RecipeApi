using RecipeApi.Models;
using RecipeApi.Models.DTOs;

namespace RecipeApi.Services;

public interface IRecipeService
{
    Task<IEnumerable<Recipe>> GetAllRecipesAsync();
    Task<Recipe?> GetRecipeByIdAsync(int id);
    Task<IEnumerable<Recipe>> SearchRecipesAsync(string term);
    Task<IEnumerable<Recipe>> GetRecipesByDifficultyAsync(string difficulty);
    Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto);
    Task<Recipe?> UpdateRecipeAsync(int id, UpdateRecipeDto dto);
    Task<bool> DeleteRecipeAsync(int id);
}