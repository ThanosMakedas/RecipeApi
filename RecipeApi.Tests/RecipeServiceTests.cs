using Xunit;
using Moq;
using RecipeApi.Models;
using RecipeApi.Models.DTOs;
using RecipeApi.Repositories;
using RecipeApi.Services;

namespace RecipeApi.Tests;

public class RecipeServiceTests
{
    private readonly Mock<IRecipeRepository> _mockRepo;
    private readonly RecipeService _service;

    public RecipeServiceTests()
    {
        _mockRepo = new Mock<IRecipeRepository>();
        _service = new RecipeService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllRecipesAsync_ReturnsListOfRecipes()
    {
        var mockRecipes = new List<Recipe> 
        { 
            new Recipe { Id = 1, Name = "Pannkakor" },
            new Recipe { Id = 2, Name = "Köttbullar" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockRecipes);

        var result = await _service.GetAllRecipesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetRecipeByIdAsync_ExistingId_ReturnsRecipe()
    {
        var recipe = new Recipe { Id = 1, Name = "Pannkakor" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(recipe);

        var result = await _service.GetRecipeByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Pannkakor", result.Name);
    }

    [Fact]
    public async Task GetRecipeByIdAsync_NonExistingId_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Recipe?)null);

        var result = await _service.GetRecipeByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateRecipeAsync_ValidDto_ReturnsRecipe()
    {
        var createDto = new CreateRecipeDto { Name = "Våfflor", Description = "Goda" };
        var expectedRecipe = new Recipe { Id = 1, Name = "Våfflor", Description = "Goda" };
        
        // Notera ReturnsAsync här
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Recipe>())).ReturnsAsync(expectedRecipe);

        var result = await _service.CreateRecipeAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("Våfflor", result.Name);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task SearchRecipesAsync_ValidTerm_ReturnsFilteredRecipes()
    {
        var searchResults = new List<Recipe> { new Recipe { Id = 1, Name = "Pannkakor" } };
        _mockRepo.Setup(r => r.SearchAsync("pannkaka")).ReturnsAsync(searchResults);

        var result = await _service.SearchRecipesAsync("pannkaka");

        Assert.Single(result);
        Assert.Equal("Pannkakor", result.First().Name);
    }
}