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
        Logger.Log(ELogLevel.Debug, "Starting - [method:GetEnums] [class:EnumController]");

        try
        {
            var isTonEnabled = await _featureToggleService.IsFeatureEnabled("weight-ton");
            var isAmuEnabled = await _featureToggleService.IsFeatureEnabled("weight-amu");
            
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
                        (isTonEnabled || e != EWeightUnit.Ton) &&
                        (isAmuEnabled || e != EWeightUnit.AtomicMassUnit)
                    )
                    .Select(e => new { name = e.ToString(), value = (int)e }),
            };

            // Log and return the result
            Logger.Log(ELogLevel.Info, $"Success - [method:GetEnums] with fetching converter library enums.");
            return Ok(enumData);
        }
        catch (Exception ex)
        {
            // Log error if any issue arises during fetching enums
            Logger.Log(ELogLevel.Error, $"Failed - [method:GetEnums] with error-message: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching enums.");
        }
    }
}