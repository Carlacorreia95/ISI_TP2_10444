using ISI_TP2_10444.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using CoreWCF;


namespace ISI_TP2_10444.Services.Soap
{
    // Define a interface do Serviço SOAP
    [ServiceContract]
    public interface IServiceMotorRules
    {
        /// <summary>
        /// Processa sinais vitais para verificar a violação de regras e gerar alertas.
        /// </summary>
        [OperationContract]
        Task<ResultProcessing> ProcessingSignalsAsync(
            Guid registID,
            Guid patientID,
            DateTime dateHourRegist,
            List<Measurement> measurement
        );

        // Outros métodos SOAP podem ser adicionados aqui (ex: GetRegrasAtivas)
    }
}

