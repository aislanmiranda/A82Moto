using Application.Dtos;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Implementation;

public class DeliveryManService : IDeliveryManService
{
    private readonly IDeliveryManRepository _repository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeliveryManService(IDeliveryManRepository repository,
        IMapper mapper, IWebHostEnvironment env,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddAsync(DeliveryManRequestDto dto)
    {
        try
        {
            bool existDeliveryMan = await _repository.ExistDeliveryManWithCnpjAndCnh(dto.Cnpj, dto.NumberCnh);
            if (existDeliveryMan)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

            var deliveryman = _mapper.Map<DeliveryMan>(dto);
            await _repository.AddAsync(deliveryman);
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task UpdatePhotoAsync(DeliveryManUpdatePhotoDto dto)
    {
        try
        {
            var model = await _repository.GetByIdAsync(dto.Id);

            var resultPhoto = SavePhoto(dto.PhotoFile);

            DeleteImage(model.PhotoCnh);

            var result = await _repository.UpdatePhotoAsync(dto.Id, resultPhoto.Item1, resultPhoto.Item2);

            if (!result)
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_403, ExceptionMessage.ERROR_403_MSG);

        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    public async Task<IEnumerable<DeliveryManResponseDto>> GetAll()
    {
        try
        {
            var model = await _repository.GetAllAsync();

            var results = _mapper.Map<IEnumerable<DeliveryManResponseDto>>(model);

            return results;
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    private Tuple<string, string> SavePhoto(IFormFile? imageFile)
    {
        try
        {
            var contentPath = this._env.ContentRootPath;
            
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ext = Path.GetExtension(imageFile?.FileName);
            var allowedExtensions = new string[] { ".png", ".bmp" };
            if (!allowedExtensions.Contains(ext))
            {
                string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                throw new HttpExceptionCustom(ExceptionMessage.ERROR_415, ExceptionMessage.ERROR_415_MSG);
            }
            string uniqueString = Guid.NewGuid().ToString();
            
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);
            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile?.CopyTo(stream);
            stream.Close();

            var pathFile = Path.Combine(PathPhoto(), newFileName);

            return new Tuple<string, string>(newFileName, pathFile);
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    private void DeleteImage(string imageFileName)
    {
        try
        {
            if (imageFileName == string.Empty)
                return;

            var wwwPath = this._env.ContentRootPath;
            var path = Path.Combine(wwwPath, "Uploads/", imageFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (HttpExceptionCustom ex)
        {
            throw new HttpExceptionCustom(ex.StatusCode, ex.Message);
        }
    }

    private string PathPhoto()
    {
        var address = _httpContextAccessor?.HttpContext?.Request.Host.Host;
        var port = _httpContextAccessor?.HttpContext?.Request.Host.Port;

        return $"http://{address}:{port}/resources/";
    }
}
