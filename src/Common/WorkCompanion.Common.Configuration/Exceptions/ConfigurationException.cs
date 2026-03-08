namespace WorkCompanion.Common.Configuration.Exceptions;

/// <summary>
/// Exception thrown when required configuration is missing or invalid
/// </summary>
public class ConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ConfigurationException class
    /// </summary>
    public ConfigurationException() { }

    /// <summary>
    /// Initializes a new instance of the ConfigurationException class with a configuration key
    /// </summary>
    /// <param name="configurationKey">The configuration key that was not found</param>
    public ConfigurationException(string? configurationKey)
        : base($"Configuration '{configurationKey}' not found.") { }

    /// <summary>
    /// Initializes a new instance of the ConfigurationException class with a message and inner exception
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception that caused this exception</param>
    public ConfigurationException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
