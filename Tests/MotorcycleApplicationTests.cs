using Application.Dtos;
using Application.Implementation;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Infra.Messaging;
using MongoDB.Driver;
using Moq;

public class MotorcycleApplicationTests
{
    private readonly Mock<IMotorcycleRepository> _mockRepository;
    private readonly Mock<INotificationRepository> _mockNotificationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRabbitMQService> _mockMessageBus;
    private readonly MotorcycleService _service;
    private readonly Mock<IMongoCollection<Motorcycle>> _mockCollection;
    
    public MotorcycleApplicationTests()
    {
        _mockRepository = new Mock<IMotorcycleRepository>();
        _mockNotificationRepository = new Mock<INotificationRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockMessageBus = new Mock<IRabbitMQService>();
        _service = new MotorcycleService(
            _mockRepository.Object,
            _mockNotificationRepository.Object,
            _mockMapper.Object,
            _mockMessageBus.Object
        );
        _mockCollection = new Mock<IMongoCollection<Motorcycle>>();
        
    }

    [Fact]
    public async Task AddAsync_ValidDto_AddsMotorcycleAndPublishesMessageFor2024()
    {
        // Arrange
        var dto = new MotorcycleRequestDto
        {
            Model = "CG 125",
            Plate = "ABC-1234",
            Year = 2024
        };

        var dtoExpected = new Motorcycle
        (
            year: 2024,
            model: "CG 125",
            plate: "ABC-1234"
        );

        var notificationRequest = new NotificationRequestDto
        {
            EventName = "Created Motorcycle",
            Message = "Created Motorcycle XYZ year 2024",
            SendDate = DateTime.Now
        };

        var notificationExpected = new Notification(
            eventName: "Created Motorcycle",
            message: "Created Motorcycle XYZ year 2024",
            sendDate: notificationRequest.SendDate
        );

        _mockRepository.Setup(repo => repo.GetByPlateAsync(dto.Plate))
            .ReturnsAsync((Motorcycle)null);

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Motorcycle>()))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(mapper => mapper.Map<Motorcycle>(dto))
            .Returns(dtoExpected);

        var _mockRabbitMQService = new Mock<IRabbitMQService>();
        
        _mockMapper.Setup(mapper => mapper.Map<Notification>(notificationRequest))
            .Returns(notificationExpected);

        _mockRabbitMQService.Object.PublishMessage(notificationExpected);

        // Act
        await _service.AddAsync(dto);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Motorcycle>()), Times.Once);
        _mockRabbitMQService.Verify(service => service.PublishMessage(notificationExpected), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PlateExists_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var dto = new MotorcycleRequestDto
        {
            Model = "CG 125",
            Plate = "ABC-1234",
            Year = 2024
        };
        var dtoExpected = new Motorcycle
        (
            year: 2024,
            model: "CG 125",
            plate: "ABC-1234"
        );

        _mockRepository.Setup(repo => repo.GetByPlateAsync(dto.Plate))
            .ReturnsAsync(dtoExpected);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.AddAsync(dto));
    }

    [Fact]
    public async Task UpdatePlateAsync_ValidDto_UpdatePlateSuccessfully()
    {
        // Arrange
        var dto = new MotocycleUpdatePlateDto { Id = Guid.NewGuid(), Plate = "XYZ5678" };

        var expectedUpdateResult = new UpdateResult.Acknowledged(1, 1, dto.Id);

        _mockRepository.Setup(repo => repo.UpdatePlateAsync(dto.Id, dto.Plate))
            .ReturnsAsync(expectedUpdateResult);

        // Act
        await _service.UpdatePlateAsync(dto);

        // Assert
        _mockRepository.Verify(repo => repo.UpdatePlateAsync(dto.Id, dto.Plate), Times.Once);
    }

    [Fact]
    public async Task UpdatePlateAsync_NoMatchedDocuments_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var dto = new MotocycleUpdatePlateDto { Id = Guid.NewGuid(), Plate = "PPQ-5678" };
        var expectedUpdateResult = new UpdateResult.Acknowledged(0, 0, dto.Id);

        _mockRepository.Setup(repo => repo.UpdatePlateAsync(dto.Id, dto.Plate))
            .ReturnsAsync(expectedUpdateResult);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.UpdatePlateAsync(dto));
    }

    [Fact]
    public async Task UpdatePlateAsync_NoModifiedDocuments_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var dto = new MotocycleUpdatePlateDto { Id = Guid.NewGuid(), Plate = "XYZ5678" };
        var expectedUpdateResult = new UpdateResult.Acknowledged(1, 0, dto.Id);
        _mockRepository.Setup(repo => repo.UpdatePlateAsync(dto.Id, dto.Plate))
            .ReturnsAsync(expectedUpdateResult);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.UpdatePlateAsync(dto));
    }

    [Fact]
    public async Task DeleteAsync_MotocycleIsRentend_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.MotocycleInUseAsync(id))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.DeleteAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_MotocycleNotFound_ThrowsHttpExceptionCustom()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.MotocycleInUseAsync(id))
            .ReturnsAsync(false);
        _mockRepository.Setup(repo => repo.DeleteAsync(id))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<HttpExceptionCustom>(() => _service.DeleteAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_ValidId_DeletesMotorcycleSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.MotocycleInUseAsync(id))
            .ReturnsAsync(false);
        _mockRepository.Setup(repo => repo.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMotorcycleResponseDtos()
    {
        // Arrange
        var motorcycles = new List<Motorcycle> { new Motorcycle(2024,"CG125","PPL-1234") };
        var responseDtos = new List<MotorcycleResponseDto> { new MotorcycleResponseDto() };
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<bool>()))
            .ReturnsAsync(motorcycles);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<MotorcycleResponseDto>>(motorcycles))
            .Returns(responseDtos);

        // Act
        var result = await _service.GetAllAsync(true);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<MotorcycleResponseDto>>(result);
    }

}
