using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EstimationApplication.API.Models;
using EstimationApplication.BusinessRule;
using EstimationApplication.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EstimationApplication.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstimationController : ControllerBase
    {
        private readonly IEstimateBusiness estimateBusiness;
        private readonly IUserBusiness userBusiness;
        private readonly Func<string, IPrintBusiness> printBusiness;
        private readonly ILogger logger;

        public EstimationController(IEstimateBusiness _estimateBusiness, IUserBusiness _userBusiness, Func<string, IPrintBusiness> _printBusiness, ILogger<EstimationController> _logger)
        {
            estimateBusiness = _estimateBusiness;
            userBusiness = _userBusiness;
            printBusiness = _printBusiness;
            logger = _logger;
        }

        [HttpPost]
        [Route("getEstimate")]
        public ActionResult<EstimateResponseModel> GetEstimate([FromForm] EstimateRequestModel estimateModel)
        {
            try
            {
                var estimate = GetEstimateModelFromRequestModel(estimateModel);
                if (estimate != null)
                {
                    estimateBusiness.CalculateEstimate(estimate);
                    return Ok(new EstimateResponseModel
                    {
                        Estimate = estimate,
                        Status = EstimationApplicationConstant.OkStatus,
                        Message = EstimationApplicationConstant.EstimationCalculationSuccessful
                    });
                }
                else return BadRequest(EstimationApplicationConstant.CustomerNotFound);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("getPrintEstimate")]
        public ActionResult<PrintResponseModel> GetPrintEstimate([FromForm] PrintRequestModel printModel)
        {
            var estimate = GetEstimateModelFromRequestModel(printModel.Estimation);
            if (estimate != null)
            {
                estimateBusiness.CalculateEstimate(estimate);
                Task<PrintResponseModel> printResp = null;
                switch (printModel.PrintType)
                {
                    case PrintType.PrintToScreen:
                        printResp = PrintEstimate(estimate, PrintType.PrintToScreen.ToString(), EstimationApplicationConstant.PrintScreenSuccessful);
                        break;
                    case PrintType.PrintToFile:
                        printResp = PrintEstimate(estimate, PrintType.PrintToFile.ToString(), EstimationApplicationConstant.PrintFileSuccessful);
                        break;
                    case PrintType.PrintToPaper:
                        printResp = PrintEstimate(estimate, PrintType.PrintToPaper.ToString(), EstimationApplicationConstant.PrintPaperNotImplemented);
                        break;
                    default:
                        break;
                }
                return Ok(printResp);
            } 
            return BadRequest(EstimationApplicationConstant.EstimationDataWrong);
        }

        private Task<PrintResponseModel> PrintEstimate(EstimationModel estimation, string printType, string printMessasge)
        {
            return Task.Run(() =>
            {
                try
                {
                    var print = printBusiness(printType).Print(estimation);
                    if (print != null)
                    {
                        return new PrintResponseModel
                        {
                            Print = print,
                            Status = EstimationApplicationConstant.OkStatus,
                            Message = printMessasge
                        };
                    }
                }
                catch (NotImplementedException ex)
                {
                    logger.LogError(ex.Message);
                    return new PrintResponseModel
                    {
                        Status = EstimationApplicationConstant.PrintWarningStatus,
                        Message = printMessasge
                    };
                }
                return null;
            });
        }

        private EstimationModel GetEstimateModelFromRequestModel(EstimateRequestModel estimateRequestModel)
        {
            CustomerModel cust = userBusiness.FindCustomerByUserName(GetUserNameFromJwtToken());
            if (cust != null)
            {
                return new EstimationModel
                {
                    Customer = cust,
                    GoldPricePerGram = estimateRequestModel.GoldPricePerGram,
                    WeightInGram = estimateRequestModel.WeightInGram
                };
            }
            return null;
        }

        private string GetUserNameFromJwtToken()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
