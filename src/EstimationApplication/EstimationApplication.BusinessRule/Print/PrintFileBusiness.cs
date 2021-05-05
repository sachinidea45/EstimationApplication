using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;
using Microsoft.Extensions.Logging;

namespace EstimationApplication.BusinessRule
{
    public class PrintFileBusiness : PrintBusinessBase
    {
        private string path;
        private readonly ILogger logger;

        public PrintFileBusiness(IConfiguration _configuraiton, ILogger<PrintFileBusiness> _logger) : base(_configuraiton)
        {
            path = configuration[EstimationApplicationConstant.OutputFilePath].ToString();
            logger = _logger;
        }

        public override PrintModel Print(EstimationModel estimation)
        {
            var estimationDataText = GetEstimationDataText(estimation);
            var filePath = WriteToFile(estimationDataText);
            return new PrintModel { Estimation = estimation, PrintMessageOutput = EstimationApplicationConstant.PrintFileMessage + filePath };
        }

        private string WriteToFile(string estimationDataText)
        {
            string fileName = DateTime.Now.ToString(EstimationApplicationConstant.PrintFileNameTimeStampFormat);
            string filePath = path + "\\" + fileName + EstimationApplicationConstant.PrintFileExtension;
            try
            {
                logger.LogInformation("Started Writing To File");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(estimationDataText);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ExstimationApplicationBusinessException(ex.Message);
            }
        }
    }
}
