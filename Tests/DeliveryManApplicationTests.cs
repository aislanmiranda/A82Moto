namespace Tests;

using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

public class DeliveryManApplicationTests
{
    private readonly Mock<IDeliveryManRepository> _repositoryMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeliveryManService _service;

    public DeliveryManApplicationTests()
    {
        _repositoryMock = new Mock<IDeliveryManRepository>();
        _mapperMock = new Mock<IMapper>();
        _envMock = new Mock<IWebHostEnvironment>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _service = new DeliveryManService(_repositoryMock.Object,
            _mapperMock.Object, _envMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenCnpjOrCnhAlreadyExists()
    {
        // Arrange
        var dto = new DeliveryManRequestDto { Cnpj = "123456789", NumberCnh = 1234567 };
        _repositoryMock.Setup(r => r.ExistDeliveryManWithCnpjAndCnh(dto.Cnpj, dto.NumberCnh)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_ShouldAddDeliveryMan_WhenCnpjAndCnhDoNotExist()
    {
        // Arrange
        var dto = new DeliveryManRequestDto { Cnpj = "123456789", NumberCnh = 1234567 };
        var deliveryman = new DeliveryMan();

        _repositoryMock.Setup(r => r.ExistDeliveryManWithCnpjAndCnh(dto.Cnpj, dto.NumberCnh)).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<DeliveryMan>(dto)).Returns(deliveryman);
        _repositoryMock.Setup(r => r.AddAsync(deliveryman)).Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(dto);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(deliveryman), Times.Once);
    }

    [Fact]
    public async Task UpdatePhotoAsync_ShouldThrowException_WhenPhotoExtensionIsInvalid()
    {
        // Arrange
        var dto = new DeliveryManUpdatePhotoDto { Id = Guid.NewGuid(), PhotoFile = Mock.Of<IFormFile>(f => f.FileName == "invalidfile.jpeg") };
        _repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(new DeliveryMan());
        _envMock.Setup(e => e.ContentRootPath).Returns("mock/path");

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.UpdatePhotoAsync(dto));
    }

    [Fact]
    public async Task UpdatePhotoAsync_ShouldUpdatePhoto_WhenPhotoExtensionIsValid()
    {
        // Arrange
        var dto = new DeliveryManUpdatePhotoDto { Id = Guid.NewGuid(), PhotoFile = Mock.Of<IFormFile>(f => f.FileName == "validfile.png") };

        _repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(new DeliveryMan());
        _repositoryMock.Setup(r => r.UpdatePhotoAsync(dto.Id, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        _envMock.Setup(e => e.ContentRootPath).Returns("mock/path");

        // Act
        await _service.UpdatePhotoAsync(dto);

        // Assert
        _repositoryMock.Verify(r => r.UpdatePhotoAsync(dto.Id, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDeliveryMan()
    {
        // Arrange
        var deliveryman = new List<DeliveryMan> { new DeliveryMan(), new DeliveryMan() };
        var expectedDtos = new List<DeliveryManResponseDto> { new DeliveryManResponseDto(), new DeliveryManResponseDto() };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(deliveryman);
        _mapperMock.Setup(m => m.Map<IEnumerable<DeliveryManResponseDto>>(deliveryman)).Returns(expectedDtos);

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.Equal(expectedDtos.Count(), result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
