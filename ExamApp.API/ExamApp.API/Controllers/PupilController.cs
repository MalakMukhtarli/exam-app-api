using ExamApp.API.Models.Pupils;
using ExamApp.API.Routes;
using ExamApp.API.Services.Pupils;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PupilController  : ControllerBase
{
    [HttpGet(ApiRoutes.Pupil.GetAll)]
    public async Task<IActionResult> GetAll([FromServices] IPupilService service)
    {
        var data = await service.GetAll();
        return Ok(data);
    }
    
    [HttpPost(ApiRoutes.Pupil.Create)]
    public async Task<IActionResult> Create([FromBody]SavePupilRequest request, [FromServices] IPupilService service)
    {
        var data = await service.Create(request);
        return Ok(data);
    }
    
    [HttpGet(ApiRoutes.Pupil.Get)]
    public async Task<IActionResult> Get([FromRoute] int pupilId, [FromServices] IPupilService service)
    {
        var data = await service.GetById(pupilId);
        return Ok(data);
    }
    
    [HttpPut(ApiRoutes.Pupil.Update)]
    public async Task<IActionResult> Update([FromRoute] int pupilId,[FromBody] UpdatePupilRequest request, [FromServices] IPupilService service)
    {
        var data = await service.Update(pupilId, request);
        return Ok(data);
    }
    

    [HttpDelete(ApiRoutes.Pupil.Delete)]
    public async Task<IActionResult> Delete([FromRoute]int pupilId,  [FromServices] IPupilService service)
    {
        await service.Delete(pupilId);
        return Ok();
    }
}