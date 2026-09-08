using FleetPulse.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Assets.Commands.CreateAsset
{
    public record CreateAssetCommand(string Name, string SerialNumber, AssetType Type) : IRequest<string>;
}
