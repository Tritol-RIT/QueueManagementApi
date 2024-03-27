using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Application.Services.SetPasswordTokenService;

public class SetPasswordTokenService : ISetPasswordTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<SetPasswordToken> _setPasswordTokenRepository;

    public SetPasswordTokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        _setPasswordTokenRepository = _unitOfWork.Repository<SetPasswordToken>();
    }

    public async Task<bool> ValidateAsync(string token)
    {
        var setPasswordtoken = await _setPasswordTokenRepository.FindByCondition(x => x.Token == token).FirstOrDefaultAsync();

        if (setPasswordtoken == null)
            return false;

        return setPasswordtoken.Active && setPasswordtoken.ExpirationDate > DateTime.UtcNow;
    }
}