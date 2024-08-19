namespace Tests;

using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Moq;
using Xunit;

public class RentApplicationTests
{
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
    private readonly Mock<IDeliveryManRepository> _deliveryManRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RentService _rentService;

    public RentApplicationTests()
    {
        _rentRepositoryMock = new Mock<IRentRepository>();
        _planRepositoryMock = new Mock<IPlanRepository>();
        _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        _deliveryManRepositoryMock = new Mock<IDeliveryManRepository>();
        _mapperMock = new Mock<IMapper>();

        _rentService = new RentService(_rentRepositoryMock.Object, _planRepositoryMock.Object,
            _motorcycleRepositoryMock.Object, _deliveryManRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task ConsultBudgetAsync_ShouldCalculateFine_WhenDaysForecastIsLessThanDaysPlan()
    {
        // Arrange
        var plan = ArrangeObjects.ListPlans(7);
        var requestDto = ArrangeObjects.CreateRequestDto(plan);
        var startDate = DateTime.Now.Date;

        _planRepositoryMock.Setup(repo => repo.GetByIdAsync(plan.Id)).ReturnsAsync(plan);

        // Act
        var result = await _rentService.ConsultBudgetAsync(requestDto);

        // Assert
        ArrangeObjects.VerifyResult(plan, requestDto, result, startDate);

        _planRepositoryMock.Verify(repo => repo.GetByIdAsync(plan.Id), Times.AtLeastOnce);
    }

    [Fact]
    public async Task AddAsync_ValidDto_AddsRentAndMarksMotorcycleInUse()
    {
        // Arrange
        var dto = new RentRequestDto
        {
            DeliveryManId = Guid.NewGuid(),
            MotocycleId = Guid.NewGuid()
        };
        var deliveryMan = new DeliveryMan { TypeCnh = "A" };
        var budgetResponse = new BudGetResponseDto
        (
            planId: Guid.NewGuid(),
            planDescription: "Test Plan",
            planPrice: 30M,
            startDate: DateTime.Now.AddDays(1),
            endDate: DateTime.Now.AddDays(7),
            forecastDate: DateTime.Now.AddDays(5),
            subTotalRent: 210.00M,
            fineValue: 0.00M,
            totalRent: 210.00M,
            description: "Test Description"
        );

        _deliveryManRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.DeliveryManId))
            .ReturnsAsync(deliveryMan);

        _mapperMock.Setup(mapper => mapper.Map<BudGetRequestDto>(dto))
            .Returns(new BudGetRequestDto());

        _planRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Plan(label: "", price: 30M, period: 7, valueFine: 0.20M, additionalDailyValue: 50));

        _mapperMock.Setup(mapper => mapper.Map<Rent>(It.IsAny<BudGetResponseDto>()))
            .Returns(new Rent());

        _rentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Rent>()))
            .Returns(Task.CompletedTask);

        _motorcycleRepositoryMock.Setup(repo => repo.MarkMotorInUse(dto.MotocycleId, true))
            .Returns(Task.CompletedTask);

        // Act
        await _rentService.AddAsync(dto);

        // Assert
        _rentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Rent>()), Times.Once);
        _motorcycleRepositoryMock.Verify(repo => repo.MarkMotorInUse(dto.MotocycleId, true), Times.Once);
    }

    [Fact]
    public async Task AddAsync_InvalidCnhType_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var dto = new RentRequestDto
        {
            DeliveryManId = Guid.NewGuid()
        };
        var deliveryMan = new DeliveryMan { TypeCnh = "B" };

        _deliveryManRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.DeliveryManId))
            .ReturnsAsync(deliveryMan);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _rentService.AddAsync(dto));
    }

    [Fact]
    public async Task ConsultBudgetAsync_ValidDto_ReturnsBudget()
    {
        // Arrange
        var dto = new BudGetRequestDto
        {
            PlanId = Guid.NewGuid(),
            ForecastDate = DateTime.Now.AddDays(7)
        };

        var plan = ArrangeObjects.ListPlans(7);

        _planRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.PlanId))
            .ReturnsAsync(plan);

        // Act
        var result = await _rentService.ConsultBudgetAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(plan.Label, result.PlanDescription);
        Assert.Equal(plan.Price, result.PlanPrice);
        Assert.Equal(plan.Period, (Convert.ToDateTime(result.EndDate).Date - DateTime.Now.Date).Days);
        Assert.Equal(plan.Period * plan.Price, result.SubTotalRent);
        Assert.Equal(0.00M, result.FineValue);

        var additionalDaysEffective = (dto.ForecastDate.Date - Convert.ToDateTime(result.StartDate).Date).Days + 1 - plan.Period;
        var expectedAdditionalValue = result.SubTotalRent + (additionalDaysEffective * plan.AdditionalDailyValue);
        Assert.Equal(expectedAdditionalValue, result.TotalRent);
        Assert.Contains("", result.Description);
    }

    [Fact]
    public async Task ConsultBudgetAsync_ShouldCalculateAdditional_WhenDaysForecastIsGreaterThanDaysPlan()
    {
        // Arrange
        var plan = ArrangeObjects.ListPlans(7);

        var requestDto = new BudGetRequestDto
        {
            PlanId = plan.Id,
            ForecastDate = DateTime.Now.AddDays(15)
        };

        _planRepositoryMock.Setup(repo => repo.GetByIdAsync(plan.Id)).ReturnsAsync(plan);

        // Act
        var result = await _rentService.ConsultBudgetAsync(requestDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(plan.Label, result.PlanDescription);
        Assert.Equal(plan.Price, result.PlanPrice);
        Assert.Equal(plan.Period, (Convert.ToDateTime(result.EndDate).Date - DateTime.Now.Date).Days);
        Assert.Equal(plan.Period * plan.Price, result.SubTotalRent);

        var additionalDaysEffective = (requestDto.ForecastDate.Date - Convert.ToDateTime(result.StartDate).Date).Days + 1 - plan.Period;
        
        var expectedFineValue = additionalDaysEffective * plan.AdditionalDailyValue;
        Assert.Equal(expectedFineValue, result.FineValue);

        var expectedTotalRentValue = result.SubTotalRent + (additionalDaysEffective * plan.AdditionalDailyValue);
        Assert.Equal(expectedTotalRentValue, result.TotalRent);

        string expectedMessage = $"Valor adicional de {plan.AdditionalDailyValue} para {additionalDaysEffective} dia(s) adicional(is)";
        Assert.Contains(expectedMessage, result.Description);
    }
}
