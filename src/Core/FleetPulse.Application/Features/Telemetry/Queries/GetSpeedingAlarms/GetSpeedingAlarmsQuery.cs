using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Telemetry.Queries.GetSpeedingAlarms
{
    public record GetSpeedingAlarmsQuery() : IRequest<List<SpeedingAlarmDto>>;
}
