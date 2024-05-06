using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.VisitorService;

public interface IVisitorService
{
    Task<Visit> Register(RegisterVisitorDto registerVisitorDto);
    Task<List<AllVisitDto>> VisitGetAll();
}