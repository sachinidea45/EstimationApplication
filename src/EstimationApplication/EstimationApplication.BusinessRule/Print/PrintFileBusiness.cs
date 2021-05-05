using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using EstimationApplication.Entities;

namespace EstimationApplication.BusinessRule
{
    public class PrintFileBusiness : PrintBusinessBase
    {
        private string path;

        public PrintFileBusiness(IConfiguration _configuraiton) : base(_configuraiton)
        {
            path = configuration[EstimationApplicationConstant.OutputFilePath].ToString();
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
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(estimationDataText);
            }
            return filePath;
        }
    }
}
