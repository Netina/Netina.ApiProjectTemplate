namespace Server.WebFramework.Bases;

[ApiController]
[ApiResultFilter]
[Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
public class BaseController : ControllerBase
{
    public bool UserIsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
}

[ApiVersion("1")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CrudController<TDto, TEntity> : BaseController
where TDto : BaseDto<TDto, TEntity>, new()
where TEntity : ApiEntity, new()
{
    protected readonly IRepositoryWrapper _repositoryWrapper;

    public CrudController(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    // GET:Get All Entity
    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var projectTo = typeof(TDto).BaseType?.GetProperty("ProjectToDto")?.GetValue(null, null);
        if (projectTo != null)
        {
            var exprss = projectTo as Expression<Func<TEntity, TDto>>;
            var entites = await _repositoryWrapper
                .SetRepository<TEntity>()
                .TableNoTracking
                .Select(exprss)
                .ToListAsync(cancellationToken);
            return Ok(entites);
        }
        else
        {
            throw new Exception("ProjectTo Not Found");
        }
    }

    // GET:Get An Entity By Id
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper.SetRepository<TEntity>().GetByIdAsync(cancellationToken, id);
        var dto = ent.Adapt<TDto>();
        return Ok(dto);
    }

    // POST:Add New Entity
    [HttpPost]
    public virtual async Task<IActionResult> PostOrginal([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper.SetRepository<TEntity>().AddAsync(ent, cancellationToken);
        return Ok(ent);
    }

    // POST:Add New Entity By Dto
    [HttpPost("Dto")]
    public async Task<IActionResult> PostDto([FromBody] TDto dto, CancellationToken cancellationToken)
    {
        await _repositoryWrapper
            .SetRepository<TEntity>()
            .AddAsync(dto.ToEntity(), cancellationToken);
        return Ok();
    }

    // PUT:Update Entity
    [HttpPut]
    public virtual async Task<IActionResult> Put([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper
            .SetRepository<TEntity>()
            .UpdateAsync(ent, cancellationToken);
        return Ok();
    }

    // DELETE:Delete Entity
    [HttpDelete, Route("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper
            .SetRepository<TEntity>()
            .GetByIdAsync(cancellationToken, id);
        await _repositoryWrapper.SetRepository<TEntity>().DeleteAsync(ent, cancellationToken);
        return Ok();
    }
}

[ApiVersion("1")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CrudController<TEntity> : BaseController
where TEntity : ApiEntity, new()
{
    protected readonly IRepositoryWrapper _repositoryWrapper;

    public CrudController(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    // GET:Get All Entity
    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _repositoryWrapper.SetRepository<TEntity>().TableNoTracking.ToListAsync());
    }

    // GET:Get An Entity By Id
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper.SetRepository<TEntity>().GetByIdAsync(cancellationToken, id);
        if (ent == null)
            throw new BaseApiException(ApiResultStatusCode.NotFound, "موردی پیدا نشد");
        return Ok(ent);
    }

    // POST:Add New Entity
    [HttpPost]
    public virtual async Task<IActionResult> Post([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper.SetRepository<TEntity>().AddAsync(ent, cancellationToken);
        return Ok(ent);
    }

    // PUT:Update Entity
    [HttpPut]
    public virtual async Task<IActionResult> Put([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper.SetRepository<TEntity>().UpdateAsync(ent, cancellationToken);
        return Ok();
    }

    // DELETE:Delete Entity
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper.SetRepository<TEntity>().GetByIdAsync(cancellationToken, id);
        await _repositoryWrapper.SetRepository<TEntity>().DeleteAsync(ent, cancellationToken);
        return Ok();
    }
}


[ApiVersion("1")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CrudController<TEntity, TSDto, TLDto> : BaseController
    where TSDto : BaseDto<TSDto, TEntity>, new()
    where TLDto : BaseDto<TLDto, TEntity>, new()
    where TEntity : ApiEntity, new()
{
    protected readonly IRepositoryWrapper _repositoryWrapper;

    public CrudController(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    // GET:Get All Entity
    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var projectTo = typeof(TSDto).BaseType?.GetProperty("ProjectToDto")?.GetValue(null, null);
        if (projectTo is Expression<Func<TEntity, TSDto>> exprss)
        {
            try
            {

                var entites = await _repositoryWrapper
                    .SetRepository<TEntity>()
                    .TableNoTracking
                    .Select(exprss)
                    .ToListAsync();
                return Ok(entites);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
            throw new AppException("ProjectTo Not Found");
    }

    // GET:Get An Entity By Id
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper
            .SetRepository<TEntity>()
            .GetByIdAsync(cancellationToken, id);
        var dto = ent.Adapt<TLDto>();
        return Ok(dto);
    }

    // POST:Add New Entity
    [HttpPost]
    public virtual async Task<IActionResult> PostOrginal([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper.SetRepository<TEntity>().AddAsync(ent, cancellationToken);
        return Ok(ent);
    }

    // POST:Add New Entity By Dto
    [HttpPost("Dto")]
    public virtual async Task<IActionResult> PostDto([FromBody] TSDto dto, CancellationToken cancellationToken)
    {
        await _repositoryWrapper
            .SetRepository<TEntity>()
            .AddAsync(dto.ToEntity(), cancellationToken);
        return Ok();
    }

    // PUT:Update Entity
    [HttpPut]
    public virtual async Task<IActionResult> Put([FromBody] TEntity ent, CancellationToken cancellationToken)
    {
        await _repositoryWrapper
            .SetRepository<TEntity>()
            .UpdateAsync(ent, cancellationToken);
        return Ok();
    }

    // DELETE:Delete Entity
    [HttpDelete, Route("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ent = await _repositoryWrapper
            .SetRepository<TEntity>()
            .GetByIdAsync(cancellationToken, id);
        await _repositoryWrapper.SetRepository<TEntity>().DeleteAsync(ent, cancellationToken);
        return Ok();
    }
}
