using RecipeApi.Models;

namespace RecipeApi.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly List<Recipe> _recipes = [];
    private int _nextRecipeId = 1;
    private int _nextIngredientId = 1;

    public Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Recipe>>(_recipes);
    }

    public Task<Recipe?> GetByIdAsync(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(recipe);
    }

    public Task<IEnumerable<Recipe>> SearchAsync(string term)
    {
        var lowerTerm = term.ToLowerInvariant();
        var result = _recipes.Where(r =>
            r.Name.Contains(lowerTerm, StringComparison.OrdinalIgnoreCase) ||
            r.Description.Contains(lowerTerm, StringComparison.OrdinalIgnoreCase) ||
            r.Ingredients.Any(i => i.Name.Contains(lowerTerm, StringComparison.OrdinalIgnoreCase)));
            
        return Task.FromResult<IEnumerable<Recipe>>(result);
    }

    public Task<IEnumerable<Recipe>> GetByDifficultyAsync(string difficulty)
    {
        var result = _recipes.Where(r => r.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<IEnumerable<Recipe>>(result);
    }

    public Task<Recipe> AddAsync(Recipe recipe)
    {
        recipe.Id = _nextRecipeId++;
        AssignIngredientIds(recipe.Ingredients);
        _recipes.Add(recipe);
        return Task.FromResult(recipe);
    }

    public Task<Recipe?> UpdateAsync(int id, Recipe recipe)
    {
        var existing = _recipes.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult<Recipe?>(null);

        existing.Name = recipe.Name;
        existing.Description = recipe.Description;
        existing.PrepTimeMinutes = recipe.PrepTimeMinutes;
        existing.CookTimeMinutes = recipe.CookTimeMinutes;
        existing.Servings = recipe.Servings;
        existing.Difficulty = recipe.Difficulty;
        existing.Ingredients = recipe.Ingredients;
        existing.Instructions = recipe.Instructions;

        AssignIngredientIds(existing.Ingredients);

        return Task.FromResult<Recipe?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);
        if (recipe is null)
            return Task.FromResult(false);

        _recipes.Remove(recipe);
        return Task.FromResult(true);
    }

    private void AssignIngredientIds(List<Ingredient> ingredients)
    {
        foreach (var ingredient in ingredients.Where(i => i.Id == 0))
        {
            ingredient.Id = _nextIngredientId++;
        }
    }
}