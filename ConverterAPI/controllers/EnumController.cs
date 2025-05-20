using Converter.Type;
using Converter.Volume;
using Converter.Weight;
using FeatureToggle;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;

namespace ConverterAPI.controllers;

[ApiController]
[Route("api/enums")]
public class EnumController : ControllerBase
{
    private readonly IFeatureToggleService _featureToggleService;
    public EnumController(IFeatureToggleService featureToggleService)
    {
        _featureToggleService = featureToggleService;
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetEnums()
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(GetEnums), nameof(EnumController)));

        try
        {
            var isMicrogramEnabled = await _featureToggleService.IsFeatureEnabled("weight-microgram");
            var isMilligramEnabled = await _featureToggleService.IsFeatureEnabled("weight-milligram");
            
            var enumData = new
            {
                converterTypes = Enum.GetValues(typeof(EConverterType))
                    .Cast<EConverterType>()
                    .Select(e => new { name = e.ToString(), value = (int)e }),

                converterFunctions = Enum.GetValues(typeof(EConverterFunction))
                    .Cast<EConverterFunction>()
                    .Select(e => new { name = e.ToString(), value = (int)e }),

                volumeUnits = Enum.GetValues(typeof(EVolumeUnit))
                    .Cast<EVolumeUnit>()
                    .Select(e => new { name = e.ToString(), value = (int)e }),

                weightUnits = Enum.GetValues(typeof(EWeightUnit))
                    .Cast<EWeightUnit>()
                    .Where(e =>
                        (isMicrogramEnabled || e != EWeightUnit.Microgram) &&
                        (isMilligramEnabled || e != EWeightUnit.Milligram)
                    )
                    .Select(e => new { name = e.ToString(), value = (int)e }),
            };

            // Log and return the result
            Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(GetEnums), "fetching enums."));
            return Ok(enumData);
        }
        catch (Exception ex)
        {
            // Log error if any issue arises during fetching enums
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(GetEnums), ex.Message));
            return StatusCode(500, "An error occurred while fetching enums.");
        }
    }
}