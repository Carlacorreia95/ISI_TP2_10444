using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_TP2_10444_SmartHealth_Data.Dtos
{
    public record MeasurementDto(string SignalType, decimal Value, string Unit);

    public record SignalsInputDto(
        Guid WearableId,
        DateTime RegistrationDateTime,
        List<MeasurementDto> Measurement
    );

    public record SignalsResponseDto(
        bool Success,
        bool AlertGenerated,
        string Message
    );
}
