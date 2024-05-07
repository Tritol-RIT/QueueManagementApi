using QueueManagementApi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagementApi.Application.Services.CheckinService
{
    public interface ICheckinService
    {
        Task<int> StartCheckin (int exhibitId, string QrCodeGUID);
        Task<bool> insuranceFind(int exhibitId);
        Task updateVisit(InsuranceDto item,string QrCodeGUID);
        void checkOut();
    }
}
