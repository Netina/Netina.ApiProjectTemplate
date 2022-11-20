using Common.Models.Exception;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Common.Models.Mapper;

/// <summary>
///     Base Dto Class initial map config between entity and dto
/// </summary>
/// <typeparam name="TDto">Type of Dto Class</typeparam>
/// <typeparam name="TEntity">Type of Entity Class</typeparam>
public abstract class BaseDto<TDto, TEntity> : INotifyPropertyChanged, IBaseDto where TEntity : class where TDto : class
{

    public static Expression<Func<TEntity, TDto>> ProjectToDto
    {
        get => GetProjectToDto();
    }
    private static Expression<Func<TEntity, TDto>> GetProjectToDto()
    {
        var assembly = typeof(TEntity).Assembly;
        var mapperName = $"{typeof(TEntity).Name}Mapper";
        var mapperType = assembly.GetTypes()?.FirstOrDefault(t => t.Name.Contains(mapperName));
        if (mapperType == null)
            throw new AppException($"{typeof(TEntity).Name}Mapper Not Found!");
        if (typeof(TDto).Name.Contains("SDto"))
        {
            var projectProperty = mapperType.GetProperty("ProjectToSDto");
            if (projectProperty == null)
                throw new AppException($"{typeof(TEntity).Name}Mapper Dont Have ProjectTo");
            return projectProperty.GetValue(null, null) as Expression<Func<TEntity, TDto>>;
        }
        else if (typeof(TDto).Name.Contains("LDto"))
        {
            var projectProperty = mapperType.GetProperty("ProjectToLDto");
            if (projectProperty == null)
                throw new AppException($"{typeof(TEntity).Name}Mapper Dont Have ProjectTo");
            return projectProperty.GetValue(null, null) as Expression<Func<TEntity, TDto>>;
        }
        else
            throw new AppException($"{typeof(TDto).Name} Projection Not Implemented");
    }
    public virtual bool Compare(object obj)
    {
        return Equals(obj);
    }

    public TDto Clone()
    {
        return (TDto)MemberwiseClone();
    }

    public TEntity ToEntity()
    {
        var assembly = typeof(TEntity).Assembly;
        var mapperName = $"{typeof(TEntity).Name}Mapper";
        var mapperType = assembly.GetTypes()?.FirstOrDefault(t => t.Name.Contains(mapperName));
        var toEntityMethodInfo = mapperType.GetMethod($"AdaptTo{typeof(TEntity).Name}");
        var parms = new[] { this };
        var entity = toEntityMethodInfo.Invoke(null, parms);
        if (entity is TEntity o)
            return o;
        return null;
    }

    public static TDto FromEntity(TEntity model)
    {
        var assembly = typeof(TEntity).Assembly;
        var mapperName = $"{typeof(TEntity).Name}Mapper";
        var mapperType = assembly.GetTypes()?.FirstOrDefault(t => t.Name.Contains(mapperName));
        var toDtoMethodInfo = mapperType.GetMethod("AdaptToDto");
        var parms = new[] { model };
        var dto = toDtoMethodInfo.Invoke(null, parms);
        if (dto is TDto o)
            return o;
        return null;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}