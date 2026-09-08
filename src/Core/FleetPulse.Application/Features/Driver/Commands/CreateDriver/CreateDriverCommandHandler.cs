using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Commands.CreateDriver
{
    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, string>
    {
        private readonly IDriverRepository _driverRepository;

        public CreateDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<string> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = await _driverRepository.GetByNationalIdAsync(request.NationalId);
            if (existingDriver != null)
            {
                throw new InvalidOperationException($"'{request.NationalId}' TC kimlik numarasına sahip sürücü zaten kayıtlı.");
            }

            var driver = new Domain.Entities.Driver
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalId = request.NationalId,
                PhoneNumber = request.PhoneNumber,
                LicenseType = request.LicenseType,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _driverRepository.AddAsync(driver);
            return driver.Id;
        }
    }
}