
namespace Reserve.Infrastructure.Telemetry;

public class LoggerExtensions<T>
{
    ILogger<T> logger;

    public LoggerExtensions(ILogger<T> logger)
    {
        this.logger = logger;
    }

    public void LogInformationWithStructure(
        string correlationId,
        [CallerMemberName] string methodName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0,
        params object[] args)
    {
        var className = System.IO.Path.GetFileNameWithoutExtension(sourceFilePath);
        var extendedArgs = args.ToList();
        
        extendedArgs.AddRange(new object[] 
        { 
            className, 
            methodName, 
            sourceLineNumber
        });
        
        this.logger.LogInformation(
            "CorrelationId: {correlationId}, Class: {className}, Method: {methodName}, Line: {sourceLineNumber}", 
            correlationId,
            className, 
            methodName, 
            sourceLineNumber, 
            args
            );
    }

    public void LogErrorWithStructure(
        Exception? exception,
        string correlationId,
        [CallerMemberName] string methodName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0,
        params object[] args)
    {
        string className = System.IO.Path.GetFileNameWithoutExtension(sourceFilePath);
        var extendedArgs = args.ToList();
        extendedArgs.AddRange(new object[] 
        { 
            className, 
            methodName, 
            sourceLineNumber
        });

        this.logger.LogError(
            exception, 
            "CorrelationId: {correlationId}, Class: {className}, Method: {methodName}, Line: {sourceLineNumber}", 
            correlationId,
            className, 
            methodName, 
            sourceLineNumber,
            args
            );
    }
}