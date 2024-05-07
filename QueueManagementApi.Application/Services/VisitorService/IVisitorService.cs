using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.VisitorService;

public interface IVisitorService
{
    Task<Visit> Register(RegisterVisitorDto registerVisitorDto);
    Task<PagedList<AllVisitDto>> VisitGetAll(int page, int pageSize, string? search);
}