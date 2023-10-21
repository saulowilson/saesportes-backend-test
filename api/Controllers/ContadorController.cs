using System.Text.Json;
using api.Hubs;
using api.Models;
using api.Providers;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace api.Controllers;

[ApiController]
[Route("contador")]
public class ContadorController : ControllerBase
{
    private readonly ILogger<ContadorController> _logger;

    public ContadorController(ILogger<ContadorController> logger)
    {
        _logger = logger;
    }

    [HttpPost("send-text", Name = "SendText")]
    public IActionResult Post(
        [FromServices] RabbitMQConnection mQConnection,
        [FromBody] TextInputDto body
    )
    {
        if (body.text == null || body.text == "")
        {
            return BadRequest("O texto não pode ser vazio");
        }

        var payload = new MessagePayload() { Text = body.text, IsValid = false };

        payload.IsValid = MessageVerify.validateMessage(payload.Text);

        mQConnection.PostMessage(payload);
        var json = JsonSerializer.Serialize(new { text = body.text });
        return Ok(json);
    }

    //get-messages é um endpoint SignalR e está mapeado em Program.cs
}

public class TextInputDto
{
    public string? text { get; set; }
}
