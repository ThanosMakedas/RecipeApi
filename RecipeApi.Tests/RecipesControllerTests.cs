using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecipeApi.Controllers;
using RecipeApi.Models;
using RecipeApi.Models.DTOs;
using RecipeApi.Services;

namespace RecipeApi.Tests;

public class RecipesControllerTests
{
    private readonly Mock<IRecipeService> _mockService;
    private readonly RecipesController _controller;

    public RecipesControllerTests()
    {
        _mockService = new Mock<IRecipeService>();
        _controller = new RecipesController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithRecipes()
    {
        var mockRecipes = new List<Recipe> { new Recipe { Id = 1, Name = "Test" } };
        _mockService.Setup(s => s.GetAllRecipesAsync()).ReturnsAsync(mockRecipes);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRecipes = Assert.IsAssignableFrom<IEnumerable<Recipe>>(okResult.Value);
        Assert.Single(returnedRecipes);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetRecipeByIdAsync(999)).ReturnsAsync((Recipe?)null);

        var result = await _controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreatedAtActionResult()
    {
        var dto = new CreateRecipeDto { Name = "Nytt Recept" };
        var createdRecipe = new Recipe { Id = 5, Name = "Nytt Recept" };
        
        _mockService.Setup(s => s.CreateRecipeAsync(dto)).ReturnsAsync(createdRecipe);

        var result = await _controller.Create(dto);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(RecipesController.GetById), createdAtResult.ActionName);
        Assert.Equal(5, createdAtResult.RouteValues?["id"]);
        
        var returnedRecipe = Assert.IsType<Recipe>(createdAtResult.Value);
        Assert.Equal(5, returnedRecipe.Id);
    }
}