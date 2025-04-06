namespace Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

public class ScriptDebuggerDetailCreateDto : BaseDto
{
	public int ScriptDebuggerId { get; set; }
	public string? ResultValue { get; set; }

}